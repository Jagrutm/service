using System;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public class ContentException : ApplicationException
	{
		public ContentException() : base()
		{
		}
		public ContentException(string message) : base(message)
		{
		}
		public ContentException(string message, Exception inner) : base(message, inner)
		{
		}
		protected ContentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}
