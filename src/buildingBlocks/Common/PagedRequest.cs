namespace BaseDatabase
{
    public class PagedRequest
    {
        #region Variable Declarations
        
        const int maxPageSize = 20;

        private int _pageSize = 10;

        #endregion

        public int PageNumber { get; set; } = 1;
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string SearchQuery { get; set; }
        public string OrderBy { get; set; }
        public string Fields { get; set; }
    }
}
