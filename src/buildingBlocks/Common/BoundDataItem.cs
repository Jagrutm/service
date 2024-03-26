namespace CredECard.Common.BusinessService
{
	using System;
	using System.ComponentModel;
	using System.Collections;
    using CredECard.Common.BusinessService;

    /// <summary>
    /// Inherit from this if you wish to use within data bound lists which support editing
    /// </summary>
    [Serializable()]
	public abstract class BoundDataItem : DataItem, ICloneable, IEditableObject
	{
		private DataCollection _parent = null;
		private BoundDataItem backupData = null; 
		private bool inTxn = false;

        /// <summary>
        /// Gets or sets the Parent
        /// </summary>
        ///<value>Datacollection</value>
		internal DataCollection Parent 
		{
			get 
			{
				return _parent;
			}
			set 
			{
				_parent = value ;
			}
		}
        
        void IEditableObject.BeginEdit() 
		{
			if (!inTxn) 
			{
				this.backupData = (BoundDataItem)this.Clone();
				inTxn = true;
			}
		}

		void IEditableObject.CancelEdit() 
		{
			if (inTxn) 
			{
				this.Fill(backupData);
				inTxn = false;
			}
		}

		void IEditableObject.EndEdit() 
		{
			if (inTxn) 
			{
				backupData = null;
				inTxn = false;
			}
		}

		public abstract object Clone();
		public abstract void Fill(DataItem fillWith);
		public abstract BoundDataItem GetBlank();
	}
}