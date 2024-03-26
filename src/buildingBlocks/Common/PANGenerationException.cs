using System;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public class PANGenerationException : ApplicationException
	{
		public PANGenerationException() : base()
		{
		}
		public PANGenerationException(string message) : base(message)
		{
		}
		public PANGenerationException(string message, Exception inner) : base(message, inner)
		{
		}
        protected PANGenerationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
		{
		}
	}
}
