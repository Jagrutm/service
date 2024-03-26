using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

using  CredECard.Common.BusinessService;
using DataLogging.LogWriters;
using System.Threading.Tasks;

namespace ContisGroup.MessageServer
{
    [Serializable()]
    public abstract class SocketWrapper
    {
        protected const int BUFFER_SIZE = 128;

        //property for getting message length
        public int MessageLengthBuffer = 0;
        public bool IsMessageLengthEnabled = false;
        internal byte[] lengthbuffer;

        //public byte[] ReceivedMessage = null;
        //private int _tempReceivedMsgLength = 0;


        protected TCPSettings _settings = null;
        protected Socket _baseSocket = null;
        protected ReceiveState _state = null;
        protected string _curClientIP = string.Empty;

        public ProcessorWrapper.DEOFCheck DEof = null;
        public ProcessorWrapper.DEOFLengthCheck DEofLength = null;
        public ProcessorWrapper.DProcessMsg ProcessFn = null;
        
        public ProcessorWrapper.DEOFCheckS DEofS = null;
        public ProcessorWrapper.DEOFLengthCheckS DEofLengthS = null;
        public ProcessorWrapper.DProcessMsgS ProcessFnS = null;

        public TimeSpan Timeout = TimeSpan.MaxValue;
        protected DateTime _lastAction = DateTime.MaxValue;


        // Event for notifying caller of client connection
        public event TCPDelegates.SrvClientEvent SocketClose;

        public byte[] LengthBuffer
        {
            get
            {
                return lengthbuffer;
            }
            set
            {
                lengthbuffer = value;
            }
        }

        private void OnSocketClose(object sender, TCPEventArgs e)
        {
            if (SocketClose != null) SocketClose(sender, e);
        }

        // Event for logging data
        public event TCPDelegates.SrvClientEvent LogError;
        private void OnSocketLog(object sender, TCPEventArgs e)
        {
            if (LogError != null) LogError(sender, e);
        }

        /// <summary>
        /// Whether or not the client is connected.  Any connections passed
        /// should be connected, so assume true by default.
        /// </summary>
        private bool _isConn = true;

        /// <summary>
        /// The mode to communicate on.  Use Binary for receiving objects and files.
        /// Use Text for standard communication.  Defaulted to Text.
        /// </summary>
        public ESocketMode SocketMode = ESocketMode.Text;

        /// <summary>
        /// The encoding which the text is encoded with.  Defaulted to ASCIIEncoding.
        /// </summary>
        public System.Text.Encoding TextEncoding = new System.Text.ASCIIEncoding();

        /// <summary>
        /// To track the total bytes read from this connection 
        /// </summary>
        protected int _totalBRead = 0;

        /// <summary>
        /// Flag to specify if Close() function has been called.  Stops more Async requests starting
        /// which close is happening, otherwise you will get ObjectDisposedException
        /// </summary>
        protected bool _bClosing = false;

        protected Object thisLock = new Object();

        protected AsyncCallback _cb = null;

        public Socket ConnectedSocket
        {
            get
            {
                return _baseSocket;
            }
        }
        /// <summary>
        /// Sends text data to the client, asyncronously
        /// </summary>
        /// <param name="data">Text data to send to the client</param>
        public void Send(String data)
        {
            // If this is closing or closed return without doing anything
            if (_bClosing) return;

            // Convert the string data to byte data using specified encoding
            byte[] byteData = _settings.TextEncoding.GetBytes(data);

            // Begin sending the data to the remote device
            this.Send(byteData);
        }

        /// <summary>
        /// Binary data to the client, asyncronously
        /// </summary>
        /// <param name="data">Binary data to send to the client</param>
        public void Send(byte[] data)
        {
            // Update the last action
            Actioned();

            // If we are closing of not connected just return
            if (_bClosing || !_isConn) return;

            try
            {
                // Send the data
                _baseSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), null);
            }
            catch (SocketException)
            {
                //RaiseLogError(ex.ToString() + "Receive socket exception");
                this.Close();
            }
            catch (Exception ex)
            {
                RaiseLogError(ex.ToString() + "Receive general exception");
                this.Close();
                FogBugzLogWriter.LogErrors(ex);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            // Update the last action
            Actioned();

            // If we are closing of not connected just return
            if (_bClosing || !_isConn) return;

            // Complete sending the data to the remote device
            int bytesSent = _baseSocket.EndSend(ar);
        }

        /// <summary>
        /// Starts receiving a message is one is available, asyncronously.  Once called this function
        /// will continue listening for messages until Close() is called.  Any long process called
        /// after this function MUST be Async, otherwise this thread will be blocked and the receive
        /// callback will be blocked, making the client seem like is is crashed.
        /// </summary>
        public void Receive()
        {
            // Update the last action
            Actioned();

            // If closing, then we don't want to do anything so return
            if (_bClosing) RaiseLogError("*Socket Closing");
            if (_bClosing) return;

            // If we've not already got one, create a length buffer of required size
            if (lengthbuffer == null) lengthbuffer = new byte[MessageLengthBuffer];

            this.SocketReceive();
        }

