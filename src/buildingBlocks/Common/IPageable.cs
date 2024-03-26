using CredECard.Common.BusinessService;
using System;

namespace CredECard.Common.BusinessService
{
    public interface IPageable
    {
        int MaxRowPerPage {get;set;}
        int CurrentPageIndex{get;set;}
        int TotalRows{get;set;}
        PageableDataCollection GetPagingData(DataItem item, int startrowIndex, int maxRowPerPage);
    }
}
