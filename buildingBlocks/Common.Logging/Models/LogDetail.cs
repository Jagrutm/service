using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Common.Logging.Modles
{
    public class LogDetail
    {
        public LogDetail()
        {
            Timestamp = DateTime.Now;
            AdditionalInfo = new Dictionary<string, object>();
        }
        public DateTime Timestamp { get; internal set; }
        public string Message { get; set; }
        // WHERE
        public string Product { get; set; }
        public string Layer { get; set; }
        public string Location { get; set; }
        public string GroupCode { get; set; }
        public string Hostname { get; set; }
        public string IPAddress { get; set; }
        // WHO
        public string UserId { get; set; }
        public string UserName { get; set; }
        //public int CustomerId { get; set; }
        public int LoginTypeId { get; set; }
        //public string CustomerName { get; set; }
        // EVERYTHING ELSE
        public long? ElapsedMilliseconds { get; set; }  // only for performance entries
        public Exception Exception { get; set; }  // the exception for error logging
        public string CorrelationId { get; set; } // exception shielding from server to client
        public Dictionary<string, object> AdditionalInfo { get; set; }  // everything else
        public override string ToString() => JsonSerializer.Serialize(this);
        public string CurrentRequestErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}
