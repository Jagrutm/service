using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using DataLogging.LogWriters;
namespace ContisGroup.MessageServer
{
    public class TCPClientSocketWrapper : ClientSocketWrapper
    {

        //int headerLength = 0;
        //byte[] headerBuffer;
        public ClientProcessorWrapper.DProcessHeader ProcessHeader = null;
        ReceiveState _headerState = null;

        public void SetDataMessageLength(int dataMessageLength)
        {
            _headerState.MessageLength = dataMessageLength;
        }


        public TCPClientSocketWrapper(TcpClient baseSocket, ClientProcessorWrapper.DEOFLengthCheck eofFn, ClientProcessorWrapper.DProcessHeader headerFn, ClientProcessorWrapper.DProcessMsg proFn, TCPSettings settings)
        {
             Actioned();

            // First load in the passed settings
            _settings = settings;

            // We need a socket passed through from the listener
            _baseSocket = baseSocket;
            _clientStream = _baseSocket.GetStream();
            DEofLength = eofFn;
            ProcessFn = proFn;
            ProcessHeader = headerFn;

            // Set the client IP into local var
            //this.GetClientIP();

            // We need a state control
            _state = new ReceiveState(BUFFER_SIZE);
            _headerState = new ReceiveState();
        }
        /// <summary>
        /// Initialises the socket wrapper object and binds the delegates
        /// </summary>
        /// <param name="baseSocket">A CONNECTED socket returned from the AcceptSocket or EndAcceptSocket functions</param>
        /// <param name="eofFn">Function to check for EOF</param>
        /// <param name="proFn">Function to process a complete message</param>
        //public VisaSocketWrapper(Socket baseSocket, ProcessorWrapper.DEOFLengthCheck eofFn, ProcessorWrapper.DProcessHeader headerFn, ProcessorWrapper.DProcessMsg proFn, TCPSettings settings)
        //{

        //    // Update the last action
        //    Actioned();

        //    // First load in the passed settings
        //    _settings = settings;

        //    // We need a socket passed through from the listener
        //    _baseSocket = baseSocket;
        //    DEofLength = eofFn;
        //    ProcessFn = proFn;
        //    ProcessHeader = headerFn;

        //    // Set the client IP into local var
        //    this.GetClientIP();

        //    // We need a state control
        //    _state = new ReceiveState(BUFFER_SIZE);
        //    _headerState = new ReceiveState();
        //}

        //protected override void SocketReceive()
        //{
        //    try
        //    {
        //        if (_headerState.MessageLength == 0)
        //        {                   
        //            lock (thisLock)
        //            {
        //                _cb = new AsyncCallback(ReadHeaderLengthCallBack);
        //            }
        //            _baseSocket.BeginReceive(lengthbuffer, 0, MessageLengthBuffer, SocketFlags.None, _cb, _baseSocket);
        //        }
        //        else
        //        {
        //            if (_state.MessageLength == 0)
        //            {
        //                //RaiseLogError("Waiting for data");
        //                lock (thisLock)
        //                {
        //                    _cb = new AsyncCallback(ReadHeaderCallback);
        //                }
        //                int tSize = (_totalBRead + BUFFER_SIZE) > _headerState.MessageLength ? (_headerState.MessageLength % BUFFER_SIZE) : BUFFER_SIZE;
        //                if (!_bClosing) _baseSocket.BeginReceive(_headerState.buffer, 0, tSize, SocketFlags.None, _cb, null);
        //            }
        //            else
        //            {
        //                //RaiseLogError("Waiting for data");
        //                lock (thisLock)
        //                {
        //                    _cb = new AsyncCallback(ReadCallback);
        //                }
        //                int remainder = _state.MessageLength % BUFFER_SIZE == 0 ? BUFFER_SIZE : _state.MessageLength % BUFFER_SIZE;
        //                int dataSize = (_totalBRead + BUFFER_SIZE) > _state.MessageLength ? remainder : BUFFER_SIZE;
        //                if (!_bClosing) _baseSocket.BeginReceive(_state.buffer, 0, dataSize, SocketFlags.None, _cb, null);
        //            }
        //        }
        //    }
        //    catch (SocketException ex)
        //    {
        //        RaiseLogError(ex.ToString() + "Receive socket exception");
        //        this.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        RaiseLogError(ex.ToString() + "Receive general exception");
        //        this.Close();
        //        FogBugzLogWriter.LogErrors(ex);
        //    }
        //}

