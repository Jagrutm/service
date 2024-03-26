using CredECard.Common.BusinessService;
using CredECard.CommonSetting.DataService;
using System;

namespace CredECard.CommonSetting.BusinessService
{
	/// <summary>
	/// Summary description for GeneralSettingCategoryList.
	/// </summary>
	public class GeneralSettingCategoryList : DataCollection
	{
		public GeneralSettingCategoryList()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		/// <author>Falguni Parikh</author>
		/// <created>11-Apr-2006</created>
		/// <summary>Add GeneralSettingCategory object in List
		/// </summary>
        /// <param name="typeToAdd">GeneralSettingCategory
		/// </param>
		public void Add(GeneralSettingCategory itemToAdd)
		{
			List.Add(itemToAdd);
		}

		/// <author>Falguni Parikh</author>
		/// <created>11-Apr-2006</created>
		/// <summary>Remove specific GeneralSettingCategory object from list
		/// </summary>
        /// <param name="itemToRemove">GeneralSettingCategory
		/// </param>
		public void Remove(GeneralSettingCategory itemToRemove)
		{
			List.Remove(itemToRemove);
		}

		/// <author>Falguni Parikh</author>
		/// <created>11-Apr-2006</created>
		/// <summary>Remove GeneralSettingCategory Object of specific index from list
		/// </summary>
		/// <param name="index">int
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// </exception>
		public void Remove(int index)
		{
			if ((index < this.Count - 1) || (index < 0))
			{
				throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
			}
			else
			{
				List.RemoveAt(index);
			}
		}

		/// <author>Falguni Parikh</author>
		/// <created>11-Apr-2006</created>
		/// <summary>Read property for getting GeneralSettingCategory at specific index from list
		/// </summary>
		/// <value>GeneralSettingCategory
		/// </value>
		/// <exception cref="IndexOutOfRangeException">throw exception if index out of range
		/// </exception>
		public new GeneralSettingCategory this[int index]
		{
			get
			{
				if ((index < this.Count - 1) || (index < 0))
				{
					throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
				}
				else
				{
					return (GeneralSettingCategory) List[index];
				}
			}
		}

		/// <author>Falguni Parikh</author>
		/// <created>11-Apr-2006</created>
		/// <summary>Fetches all records for GeneralSettingCategory
		/// </summary>
		/// <returns>This will return general settings list.
		/// </returns>
		public static GeneralSettingCategoryList LoadGeneralSettingCategoryList()
		{
			return ReadGeneralSettingCategory.LoadGeneralSettingCategoryList();
		}
	}
}
