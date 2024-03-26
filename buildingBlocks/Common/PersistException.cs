using System;
using System.Collections.Generic;
using System.Text;

namespace CredECard.Common.BusinessService
{
	[Serializable()]
	public class PersistException : ApplicationException
	{
		public PersistException() : base()
		{
		}
		public PersistException(string message) : base(message)
		{
		}
		public PersistException(string message, Exception inner) : base(message, inner)
		{
		}
		protected PersistException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}
	}
}
