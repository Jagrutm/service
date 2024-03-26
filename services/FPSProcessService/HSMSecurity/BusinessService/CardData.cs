using System;
using ContisGroup.CardUtil.DataService;
using CredEcard.CredEncryption.BusinessService;
using CredECard.CardProduction.Enums;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;
using CredECard.Common.Enums.Card;
using CredECard.Common.Enums.Transaction;
using System.Text;
using CredECard.BugReporting.BusinessService;

namespace ContisGroup.CardUtil.BusinessService
{
    [Serializable]
    public class CardData
    {
        internal int _cardID = 0;
        internal int _cardDetailID = 0;
        internal string _pan = string.Empty;
        internal short _statusID = 0;

        internal DateTime _expiryDate = DateTime.MinValue;
        internal string _pvv = string.Empty;
        internal string _encPAN = string.Empty;
        internal string _pinBlock = string.Empty;
        internal int _serviceCode = 0;
        internal int _iCvvCode = 0;
        internal short _pvki = 0;

        internal int _failedPinAttempt = 0;
        internal int _maxFailedPinAttempt = 3;
        internal string _activationCode = string.Empty;
        internal short _cardVerificationMethodID = 0;
        internal int _schemeID = 0;

        internal DateTime _dob = DateTime.MinValue;
        internal string _postCode = string.Empty;
        internal string _surname = string.Empty;
        private string _cardStatusChangeWSURL = string.Empty;
        internal int _cardTypeID = 0;
        //internal string _institute_TDES_KEY = string.Empty;
        //internal string _institute_TDES_IV = string.Empty;
        internal bool _checkTVR = false;
        internal bool _isAllowTokenization = false;

        internal int _failedCVV2Attempt = 0;
        internal string _line1 = string.Empty;
        internal string _line2 = string.Empty;
        internal string _line3 = string.Empty;
        internal bool _checkForDuplicateAuth = false;
        internal Int16 _cardProgramID = 0;
        internal bool _isCountryLevelBlock = false;
        internal string _authServiceAPIURL = string.Empty;
        internal string _authClearingServiceApiUrl = string.Empty; // RS#128206
        internal short _pinUnBlockAttempt = 0;
        internal bool _isSendDeclineAuth = false;
        internal bool _isAllowContactlessTokenATMWithdrawal = false;
        internal bool _isAllowVISADirectPayment = false;
        internal bool _isUseEMVA = false;
        internal bool _isUseEMVM = false;
        internal bool _isUseEMVP = false;
        internal bool _isAllowNonAuthenticatedECI = false;
        internal bool _isSCARequire = false;
        internal string _cardDisplayName = string.Empty;
        internal string _decryptedName = string.Empty;
        internal Int64 _cardFraudSetupID = 0; //Mantan Bhatti : Case : 13722
        internal int _invalidExpiryDateCount = 0; //Mantan Bhatti : Case : 13722
        internal int _maximumAllowedExpiryDateAttempts = 0; //Mantan Bhatti : Case : 13722
        internal int _blockedCardResetHours = 0; //Mantan Bhatti : Case : 13722
        internal int _latestATC = 0; //Mantan Bhatti : Case : 115821

        internal string _city = string.Empty; // Niken Shah Case 119275
        internal string _state = string.Empty; // Niken Shah Case 119275
        internal string _countryCode = string.Empty; // Niken Shah Case 119275
        internal EnumProductType _enumProductType = EnumProductType.Visa; // Niken Shah

        internal int _statusChangeReasonCodeID = 0; // Niken Shah Case 123359
        internal bool _isStepupRequiredForDeviceBinding = false; // Niken Shah Case 128147

        internal int _intPrevATMPOSTransCurrencyCode = 0; // Niken Shah Case 139421
        internal int _intCardHolderCurrencyCode = 0; // Niken Shah Case 139421
        internal bool _isCBPR2Required = false; // Niken Shah Case 139421
        internal bool _isNameOnlyCard = false; // Tejas Choksi Case 153103
        internal int _softDeclineECI7HigherLimit = 0; //vipul 140608  
        internal int _softDeclineECI6HigherLimit = 0; //vipul 140608  
        internal int _lowvalueLimit = 0;
        internal string _useAquirerTRAFlag = "0";
        internal bool _isProblemCard = false;


