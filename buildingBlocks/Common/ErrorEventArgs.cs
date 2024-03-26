using System;

namespace CredECard.Common.BusinessService
{
	/// <summary>
	/// Summary description for ErrorEventArgs.
	/// </summary>
	public class ExceptionEventArgs : EventArgs
	{
		public ExceptionEventArgs(Exception ex)
		{
			this.ex = ex;
		}

		public readonly Exception ex;
	}
}
