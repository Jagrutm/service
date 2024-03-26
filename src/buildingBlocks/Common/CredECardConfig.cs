using System;
using System.Configuration;

namespace CredECard.Common.BusinessService
{
    public class CredECardConfig
    {
        private static string _connectionStringUserDB = string.Empty;
        private static string _connectionStringUserDBWrite = string.Empty;
        private static string _ahsCertPath = string.Empty;
        private static string _singinCertSubject = string.Empty;
        private static bool _ahsCallChecked = false;
        private static bool _isAhsCallOnline = false;
        private static string _singinCertPath = string.Empty;
        private static bool _isUseSignInCertPath = false;
        private static string _singinCertPassword= string.Empty;

        /// <author>Gaurang Majithiya</author>
        /// <CreatedOn>12-Jun-2007</CreatedOn>
        /// <summary>
        /// Gets connection string
        /// </summary>
        /// <returns>Returns string</returns>
        public static string GetReadOnlyConnectionString()
        {
            if (_connectionStringUserDB == string.Empty)
            {
                if (ConfigurationManager.AppSettings["IsUseEncryptedConnectionString"] == "1")
                {
                    SymmCrypto objCrypto = new SymmCrypto();

                    string connectionString = ConfigurationManager.ConnectionStrings["ReadOnly"].ConnectionString;

                    _connectionStringUserDB = objCrypto.Decrypting(connectionString);
                }
                else
                    _connectionStringUserDB = ConfigurationManager.ConnectionStrings["ReadOnly"].ConnectionString;
            }

            return _connectionStringUserDB;
        }
        public static string WriteConnectionString
        {
            get
            {
                if (_connectionStringUserDBWrite == string.Empty)
                {
                    if (ConfigurationManager.AppSettings["IsUseEncryptedConnectionString"] == "1")
                    {
                        SymmCrypto objCrypto = new SymmCrypto();

                        string connectionString = ConfigurationManager.ConnectionStrings["ReadWrite"].ConnectionString;

                        _connectionStringUserDBWrite = objCrypto.Decrypting(connectionString);
                    }
                    else
                        _connectionStringUserDBWrite = ConfigurationManager.ConnectionStrings["ReadWrite"].ConnectionString;
                }

                return _connectionStringUserDBWrite;
            }
        }
        
        public static string ReadConnectionString
        {
            get
            {
                if (_connectionStringUserDB == string.Empty)
                {
                    if (ConfigurationManager.AppSettings["IsUseEncryptedConnectionString"] == "1")
                    {
                        SymmCrypto objCrypto = new SymmCrypto();

                        string connectionString = ConfigurationManager.ConnectionStrings["ReadOnly"].ConnectionString;

                        _connectionStringUserDB = objCrypto.Decrypting(connectionString);
                    }
                    else
                        _connectionStringUserDB = ConfigurationManager.ConnectionStrings["ReadOnly"].ConnectionString;
                }

                return _connectionStringUserDB;
            }
        }
        
        /// <author>VIpul Patel</author>
        /// <CreatedOn>7-Sep-2017</CreatedOn>
        /// <summary>
        /// Gets connection string
        /// </summary>
        /// <returns>Returns string</returns>
        public static string GetReadWriteConnectionString()
        {
            if (_connectionStringUserDBWrite == string.Empty)
            {
                if (ConfigurationManager.AppSettings["IsUseEncryptedConnectionString"] == "1")
                {
                    SymmCrypto objCrypto = new SymmCrypto();

                    string connectionString = ConfigurationManager.ConnectionStrings["ReadWrite"].ConnectionString;

                    _connectionStringUserDBWrite = objCrypto.Decrypting(connectionString);
                }
                else
                    _connectionStringUserDBWrite = ConfigurationManager.ConnectionStrings["ReadWrite"].ConnectionString;
            }

            return _connectionStringUserDBWrite;
        }

        public static string WebsitePath
        {
            get
            {
                return ConfigurationManager.AppSettings["WebSitePath"];
            }
        }

        public static string AHSClientCertificatePath
        {
            get
            {
                if(_ahsCertPath == string.Empty)
                _ahsCertPath = ConfigurationManager.AppSettings["AHSClientCertPath"];

                return _ahsCertPath;
            }
        }

        public static string ACSSignCertificateSubject
        {
            get
            {
                if (_singinCertSubject == string.Empty)
                    _singinCertSubject =ConfigurationManager.AppSettings["SignInCertificate"];

                return _singinCertSubject;
            }
        }

        public static string ACSSignCertificatePath
        {
            get
            {
                if (_singinCertPath == string.Empty)
                    _singinCertPath = ConfigurationManager.AppSettings["SignInCertPath"];

                return _singinCertPath;
            }
        }

        public static string ACSSignCertificatePassword
        {
            get
            {
                if (_singinCertPassword == string.Empty)
                    _singinCertPassword = ConfigurationManager.AppSettings["SignInCertPassword"];

                return _singinCertPassword;
            }
        }

        public static bool IsAHSCallOnline
        {
            get
            {
                if (!_ahsCallChecked)
                {
                    _ahsCallChecked = true;
                    _isAhsCallOnline = Convert.ToBoolean((Convert.ToInt32(ConfigurationManager.AppSettings["IsAHSCallOnline"].ToString())));
                }

                return _isAhsCallOnline;
            }
        }

        public static bool IsUseSignInCertPath
        {
            get
            {
                if (!_isUseSignInCertPath)
                {
                    _isUseSignInCertPath = true;
                    _isUseSignInCertPath = Convert.ToBoolean((Convert.ToInt32(ConfigurationManager.AppSettings["IsUseSignInCertPath"].ToString())));
                }

                return _isUseSignInCertPath;
            }
        }
    }
}