        /// <author>Vipul Patel</author>
        /// <created>15-Mar-2021</created>
        /// <summary>
        /// Get UseAquirerTRAFlag
        /// </summary>
        public string UseAquirerTRAFlag
        {
            get { return _useAquirerTRAFlag; }
        }

        /// <author>Vipul Patel</author>
        /// <created>15-Mar-2021</created>
        /// <summary>
        /// Get LowvalueLimit
        /// </summary>
        public int LowvalueLimit
        {
            get { return _lowvalueLimit; }
        }
        /// <author>Vipul Patel</author>
        /// <created>15-Mar-2021</created>
        /// <summary>
        /// Get SoftDeclineECI7HigherLimit
        /// </summary>
        public int SoftDeclineECI7HigherLimit
        {
            get { return _softDeclineECI7HigherLimit; }
        }
        /// <author>Vipul Patel</author>
        /// <created>15-Mar-2021</created>
        /// <summary>
        /// Get SoftDeclineECILimit
        /// </summary>
        public int SoftDeclineECI6HigherLimit
        {
            get { return _softDeclineECI6HigherLimit; }
        }
        /// <author>Keyur</author>
        /// <created>05-Mar-2019</created>
        /// <summary>
        /// Use EMVA command for ARQC verification or not?
        /// </summary>
        public bool IsUseEMVA
        {
            get { return _isUseEMVA; }
        }

        /// <author>Keyur</author>
        /// <created>05-Mar-2019</created>
        /// <summary>
        /// Use EMVM command for MAC or not?
        /// </summary>
        public bool IsUseEMVM
        {
            get { return _isUseEMVM; }
        }

        /// <author>Keyur</author>
        /// <created>05-Mar-2019</created>
        /// <summary>
        /// Use EMVA command for Change PIN or not?
        /// </summary>
        public bool IsUseEMVP
        {
            get { return _isUseEMVP; }
        }

        /// <author>Vipul Pate</author>
        /// <created>25-Feb-2019</created>
        /// <summary>
        /// Get Is Allow Non Authenticated ECI
        /// </summary>
        public bool IsAllowNonAuthenticatedECI
        {
            get { return _isAllowNonAuthenticatedECI; }
        }

        /// <author>Manthan Bhatti</author>
        /// <created>02-Aug-2019</created>
        /// <summary>
        /// Get Is flag to SCA Requires or not
        /// </summary>
        public bool IsSCARequire
        {
            get { return _isSCARequire; }
        }

        /// <author>Vipul Pate</author>
        /// <created>26-Nov-2018</created>
        /// <summary>
        /// Get Is Allow VISA Direct Payment
        /// </summary>
        public bool IsAllowVISADirectPayment
        {
            get { return _isAllowVISADirectPayment; }
        }

        /// <author>Vipul Pate</author>
        /// <created>18-Sep-2018</created>
        /// <summary>
        /// Get Is Allow Contactless Token ATM Withdrawal
        /// </summary>
        public bool IsAllowContactlessTokenATMWithdrawal
        {
            get { return _isAllowContactlessTokenATMWithdrawal; }
        }


        /// <author>Vipul Pate</author>
        /// <created>15-May-2018</created>
        /// <summary>
        /// Get Is send Decline Auth
        /// </summary>
        public bool IsSendDeclineAuth
        {
            get { return _isSendDeclineAuth; }
        }

        /// <author>Keyur</author>
        /// <created>20-Nov-2017</created>
        /// <summary>
        /// Get Whether Country level block for card program or not
        /// </summary>
        public bool IsCardProgramBlockedForCountry
        {
            get { return _isCountryLevelBlock; }
        }


        /// <author>Keyur</author>
        /// <created>20-Jun-2017</created>
        /// <summary>
        /// Check for duplicate Authorization or not
        /// </summary>
        public bool CheckForDuplicateAuth
        {
            get { return _checkForDuplicateAuth; }
        }

        /// <author>Keyur</author>
        /// <created>16-Jun-2017</created>
        /// <summary>Get Address without postcode
        /// </summary>
        public string AVSAddress
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(_line1);

                if (_line2.Length > 0)
                {
                    sb.Append(" ");
                    sb.Append(_line2);
                }

                if (_line3.Length > 0)
                {
                    sb.Append(" ");
                    sb.Append(_line3);
                }

