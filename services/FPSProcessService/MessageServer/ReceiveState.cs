using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace ContisGroup.MessageServer
{
    [Serializable()]
	public class ReceiveState
	{
		#region private variables
		// The default buffer size
		private const int DEFAULT_BUFFER = 64;
        
		#endregion

		#region public variables
		// Client socket.
		internal Socket workSocket = null;

		// The buffer size
		internal int BufferSize = DEFAULT_BUFFER;
        

		// Receive buffer.
		internal byte[] buffer;
        
        internal int MessageLength = 0;
		// Stream for received data if in binary mode
		internal MemoryStream msData = null;

		// String to store the data if in text mode
		internal StringBuilder TextData = null;

		#endregion

		#region constructors
		internal ReceiveState(): this(DEFAULT_BUFFER)
		{}

		internal ReceiveState(int bufferSize)
		{
			buffer = new byte[bufferSize];
		}
		#endregion

		internal void WriteBuffer(int numBytes)
		{
			// If nothing passed, do nothing
			if (numBytes <= 0) return;

			// Create stream if needed and append bytes to it
			if (msData == null) msData = new MemoryStream();
			msData.Write(buffer, 0, numBytes);
		}

		internal void WriteBufferS(int numBytes, System.Text.Encoding encoding)
		{
			// If nothing passed, do nothing
			if (numBytes <= 0) return;

			// Create stringbuilder if needed and append bytes to it
			if (TextData == null) TextData = new StringBuilder();
			TextData.Append(encoding.GetString(buffer, 0, numBytes));
		}

		internal byte[] ReadData()
		{
			// Check if null and return blank if it is
			if (msData == null) return new byte[0];

			// Ensure the data is written to the stream before returning
			msData.Flush();
			return msData.ToArray();
		}

		internal string ReadString()
		{
			// Check the mode and return the data from the correct store accordingly
			if (TextData == null) return string.Empty;
			return TextData.ToString();
		}

		internal void Close()
		{
			// Clear out both the stores (just in case)
			if (msData != null)
			{
				msData.Close();
				msData = null;
			}
			if (TextData != null) TextData = null;
            MessageLength = 0;
		}

		#region destructor
		~ReceiveState()
		{
			// Close down all the data by default
			this.Close();
		}
		#endregion
	}
}
