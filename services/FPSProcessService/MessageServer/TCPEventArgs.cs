using System;
using System.Collections.Generic;
using System.Text;

namespace ContisGroup.MessageServer
{
    [Serializable()]
	public class TCPEventArgs : System.EventArgs
	{
		public string EventData = string.Empty;
		public TCPSettings ServerSettings = null;
		public string ClientIP = string.Empty;
		public DateTime EventTime = DateTime.MinValue;
        public Exception ex = null;
        public ClientSocketWrapper clientWrapper = null;
        public SocketWrapper socketWrapper = null;

        public TCPEventArgs(string eventData, TCPSettings serverSet, SocketWrapper socket, DateTime eventTime)
		{
			EventData = eventData;
			ServerSettings = serverSet;
			ClientIP = socket.GetClientIP();
			EventTime = eventTime;
            socketWrapper = socket;
        }

        public TCPEventArgs(string eventData, TCPSettings serverSet, ClientSocketWrapper socket, DateTime eventTime)
        {
            EventData = eventData;
            ServerSettings = serverSet;
            if (socket != null) ClientIP = socket.GetClientIP();
            EventTime = eventTime;
            clientWrapper = socket;
        }

        public TCPEventArgs(Exception e, TCPSettings serverSet, SocketWrapper socket, DateTime eventTime)
        {
            ex = e;
            ServerSettings = serverSet;
            ClientIP = socket.GetClientIP();
            EventTime = eventTime;
        }

        public TCPEventArgs(Exception e, TCPSettings serverSet, ClientSocketWrapper socket, DateTime eventTime)
        {
            ex = e;
            ServerSettings = serverSet;
            ClientIP = socket.GetClientIP();
            EventTime = eventTime;
            clientWrapper = socket;
        }

		public override string ToString()
		{
			StringBuilder data = new StringBuilder();
            if (ex == null)
            {
                data.AppendFormat("Event {0}, Client {1}, Time {2}", EventData, ClientIP, EventTime.ToString());
            }
            else
            {
                data.AppendFormat("Client : {0}, Time : {1} \r\n", ClientIP, EventTime.ToString());
                data.AppendFormat("Exception : {0} \r\n", ex.ToString(), ClientIP, EventTime.ToString());
            }

			return data.ToString();
		}
	}
}
