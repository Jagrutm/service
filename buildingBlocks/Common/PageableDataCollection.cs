namespace CredECard.Common.BusinessService
{
    using CredECard.Common.BusinessService;
    using System;
	using System.Collections;

	[Serializable()]
	public class PageableDataCollection : DataCollection, IPageable 
	{
        private int _maxRowPerPage = 0;
        private int _currentRowIndex = 0;
        private int _totalRows = 0;

        #region IPageable Members

        /// <summary>
        /// Gets or sets the Max rows per page
        /// </summary>
        /// <value>int</value>
        public int MaxRowPerPage
        {
            get
            {
                return _maxRowPerPage;
            }
            set
            {
                _maxRowPerPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the current page index
        /// </summary>
        /// <value>int</value>
        public int CurrentPageIndex
        {
            get
            {
                return _currentRowIndex;
            }
            set
            {
                _currentRowIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the Total rows
        /// </summary>
        /// <value>int</value>
        public int TotalRows
        {
            get
            {
                return _totalRows;
            }
            set
            {
                _totalRows = value;
            }
        }

        int IPageable.MaxRowPerPage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IPageable.CurrentPageIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IPageable.TotalRows { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// FEtches data to be paged 
        /// </summary>
        /// <param name="item">Dataitem </param>
        /// <param name="startrowIndex">int</param>
        /// <param name="maxRowPerPage">int</param>
        /// <returns></returns>
        public virtual PageableDataCollection GetPagingData(DataItem item, int startrowIndex, int maxRowPerPage)
        {
            return null;
        }

        /// <summary>
        /// Fetch data to be paged
        /// </summary>
        /// <param name="item">object</param>
        /// <param name="startrowIndex">int</param>
        /// <param name="maxRowPerPage">int</param>
        /// <returns></returns>
        public virtual PageableDataCollection GetPagingData(object item, int startrowIndex, int maxRowPerPage)
        {
            return null;
        }

    
        #endregion
    }
}
