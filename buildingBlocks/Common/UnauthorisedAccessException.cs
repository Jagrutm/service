using System;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public class UnauthorisedAccessException: ApplicationException
    {
        public UnauthorisedAccessException(): base()
        {

        }
        
        public UnauthorisedAccessException(string message) : base(message)
		{

		}
		
        public UnauthorisedAccessException(string message, Exception inner) : base(message, inner)
		{

		}

        protected UnauthorisedAccessException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
		{

		}
    }
}
