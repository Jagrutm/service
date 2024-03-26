using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLogging.LogEntries;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    public class ElementLogEntry : ILogEntry
    {
        private const string DEFAULT_DELIMITER = ",";

        private DateTime _curDate = System.DateTime.Now;
        private int _errorCode = 0;
        private string _refData = String.Empty;
        private string _errorDesc = String.Empty;
        private string _delimiter = DEFAULT_DELIMITER;
        private string _msgType = string.Empty;

        public ElementLogEntry(string messageType, string elementData)
		{
            this.ReferenceData = elementData;
            this.MessageType = messageType;
		}

        public int ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public string MessageType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        public string ErrorDescription
        {
            get { return _errorDesc; }
            set { _errorDesc = value; }
        }

        public string ReferenceData
        {
            get { return _refData; }
            set { _refData = value; }
        }

        public string DateStamp
        {
            get { return _curDate.ToString("yyyyMMdd"); }
        }

        public string TimeStamp
        {
            get { return _curDate.ToString("hh:mm:ss"); }
        }

        protected string Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value; }
        }

        public override string ToString()
        {
            return this.DateStamp + this.Delimiter
                + this.TimeStamp
                + this.Delimiter
                + this.MessageType
                + this.Delimiter
                + this.ReferenceData;
        }
    }
}