                return sb.ToString();
            }
        }

        /// <author>Keyur</author>
        /// <created>16-Jun-2017</created>
        /// <summary>
        /// Get or Set Card Status Change URL
        /// </summary>
        public string CardStatusChangeURL
        {
            set { _cardStatusChangeWSURL = value; }
            get { return _cardStatusChangeWSURL; }
        }

        /// <author>Vipul patel</author>
        /// <created>15-Dec-2017</created>
        /// <summary>
        /// Get AuthServiceAPIURL
        /// </summary>
        public string AuthServiceAPIURL
        {
            get { return _authServiceAPIURL; }
        }

        /// <author>Rikunj Suthar</author>
        /// <created>05-Aug-2020</created>
        /// <summary>
        /// Authorisation clearing sevice api url.
        /// </summary>
        public string AuthClearingServiceApiUrl
        {
            get { return _authClearingServiceApiUrl; }
        }


        internal string _panReferenceID = string.Empty;


        /// <author>Vipul Patel</author>
        /// <created>05-Apr-2017</created>
        /// <summary>
        /// Get IsAllowTokenization
        /// </summary>
        public bool IsAllowTokenization
        {
            get { return _isAllowTokenization; }
        }


        /// <author>Keyur Parekh</author>
        /// <created>18-Nov-2014</created>
        /// <summary>
        /// Get Check Terminal Verification Result or not
        /// </summary>
        public bool IsCheckTVR
        {
            get { return _checkTVR; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>26-Dec-2012</created>
        /// <summary>
        /// Get Scheme ID
        /// </summary>
        public int SchemeID
        {
            get { return _schemeID; }
        }
        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get Card Detail ID
        /// </summary>
        public int CardDetailID
        {
            get { return _cardDetailID; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get Card ID
        /// </summary>
        public int CardID
        {
            get { return _cardID; }
        }

        /// <author>Vipul Patel</author>
        /// <created>1-Nov-2017</created>
        /// <summary>
        /// Get CardProgramID
        /// </summary>
        public Int16 CardProgramID
        {
            get { return _cardProgramID; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get Activation Code
        /// </summary>
        public string ActivationCode
        {
            get { return _activationCode; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get Pan
        /// </summary>
        public string PAN
        {
            get
            {
                if (_pan == string.Empty)
                {
                    MasterCardDecrypt objEecrypt = new MasterCardDecrypt();
                    _pan = objEecrypt.DecryptPANUsingKMS(_encPAN);
                }

                return _pan;
            }
        }


        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get Card Status
        /// </summary>
        public EnumCardStatus Status
        {
            get { return (EnumCardStatus)_statusID; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get Expiry Date
        /// </summary>
        public DateTime ExpiryDate
        {
            get { return _expiryDate; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get Service Code
        /// </summary>
        public int ServiceCode
        {
            get { return _serviceCode; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get ICVV Code
        /// </summary>
        public int ICvvCode
        {
            get { return _iCvvCode; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get PVV
        /// </summary>
        public string PVV
        {
            get { return _pvv; }
        }
        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get Pinblock
        /// </summary>
        public string PinBlock
        {
            get { return _pinBlock; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get PVKI
        /// </summary>
        public short PVKI
        {
            get { return _pvki; }
        }

        /// <author>Vipul Patel</author>
        /// <created>20-Dec-2017</created>
        /// <summary>
        /// Get PinUnBlockAttempt
        /// </summary>
        public short PinUnBlockAttempt
        {
            get { return _pinUnBlockAttempt; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>25-Dec-2012</created>
        /// <summary>
        /// Get Card Verification Method Name
        /// </summary>
        public EnumCardVerificationMethods CardVerificationMethod
        {
            get { return (EnumCardVerificationMethods)_cardVerificationMethodID; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Jun-2013</created>
        /// <summary>
        /// Get Date of Birth
        /// </summary>
        public DateTime DateOfBirth
        {
            get
            {
                return _dob;
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Jun-2013</created>
        /// <summary>
        /// Get Postcode
        /// </summary>
        public string PostCode
        {
            get
            {
                return _postCode;
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>13-Jun-2013</created>
        /// <summary>
        /// Get Surname
        /// </summary>
        public string Surname
        {
            get
            {
                return _surname;
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>16-Oct-2013</created>
        /// <summary>
        /// Get Card Type ID
        /// </summary>
        public int CardTypeID
        {
            get
            {
                return _cardTypeID;
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Nov-2015</created>
        /// <summary>
        /// Get Failed CVV2 Attempt
        /// </summary>
        public int FailedCVV2Attempt
        {
            get { return _failedCVV2Attempt; }
        }

        /// <author>Vipul Patel</author>
        /// <created>05-May-2017</created>
        /// <summary>        
        /// </summary>
        public string PanReferenceID
        {
            get { return _panReferenceID; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>07-Oct-2010</created>
        /// <summary>
        /// Get Decrypted Card Holder Name
        /// </summary>
        public string DecryptedCardDisplayName
        {
            get
            {
                if (_cardDisplayName != string.Empty && _decryptedName == string.Empty)
                {
                    MasterCardDecrypt objDecrypt = new MasterCardDecrypt();
                    _decryptedName = objDecrypt.DecryptCardHolderUsingKMS(_cardDisplayName);
                }

                return _decryptedName;
            }
        }

        /// <author>Manthan Bhatti</author>
        /// <created>03-Mar-2020</created>
        /// <summary>
        /// Get Card Fraud Setup ID
        /// </summary>
        public Int64 CardFraudSetupID
        {
            get { return _cardFraudSetupID; }
        }

        /// <author>Manthan Bhatti</author>
        /// <created>03-Mar-2020</created>
        /// <summary>
        /// Get Invalid Expiry Date Count
        /// </summary>
        public int InvalidExpiryDateCount
        {
            get { return _invalidExpiryDateCount; }
        }

        /// <author>Manthan Bhatti</author>
        /// <created>03-Mar-2020</created>
        /// <summary>
        /// Get Maximum Allowed Expiry Date Attempts
        /// </summary>
        public int MaximumAllowedExpiryDateAttempts
        {
            get { return _maximumAllowedExpiryDateAttempts; }
        }

        /// <author>Manthan Bhatti</author>
        /// <created>03-Mar-2020</created>
        /// <summary>
        /// Get Blocked Card Reset Hours
        /// </summary>
        public int BlockedCardResetHours
        {
            get { return _blockedCardResetHours; }
        }

        /// <author>Manthan Bhatti</author>
        /// <created>27-Apr-2020</created>
        /// <summary>
        /// Get Latest ATC
        /// </summary>
        public int LatestATC
        {
            get { return _latestATC; }
        }

        /// <author>Niken Shah</author>
        /// <created>17-June-2020</created>
        /// <summary>
        /// Get Set City
        /// </summary>
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <author>Niken Shah</author>
        /// <created>17-June-2020</created>
        /// <summary>
        /// Get Set City
        /// </summary>
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <author>Niken Shah</author>
        /// <created>17-June-2020</created>
        /// <summary>
        /// Get Set Country
        /// </summary>
        public string CountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        /// <author>Niken Shah</author>
        /// <created>17-June-2020</created>
        /// <summary>
        /// Get Set Line1
        /// </summary>
        public string Line1
        {
            get { return _line1; }
            set { _line1 = value; }
        }

        /// <author>Niken Shah</author>
        /// <created>17-June-2020</created>
        /// <summary>
        /// Get Set Line1
        /// </summary>
        public string Line2
        {
            get { return _line2; }
            set { _line2 = value; }
        }

        /// <author>Niken Shah</author>
        /// <created>01-Sep-2020</created>
        /// <summary>
        /// Card's product type.
        /// </summary>
        public EnumProductType EnumProductType
        {
            get { return _enumProductType; }
            set { _enumProductType = value; }
        }

        /// <author>Niken Shah</author>
        /// <created>17-June-2020</created>
        /// <summary>
        /// Get StatusChangeReasonCodeID
        /// </summary>
        public int StatusChangeReasonCodeID
        {
            get { return _statusChangeReasonCodeID; }
        }

        /// <author>Niken Shah</author>
        /// <created>25-Sep-2020</created>
        /// <summary>
        /// Get bSteupRequiredForDeviceBinding
        /// </summary>
        public bool IsStepupRequiredForDeviceBinding
        {
            get { return _isStepupRequiredForDeviceBinding; }
        }

        /// <author>Niken Shah</author>
        /// <created>08-Feb-2021</created>
        /// <summary>
        /// Get PrevATMPOSTransCurrencyCode
        /// </summary>
        public int PrevATMPOSTransCurrencyCode
        {
            get { return _intPrevATMPOSTransCurrencyCode; }
        }

        /// <author>Niken Shah</author>
        /// <created>08-Feb-2021</created>
        /// <summary>
        /// Get CardHolderCurrencyCode
        /// </summary>
        public int CardHolderCurrencyCode
        {
            get { return _intCardHolderCurrencyCode; }
        }

        /// <author>Niken Shah</author>
        /// <created>08-Feb-2021</created>
        /// <summary>
        /// Get CBPR2Required
        /// </summary>
        public bool CBPR2Required
        {
            get { return _isCBPR2Required; }
        }

        /// <author>Tejas Choksi</author>
        /// <created>26-Feb-2021</created>
        /// <summary>
        /// Get IsNameOnlyCard
        /// </summary>
        // Tejas Choksi Case 153103
        public bool IsNameOnlyCard
        {
            get { return _isNameOnlyCard; }
        }

        public bool IsProblemCard
        {
            get { return _isProblemCard; }
        }
        

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get speciric card data
        /// </summary>
        /// <param name="cardNumber">Card Number</param>
        /// <returns>CardData</returns>
        public static CardData Specific(string cardNumber)
        {
            MasterCardEncrypt objEncrypt = new MasterCardEncrypt();
            string encPan = objEncrypt.EncryptPANUsingKey(cardNumber);

            CardData objCardData = ReadCardData.ReadCard(encPan);
            if (objCardData != null)
            {
                objCardData._encPAN = encPan;
                objCardData._pan = cardNumber;
            }

            return objCardData;
        }

        /// <author>Keyur Parekh</author>
        /// <created>16-Jun-2017</created>
        /// <summary>
        /// Get speciric card data, Pan Encryption key is supplied
        /// </summary>
        /// <param name="cardNumber">Card Number</param>
        /// <param name="objPanEnckey">Pan Encryption Key</param>
        /// <returns>CardData</returns>
        public static CardData Specific(string cardNumber, Encryptionkey objPanEnckey)
        {
            MasterCardEncrypt objEncrypt = new MasterCardEncrypt();
            string encPan = objEncrypt.EncryptPANUsingKey(cardNumber, objPanEnckey);

            CardData objCardData = ReadCardData.ReadCard(encPan);
            if (objCardData != null)
            {
                objCardData._encPAN = encPan;
                objCardData._pan = cardNumber;
            }

            return objCardData;
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get speciric card data
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static CardData Specific(int cardID)
        {
            CardData objCardData = ReadCardData.ReadCard(cardID);
            return objCardData;
        }

        /// <author>Manthan Bhatti</author>
        /// <created>04-Mar-2020</created>
        /// <summary>
        /// Get Card Fraud Data
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        public static CardData GetCardFraudData(int cardID)
        {
            CardData objCardData = ReadCardData.ReadCardFraudData(cardID);
            return objCardData;
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Update Faile Attempt
        /// </summary>
        public void UpdateFailedAttempt(EnumCardStatus CardStatus)
        {
            int cardStatusHistoryID = 0;
            bool flag = false;

            SafeDataController conn = new SafeDataController();
            conn.BeginTransaction(CredECardConfig.GetReadWriteConnectionString(), "Update Failed Attempt");

            try
            {
                WriteCardData wrCD = new WriteCardData(conn);
                wrCD.UpdateFailedAttempt(_encPAN, CardStatus, out cardStatusHistoryID);

                conn.CommitTransaction();
                flag = true;
            }
            catch (Exception ex)
            {
                conn.RollbackTransaction();
                throw ex;
            }

            if (flag)
            {
                if (cardStatusHistoryID > 0 && _isAllowTokenization) SuspendAllTokensOfCard(_cardDetailID, EnumStatusChangeReasonCode.BLOCKED_BY_SYSTEM_TO_PREVENT_FRAUD);
                //updateClientCardStatus(cardStatusHistoryID, CardStatus);
            }
        }

        /// <author>Vipul Patel</author>
        /// <created>20-Dec-2017</created>
        /// <summary>
        /// Update pinunblock attempt
        /// </summary>
        public void UpdatePinUnblockAttempt()
        {
            DataController conn = new DataController();
            conn.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                WriteCardData objWrite = new WriteCardData(conn);
                objWrite.UpdatePinUnblockAttempt(CardID);
            }
            finally
            {
                conn.EndDatabase();
            };
        }

        /// <author>Vipul Patel</author>
        /// <created>12-Feb-2020</created>
        /// <summary>
        /// Block/Unblock GetCVM OTP
        /// </summary>
        public static void BlockUnblockGetCVMOTP(int cardID, bool blockUnblock)
        {
            DataController conn = new DataController();
            conn.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                WriteCardData objWrite = new WriteCardData(conn);
                objWrite.BlockUnblockGetCVMOTP(cardID, blockUnblock);
            }
            finally
            {
                conn.EndDatabase();
            };
        }
        ///// <author>Keyur Parekh</author>
        ///// <created>19-Mar-2013</created>
        ///// <summary>
        ///// update Client card status
        ///// </summary>
        ///// <param name="isCardBlocked">Bool</param>
        ///// <param name="cardStatusHistoryID">Int</param>
        ///// <param name="conn">DataController</param>
        //private void updateClientCardStatus(int cardStatusHistoryID, EnumCardStatus cardStatus)
        //{
        //    if (cardStatusHistoryID > 0)
        //    {
        //        StatusChangeServiceCaller callService = new StatusChangeServiceCaller(this);
        //        callService.ChangeCardStatus(cardStatusHistoryID, cardStatus);
        //    }
        //}

        /// <author>Vipul Patel</author>
        /// <created>11-Jan-2019</created>
        /// <summary>
        /// Suspend All Tokens Of Card  
        /// </summary>
        public static void SuspendAllTokensOfCard(int cardDetailID, EnumStatusChangeReasonCode reasonCode)
        {
            DataController conn = new DataController();
            conn.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                WriteCardData objWrite = new WriteCardData(conn);
                objWrite.SuspendAllTokensOfCard(cardDetailID, (int)reasonCode);
            }
            finally
            {
                conn.EndDatabase();
            };

        }

        /// <author>Vipul Patel</author>
        /// <created>11-Jan-2019</created>
        /// <summary>
        /// Suspend All Tokens Of Card  
        /// </summary>
        public static void TokenVerificationFailAction(int cardDetailID, string activationVerificationResult, Int64 tokenRequestorID, string encToken)
        {
            DataController conn = new DataController();
            conn.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                WriteCardData objWrite = new WriteCardData(conn);
                objWrite.TokenVerificationFailAction(cardDetailID, activationVerificationResult, tokenRequestorID, encToken);
            }
            finally
            {
                conn.EndDatabase();
            };

        }

        /// <author>Vipul Patel</author>
        /// <created>03-Mar-2020</created>
        /// <summary>
        /// Delete All Tokens Of Card  
        /// </summary>
        public static void DeleteAllTokensOfCard(int cardDetailID)
        {
            DataController conn = new DataController();
            conn.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                WriteCardData objWrite = new WriteCardData(conn);
                objWrite.DeleteAllTokensOfCard(cardDetailID);
            }
            finally
            {
                conn.EndDatabase();
            };

        }

        /// <author>Vipul Patel</author>
        /// <created>11-Jan-2019</created>
        /// <summary>
        /// Resume All Tokens Of Card  
        /// </summary>
        public static void ResumeAllTokensOfCardSuspendedBySystem(int cardDetailID)
        {
            DataController conn = new DataController();
            conn.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                WriteCardData objWrite = new WriteCardData(conn);
                objWrite.ResumeAllTokensOfCardSuspendedBySystem(cardDetailID);
            }
            finally
            {
                conn.EndDatabase();
            };

        }


        /// <author>Keyur Parekh</author>
        /// <created>19-Mar-2013</created>
        /// <summary>
        /// Update card status history status
        /// </summary>
        /// <param name="cardStatusHisotryID">Int</param>
        /// <param name="status">Status</param>
        /// <param name="conn">DataController</param>
        public static void UpdateCardStatusHistoryProcessStatus(int cardStatusHisotryID, EnumProcessStatuses status)
        {
            DataController conn = new DataController();
            conn.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                WriteCardData objWrite = new WriteCardData(conn);
                objWrite.UpdateCardStatusHistory(cardStatusHisotryID, (int)status);
            }
            finally
            {
                conn.EndDatabase();
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Update Card PVV, when pin is changed
        /// </summary>
        /// <param name="encPAN"></param>
        /// <param name="pvv"></param>
        /// <param name="pinBlock"></param>
        public static void UpdatePVV(string encPAN, string pvv, string pinBlock)
        {
            MasterCardEncrypt objEncrypt = new MasterCardEncrypt();
            string encInfo = objEncrypt.EncryptCardInfoUsingKey(pinBlock);

            SafeDataController con = new SafeDataController();
            con.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                con.BeginTransaction("Update PVV");

                WriteCardData wrCD = new WriteCardData(con);
                wrCD.UpdateCardData(encPAN, pvv);

                wrCD.WriteCardInfo(encPAN, encInfo);
                con.CommitTransaction();

            }
            catch (Exception ex)
            {
                con.RollbackTransaction();
                throw ex;

            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Encrypt Pan
        /// </summary>
        /// <param name="pan"></param>
        /// <returns></returns>
        public static string GetEncryptedPAN(string pan)
        {
            MasterCardEncrypt objEncrypt = new MasterCardEncrypt();
            return objEncrypt.EncryptPANUsingKey(pan);
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Block Card
        /// </summary>
        public void UnblockCard()
        {
            bool flag = false;
            int cardStatusHistoryID = 0;
            SafeDataController conn = new SafeDataController();
            conn.BeginTransaction(CredECardConfig.GetReadWriteConnectionString(), "Unblock Card");

            try
            {
                WriteCardData wrCD = new WriteCardData(conn);
                wrCD.UnblockCard(_encPAN, out cardStatusHistoryID);
                conn.CommitTransaction();
                flag = true;
            }
            catch (Exception ex)
            {
                conn.RollbackTransaction();
                throw ex;
            }

            //if (flag) updateClientCardStatus(cardStatusHistoryID, EnumCardStatus.Normal);
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Block Card
        /// </summary>
        public static void BlockCard(string cardNumber, string CardStatusChangeURL, int CardID)
        {
            int cardStatusHistoryID = 0;
            MasterCardEncrypt objEncrypt = new MasterCardEncrypt();
            string encPan = objEncrypt.EncryptPANUsingKey(cardNumber);
            bool flag = false;
            SafeDataController conn = new SafeDataController();
            conn.BeginTransaction(CredECardConfig.GetReadWriteConnectionString(), "Block Card");

            try
            {
                WriteCardData wrCD = new WriteCardData(conn);
                wrCD.BlockCard(encPan, out cardStatusHistoryID);
                conn.CommitTransaction();
                flag = true;
            }
            catch (Exception ex)
            {
                conn.RollbackTransaction();
                throw ex;
            }

            if (flag)
            {
                CardData objCardData = new CardData();
                objCardData._pan = cardNumber;
                objCardData._cardID = CardID;
                objCardData.CardStatusChangeURL = CardStatusChangeURL;
                //objCardData.updateClientCardStatus(cardStatusHistoryID, EnumCardStatus.PIN_Tries_Exceeded);
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Get Card Info
        /// </summary>
        /// <param name="cardID">Card ID</param>
        /// <returns>Info</returns>
        public static string GetCardInfo(string cardNumber)
        {
            MasterCardEncrypt objEncrypt = new MasterCardEncrypt();
            string encPan = objEncrypt.EncryptPANUsingKey(cardNumber);

            string info = string.Empty;
            string encInfo = ReadCardData.GetCardInfo(encPan);

            if (encInfo.Length > 0)
            {
                MasterCardDecrypt objDecrypt = new MasterCardDecrypt();
                info = objDecrypt.DecryptCardInfoUsingKey(encInfo);
            }


            return info;
        }

        /// <author>Keyur Parekh</author>
        /// <created>17-Apr-2012</created>
        /// <summary>
        /// Reverse Pin Change
        /// </summary>
        /// <param name="encPAN"></param>
        public static void ReversePin(string cardNumber)
        {
            MasterCardEncrypt objEncrypt = new MasterCardEncrypt();
            string encPan = objEncrypt.EncryptPANUsingKey(cardNumber);

            SafeDataController con = new SafeDataController();
            con.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                con.BeginTransaction("Reverse Pin");

                WriteCardData wrCD = new WriteCardData(con);
                wrCD.ReverseCardPVV(encPan);

                wrCD.ReverseCardInfo(encPan);
                con.CommitTransaction();

            }
            catch (Exception ex)
            {
                con.RollbackTransaction();
                throw ex;

            }
        }

        ///// <summary>
        ///// Block Card
        ///// </summary>
        //public void ResetFailedPinAttemptOfCard()
        //{
        //    SafeDataController conn = new SafeDataController();
        //    conn.BeginTransaction(CredECardConfig.GetConnectionString(), "Reset Failed Attempt");

        //    try
        //    {
        //        WriteCardData wrCD = new WriteCardData(conn);
        //        wrCD.ResetFailedPinAttemptOfCard(_encPAN);

        //        conn.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        conn.RollbackTransaction();
        //        throw ex;
        //    }
        //}

        /// <author>Keyur Parekh</author>
        /// <created>18-Apr-2012</created>
        /// <summary>
        /// Check whether current transaction type is valid for for current status of card or not?
        /// </summary>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        public bool IsValidTransactionForCard(EnumISOTransactionType transactionType, string expiryDate, out string errorMsg, bool isTokenTransaction = false)
        {
            bool flag = false;
            EnumCardStatus cardStatus = Status;
            errorMsg = string.Empty;

            if (ExpiryDate.Date < DateTime.Now.Date)
            {
                _statusID = Convert.ToInt16(EnumCardStatus.Card_Expired);
            }
            else if (expiryDate != string.Empty && ExpiryDate.ToString("yyMM") != expiryDate)
            {
                if (cardStatus == EnumCardStatus.Normal || cardStatus == EnumCardStatus.Inactive)// Case :121374 : Denish
                {
                    UpdateInValidExpiryDateCount(this._cardID, _statusID);
                }
                errorMsg = CommonMessage.GetMessage(EnumErrorConstants.INCORRECT_EXPIRY_DATE);
            }
            else
            {
                //All transactions are valid if card status is normal
                if (cardStatus == EnumCardStatus.Normal)
                    flag = true;
                else
                {

                    if (transactionType == EnumISOTransactionType.PIN_Unblock_Prepaid_Activation)
                    {
                        if (cardStatus == EnumCardStatus.PIN_Tries_Exceeded && PinUnBlockAttempt > 0)
                            flag = true;
                    }
                }

                if (isTokenTransaction) flag = true;
            }

            return flag;
        }

        /// <author>Mantan Bhatti</author>
        /// <created>03-Mar-2020</created>
        /// <summary>
        /// Update pinunblock attempt
        /// </summary>
        public static void UpdateInValidExpiryDateCount(int cardID, short currentCardStatusID)
        {
            DataController conn = new DataController();
            conn.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                WriteCardData objWrite = new WriteCardData(conn);
                objWrite.UpdateInValidExpiryDateCount(cardID, currentCardStatusID);
            }
            finally
            {
                conn.EndDatabase();
            };


        }

        /// <author>Vipul patel</author>
        /// <created>06-Mar-2020</created>
        /// <summary>
        /// Notify to cardholder
        /// </summary>
        public static void NotifyTocardHolder(int cardID, EnumNotificationEvents eventID, string remarks)
        {
            DataController conn = new DataController();
            conn.StartDatabase(CredECardConfig.GetReadWriteConnectionString());

            try
            {
                WriteCardData objWrite = new WriteCardData(conn);
                objWrite.NotifyTocardHolder(cardID, (int)eventID, remarks);
            }
            finally
            {
                conn.EndDatabase();
            };
        }

        /// <author>Manthan Bhatti</author>
        /// <created>27-Apr-2020</created>
        /// <summary>
        /// Update Card ATC
        /// </summary>
        public static void UpdateCardATC(int cardID, int ATC, DataController con, int PrevATMPOSTransCurrencyCode = 0)
        {
            WriteCardData wrCD = new WriteCardData(con);
            wrCD.UpdateCardATC(cardID, ATC, PrevATMPOSTransCurrencyCode);
        }
    }
}
