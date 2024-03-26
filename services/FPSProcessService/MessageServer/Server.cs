using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Timers;

using DataLogging.LogEntries;
using DataLogging.LogWriters;
using System.Configuration;
using CredECard.Common.Enums.Authorization;
//using CredECard.ServiceProvider.BusinessService;

namespace ContisGroup.MessageServer
{
    [Serializable()]
    public class TCPServer
    {
        public event TCPDelegates.SrvClientEvent ClientConnect;
        public event TCPDelegates.SrvClientEvent SocketClose;
        public event TCPDelegates.SrvClientEvent LogError;

        private void OnClientConnect(object sender, TCPEventArgs e)
        {
            if (ClientConnect != null) ClientConnect(sender, e);
        }

        private void OnSocketClose(object sender, TCPEventArgs e)
        {
            if (SocketClose != null) SocketClose(sender, e);
        }

        private void OnSocketLog(object sender, TCPEventArgs e)
        {
            if (LogError != null) LogError(sender, e);
        }

        // Timer for cleaning up old sockets
        private System.Timers.Timer _tmCleanup = null;

        // Settings for this server
        private TCPSettings _settings = null;

        // Our socket collection
        private ArrayList _sockets = null;

        // This is our listener
        private TcpListener _listen = null;

        // This is our processor (just 1 for now)
        public ProcessorWrapper Processor = null;

        // This is our logging object
        private ILogWriter _log = null;

        //private OnlineProviderPort _providerDetails = null;

        public TCPServer(TCPSettings settings, ProcessorWrapper pro, ILogWriter log)
        {
            _settings = settings;
            this.Processor = pro;
            _log = log;
            _tmCleanup = new System.Timers.Timer();
            _tmCleanup.Interval = 60000;
            _tmCleanup.Elapsed += new ElapsedEventHandler(_tmCleanup_Elapsed);
            _tmCleanup.AutoReset = false;
            _tmCleanup.Start();
        }

        private void _tmCleanup_Elapsed(object sender, ElapsedEventArgs e)
        {
            CleanUpUnused();
            _tmCleanup.Start();
        }

        public TCPSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        /// <summary>
        /// Starts listening for connections on the specified ip and port.
        /// Any exception thrown by default will go to the application event log, 
        /// as this function is executed syncronously when starting the
        /// service.
        /// </summary>
        public void StartListening()
        {
            // Create the listener
            //if (this._settings.ListenType == 1)
            //{
            _listen = new TcpListener(IPAddress.Parse(_settings.ListenIP), _settings.ListenPort);
            //}
            //else
            //{
            //    _listen = new TcpListener(IPAddress.Any, _settings.ListenPort);
            //}

            // And our socket array
            _sockets = new ArrayList();

            // Start listening
            _listen.Start(_settings.MaxPending);

            // Accept socket
            this.AcceptSocket();
        }

        /// <summary>
        /// Stops accepting incomming connections on this port and IP.
        /// Any clients waiting will get a SocketException error (or will just be disconnected).
        /// Any error raised should go to the application event log, as this function is only 
        /// called when stopping the service.
        /// </summary>
        public void StopListening()
        {
            // Stop the listener
            _listen.Stop();
        }

        /// <summary>
        /// Sends of the async request to get a new client socket from the pending clients.
        /// Any exceptions should go to the application event log, as this function is only
        /// called when starting the service (in theory).
        /// </summary>
        private void AcceptSocket()
        {
            // Accept the socket async
            _listen.BeginAcceptSocket(new AsyncCallback(OnClientConneted), null);
        }

        private void OnClientConneted(IAsyncResult result)
        {

            try
            {
                // Retrieve the socket ending the async accept
                Socket skClient = _listen.EndAcceptSocket(result);

                // Create a new socket binding the correct type of delegate
                SocketWrapper clientSock = null;

                int productType = Convert.ToInt32(ConfigurationManager.AppSettings["ProductType"].ToString());

                if (productType == (int)EnumProductType.MasterCard)
                    clientSock = GetSocketInstance(clientSock, skClient);
                else if (productType == (int)EnumProductType.FPS)
                    clientSock = GetFPSSocketInstance(clientSock, skClient);
                else
                    clientSock = GetVisaSocketInstance(clientSock, skClient);

                // Add it to out sockets collection
                int index = _sockets.Add(clientSock);

                // Let everyone know we have received a connection
                TCPEventArgs tcpE = new TCPEventArgs("Connection received " + index.ToString(), _settings, clientSock, DateTime.Now);
                OnClientConnect(this, tcpE);

                // Set the processor off doing it's work
                Processor.StartConversation(clientSock);

                // We have received one, so get another one
                this.AcceptSocket();
            }
            catch (SocketException ex)
            {
                _log.LogEntry(new TCPLogEntry(_settings.ToString(), ex.ErrorCode, ex.ToString()));
                FogBugzLogWriter.LogErrors(ex);
            }
            catch (Exception ex)
            {
                _log.LogEntry(new TCPLogEntry(_settings.ToString(), 0, ex.ToString()));
                FogBugzLogWriter.LogErrors(ex);
                CloseAllSockets();
            }
        }

