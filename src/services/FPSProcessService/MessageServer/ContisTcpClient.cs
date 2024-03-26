using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using CredEcard.CredEncryption.BusinessService;
using CredECard.Common.Enums.Authorization;
using DataLogging.LogWriters;
//using CredECard.ServiceProvider.BusinessService;

namespace ContisGroup.MessageServer
{
    [Serializable()]
	public class ContisTcpClient
	{
		public event TCPDelegates.SrvClientEvent ClientConnect;
		public event TCPDelegates.SrvClientEvent SocketClose;
        public event TCPDelegates.SrvClientEvent LogError;

        private Encryptionkey _logEncryptionKey = null;

        ClientSocketWrapper _clientSock = null;
        bool _isServiceStopped = false;

        /// <author>Nidhi Thakrar</author>
        /// <created>24-Apr-17</created>
        /// <summary>Gets the client socket.</summary>
        /// <value>The client socket.</value>
        public ClientSocketWrapper ClientSocket
        {
            get { return _clientSock; }
        }

		private void OnClientConnect(object sender, TCPEventArgs e)
		{
			if (ClientConnect != null) ClientConnect(sender, e);
		}

		private void OnSocketClose(object sender, TCPEventArgs e)
		{
			if (SocketClose != null) SocketClose(sender, e);

            if (!_isServiceStopped) this._tmReConnect.Start();
		}

        private void OnSocketLog(object sender, TCPEventArgs e)
        {
            if (LogError != null) LogError(sender, e);
        }

		// Timer for cleaning up old sockets
		private System.Timers.Timer _tmReConnect = null;

		// Settings for this server
		private TCPSettings _settings = null;

		// Our socket collection
        private ArrayList _sockets = null;

		// This is our listener
		private TcpClient _client = null;

		// This is our processor (just 1 for now)
		public ClientProcessorWrapper Processor = null;

		// This is our logging object
		private ILogWriter _log = null;

        //private OnlineProviderPort _providerDetails = null;

        public ContisTcpClient(TCPSettings settings, ClientProcessorWrapper pro, ILogWriter log, Encryptionkey logEncryptionKey)
		{
			_settings = settings;
            _logEncryptionKey = logEncryptionKey;
			this.Processor = pro;
			_log = log;
			_tmReConnect = new System.Timers.Timer();
			_tmReConnect.Interval = 60000;
			_tmReConnect.Elapsed += new ElapsedEventHandler(_tmCleanup_Elapsed);
			_tmReConnect.AutoReset = true;
            _tmReConnect.Start();
		}

		private void _tmCleanup_Elapsed(object sender, ElapsedEventArgs e)
		{
            if (!Connected())
                this.AcceptConnect();		
		}

        bool Connected()
        {
            bool blockingState = _client.Client.Blocking;

            try
            {
                byte[] tmp = new byte[1];

                _client.Client.Blocking = false;
                _client.Client.Send(tmp, 0, 0);
            }
            catch (Exception e)
            {
                LogData(DateTime.Now.ToString("DD-mmm-yyyy HH:mm:ss.fff tt") + " " + e.Message);
            }
            finally
            {
                try
                {
                    _client.Client.Blocking = blockingState;
                }
                catch (Exception)
                { }
            }

            return _client.Client.Connected;
        }

        public TCPSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        public bool IsServiceStopped
        {
            get { return _isServiceStopped; }
            set { _isServiceStopped = value; }
        }

		/// <summary>
		/// Starts listening for connections on the specified ip and port.
		/// Any exception thrown by default will go to the application event log, 
		/// as this function is executed syncronously when starting the
		/// service.
		/// </summary>
		public void StartClient()
		{
            //_listen.LocalEndpoint = 
			// Create the listener
            //CheckUseFixedPort();
            //// And our socket array
			_sockets = new ArrayList();
			
			// Start listening
			//_listen.Start(_settings.MaxPending);
            //_client.
          
			// Accept socket
			this.AcceptConnect();
		}

        private void CheckUseFixedPort()
        {


            if (_client != null && _client.Client != null)
            {
                try
                {
                    _client.Client.Shutdown(SocketShutdown.Both);
                    _client.Client.Close();
                }
                catch{}
            }

            _client = new TcpClient();

            _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            // _settings.ListenIP, _settings.ListenPort);

            string useFixedLocalPort = ConfigurationManager.AppSettings["UseFixedLocalPort"];

            if (useFixedLocalPort == "1")
            {
                string serverIp = ConfigurationManager.AppSettings["ClientHostIP"].ToString();
                int portNo = 0;
                int.TryParse(ConfigurationManager.AppSettings["ClientHostPortNo"], out portNo);
                IPEndPoint listionerEndPoint = new IPEndPoint(GetIPAddress(serverIp), portNo);

                //socket.Bind(listionerEndPoint);
                _client.Client.Bind(listionerEndPoint);
            }
        }

