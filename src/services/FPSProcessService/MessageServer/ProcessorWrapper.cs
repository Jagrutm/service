using System;
using System.Collections.Generic;
using System.Text;

namespace ContisGroup.MessageServer
{
    [Serializable()]
	public abstract class ProcessorWrapper
	{
		#region delegates (binary mode versions)
        public delegate bool DEOFCheck(byte[] message);
        public delegate bool DEOFLengthCheck(byte[] message, int messageLength);
		public delegate void DProcessMsg(byte[] message, SocketWrapper socket);
        public delegate void DProcessHeader(byte[] header, SocketWrapper socket);
		#endregion

		#region delegates (text mode versions)
        public delegate bool DEOFCheckS(string message);
        public delegate bool DEOFLengthCheckS(string message, int messageLength);
		public delegate void DProcessMsgS(string message, SocketWrapper socket);
		#endregion

		/// <summary>
		/// Checks if a message is complete or not.
		/// </summary>
		/// <param name="message">The message which has been received so far</param>
		/// <returns>true if complete false if not. false by default.</returns>
		public virtual bool IsEOF(byte[] message)
		{
			return false;
		}

        public virtual bool IsEOF(byte[] message,int messageLength)
        {
            return false;
        }
		/// <summary>
		/// Override this for handling EOF on an ASCII basis.  
		/// NOTE: Very slow if message handling setting is in Binary, as it needs 
		/// to encode the whole message into ASCII each time.
		/// </summary>
		/// <param name="message">The message which has been received so far</param>
		/// <returns>true if complete false if not. false by default.</returns>
		public virtual bool IsEOF(string message)
		{
			return false;
		}

        public virtual bool IsEOF(string message, int messageLength)
        {
            return false;
        }

		/// <summary>
		/// When a message is received and agree to be complete by IsEOF, this function
		/// is called with the full message body.  In this case in binary format.
		/// Override this to provide handling to response and processing to different
		/// types of message.
		/// </summary>
		/// <param name="message">The final complete message, as agreed by IsEOF</param>
		/// <param name="socket">The connected socket to communicate on</param>
		public virtual void ProcessMsg(byte[] message, SocketWrapper socket)
		{
		}

		/// <summary>
		/// When a message is received and agree to be complete by IsEOF, this function
		/// is called with the full message body.  In this case in string format.
		/// Override this to provide handling to response and processing to different
		/// types of message.
		/// NOTE: Very slow if message handling setting is in Binary, as it needs 
		/// to encode the whole message into ASCII each time.
		/// </summary>
		/// <param name="message">The final complete message, as agreed by IsEOF</param>
		/// <param name="socket">The connected socket to communicate on</param>
		public virtual void ProcessMsg(string message, SocketWrapper socket)
		{
		}


        /// <summary>
        /// When a header is received from message, we need to parse it and fetch the 
        /// related data from header and need to process the message.
        /// NOTE: Very slow if message handling setting is in Binary, as it needs 
        /// to encode the whole message into ASCII each time.
        /// </summary>
        /// <param name="message">The final complete message, as agreed by IsEOF</param>
        /// <param name="socket">The connected socket to communicate on</param>
        public virtual void ProcessHeader(byte[] header, SocketWrapper socket)
        {
        }

		/// <summary>
		/// Starts the message conversation.  All flow logic should be handled with this
		/// function. By default it sends HELLO message and waits for a response.
		/// </summary>
		/// <param name="socket">The connected socket to communicate on</param>
		public virtual void StartConversation(SocketWrapper socket)
		{
			_sendHello(socket);
			socket.Receive();
		}

		private void _sendHello(SocketWrapper socket)
		{
			socket.Send("HELLO\r\n");
		}

	}
}
