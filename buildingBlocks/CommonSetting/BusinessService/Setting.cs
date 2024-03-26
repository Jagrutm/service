using CredECard.Common.BusinessService;
using System;

namespace CredECard.CommonSetting.BusinessService
{
    /// <author>Chirag Khilosia</author>
    /// <created>10/6/2005</created>
    /// <summary>
    /// class related to general application settings
    /// </summary>
    [Serializable]
    public abstract class Setting : DataItem
    {
        internal int _settingID = 0;
        internal string _settingName = string.Empty;
        internal string _settingValue = string.Empty;
        internal string _settingDescription = string.Empty;
        internal bool _isEncrypted = false;
        internal int _settingCategoryID = 0;
        internal string _settingCategoryDescription = string.Empty;

        /// <summary>
        /// get or set setting id
        /// </summary>
        /// <author>Chirag Khilosia</author>
        /// <created>10/6/2005</created>
        /// <value>
        /// integer
        /// </value>
        public int SettingID
        {
            get
            {
                return _settingID;
            }
            set
            {
                _settingID = value;
            }
        }


        /// <summary>
        /// get or set setting name
        /// </summary>
        /// <author>Chirag Khilosia</author>
        /// <created>10/6/2005</created>
        /// <value>
        /// string
        /// </value>
        public string SettingName
        {
            get
            {
                return _settingName;
            }
            set
            {
                _settingName = value;
            }
        }

        /// <summary>
        /// get or set setting value
        /// </summary>
        /// <author>Chirag Khilosia</author>
        /// <created>10/6/2005</created>
        /// <value>
        /// string
        /// </value>
        public string SettingValue
        {
            get
            {
                if (_isEncrypted) return "****";
                else return _settingValue;
            }

            set
            {
                if (_isEncrypted)
                {
                    if (value != string.Empty)
                    {
                        SymmCrypto objCrypto = new SymmCrypto(SymmCrypto.SymmProvEnum.Rijndael);
                        _settingValue = objCrypto.Encrypting(value);
                    }
                    else _settingValue = value;
                }
                else
                {
                    _settingValue = value;
                }
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>11-Apr-2006</created>
        /// <summary>This will decrypt seting value
        /// </summary>
        /// <value>string
        /// </value>
        public string DecryptSettingValue
        {
            get
            {
                if (_isEncrypted)
                {
                    SymmCrypto objCrypto = new SymmCrypto(SymmCrypto.SymmProvEnum.Rijndael);
                    return objCrypto.Decrypting(_settingValue);
                }
                else return _settingValue;
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>11-Apr-2006</created>
        /// <summary>This is Seting descrption 
        /// </summary>
        /// <value>string
        /// </value>
        public string SettingDescription
        {
            get
            {
                return _settingDescription;
            }

            set
            {
                _settingDescription = value;
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>11-Apr-2006</created>
        /// <summary>This is Encryption flag for seting value.
        /// </summary>
        /// <value>bool
        /// </value>
        public bool IsEncrypted
        {
            get
            {
                return _isEncrypted;
            }

            set
            {
                _isEncrypted = value;
            }
        }


        /// <author>Arvind Ashapuri</author>
        /// <created>11-Apr-2006</created>
        /// <summary>This is category ID in which this setting belongs to.
        /// </summary>
        /// <value>int
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

        /// <author>Arvind Ashapuri</author>
        /// <created>11-Apr-2006</created>
        /// <summary>THis is category description
        /// </summary>
        /// <value>string
        /// </value>
        public string SettingCategoryDescription
        {
            get
            {
                return _settingCategoryDescription;
            }
            set
            {
                _settingCategoryDescription = value;
            }
        }

        /// <author>Chirag Khilosia</author>
        /// <created>10/6/2005</created>
        /// <summary>
        /// Validate if setting name and value is empty string
        /// </summary>
        /// <returns>ValidateResult
        /// </returns>
        public override ValidateResult Validate()
        {
            FieldErrorList newErrors = new FieldErrorList();
            if (this._settingName == string.Empty) newErrors.Add(new FieldError("Name", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));
            if (this._settingValue == string.Empty) newErrors.Add(new FieldError("Value", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));

            ValidateResult result = new ValidateResult(newErrors);
            return result;
        }

    }

    /// <summary>
    /// EnumGeneralSettings
    /// </summary>
    public enum EnumGeneralSettings : int
    {
        Bank_code_of_submitter = 1,
        Submitter_centre_number = 2,
        Bank_code_of_paying_bank = 3,
        DDI_Return_Cutoff_Time = 4,
        MaxRecordPerFetch = 5,
        ServiceUserNumber_OwnweIdentifier = 6,
        AdminUserID = 9,
        PRIMARY_HSM_LONG_IP = 10,
        SECONDARY_HSM_LONG_IP = 11,
        HSM_EXCRYPT_PORT = 12,
        ContisDebitPlatform_SignKeyIndex = 13,
        ContisDebitPlatform_ClientPublicKey = 14,
        BACSAdviceFileContentFormat = 15,
        BACSPKCS7FileFormat = 16,
        BACSSigningAttribute = 17, //SHA256
        BACSPKCS7SignatureFormat = 18, //SHA256
        BACSBoundaryPrefix = 19,
        BACSSigningPrivateKeyIndex = 20,
        BACS_Contis_BIC8 = 21,
        //BACSTestOrLiveIndicator = 22,
        BACSInFileSigningPublicKeyIndex = 23,
        BACSSignVerificationCertificate1 = 24,
        BACSOutputPKCS7FileFormat_Inbound = 25,
        //PasswordLockout = 25,
        FPSInstitution = 26,
        BACSInputPKCS7FileFormat_Outbound = 27,
        //BACSSigningAttribute = 28, //SHA1
        //BACSPKCS7SignatureFormat = 29, //SHA1
        IsVeifyBACSOutputHSMSingature = 30,
        IsVeifyFAFile = 33,
        BACSSignVerificationCertificate2 = 34,
        BACSSignVerificationCertificate_BetaTesting = 35,
        BACS_VL_BIC8 = 36,
        FogbugzUserName = 108,
        BugScoutURL = 110,
        SecurityKeyForKeyEncryption = 225,
        VerifyZMK = 248,
        JWTIssuer = 249,
        JWTAudience = 250,
        JWTExpireInMinutes = 251,
        WrokingKeyIn = 252,
        ProcessFailedDurationInHours = 253,
        CompanionFileTemplate = 255,
        CompanionFileHashKey = 256,
        FPSOutboundPaymentRepeatCount = 257,
        BACSDCRecallWithARUCS = 258,
        SuccessResponseList = 259,
        CertificateKeyforEncDec = 260,
        MonitoringRecipients = 261
    }
}