using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
namespace ContisGroup.MessageServer
{
    public class MCSocketWrapper : SocketWrapper
    {

         /// <summary>
        /// Initialises the socket wrapper object and binds the delegates
        /// </summary>
        /// <param name="baseSocket">A CONNECTED socket returned from the AcceptSocket or EndAcceptSocket functions</param>
        /// <param name="eofFn">Function to check for EOF</param>
        /// <param name="proFn">Function to process a complete message</param>
        public MCSocketWrapper(Socket baseSocket, ProcessorWrapper.DEOFCheck eofFn, ProcessorWrapper.DProcessMsg proFn, TCPSettings settings)
        {

            // Update the last action
            Actioned();

            // First load in the passed settings
            _settings = settings;

            // We need a socket passed through from the listener
            _baseSocket = baseSocket;
            DEof = eofFn;
            ProcessFn = proFn;

            // Set the client IP into local var
            this.GetClientIP();

            // We need a state control
            _state = new ReceiveState(BUFFER_SIZE);
        }


        /// <summary>
        /// Initialises the socket wrapper object and binds the delegates
        /// </summary>
        /// <param name="baseSocket">A CONNECTED socket returned from the AcceptSocket or EndAcceptSocket functions</param>
        /// <param name="eofFn">Function to check for EOF</param>
        /// <param name="proFn">Function to process a complete message</param>
        public MCSocketWrapper(Socket baseSocket, ProcessorWrapper.DEOFLengthCheck eofFn, ProcessorWrapper.DProcessMsg proFn, TCPSettings settings)
        {

            // Update the last action
            Actioned();

            // First load in the passed settings
            _settings = settings;

            // We need a socket passed through from the listener
            _baseSocket = baseSocket;
            DEofLength = eofFn;
            ProcessFn = proFn;

            // Set the client IP into local var
            this.GetClientIP();

            // We need a state control
            _state = new ReceiveState(BUFFER_SIZE);
        }


        /// <summary>
        /// Initialises the socket wrapper object and binds the delegates. Text mode version.
        /// </summary>
        /// <param name="baseSocket">A CONNECTED socket returned from the AcceptSocket or EndAcceptSocket functions</param>
        /// <param name="eofFn">Function to check for EOF</param>
        /// <param name="proFn">Function to process a complete message</param>
        public MCSocketWrapper(Socket baseSocket, ProcessorWrapper.DEOFCheckS eofFn, ProcessorWrapper.DProcessMsgS proFn, TCPSettings settings)
        {

            // Update the last action
            Actioned();

            // First load in the passed settings
            _settings = settings;

            // We need a socket passed through from the listener
            _baseSocket = baseSocket;
            DEofS = eofFn;
            ProcessFnS = proFn;

            // Set the client IP into local var
            this.GetClientIP();

            // We need a state control
            _state = new ReceiveState(BUFFER_SIZE);
        }
        /// <summary>
        /// Initialises the socket wrapper object and binds the delegates. Text mode version.
        /// </summary>
        /// <param name="baseSocket">A CONNECTED socket returned from the AcceptSocket or EndAcceptSocket functions</param>
        /// <param name="eofFn">Function to check for EOF</param>
        /// <param name="proFn">Function to process a complete message</param>
        public MCSocketWrapper(Socket baseSocket, ProcessorWrapper.DEOFLengthCheckS eofFn, ProcessorWrapper.DProcessMsgS proFn, TCPSettings settings)
        {

            // Update the last action
            Actioned();

            // First load in the passed settings
            _settings = settings;

            // We need a socket passed through from the listener
            _baseSocket = baseSocket;
            DEofLengthS = eofFn;
            ProcessFnS = proFn;

            // Set the client IP into local var
            this.GetClientIP();

            // We need a state control
            _state = new ReceiveState(BUFFER_SIZE);
        }


    }
}
