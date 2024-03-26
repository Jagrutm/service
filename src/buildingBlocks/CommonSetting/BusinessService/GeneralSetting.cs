using CredECard.Common.BusinessService;
using CredECard.CommonSetting.DataService;
using System;
using System.Web;

namespace CredECard.CommonSetting.BusinessService
{
    /// <author>Chirag Khilosia</author>
    /// <created>10/6/2005</created>
    /// <summary>
    /// class related to general application settings
    /// </summary>
    [Serializable]
    public class GeneralSetting : Setting, IPersistableV2
    {
        //private static int _createdByStaffID = 0;
        //private static double _taxPercent = 0;


        #region IPersistableV2 Members

        /// <author>Chirag Khilosia</author>
        /// <created>10/6/2005</created>
        /// <summary>
        ///  save record (need to pass DataController)
        /// </summary>
        /// <param name="conn">DataController
        /// </param>
        public void Save(DataController conn)
        {
            WriteGeneralSetting wr = new WriteGeneralSetting(conn);
            wr.WriteGeneralSettingData(this);
        }

        #endregion

        /// <author>Chirag Khilosia</author>
        /// <created>10/6/2005</created>
        /// <summary>
        /// returs GeneralSetting object based on id
        /// </summary>
        /// <param name="settingID">int
        /// </param>
        /// <returns>
        /// GeneralSetting
        /// </returns>
        public static GeneralSetting Specific(int settingID)
        {
            return ReadGeneralSetting.ReadGeneralSettingDetails(settingID);
        }

        /// <author>Hetal Shah</author>
        /// <created>12/6/2005</created>
        /// <summary>returns general setting value based on id
        /// </summary>
        /// <param name="settingID">int
        /// </param>
        /// <returns>string
        /// </returns>
        public static string GetSettingValue(int settingID)
        {
            GeneralSetting objSetting = null;

            objSetting = ReadGeneralSetting.ReadGeneralSettingDetails(settingID);

            return (objSetting ?? new GeneralSetting()).DecryptSettingValue;
        }

        /// <author>Prashant Soni</author>
        /// <created>01-Mar-2006</created>
        /// <summary>returns general setting value based on Enum
        /// </summary>
        /// <param name="settingEnum">It is setting enum.
        /// </param>
        /// <returns>it returns string value for the enum passed.
        /// </returns>
        public static string GetSettingValue(EnumGeneralSettings settingEnum)
        {
            return GetSettingValue((int)settingEnum);
        }

        /// <author>Prashant Soni</author>
        /// <created>20-Jan-2006</created>
        /// <summary>returns general setting value based on id and connectionstring 
        /// </summary>
        /// <param name="settingID">int
        /// </param>
        /// <param name="connectString">string
        /// </param>
        /// <returns>string
        /// </returns>
        public static string GetSettingValue(int settingID, string connectString)
        {
            GeneralSetting objSetting = ReadGeneralSetting.ReadGeneralSettingDetails(settingID, connectString);
            return objSetting.DecryptSettingValue;
        }

        /// <author>Keyur Parekh</author>
        /// <created>14-Jul-2010</created>
        /// <summary>
        /// Read General setting value with open connection
        /// </summary>
        /// <param name="settingEnum">EnumGeneralSettings</param>
        /// <param name="conn">DataController</param>
        /// <returns>String</returns>
        public static string GetSettingValueWithConnection(EnumGeneralSettings settingEnum, DataController conn)
        {
            int settingID = (int)settingEnum;
            GeneralSetting objSetting = null;

            if (HttpContext.Current != null)
            {
                SiteGeneralSettingList objList = SiteGeneralSettingList.Current(GeneralSettingCategory.GetCategoryID(settingID));
                if (objList != null) objSetting = objList[settingID];
            }
            else
                objSetting = ReadGeneralSetting.ReadGeneralSettingDetailsWithConnection(settingID, conn);

            return objSetting.DecryptSettingValue;
        }
    }
}