        private IPAddress GetIPAddress(string ipAddress)
        {
            IPAddress ip = new IPAddress(0);
            IPHostEntry local = Dns.GetHostEntry(ipAddress);
            foreach (IPAddress ipaddress in local.AddressList)
            {
                ip = ipaddress;
            }

            return ip;
        }

		/// <summary>
		/// Stops accepting incomming connections on this port and IP.
		/// Any clients waiting will get a SocketException error (or will just be disconnected).
		/// Any error raised should go to the application event log, as this function is only 
		/// called when stopping the service.
		/// </summary>
		public void StopListening()
		{
            if (_client.Connected)
            {
                //ClientSocketWrapper clientSock = null;
                TCPEventArgs tcpE = new TCPEventArgs("Server Disconnected", _settings, _clientSock, DateTime.Now);
                OnSocketClose(this, tcpE);
            }
            //this.OnSocketClose(this, e);

            if (_client.Client.Connected) _client.Client.Shutdown(SocketShutdown.Both);
            _client.Client.Close();

            _client.Close();
		}

		/// <summary>
		/// Sends of the async request to get a new client socket from the pending clients.
		/// Any exceptions should go to the application event log, as this function is only
		/// called when starting the service (in theory).
		/// </summary>
		private void AcceptConnect()
		{
			// Accept the socket async
			//_listen.BeginAcceptSocket(new AsyncCallback(OnClientConneted), null);
            string serverIp = ConfigurationManager.AppSettings["VisaHostIP"].ToString();
            int portNo = 0;
            int.TryParse(ConfigurationManager.AppSettings["VisaHostPortNo"], out portNo);
            CheckUseFixedPort();

            try
            {
                _client.BeginConnect(serverIp, portNo, new AsyncCallback(OnClientConneted), null);
            }
            catch (Exception ex)
            {
                this.StartClient();
            }
            //_client.Connect(serverIp, portNo);
		}

		private void OnClientConneted(IAsyncResult result)
		{
            ClientSocketWrapper clientSock = null;
            try
            {
                //Socket skClient = null;
                _client.EndConnect(result);

                int productType = Convert.ToInt32(ConfigurationManager.AppSettings["ProductType"].ToString());

                clientSock = GetVisaSocketInstance(clientSock, _client);
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
            finally
            {
                if (_client.Connected)
                {
                    TCPEventArgs tcpE = new TCPEventArgs("Server Connected", _settings, clientSock, DateTime.Now);
                    OnClientConnect(this, tcpE);
                    _clientSock = clientSock;
                    _tmReConnect.Interval = 90000;
                }
                //else
                //{
                    
                //}
            }
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
            return _client.Client.LocalEndPoint;
        }

        ClientSocketWrapper GetVisaSocketInstance(ClientSocketWrapper clientSock, TcpClient skClient)
        {
            int messagelength = 0;
                    
            messagelength = this.Settings.MessageByteLength;

            clientSock = new TCPClientSocketWrapper(skClient, new ClientProcessorWrapper.DEOFLengthCheck(Processor.IsEOF), new ClientProcessorWrapper.DProcessHeader(Processor.ProcessHeader),
                new ClientProcessorWrapper.DProcessMsg(Processor.ProcessMsg), _settings);
            
            clientSock.IsMessageLengthEnabled = true;
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

            OnSocketLog(sender,e);

            this.LogData(e);

        }

        void LogData(string data)
        {
            if (_settings.LogData == "1")
            {
                SimpleLogWriter write = new SimpleLogWriter(ConfigurationManager.AppSettings["Detaillogpath"].ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log");
                write.LogEntry(new TCPLogEntry("Log :", 0, data));
            }
        }

        void LogData(TCPEventArgs e)
        {
            if (_settings.LogData == "1")
            {
                SimpleLogWriter write = new SimpleLogWriter(ConfigurationManager.AppSettings["Detaillogpath"].ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHH") + ".log");
                write.LogEntry(new TCPLogEntry("Received  :" + e.ServerSettings.ListenIP, 0, e.ToString(), _logEncryptionKey));
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

        public void SendData()
        {


        }


    }
}