        private SocketWrapper GetFPSSocketInstance(SocketWrapper clientSock, Socket skClient)
        {
            int messagelength = 0;

            messagelength = this.Settings.MessageByteLength;

            clientSock = new FPSSocketWrapper(
                skClient, 
                new ProcessorWrapper.DEOFLengthCheck(Processor.IsEOF), 
                new ProcessorWrapper.DProcessHeader(Processor.ProcessHeader),
                new ProcessorWrapper.DProcessMsg(Processor.ProcessMsg), 
                _settings);

            clientSock.IsMessageLengthEnabled = true;
            clientSock.MessageLengthBuffer = messagelength;

            // Wire up the socket events
            clientSock.SocketClose += new TCPDelegates.SrvClientEvent(clientSock_SocketClose);
            clientSock.LogError += new TCPDelegates.SrvClientEvent(clientSock_LogError);

            // Set a timeout for this socket as per passed settings
            clientSock.Timeout = new TimeSpan(0, 0, _settings.TimeoutSeconds);

            return clientSock;
        }

        private IPEndPoint GetVisaEndpoint()
        {
            IPEndPoint endpoint = null;
            IPAddress ip = new IPAddress(0);
            IPHostEntry local = Dns.GetHostEntry(ConfigurationManager.AppSettings["VisaHostIP"]);
            foreach (IPAddress ipaddress in local.AddressList)
            {
                ip = ipaddress;
            }
            int portNo = 0;
            int.TryParse(ConfigurationManager.AppSettings["VisaHostPortNo"], out portNo);
            endpoint = new IPEndPoint(ip, portNo);
            return endpoint;
        }

        private EndPoint GetLocalEndPoint()
        {
            return _listen.LocalEndpoint;
        }

        SocketWrapper GetVisaSocketInstance(SocketWrapper clientSock, Socket skClient)
        {
            int messagelength = 0;

            messagelength = this.Settings.MessageByteLength;

            clientSock = new VisaSocketWrapper(skClient, new ProcessorWrapper.DEOFLengthCheck(Processor.IsEOF), new ProcessorWrapper.DProcessHeader(Processor.ProcessHeader),
                new ProcessorWrapper.DProcessMsg(Processor.ProcessMsg), _settings);

            clientSock.IsMessageLengthEnabled = true;
            clientSock.MessageLengthBuffer = messagelength;

            // Wire up the socket events
            clientSock.SocketClose += new TCPDelegates.SrvClientEvent(clientSock_SocketClose);
            clientSock.LogError += new TCPDelegates.SrvClientEvent(clientSock_LogError);

            // Set a timeout for this socket as per passed settings
            clientSock.Timeout = new TimeSpan(0, 0, _settings.TimeoutSeconds);

            return clientSock;
        }

