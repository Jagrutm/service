using System;

namespace CredECard.Common.BusinessService
{
	/// <summary>
	/// Summary description for ValidateException.
	/// </summary>
    [Serializable()]
	public class ValidateException : Exception
	{
		private int _errorNumber = 0;
		
		public ValidateException(int ErrorNumber, string ErrorMessage) : base(ErrorMessage)
		{
			_errorNumber = ErrorNumber;
		}
		
		/// <summary>
		/// Gets or sets The error number that occured.  0 if no error occured.
		/// </summary>	
        /// <value>int</value>
		public int ErrorNumber
		{
			get
			{
				return _errorNumber;
			}
			set
			{
				_errorNumber=value;
			}
		}
	}
}
