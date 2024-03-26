using System;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public class DeleteException : ApplicationException
	{
		public DeleteException() : base()
		{
		}
		public DeleteException(string message) : base(message)
		{
		}
		public DeleteException(string message, Exception inner) : base(message, inner)
		{
		}
		protected DeleteException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}