        //private void ReadHeaderLengthCallBack(IAsyncResult ar)
        //{
        //    Actioned();

        //    //RaiseLogError("Message Received - Checking Length");

        //    // If closing, then we don't want to do anything so return
        //    if (_bClosing) RaiseLogError("*Socket Closing");
        //    if (_bClosing) return;

        //    int bytesRead = 0;
        //    try
        //    {
        //        bytesRead = _baseSocket.EndReceive(ar);
        //        //RaiseLogError("Checking Length - Bytes Read : " + bytesRead.ToString());

        //        // Only if we have read some data do we want to continue, otherwise something is wrong
        //        if (bytesRead > 0)
        //        {
        //            //_headerState.MessageLength = GetMessageLength(lengthbuffer);
        //            _headerState.MessageLength = GetMessageLength(lengthbuffer);

        //            if (_headerState.MessageLength > 0) _headerState.MessageLength -= 1;
        //            //CreateLogMessage(lengthbuffer);
        //            //RaiseLogError("Checking Length - Message Length Read : " + _state.MessageLength.ToString());
        //            this.Receive();
        //        }
        //        else
        //        {
        //            //RaiseLogError("Checking Length - Message length read failed, closing");
        //            this.Close();
        //        }
        //    }
        //    catch (SocketException ex)
        //    {
        //        RaiseLogError(ex.ToString() + "Checking Length - exception");
        //        //FogBugzLogWriter.LogErrors(ex);
        //        //this.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        RaiseLogError(ex.ToString() + "Checking Length - exception");
        //        FogBugzLogWriter.LogErrors(ex);
        //        //this.Close();
        //    }
        //}

        //private void ReadHeaderCallback(IAsyncResult ar)
        //{
        //    // Update the last action
        //    Actioned();

        //    //RaiseLogError("Message Received - Reading Data");

        //    // If closing, then we don't want to do anything so return
        //    if (_bClosing) RaiseLogError("*Socket Closing");
        //    if (_bClosing) return;

        //    int bytesRead = 0;
        //    // Read and save the data from the socket
        //    try
        //    {
        //        bytesRead = _baseSocket.EndReceive(ar);
        //        //RaiseLogError("Reading Data - Bytes Read : " + bytesRead.ToString());

        //        lock (thisLock)
        //        {
        //            _totalBRead += bytesRead;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FogBugzLogWriter.LogErrors(ex);
        //        if (_bClosing) return;
        //    }

        //    // If read something write it and increment read bytes
        //    if (bytesRead > 0)
        //    {
        //        _headerState.WriteBuffer(bytesRead);

        //        // Get the message data as loaded
        //        byte[] message = _headerState.ReadData();
        //        //RaiseLogError("Reading Data - EOF found, processing msg");
        //        try
        //        {
        //            //CreateLogMessage(message);
        //            ProcessHeader(message, this);
        //            int totalLength = GetMessageLength(this.lengthbuffer);
        //            _state.MessageLength = totalLength - _headerState.MessageLength;
        //            this.Receive();
        //        }
        //        catch (Exception ex)
        //        {
        //            RaiseLogError(ex);
        //            FogBugzLogWriter.LogErrors(ex);
        //        }
        //    }
        //    else
        //        this.Close();
        //}

        protected override void SubCleanUp()
        {
            _headerState.Close();
            _headerState = new ReceiveState(BUFFER_SIZE);
        }
    }
}
