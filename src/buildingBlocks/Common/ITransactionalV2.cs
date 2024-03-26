namespace CredECard.Common.BusinessService
{
	using System;

	public interface ITransactionalV2
	{
		void ProcessSaveTransaction(DataController conn);
		void ProcessUpdateTransaction(DataController conn);
	}
}
