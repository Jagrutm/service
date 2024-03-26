using System;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public class SendException : ApplicationException
	{
		public SendException() : base()
		{
		}
		public SendException(string message) : base(message)
		{
		}
		public SendException(string message, Exception inner) : base(message, inner)
		{
		}
		protected SendException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}
