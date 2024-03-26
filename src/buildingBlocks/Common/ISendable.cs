namespace CredECard.Common.BusinessService
{
    using CredECard.Common.BusinessService;
    using System;

	public interface ISendable
	{
        void Email(DataController conn);
		string Post(DataController conn);
	}
}
