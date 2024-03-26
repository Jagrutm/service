using System;

namespace Common.CustomException
{
    [Serializable]
    public class InsufficientFundException : ApplicationException
    {
        public InsufficientFundException()
        {
            
        }        
        public InsufficientFundException(string message):base(message)
        {

        }        
        public InsufficientFundException(string message, Exception innerException): base(message, innerException)
        {

        }
    }
}
