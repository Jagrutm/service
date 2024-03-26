using System;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using CredECard.CommonSetting.BusinessService;

namespace CredEcard.CredEncryption.BusinessService
{
    /// <author>Arvind Ashapuri</author>
    /// <created>29-May-2008</created>
    /// <summary>this class contain security key detail to encrypt data
    /// </summary>
    /// 
    [Serializable()]
    public abstract class MasterCardCrypto
    {
        protected const string CARD_PAN = "PAN";
        protected const string CARD_EXPIRY = "EXPIRY";
        protected const string CARD_HOLDERNAME = "CARDHOLDERNAME";

        protected SymmetricAlgorithm objCryptoService = null;
        protected KMSconfig objKMSconfig = null;
        //private static GeneralSettingList _settingList = null;


        ///// <author>Gaurang Majithiya</author>
        ///// <created>19-Nov-2009</created>
        ///// <summary>Gets setting list of KMS category from Cache or Database
        ///// </summary>
        ///// <value>GeneralSettingList
        ///// </value>
        //protected static GeneralSettingList SettingList
        //{
        //    get
        //    {
        //        if (_settingList == null)
        //        {
        //            if (KMSconfig.Cache["SettingList"] == null)
        //            {
        //                _settingList = GeneralSettingList.LoadGeneralSettingsByCategory((int)EnumGeneralSettingsCategory.KMS);
        //                KMSconfig.SetCacheValue("SettingList", _settingList);
        //            }
        //            else
        //            {
        //                _settingList = (GeneralSettingList)KMSconfig.Cache["SettingList"];
        //            }
        //        }

        //        return _settingList;
        //    }
        //}

        ///// <author>Arvind Ashapuri</author>
        ///// <created>25-Nov-2008</created>
        ///// <summary>This will get the KMS configuration
        ///// </summary>
        ///// <returns>KMSconfig object
        ///// </returns>
        //protected virtual KMSconfig GetConfigValues()
        //{
        //    KMSconfig keyConfig = new KMSconfig();

        //    int portNo = 0;
        //    int.TryParse(MasterCardCrypto.SettingList[(int)EnumGeneralSettings.KMSClientPort].SettingValue, out portNo);

        //    keyConfig.KMSClientPort = portNo;
        //    keyConfig.PrimaryKMSClientIP = MasterCardCrypto.SettingList[(int)EnumGeneralSettings.PrimaryKMSClientIP].SettingValue;
        //    keyConfig.SecondaryKMSClientIP = MasterCardCrypto.SettingList[(int)EnumGeneralSettings.SecondaryKMSClientIP].SettingValue;

        //    keyConfig.ClientKeysPath = ConfigurationManager.AppSettings["ClientKeysPath"];

        //    return keyConfig;
        //}

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>This will fetch security key from resource file
        /// </summary>
        /// <param name="securityKey">string
        /// </param>
        /// <returns>byte array
        /// </returns>
        protected byte[] GetSecurityKey(string securityKey)
        {
            ResourceManager rm = new ResourceManager("CredEncryption.CommonResource", Assembly.GetExecutingAssembly());
            return Convert.FromBase64String(rm.GetString(securityKey));
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>Gets security key for File from resource file
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityKeyForFile
        {
            get
            {
                return GetSecurityKey("SecurityKeyForFile");
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>Gets security IV for card holder name from resource file
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityIVForFile
        {
            get
            {
                return GetSecurityKey("SecurityIVForFile");
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>Gets security key for card expiry date from resource file
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityKeyForExpiryDate
        {
            get
            {
                return GetSecurityKey("SecurityKeyForExpiryDate");
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>Gets security IV for card expiry date from resource file
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityIVForExpiryDate
        {
            get
            {
                return GetSecurityKey("SecurityIVForExpiryDate");
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>Gets security key for card holder name from resource file
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityKeyForCardHolder
        {
            get
            {
                return GetSecurityKey("SecurityKeyForCardHolder");
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>Gets security IV for card holder name from resource file
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityIVForCardHolder
        {
            get
            {
                return GetSecurityKey("SecurityIVForCardHolder");
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>Gets security IV for card holder name from resource file
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityKeyForPAN
        {
            get
            {
                return GetSecurityKey("SecurityKeyForPAN");
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>Gets security IV for card expiry date from resource file
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityIVForPAN
        {
            get
            {
                return GetSecurityKey("SecurityIVForPAN");
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>09-Nov-2011</created>
        /// <summary>Gets security Key for Card Info
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityKeyForCardInfo
        {
            get
            {
                return GetSecurityKey("SecurityKeyForCardInfo");
            }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>12-Mar-2015</created>
        /// <summary>Gets the security key for data.</summary>
        protected byte[] SecurityKeyForData
        {
            get
            {
                return GetSecurityKey("SecurityKeyForData");
            }
        }

        /// <author>Vipul Patel</author>
        /// <created>19-June-2014</created>
        /// <summary>Gets security Key for KeyEncryption
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityKeyForKeyEncryption
        {
            get
            {
                return Convert.FromBase64String(GeneralSetting.GetSettingValue((int)EnumGeneralSettings.SecurityKeyForKeyEncryption));
            }
        }

         /// <author>Vipul Patel</author>
        /// <created>19-June-2014</created>
        /// <summary>Gets security Key for KeyEncryption
        /// </summary>
        /// <value>byte array
        /// </value>
        protected byte[] SecurityIVForKeyEncryption
        {           

            get
            {
                return GetSecurityKey("SecurityIVForkeyEncryption");
            }
        }

    }
}
