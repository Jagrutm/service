using System;
using System.Collections.Generic;
using System.Text;

namespace ContisGroup.MessageServer
{
    [Serializable()]
    public class TCPSettings
    {
        public ESocketMode SocketMode = ESocketMode.Text;
        public string ListenIP = string.Empty;
        public int ListenPort = 0;
        public int MaxPending = 0;
        public int TimeoutSeconds = 0;
        public Encoding TextEncoding = null;
        public string LogPath = string.Empty;
        public string DataLogPath = string.Empty;
        public string LogData = string.Empty;
        public int MessageByteLength = 4;
        public bool ISMessageLengthEnabled = false;
        public string LogElements = string.Empty;
        public int ListenType = 0;
        public bool IsSenderService = false;


        public override string ToString()
        {
            StringBuilder data = new StringBuilder();
            data.AppendFormat("IP {0} PORT {1} MODE {2}", ListenIP, ListenPort, SocketMode.ToString());
            return data.ToString();
        }
    }
}
