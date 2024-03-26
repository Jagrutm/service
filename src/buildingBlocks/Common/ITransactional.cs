namespace CredECard.Common.BusinessService
{
	using System;

	public interface ITransactional
	{
		void ProcessSaveTransaction();
		void ProcessUpdateTransaction();
		
		
	}
}
