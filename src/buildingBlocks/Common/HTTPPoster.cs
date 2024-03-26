using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace CredECard.Common.BusinessService
{

	public delegate void OnSendingEventHandler(object sender, SendingEventArgs args);
	public delegate void OnRequestComplete(object sender,RequestCompleteEventArgs e);
    public delegate void OnRequestFailed(object sender, RequestFailedEventArgs e);

	public class SendingEventArgs:EventArgs
	{
		public int ProgressSoFar;
		public int TotalProgress;
		public bool IsComplete;

        /// <summary>
        /// Constructor initialises iscomplete, progressSofar and total progress
        /// </summary>
        /// <param name="isComplete"></param>
        /// <param name="progressSoFar"></param>
        /// <param name="totalProgress"></param>
		public SendingEventArgs(bool isComplete,int progressSoFar,int totalProgress)
		{
			ProgressSoFar=progressSoFar;
			TotalProgress=totalProgress;
			IsComplete=isComplete;
		}
	}

	public class RequestCompleteEventArgs:EventArgs
	{
		public string Result;
        private Int64 _referenceNo = 0;

        /// <summary>
        /// Initializes the result field in the new instance of this class 
        /// </summary>
        /// <param name="_result">RequestCompleteEventArgsv</param>
		public RequestCompleteEventArgs(string _result)
		{
			Result=_result;
		}
        /// <summary>
        /// Initializes the result and reference nofield in the new instance of this class 
        /// </summary>
        /// <param name="_result"></param>
        /// <param name="referenceNo"></param>
        public RequestCompleteEventArgs(string _result,Int64 referenceNo)
        {
            Result = _result;
            _referenceNo = referenceNo;
        }

        /// <summary>
        /// Gets the reference no
        /// </summary>
        public Int64 ReferenceNo
        {
            get
            {
                return _referenceNo;
            }
        }
	}

    public class RequestFailedEventArgs : EventArgs
    {
        private string _result = string.Empty;
        private Int64 _referenceID = 0;

        public RequestFailedEventArgs(string result)
        {
            _result = result;
        }
        public RequestFailedEventArgs(string result, Int64 referenceID)
        {
            _result = result;
            _referenceID = referenceID;
        }

        /// <summary>
        /// Gets the result
        /// </summary>
        public string Result
        {
            get
            {
                return _result;
            }
        }
        
        /// <summary>
        /// Gets the reference ID
        /// </summary>
        public Int64 ReferenceID
        {
            get
            {
                return _referenceID;
            }
        }
    }

    public class RequestState
	{
		// This class stores the State of the request.
		const int BUFFER_SIZE = 1024;
		public StringBuilder requestData;
		public byte[] BufferRead;
		public HttpWebRequest request;
		public HttpWebResponse response;
		public Stream streamResponse;
		public RequestState()
		{
			BufferRead = new byte[BUFFER_SIZE];
			requestData = new StringBuilder("");
			request = null;
			streamResponse = null;
		}
	}

	public  enum    CertificateProblem  : long
	{
		CertEXPIRED                   = 0x800B0101,
		CertVALIDITYPERIODNESTING     = 0x800B0102,
		CertROLE                      = 0x800B0103,
		CertPATHLENCONST              = 0x800B0104,
		CertCRITICAL                  = 0x800B0105,
		CertPURPOSE                   = 0x800B0106,
		CertISSUERCHAINING            = 0x800B0107,
		CertMALFORMED                 = 0x800B0108,
		CertUNTRUSTEDROOT             = 0x800B0109,
		CertCHAINING                  = 0x800B010A,
		CertREVOKED                   = 0x800B010C,
		CertUNTRUSTEDTESTROOT         = 0x800B010D,
		CertREVOCATION_FAILURE        = 0x800B010E,
		CertCN_NO_MATCH               = 0x800B010F,
		CertWRONG_USAGE               = 0x800B0110,
		CertUNTRUSTEDCA               = 0x800B0112
	}

	public  enum    EmailTemplateType
	{
		CustomerPurchaseRequestEmail  = 5,
		MerchantPurchaseNotificationEmail = 7,
		MerchantCallbackFailNotificationEmail = 8
		
	}
}
