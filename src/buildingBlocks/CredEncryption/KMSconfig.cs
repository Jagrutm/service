using System;
using System.Configuration;
using System.Web;
//using System.Web.Caching;
using CredECard.CommonSetting.BusinessService;

namespace CredEcard.CredEncryption.BusinessService
{
    /// <author>Arvind Ashapuri</author>
    /// <created>25-Nov-2008</created>
    /// <summary>This will used to set KMS configuration
    /// </summary>
    public class KMSconfig
    {
        private string _clientKeysPath = string.Empty;
        private string _connectionString = string.Empty;
        private int _cecSystemID = 0;
        private string _postToBugScoutValue = string.Empty;

        /// <author>Gaurang Majithiya</author>
        /// <created>26-Nov-2008</created>
        /// <summary>
        /// Gets or sets Post to bug scout
        /// </summary>
        public string PostToBugScoutValue
        {
            get
            {
                return _postToBugScoutValue;
            }
            set
            {
                _postToBugScoutValue = value;
            }
        }

        /// <author>Gaurang Majithiya</author>
        /// <created>26-Nov-2008</created>
        /// <summary>
        /// Gets or sets CEC System ID
        /// </summary>
        public int CECSystemID
        {
            get
            {
                return _cecSystemID;
            }
            set
            {
                _cecSystemID = value;
            }
        }

        /// <author>Gaurang Majithiya</author>
        /// <created>26-Nov-2008</created>
        /// <summary>
        /// Gets or sets Connections string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        ///// <author>Gaurang Majithiya</author>
        ///// <created>19-Nov-2009</created>
        ///// <summary>Gets HttpRuntime.Cache
        ///// </summary>
        ///// <value>Object of Cache
        ///// </value>
        //public static Cache Cache
        //{
        //    get
        //    {
        //        return HttpRuntime.Cache;
        //    }
        //}

        ///// <author>Gaurang Majithiya</author>
        ///// <created>19-Nov-2009</created>
        ///// <summary>Sets specified value to cache at specified key
        ///// </summary>
        ///// <param name="key">Name of key for the cache
        ///// </param>
        ///// <param name="value">Value for the cache
        ///// </param>
        //public static void SetCacheValue(string key, object value)
        //{
        //    Cache.Add(key, value, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.BelowNormal, null);
        //}        

        ///// <author>Arvind Ashapuri</author>
        ///// <created>25-Nov-2008</created>
        ///// <summary>Gets or Sets KMS key path
        ///// </summary>
        ///// <value>string
        ///// </value>
        //public string ClientKeysPath
        //{
        //    get
        //    {
        //        if (_clientKeysPath == string.Empty)
        //        {
        //            if (KMSconfig.Cache["ClientKeysPath"] == null)
        //            {
        //                _clientKeysPath = ConfigurationManager.AppSettings["ClientKeysPath"];
        //                SetCacheValue("ClientKeysPath", _clientKeysPath);
        //            }
        //            else
        //            {
        //                _clientKeysPath = (string)KMSconfig.Cache["ClientKeysPath"];
        //            }
        //        }

        //        return _clientKeysPath;
        //    }
        //    set
        //    {
        //        _clientKeysPath = value;
        //        if (_clientKeysPath != null) SetCacheValue("ClientKeysPath", _clientKeysPath);
        //    }
        //}        
    }
}
