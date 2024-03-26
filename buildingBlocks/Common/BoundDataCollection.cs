namespace CredECard.Common.BusinessService
{
	using System;
	using System.ComponentModel;
	using System.Collections;
    using CredECard.Common.BusinessService;

    /// <summary>
    /// Inherit from this if you need to perform binding to lists which support editing
    /// </summary>
    [Serializable()]
	public abstract class BoundDataCollection : DataCollection, ICloneable, IBindingList
	{
		[NonSerialized]
		private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);

		[NonSerialized]
		private ListChangedEventHandler onListChanged;

		protected abstract BoundDataItem AddNewItem();
		protected abstract BoundDataCollection GetBlank();

        /// <summary>
        /// Implement to clone object
        /// </summary>
        /// <returns>object</returns>
		public virtual object Clone()
		{
			return ((ICloneable)this).Clone();
		}

        /// <summary>
        /// Clones all items in the list
        /// </summary>
        /// <returns>object</returns>
		object ICloneable.Clone()
		{
			BoundDataCollection newCol = this.GetBlank();
			foreach (BoundDataItem cloneMe in this)
			{
				BoundDataItem newClone = (BoundDataItem)cloneMe.Clone();
				List.Add(newClone);
			}
			return newCol;
		}

		/// <summary>
		/// Adds or removes ListChanged event handler
		/// </summary>
        /// <value>ListChangedEventHandler</value>
		public event ListChangedEventHandler ListChanged 
		{
			add 
			{
				onListChanged += value;
			}
			remove 
			{
				onListChanged -= value;
			}
		}

        /// <summary>
        /// Method to hanndle on listchanged event
        /// </summary>
        /// <param name="ev"></param>
		protected virtual void OnListChanged(ListChangedEventArgs ev) 
		{
			if (onListChanged != null) 
			{
				onListChanged(this, ev);
			}
		}

        /// <summary>
        /// Hanldes onclear event
        /// </summary>
		protected override void OnClear() 
		{
			foreach (BoundDataItem c in List) 
			{
				c.Parent = null;
			}
		}
		
        /// <summary>
        /// Hanldes onClearComplete event
        /// </summary>
		protected override void OnClearComplete() 
		{
			OnListChanged(resetEvent);
		}

        /// <summary>
        /// Handles onInserComplete event
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="value">object</param>
		protected override void OnInsertComplete(int index, object value) 
		{
			BoundDataItem c = (BoundDataItem)value;
			c.Parent = this;
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
		}

        /// <summary>
        /// Handles On remove complete event
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="value">object</param>
		protected override void OnRemoveComplete(int index, object value) 
		{
			BoundDataItem c = (BoundDataItem)value;
			c.Parent = this;
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
		}

        /// <summary>
        /// Handles on set complete 
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="oldValue">object</param>
        /// <param name="newValue">object</param>
		protected override void OnSetComplete(int index, object oldValue, object newValue) 
		{
			if (oldValue != newValue) 
			{
				BoundDataItem oldComm = (BoundDataItem)oldValue;
				BoundDataItem newComm = (BoundDataItem)newValue;
                
				oldComm.Parent = null;
				newComm.Parent = this;
                            
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
			}
		}
				
        /// <summary>
        /// Mthod Called by DataItem when it changes.
        /// </summary>
        /// <param name="item">BoundDataItem</param>
		internal void BoundDataItemChanged(BoundDataItem item)
		{
			int index = List.IndexOf(item);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
		}

		// Implements IBindingList.
		bool IBindingList.AllowEdit 
		{ 
			get { return true ; }
		}

		bool IBindingList.AllowNew 
		{ 
			get { return true ; }
		}

		bool IBindingList.AllowRemove 
		{ 
			get { return true ; }
		}

		bool IBindingList.SupportsChangeNotification 
		{ 
			get { return true ; }
		}
        
		bool IBindingList.SupportsSearching 
		{ 
			get { return false ; }
		}

		bool IBindingList.SupportsSorting 
		{ 
			get { return false ; }
		}


		// Methods.
		object IBindingList.AddNew() 
		{
			BoundDataItem newItem = ((BoundDataCollection)this).AddNewItem();
			return (object)newItem;
		}

		// Unsupported properties.
		bool IBindingList.IsSorted 
		{ 
			get { throw new NotSupportedException(); }
		}

		ListSortDirection IBindingList.SortDirection 
		{ 
			get { throw new NotSupportedException(); }
		}


		PropertyDescriptor IBindingList.SortProperty 
		{ 
			get { throw new NotSupportedException(); }
		}


		// Unsupported Methods.
		void IBindingList.AddIndex(PropertyDescriptor property) 
		{
			throw new NotSupportedException(); 
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction) 
		{
			throw new NotSupportedException(); 
		}

		int IBindingList.Find(PropertyDescriptor property, object key) 
		{
			throw new NotSupportedException(); 
		}

		void IBindingList.RemoveIndex(PropertyDescriptor property) 
		{
			throw new NotSupportedException(); 
		}

		void IBindingList.RemoveSort() 
		{
			throw new NotSupportedException(); 
		}
	}
}