        protected virtual void SocketReceive()
        {
            try
            {
                if (IsMessageLengthEnabled && _state.MessageLength == 0)
                {
                    //RaiseLogError("Waiting for length");
                    lock (thisLock)
                    {
                        _cb = new AsyncCallback(ReadLengthCallBack);
                    }
                    _baseSocket.BeginReceive(lengthbuffer, 0, MessageLengthBuffer, SocketFlags.None, _cb, _baseSocket);
                }
                else if (IsMessageLengthEnabled)
                {
                    //RaiseLogError("Waiting for data");
                    lock (thisLock)
                    {
                        _cb = new AsyncCallback(ReadCallback);
                    }
                    int tSize = (_totalBRead + BUFFER_SIZE) > _state.MessageLength ? (_state.MessageLength % BUFFER_SIZE) : BUFFER_SIZE;
                    if (!_bClosing) _baseSocket.BeginReceive(_state.buffer, 0, tSize, SocketFlags.None, _cb, null);
                }
                else
                {
                    //RaiseLogError("Waiting for data");
                    if (!_bClosing) _baseSocket.BeginReceive(_state.buffer, 0, _state.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), null);
                }
            }
            catch (SocketException ex)
            {
                RaiseLogError(ex.ToString() + "Receive socket exception");
                this.Close();
            }
            catch (Exception ex)
            {
                RaiseLogError(ex.ToString() + "Receive general exception");
                this.Close();
                FogBugzLogWriter.LogErrors(ex);
            }
        }

        protected void ReadLengthCallBack(IAsyncResult ar)
        {
            Actioned();

            //RaiseLogError("Message Received - Checking Length");

            // If closing, then we don't want to do anything so return
            if (_bClosing) RaiseLogError("*Socket Closing");
            if (_bClosing) return;

            int bytesRead = 0;
            try
            {
                bytesRead = _baseSocket.EndReceive(ar);
                //RaiseLogError("Checking Length - Bytes Read : " + bytesRead.ToString());

                if (bytesRead > 0)
                {
                    RaiseLogError("Byte : " + HexEncoding.ToString(lengthbuffer));
                }

                // Only if we have read some data do we want to continue, otherwise something is wrong
                if (bytesRead > 0)
                {
                    //_state.MessageLength = GetMessageLength(lengthbuffer);
                    _state.MessageLength = GetVisaMessageLength(lengthbuffer);

                    //CreateLogMessage(lengthbuffer);
                    
                    this.Receive();
                }
                else
                {
                    //RaiseLogError("Checking Length - Message length read failed, closing");
                    this.Close();
                }
            }
            catch (SocketException ex)
            {
                RaiseLogError(ex.ToString() + "Checking Length - exception");
                //FogBugzLogWriter.LogErrors(ex);
                //this.Close();
            }
            catch (Exception ex)
            {
                RaiseLogError(ex.ToString() + "Checking Length - exception");
                FogBugzLogWriter.LogErrors(ex);
                //this.Close();
            }
        }

        protected void ReadCallback(IAsyncResult ar)
        {
            // Update the last action
            Actioned();

            //RaiseLogError("Message Received - Reading Data");

            // If closing, then we don't want to do anything so return
            if (_bClosing) RaiseLogError("*Socket Closing");
            if (_bClosing) return;

            bool eof = false;
            int bytesRead = 0;
            // Read and save the data from the socket
            try
            {
                bytesRead = _baseSocket.EndReceive(ar);
                RaiseLogError("Reading Data - Bytes Read : " + bytesRead.ToString());
                
                lock (thisLock)
                {
                    _totalBRead += bytesRead;
                }
            }
            catch (SocketException ex)
            {
                RaiseLogError(ex.ToString() + "Reading Data - exception");
                FogBugzLogWriter.LogErrors(ex);
                if (_bClosing) return;
            }
            catch (Exception ex)
            {
                FogBugzLogWriter.LogErrors(ex);
                if (_bClosing) return;
            }


            // Call text of binary version depending on mode
            if (_settings.SocketMode == ESocketMode.Binary)
            {
                //RaiseLogError("Reading Data - Binary Type Selected");

                // If read something write it and increment read bytes
                if (bytesRead > 0) _state.WriteBuffer(bytesRead);

                // Get the message data as loaded
                byte[] message = _state.ReadData();
                bool isEOF = false;

                //logic for message is completed or not.
                if (IsMessageLengthEnabled)
                {
                    //RaiseLogError("Reading Data - Checking Length EOF");
                    isEOF = DEofLength(message, _state.MessageLength);
                }
                else
                {
                    //RaiseLogError("Reading Data - Checking Text EOF Logic");
                    isEOF = DEof(message);
                }

                // See if message is complete
                if (isEOF)
                {
                    //RaiseLogError("Reading Data - EOF found, processing msg");
                    try
                    {
                        RaiseLogError("Reading Data -: " + HexEncoding.ToString(message));
                        //CreateLogMessage(message);

                        //ProcessFn(message, this);
                        var Taskfn = processFnRecord(message, this);
                    }
                    catch (Exception ex)
                    {
                        RaiseLogError(ex);
                        FogBugzLogWriter.LogErrors(ex);
                    }


                    eof = true;
                }
            }
            else if (_settings.SocketMode == ESocketMode.Text)
            {
                // If read something write it and increment read bytes
                if (bytesRead > 0) _state.WriteBufferS(bytesRead, this.TextEncoding);

                // Get the message data as loaded
                string message = _state.ReadString();

                bool isSEOF = false;
                if (IsMessageLengthEnabled)
                {
                    isSEOF = DEofLengthS(message, _state.MessageLength);
                }
                else
                {
                    isSEOF = DEofS(message);
                }

                // See if message is complete
                if (isSEOF)
                {
                    try
                    {
                        ProcessFnS(message, this);
                    }
                    catch(Exception ex)
                    {
                        FogBugzLogWriter.LogErrors(ex);
                    }
                    eof = true;
                }
            }

            // If the message was complete we can clear out the state, 
            // otherwise we need to keep receiving.  If bytesread = 0
            // then we are closing also, as the only instance when bytes
            // are 0 is when the client has closed.
            //RaiseLogError("Reading Data - Keep reading");
            if (eof)
            {
                //RaiseLogError("Reading Data - EOF, wait for the next message");
                _cleanUpForNext();
                this.Receive();
            }
            else if (bytesRead == 0)
            {
                //RaiseLogError("Reading Data - No more data to read so closing this socket");
                this.Close();
            }
            else
            {
                //RaiseLogError("Reading Data - Not EOF yet keep reading");
                this.Receive();
            }
        }

        /// <summary>
        ///   Async task process call
        /// </summary>
        /// <param name="message"></param>
        /// <param name="objSocket"></param>
        /// <returns></returns>
        private async Task processFnRecord(byte[] message, SocketWrapper objSocket)
        {
            await Task.Delay(1);
            ProcessFn(message, objSocket);
        }


        private void _cleanUpForNext()
        {
            lock (thisLock)
            {
                this.SubCleanUp();
                _cb = null;
                lengthbuffer = new byte[MessageLengthBuffer];
                _totalBRead = 0;
                _state.Close();
                _state = new ReceiveState(BUFFER_SIZE);
            }
        }

        protected virtual void SubCleanUp()
        {
        }

        public void RaiseLogError(string log)
        {
            TCPEventArgs tcpE = new TCPEventArgs(log, _settings, this, DateTime.Now);
            OnSocketLog(this, tcpE);
        }

        public void RaiseLogError(Exception ex)
        {
            TCPEventArgs tcpE = new TCPEventArgs(ex, _settings, this, DateTime.Now);
            OnSocketLog(this, tcpE);
        }

        /// <summary>
        /// Closes down and disposes any resources required to be disposed
        /// </summary>
        public void Close()
        {
            // Update the last action
            Actioned();

            lock (thisLock)
            {
                // If already closing, return
                if (_bClosing) return;

                // Set flag
                _bClosing = true;

                // Close the state object
                _state.Close();

                // Close down the socket
                _baseSocket.Shutdown(SocketShutdown.Both);
                _baseSocket.Close();

                // Let everyone know that this socket is shutting down
                TCPEventArgs tcpE = new TCPEventArgs("Socket shutdown", _settings, this, DateTime.Now);
                this.OnSocketClose(this, tcpE);
            }
        }

        public bool Closing
        {
            get
            {
                return _bClosing;
            }
        }

        public string GetClientIP()
        {
            if (_curClientIP == string.Empty && _baseSocket != null && !this.Closing) _curClientIP = _baseSocket.RemoteEndPoint.ToString();
            return _curClientIP;
        }

        protected void Actioned()
        {
            lock (thisLock)
            {
                _lastAction = DateTime.Now;
            }
        }

        public bool IsTimedOut
        {
            get
            {
                return ((_lastAction + this.Timeout) < DateTime.Now) ? true : false;
            }
        }

        public TCPSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        public int GetVisaMessageLength(byte[] length)
        {
            int msgLength = HexEncoding.HexToDecimal(HexEncoding.ToString(length));

            if (msgLength != 0 && msgLength < 1600)
                return msgLength;

            byte[] visaLength = new byte[2];
            if (length.Length > 2)
            {
                for (int i = 0; i < visaLength.Length; i++)
                {
                    visaLength[i] = length[i];
                }
            }

            string hexLength = HexEncoding.ToString(visaLength);
            return HexEncoding.HexToDecimal(hexLength);
        }
        
        protected int GetMessageLength(byte[] length)
        {
            string hexLength = HexEncoding.ToString(length);
            return HexEncoding.HexToDecimal(hexLength);
        }

    }
}

