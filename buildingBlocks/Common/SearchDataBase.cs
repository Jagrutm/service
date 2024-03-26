using System;

namespace CredECard.Common.BusinessService
{
    [Serializable]
    public abstract class SearchDataBase
    {
        int _pageIndex = 1;
        public int LastPageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }
    }
}
