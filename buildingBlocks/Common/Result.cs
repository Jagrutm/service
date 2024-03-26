using System;
using System.Resources;
using System.Text;


using System.Reflection;
using CredECard.Common.BusinessService;

namespace CredECard.Common.BusinessService
{
	/// <summary>
	/// Summary description for Result.
	/// </summary>
	public class Result
	{
		#region Constructor
		public Result()
		{		}
		#endregion

		#region Private Members
		private string _errorMessage= string.Empty;
		private int _errorNumber = 0;
		private bool _success = false;
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets or sets the error number that occured.  0 if no error occured.
		/// </summary>	
        /// <value>string</value>
		public string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
			set
			{
				_errorMessage=value;
			}
		}

		
		/// <summary>
		/// Gets or sets the error number that occured.  0 if no error occured.
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

		/// <summary>
		/// Gets or sets if the method was successful or not.
		/// </summary>
        /// <value>bool</value>
		public bool Success
		{
			get
			{
				return _success;
			}
			set
			{
				_success=value;
			}
		}

		#endregion

        /// <summary>
        /// Gets the error message for the error number in the result
        /// </summary>
        /// <returns></returns>
		public string GetMessage()
		{
			return CommonMessage.GetMessage(this._errorNumber);
		}
	}
}