        SocketWrapper GetSocketInstance(SocketWrapper clientSock, Socket skClient)
        {
            int messagelength = 0;
            bool isMessageLengthEnabled = false;

            try
            {
                isMessageLengthEnabled = true;
                messagelength = 2;
            }
            catch (Exception ex)
            {
                _log.LogEntry(new TCPLogEntry(_settings.ToString(), 0, ex.ToString()));
                FogBugzLogWriter.LogErrors(ex);

                isMessageLengthEnabled = this.Settings.ISMessageLengthEnabled;
                messagelength = this.Settings.MessageByteLength;

            }

            //if (_settings.SocketMode == ESocketMode.Binary)
            //{
            //    if (isMessageLengthEnabled)
            //    {
            //        clientSock = new MCSocketWrapper(skClient, new ProcessorWrapper.DEOFLengthCheck(Processor.IsEOF),
            //            new ProcessorWrapper.DProcessMsg(Processor.ProcessMsg), _settings);
            //    }
            //    else
            //    {
            //        clientSock = new MCSocketWrapper(skClient, new ProcessorWrapper.DEOFCheck(Processor.IsEOF),
            //            new ProcessorWrapper.DProcessMsg(Processor.ProcessMsg), _settings);
            //    }
            //}
            //else
            //{
            //    if (isMessageLengthEnabled)
            //    {
            //        clientSock = new MCSocketWrapper(skClient, new ProcessorWrapper.DEOFLengthCheckS(Processor.IsEOF),
            //            new ProcessorWrapper.DProcessMsgS(Processor.ProcessMsg), _settings);
            //    }
            //    else
            //    {
            //        clientSock = new MCSocketWrapper(skClient, new ProcessorWrapper.DEOFCheckS(Processor.IsEOF),
            //         new ProcessorWrapper.DProcessMsgS(Processor.ProcessMsg), _settings);
            //    }
            //}

            clientSock.IsMessageLengthEnabled = isMessageLengthEnabled;
            clientSock.MessageLengthBuffer = messagelength;

            // Wire up the socket events
            clientSock.SocketClose += new TCPDelegates.SrvClientEvent(clientSock_SocketClose);
            clientSock.LogError += new TCPDelegates.SrvClientEvent(clientSock_LogError);

            // Set a timeout for this socket as per passed settings
            clientSock.Timeout = new TimeSpan(0, 0, _settings.TimeoutSeconds);

            return clientSock;
        }

        void clientSock_LogError(object sender, TCPEventArgs e)
        {

            OnSocketLog(sender, e);

            if (_settings.LogData == "1" && e.ToString().Length > 0)
            {
                SimpleLogWriter write = new SimpleLogWriter(ConfigurationManager.AppSettings["Detaillogpath"].ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log");
                write.LogEntry(new TCPLogEntry("Received :" + e.ServerSettings.ListenIP, 0, e.ToString()));
            }

        }

        private void CleanUpUnused()
        {
            //// We need to lock the sockets collection while we do this, 
            //// as this timer event is fired on a separate thread than the
            //// socket thread so the collection can be modified in 2
            //// places.
            if (_sockets == null) return;

            lock (_sockets.SyncRoot)
            {
                // No sockets? don't bother
                if (_sockets.Count == 0) return;

                int pos = 0;
                SocketWrapper tmp = null;
                do
                {
                    if (_sockets.Count > 0) tmp = _sockets[pos] as SocketWrapper;

                    // If it's not closing already and it's timed out, then close it
                    if (!tmp.Closing && tmp.IsTimedOut)
                    {
                        tmp.Close();
                    }

                    // If the socket is closing, remove it from our socket collection if not, move on to next socket
                    if (tmp.Closing) _sockets.RemoveAt(pos); else pos++;

                } while (pos <= (_sockets.Count - 1));
            }
        }

        //private void CleanUpUnused()
        //{
        //    // We need to lock the sockets collection while we do this, 
        //    // as this timer event is fired on a separate thread than the
        //    // socket thread so can the collection to be modified in 2
        //    // places.
        //    lock (_sockets.SyncRoot)
        //    {
        //        for (int i = 0; i < _sockets.Count; i++)
        //        {
        //            SocketWrapper tmp = _sockets[i] as SocketWrapper;

        //            // If it's not closing already and it's timed out, then close it
        //            if (!tmp.Closing && tmp.IsTimedOut) tmp.Close();

        //            // If the socket is closing, remove it from our socket collection
        //            if (tmp.Closing) _sockets.RemoveAt(i);
        //        }
        //    }
        //}

        private void clientSock_SocketClose(object sender, TCPEventArgs e)
        {
            //_sockets.Remove(sender);
            this.OnSocketClose(this, e);
        }

        public void CloseAllSockets()
        {
            // Close all the sockets
            lock (_sockets.SyncRoot)
            {
                foreach (SocketWrapper socket in _sockets)
                {
                    // If the socket is not null and not closing
                    if (socket != null) socket.Close();
                }
            }
        }

        public void SendAllSockets(string data)
        {
            // Send same message to all sockets
            lock (_sockets.SyncRoot)
            {
                foreach (SocketWrapper socket in _sockets)
                {
                    // If the socket is not null and not closing
                    if (socket != null) socket.Send(data);
                }
            }
        }

        public void SendAllSockets(byte[] data)
        {
            // Send same message to all sockets
            lock (_sockets.SyncRoot)
            {
                foreach (SocketWrapper socket in _sockets)
                {
                    if (socket != null) socket.Send(data);
                }
            }
        }




    }
}

