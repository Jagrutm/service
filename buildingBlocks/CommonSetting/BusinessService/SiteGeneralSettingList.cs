using CredECard.Common.BusinessService;
using CredECard.CommonSetting.DataService;
using System;
using System.Web;

namespace CredECard.CommonSetting.BusinessService
{
	/// <author>Arvind Ashapuri</author>
	/// <created>11-Apr-2006</created>
	/// <summary>This will be use in any Web site projects to get the general settings values
	/// </summary>
	public class SiteGeneralSettingList : GeneralSettingList
	{
		/// <author>Arvind Ashapuri</author>
		/// <created>11-Apr-2006</created>
		/// <summary>This will get the general setting list for the given category from Cache or get from DB for the given category. 
		/// </summary>
		/// <param name="categoryID">int
		/// </param>
		/// <returns>This will return general settings list.
		/// </returns>
		public static SiteGeneralSettingList Current(int categoryID)
		{
			// Try to get the cached instance
			SiteGeneralSettingList curSetting = SiteGeneralSettingList.GetCachedSettings(categoryID);
			if (curSetting != null) return curSetting;

			// we need to load from db and re-cache
			curSetting = ReadGeneralSetting.LoadSiteGeneralSettingsByCategory(categoryID);

			if (curSetting!=null) CachedSettings = curSetting;

			return curSetting;
		}

		/// <author>Arvind Ashapuri</author>
		/// <created>11-Apr-2006</created>
		/// <summary>This will get the general setting list for the given category from Cache for given category
		/// </summary>
		/// <param name="categoryID">int
		/// </param>
		/// <returns>This will return the general setting list
		/// </returns>
		public static SiteGeneralSettingList GetCachedSettings(int categoryID)
		{
            if (categoryID == 0) throw new PersistException("Invalid Category for general settings");

			if (HttpContext.Current == null) return null;
			var curContext = HttpContext.Current;
			Object curSetting=null;
			////curSetting = curContext.Cache["setting_" + categoryID];

			if ( curSetting  == null) {return null;}

			return (SiteGeneralSettingList)curSetting;
		}

		/// <author>Arvind Ashapuri</author>
		/// <created>11-Apr-2006</created>
		/// <summary>This will cache the given general setting list.
		/// </summary>
		/// <value>SiteGeneralSettingList
		/// </value>
		public static SiteGeneralSettingList CachedSettings
		{
			set
			{   
				////HttpContext curContext = HttpContext.Current;
				////curContext.Cache.Remove("setting_" + value.CategoryID.ToString());
				////curContext.Cache.Add("setting_" + value.CategoryID.ToString(), value, null,DateTime.Now.AddDays(1), Cache.NoSlidingExpiration,CacheItemPriority.Normal, null);
                
			}
		}
	}
}

