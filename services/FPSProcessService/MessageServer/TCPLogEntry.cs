using System;
using CredEcard.CredEncryption.BusinessService;
using DataLogging.LogEntries;

namespace ContisGroup.MessageServer
{
    [Serializable()]
	public sealed class TCPLogEntry : BaseLogEntry
	{
        public TCPLogEntry(string refData, int errorCode, string errorDesc, Encryptionkey logEncryptionKey = null)
		{
			this.ReferenceData = refData;
			this.ErrorCode = errorCode;
			this.ErrorDescription = errorDesc;
            this.LogEncryptionKey = logEncryptionKey;
		}
	}
}
