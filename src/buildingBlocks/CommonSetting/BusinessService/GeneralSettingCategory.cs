using System;
using CredECard.CommonSetting.DataService;

namespace CredECard.CommonSetting.BusinessService
{
    /// <summary>
    /// Summary description for GeneralSettingCategory.
    /// </summary>
    public class GeneralSettingCategory
    {

        #region Variables
        private int _settingCategoryID = 0;
        private string _SettingCategoryDescription = string.Empty;
        #endregion

        #region Constructor

        /// <summary>
        /// constructor of GeneralSettingCategory object
        /// </summary>
        public GeneralSettingCategory()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region Public Properties

        /// <author>Falguni Parikh</author>
        /// <created>11-Apr-2006</created>
        /// <summary>get or set setting category id</summary>
        /// <value>Int
        /// </value>
        public int SettingCategoryID
        {
            get
            {
                return _settingCategoryID;
            }

            set
            {
                _settingCategoryID = value;
            }
        }

        /// <author>Falguni Parikh</author>
        /// <created>11-Apr-2006</created>
        /// <summary>get or set setting category description</summary>
        /// <value>String
        /// </value>
        public string SettingCategoryDescription
        {
            get
            {
                return _SettingCategoryDescription;
            }
            set
            {
                _SettingCategoryDescription = value;
            }
        }
        #endregion

        #region Public Methods
        /// <author>Falguni Parikh</author>
        /// <created>11-Apr-2006</created>
        /// <summary>
        /// Fetches record for specific SettingCategory
        /// </summary>
        /// <param name="SettingCategoryID">int
        /// </param>
        /// <returns>This will return general setting.
        /// </returns>
        public static GeneralSettingCategory Specific(int SettingCategoryID)
        {
            return ReadGeneralSettingCategory.LoadGeneralSettingCategory(SettingCategoryID);
        }

        /// <author>Keyur Parekh</author>
        /// <created>01-Dec-2009</created>
        /// <summary>
        /// Returns Category Id from setting id
        /// </summary>
        /// <param name="settingID">Integer</param>
        /// <returns>Integer</returns>
        public static int GetCategoryID(int settingID)
        {
            Int16 categoryID = 0;
            EnumGeneralSettingsCategory category = EnumGeneralSettingsCategory.None;

            switch (settingID)
            {
                case 9:
                case 10:
                case 11:
                case 12:
                    category = EnumGeneralSettingsCategory.User;
                    break;

                case 24:
                case 1:
                case 4:
                case 187:
                case 225:
                case 247:
                    category = EnumGeneralSettingsCategory.Security;
                    break;

                case 207:
                case 208:
                case 209:
                case 64:
                case 65:
                case 66:
                case 67:
                case 68:
                case 70:
                case 71:
                case 72:
                case 75:
                case 88:
                case 89:
                case 90:
                case 2:
                case 163:
                case 164:
                case 147:
                case 148:
                case 227:
                case 228:
                case 229:
                    category = EnumGeneralSettingsCategory.Authorisation;
                    break;

                case 152:
                case 25:
                case 34:
                case 3:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                case 110:
                case 210:
                    category = EnumGeneralSettingsCategory.Others;
                    break;
                case 211:
                case 212:
                case 213:
                case 214:
                case 215:
                case 216:
                case 217:
                case 218:
                case 219:
                case 220:
                case 221:
                case 222:
                case 223:
                case 224:
                    category = EnumGeneralSettingsCategory.ThreeDSecure;
                    break;


            }

            if(category == 0) //Check in database
            {
                GeneralSetting objSetting = GeneralSetting.Specific(settingID);

                if (objSetting != null)
                {
                    categoryID = Convert.ToInt16(objSetting.SettingCategoryID);
                }
            }

            return Convert.ToInt16(category);

            
        }

        #endregion

    }

    #region Enum
    /// <summary>
    /// Enum General Settings Category
    /// </summary>
    public enum EnumGeneralSettingsCategory
    {
        None = 0,
        User = 1,
        Staff = 2,
        Security = 3,
        Authorisation = 4,
        Others = 5,
        ThreeDSecure = 6,
        SendMail = 7,
    }
    #endregion

}
