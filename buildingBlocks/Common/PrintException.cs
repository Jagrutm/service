using System;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public class PrintException : ApplicationException
	{
		public PrintException() : base()
		{
		}
		public PrintException(string message) : base(message)
		{
		}
		public PrintException(string message, Exception inner) : base(message, inner)
		{
		}
		protected PrintException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}
