using CredECard.Common.BusinessService;
using CredECard.CommonSetting.DataService;
using System;

namespace CredECard.CommonSetting.BusinessService
{
	/// <author>Chirag Khilosia</author>
	/// <created>10/6/2005</created>
	/// <summary>
	/// List of general settings
	/// </summary>
	/// <exception cref="IndexOutOfRangeException">
	/// </exception>
	public class GeneralSettingList : DataCollection
	{
		private int _categoryID=0;

		/// <author>Arvind Ashapuri</author>
		/// <created>11-Apr-2006</created>
		/// <summary>This is category ID. and used in SiteGeneralSettingList class
		/// </summary>
		/// <value>int
		/// </value>
		public int CategoryID
		{
			get
			{
				return _categoryID;
			}
			set
			{
				_categoryID=value;
			}
		}

		/// <author>Chirag Khilosia</author>
		/// <created>10/6/2005</created>
		/// <summary>
		/// return all general setting records
		/// </summary>
		/// <returns>
		/// GeneralSettingList
		/// </returns>
		public static GeneralSettingList LoadGeneralSettings()
		{
            return ReadGeneralSetting.LoadGeneralSettings();
		}

		/// <author>Arvind Ashapuri</author>
		/// <created>11-Apr-2006</created>
		/// <summary>
        /// call LoadGeneralSettingsByCategory from ReadGeneralSetting
		/// </summary>
		/// <param name="categoryID">int
		/// </param>
        /// <returns>GeneralSettingList
		/// </returns>
		public static GeneralSettingList LoadGeneralSettingsByCategory(int categoryID)
		{
			return ReadGeneralSetting.LoadGeneralSettingsByCategory(categoryID);
		}
	
		/// <author>Chirag Khilosia</author>
		/// <created>10/6/2005</created>
		/// <summary>
		/// Add a record to list
		/// </summary>
        /// <param name="itemToAdd">GeneralSetting
		/// </param>
		public void Add(GeneralSetting itemToAdd)
		{
			List.Add(itemToAdd);
		}

		/// <author>Chirag Khilosia</author>
		/// <created>10/6/2005</created>
		/// <summary>
		/// Remove a item from a specific index
		/// </summary>
		/// <param name="index">int
		/// </param>
		/// <exception cref="IndexOutOfRangeException">throw exception if index out of range
		/// </exception>
		public void Remove(int index)
		{
			if (index > this.Count - 1 || index < 0)
			{
				throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
			}
			else
			{
				List.RemoveAt(index); 
			}
		}

		/// <author>Chirag Khilosia</author>
		/// <created>10/6/2005</created>
		/// <summary>
		///  remove a particular item
		/// </summary>
        /// <param name="itemToRemove">GeneralSetting
		/// </param>
		public void Remove(GeneralSetting itemToRemove)
		{
			List.Remove(itemToRemove);
		}

		/// <author>Chirag Khilosia</author>
		/// <created>10/6/2005</created>
		/// <summary>
		/// return a item from a specific index
		/// </summary>
		/// <param name="index">int
		/// </param>
		/// <returns>
		/// GeneralSetting
		/// </returns>
        /// <value>GeneralSetting
		/// </value>
		public new GeneralSetting this[int settingID]
		{
			get
			{
				for(int i=0; i < this.Count; i++)
				{
					if (((GeneralSetting)List[i])._settingID == settingID)
						return  (GeneralSetting) List[i];
				}

				// Arvind : 12-Apr-2006 : if object not found in list then take object from DB
				return GeneralSetting.Specific(settingID);
			}
		}
	}
}

