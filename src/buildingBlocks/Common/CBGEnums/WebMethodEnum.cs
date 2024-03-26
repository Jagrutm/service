using System.ComponentModel;

namespace Common.CBGEnums
{
    public enum EnumWebMethodCategory : long
    {
        None = 0
        , Account = 1 << 0
        , AccountOnly = 1 << 1
        , AdvancedCompany = 1 << 2
        , BasicCompany = 1 << 3
        , Business = 1 << 4
        , Card = 1 << 5
        , Communication = 1 << 6
        , Consumer = 1 << 7
        , Department = 1 << 8
        , DirectDebit = 1 << 9
        , Director = 1 << 10
        , Envelope = 1 << 11
        , Internal = 1 << 12
        , JuniorConsumer = 1 << 13
        , LimitedCompany = 1 << 14
        , MCCControl = 1 << 15
        , P2P = 1 << 16
        , Partner = 1 << 17
        , Scheme = 1 << 18
        , Security = 1 << 19
        , SoleTrader = 1 << 20
        , SSO = 1 << 21
        , StandingOrder = 1 << 22
        , Transfer = 1 << 23
        , Virtual = 1 << 24
        , Mobile = 1 << 25
        , ConsumerSecurity = 1 << 26 // Case : 70179 : Manthan Bhatti
        , QR = 1 << 27 // Case : 67303 Aarti Meswania
        , SavingEnvelope = 1 << 28
        , DualAuthorisation = 1 << 29  //Dharati Metra: Case 67302
        , KYCDigital = 1 << 30 //Binal : 70341
        , Bonus = ((long)1) << 31 //Mahesh Vala : 86659
        , Consent = ((long)1) << 32 // RS#110192
        , XPay = ((long)1) << 33 // Niken Shah Case 127349        
        , Dashboard = ((long)1) << 34   // Pulin Case 129651/129956
        , OrdoPay = ((long)1) << 35//Denish Makwana : 122977
        , Client = ((long)1) << 37 //AP : C#154668
        , ProgramManagement = ((long)1) << 38 //VS#154398
        , Tpp = ((long)1) << 39 //RS#153959
        , SCA = ((long)1) << 40 //BP#155605
    }

    public enum EnumWebMethod
    {
        None = 0,
        LOGIN = 1,
        LOGOUT = 2,

        //User Controller
        LIST_USER_ROLES = 3,
        SAVE_USER_ROLE = 4,
        GET_USER = 5,
        CREATE_USER = 6,
        CHANGE_PASSWORD = 7, // User Change Password 
        UPDATE_USER = 8, // User update name and status 

        // Role Controller
        LIST_ROLE = 9,
        LIST_ROLE_PRIVILEGE = 10,
        SAVE_ROLE_PRIVILEGE = 11,

        //Payment Controller
        POST_PAYMENT = 12, //Post FPS Outbound Payment
        GET_UNPAID_TRANSACTIONS_LIST_ACCOUNTIDENTIFIER = 13, //GetUnpaidTransactionByAccountIdentifier
        GET_UNPAID_TRANSACTIONS_SORTCODE = 14, //GetTransactionsListBySortCode
        GET_COLLECTED_PAYMENT = 15, //GetCollectedPayments
        GET_INBOUND_TRANSACTIONS_RETURN_ACCOUNTIDENTIFIER = 16, // InboundTransactionsByAccountIdentifier
        GET_INBOUND_TRANSACTIONS_RETURN_SORTCODE = 17, // InboundTransactionsBySortCode
        CREATE_FPS_INBOUND_RETURN = 42,

        //Institution Controller
        LIST_INSTITUTION = 18, // Get list of institutions
        GET_SPECIFIC_INSTITUTION = 19,
        SAVE_INSTITUTION = 20,
        GET_INSTITUTION_SORTCODE = 21, // Get institution by sortcode
        UPDATE_INSTITUTION = 22,
        GET_INSTITUTION_API_METHODS = 23,
        UPDATE_INSTITUTION_API_METHOD = 24,

        // BACS Awacs
        SAVE_AWACS = 25, //Save BACS Awacs

        //Account Controller
        LIST_ACCOUNT_MANDATES = 26, // Fetch Account's DD mandates
        SAVE_MANDATES = 27,
        AMMED_ACCOUNTIDENTIFIER_MANDATES_MANDATEIDENTIFIER = 28,  // Amend DD mandate
        CANCEL_ACCOUNTIDENTIFIER_MANDATES_MANDATEIDENTIFIER = 29, // Cancel DD mandate
        ACCOUNTIDENTIFIER_MANDATES_MANDATEIDENTIFIER_RETURNS = 30, //Return DD Mandate
        ACCOUNTIDENTIFIER_DIRECTDEBIT_RETURNS = 31, // Return DD Payments
        ACCOUNTIDENTIFIER_DIRECTCREDIT_RETURNS = 32, // Return DirectCredit Payments
        HOLDING_ACCOUNT_LIST = 33, // GetHoldingAccountList
        SPECIFIC_ACCOUNT_ACCOUNTIDENTIFIER = 34, // GetSpecificAccount 
        CREATE_ACCOUNT = 35,
        UPDATE_ACCOUNT_ACCOUNTIDENTIFIER = 36,
        DELETE_ACCOUNT_ACCOUNTIDENTIFIER = 37,
        ACCOUNT_ACCOUNTIDENTIFIER_TRANSACTIONS = 38, //GetTransactionByAccountIdentifier
        ACCOUNTIDENTIFIER_TRANSACTIONS_TRANSACTIONIDENTIIFER = 39,//GetTransactionTransactionIdentifier
        LIST_HOLIDAY = 40, // Get list of Holidays
        SAVE_HOLIDAY = 41, // Add holiday
        CREATE_BACS_DIRECTCREDIT_RECALL = 43, // Creates BACS DirectCredit Recall   
        DELETE_HOLIDAY = 44,
        INSTITUTION_WEBHOOK_LIST = 45,
        INSTITUTION_WEBHOOK_UPDATE = 46,
        INSTITUTION_CERTIFICATE_SAVE = 47,
        INSTITUTION_CERTIFICATE_REVOKE= 48,
        INSTITUTION_CERTIFICATE_LIST = 49,
        INSTITUTION_CERTIFICATE_DOWNLOAD = 50,
        INSTITUTION_CERTIFICATE_ACTIVATE = 51,
        GET_SPECIFIC_MANDATE = 52,
        LIST_INSTITUTION_USERS = 53,
        LIST_STAFF_USERS = 54,
        LIST_COUNTRY = 55,
        GET_DIRECT_CREDIT_PAYMENTS = 56,
        SET_SECURITY_PIN = 57,
        VERIFY_SECURITY_PIN = 58,
        LIST_INSTITUTE_MANDATES = 59,
        LIST_DD_PAYMENT_RETURNS = 60,        
        GET_BACS_PENDING_PAYMENTS = 61,
        LIST_DC_PAYMENT_RETURNS = 62,
        BUG_REPORT = 63
    }

    public enum EnumSchemeWebMethod
    {
        None = 0,
        BALTIC_AUTHENTICATE_USER = 1,
        BALTIC_ENROL_MEMBER = 2,
        BALTIC_RETRIEVE_REWARD_NUMBER = 3,
        BALTIC_COMARCH_RETRIEVE_CUSTOMER = 4,
        BALTIC_COMARCH_ENROLL_CUSTOMER = 5
    }

    public enum EnumSchemeWebMethodResult
    {
        Success = 1,
        CUSTOMER_PROFILE_COMPANYCODEMANDATORY = 2,	//Company Code Mandatory
        PROGRAM_MEMBER_DUPLICATEMEMBERPROFILESEXISTSASPERALGORITHM = 3,
        COMMON_MASTER_COUNTRY_EXCEPTION_COUNTRYCODEINVALIDEXCEPTION = 4,
        COMMON_MASTER_LANGUAGE_EXCEPTION_LANGUAGECODEINVALIDEXCEPTION = 5,
        PROGRAM_MEMBER_INVALIDREFERENCEMEMBER = 6,
        PROGRAM_MEMBER_STOCKNOTAVAILABLE = 7,
        PROGRAM_TIER_TIERNOTAVAILABLE = 8,
        PROGRAM_MEMBER_ENROLMENTSOURCECODEMISSING = 9,
        PROGRAM_MEMBER_INVALIDMEMBERSHIPTYPEFORPROGRAM = 10,
        PROGRAM_MEMBER_INDIVIDUALGIVENNAMEMISSING = 11,
        PROGRAM_MEMBER_INDIVIDUALFAMILYNAMEMISSING = 12,
        PROGRAM_MEMBER_INDIVIDUALPREFERREDLANGUAGEMISSING = 13,
        PROGRAM_MEMBER_MEMBERTITLEMISSING = 14,
        PROGRAM_MEMBER_MEMBERTITLEINVALID = 15,
        PROGRAM_MEMBER_MEMBERGENDERMISSING = 16,
        PROGRAM_MEMBER_INVALIDMEMBERGENDER = 17,
        PROGRAM_MEMBER_MARITALSTATUSINVALID = 18,
        PROGRAM_MEMBER_COUNTRYOFRESIDENCEMISSING = 19,
        PROGRAM_MEMBER_MEMBERNATIONALITYINVALID = 20,
        PROGRAM_MEMBER_INDUSTRYTYPEINVALID = 21,
        PROGRAM_MEMBER_INCOMEBANDINVALID = 22,
        PROGRAM_MEMBER_INVALIDINDIVIDUALEMAILADDRESS = 23,
        PROGRAM_MEMBER_DATEOFBIRTHMANDATORY = 24,
        PROGRAM_MEMBER_INVALIDDATEOFBIRTH = 25,
        PROGRAM_MEMBER_PREFERENCECODEINVALID = 26,
        PROGRAM_MEMBER_CORPORATEBUSINESSADDRESSDATAMISSING = 27,
        PROGRAM_MEMBER_INDIVIDUALCONTACTINFOCOUNTRYINVALID = 28,
        PROGRAM_MEMBER_DYNAMICATTRIBUTECODEINVALID = 29,
        PROGRAM_MEMBER_ATTRIBUTETYPEINVALID = 30,
        PROGRAM_MEMBER_DYNAMICATTRIBUTEVALUEMISSING = 31,
        PROGRAM_MEMBER_ENROLLMENTSOURCEMANDATORY = 32,
        PROGRAM_MEMBER_PREFERENCETYPEINVALID = 33,
        PROGRAM_MEMBER_PARTNERDOESNOTEXIST = 34,
        PROGRAM_MEMBER_COMPANYCODEMANDATORY = 35,
        PROGRAM_MEMBER_PROGRAMCODEMANDATORY = 36,
        PROGRAM_MEMBER_MEMBERSHIPSTATUSMISSING = 37,
        PROGRAM_MEMBER_MEMBERSHIPNUMBERMANDATORY = 38,
        PROGRAM_MEMBER_CUSTOMERNUMBERMANDATORYFORUPDATE = 39,
        PROGRAM_MEMBER_INVALIDACCOUNTPERIODTYPE = 40,
        PROGRAM_MEMBER_ACCOUNTPERIODCANNOTBEZEROFORLIMITED = 41,
        PROGRAM_MEMBER_PREFEREDADDRESSINVALID = 42,
        PROGRAM_MEMBER_CONTACTADDRESSTYPEINVALID = 43,
        PROGRAM_MEMBER_MEMBERCONTACTINFODUPLICATEEXISTS = 44,
        PROGRAM_MEMBER_PREFEREDADDRESSCONTACTINFOMISSING = 45,
        PROGRAM_MEMBER_CORPORATEPREFFEREDLANGUAGEMISSING = 46,
        PROGRAM_MEMBER_PROGRAMPREFERENCEOPTIONVALUEINVALID = 47,
        PROGRAM_MEMBER_DUPLICATEPREFERENCEEXISTS = 48,
        PROGRAM_MEMBER_CUSTOMERPREFERENCEOPTIONVALUEINVALID = 49,
        PROGRAM_MEMBER_MANDATORYPREFERENCESMISSING = 50,
        CUSTOMER_COMMON_MANDATORYPREFERENCESMISSING = 51,
        PROGRAM_MEMBER_DUPLICATEDYNAMICATTRIBUTEEXISTS = 52,
        PROGRAM_MEMBER_DUPLICATEMEMBERPROFILESEXIST = 53,
        CUSTOMER_COMMON_MANDATORYDYNAMICATTRIBUTESMISSING = 54,
        PROGRAM_MEMBER_PROGRAMDYNAMICATTRIBUTESNOTCONFIGURED = 55,
        PROGRAM_MEMBER_MANDATORYDYNAMICATTRIBUTESMISSING = 56,
        PROGRAM_MEMBER_CUSTOMERPREFERENCESNOTCONFIGURED = 57,
        PROGRAM_MEMBER_PROGRAMPREFERENCESNOTCONFIGURED = 58,
        PROGRAM_MEMBER_PREFERENCEVALUEMISSING = 59,
        PROGRAM_MEMBER_INVALIDENROLLMENTDATE = 60,
        PROGRAM_MEMBER_ENROLLMENTDATEGREATERTHANCURRENTDATE = 61,
        PROGRAM_MEMBER_DATEOFBIRTHGREATERTHANCURRENTDATE = 62,
        PROGRAM_MEMBER_DATEOFBIRTHLESSTHANCURRENTDATE = 63,
        PROGRAM_MEMBER_TITLEGENDERMISMATCH = 64,
        PROGRAM_MEMBER_ENROLLMENTDATELESSER = 65,
        PROGRAM_MEMBER_INVALIDPROGRAMCODE = 66,
        PROGRAM_MEMBER_ENROLLMENTSOURCEINVALID = 67,
        PROGRAM_MEMBER_ACCOUNTSTATUSMISSING = 68,
        PROGRAM_MEMBER_ACCOUNTSTATUSINVALID = 69,
        PROGRAM_MEMBER_MEMBERSHIPSTATUSINVALID = 70,
        PROGRAM_MEMBER_REFERENCETYPEINVALID = 71,
        PROGRAM_MEMBER_REFERENCEDETAILSMISSING = 72,
        PROGRAM_MEMBER_PARTNERCODEMISSING = 73,
        PROGRAM_MEMBER_INVALIDENROLLMENTSOURCECODE = 74,
        PROGRAM_COMMON_MEMBERSHIPTYPENOTAVAILABLE = 75,
        PROGRAM_MEMBER_CORPORATECOMPANYNAMEMISSING = 76,
        PROGRAM_MEMBER_INVALIDCONTACTEMAILADDRESS = 77,
        PROGRAM_MEMBER_INVALIDCORPORATEEMAILADDRESS = 78,
        PROGRAM_MEMBER_CORPORATECONTACTINFOCOUNTRYINVALID = 79,
        PROGRAM_MEMBER_CORPORATEBUSINESSADDRESSMISSING = 80,
        PROGRAM_MEMBER_NUMBEROFEMPLOYESINVALID = 81,
        PROGRAM_MEMBER_LIMITEDACCOUNTGIVENFORUNLIMITEDPROGRAM = 82,
        PROGRAM_MEMBER_INVALIDACCOUNTPERIOD = 83,
        PROGRAM_MEMBER_INAVALIDMEMBERSHIPNUMBERFORMAT = 84,
        PROGRAM_MEMBER_UNLUCKYMEMBERSHIPNUMBER = 85,




        CUSTOMER_PROFILE_CUSTOMERDOESNOTEXISTEXCEPTION = 998,	//Customer Does not exist exception
        Failed = 999, //General Failed

        /*******Comarch Common Error Start***************************************/

        COMARCH_INVALID_PARAMS = 10001,                             //	Invalid parameter, does not match to required
        COMARCH_MISSING_MANDATORY_PARAMETER = 10002,                //	Missing parameter value that was mandatory. <parameter name> contains name of parameter that caused the problem.
        COMARCH_TRANSACTION_WITH_GIVEN_ID_DOES_NOT_EXIST = 10003,   //	For cancel or accept operation, thrown if given transaction ID does not exist in the system.
        COMARCH_INVALID_TRANSACTION_STATE = 10004,                  //  Only pending transactions can be cancelled.	Cancel operation can be invoked only on a transactions with a proper state.
        COMARCH_LOYALTY_CARD_DOES_NOT_EXIST = 10005,                //	When operation was performed on non-existing Loyalty card number.
        COMARCH_OPERATION_IS_PROHIBITED_FOR_THE_ACCOUNT = 10006,    //	Operation cannot be invoked on account with invalid status, for example on a closed account.
        COMARCH_INSUFFICIENT_BALANCE_ON_THE_ACCOUNT_TO_PERFORM_THE_OPERATION = 10007,//	There are not enough points on the account to perform the operation
        COMARCH_UNAUTHORIZED_CALL_OF_THE_WEB_SERVICE = 10008,       //	Occurs when caller ID sent in UserContext is not valid.
        COMARCH_IMAGE_WITH_SPECIFIED_CODE_DOES_NOT_EXISTS = 10009,  //	Occurs when image with specified code does not exists.
        COMARCH_DUPLICATE_CARD_NUMBER = 10010,                      //	Account already exist
        COMARCH_INVALID_USER_OR_PASSWORD = 10012,                   //	User didn’t pass authentication.
        COMARCH_ACCOUNT_NOT_FOUND = 10013,                          //	Account that was requested does not exist in CLM
        COMARCH_UNEXPECTED_ERROR = 10100,                           //	Any other error not listed here

        /*******Comarch Common Error End***************************************/
    }

    ///<author>Binal Prajapati</author>
    ///<created>24-Sep-2018</created>
    ///<summary>
    /// Enum Values for web method access
    ///</summary>
    public enum EnumWebMethodAccess
    {
        AccessDenied = 0,
        FreeAccess = 1,
        ChargeableAccess = 2
    }
}
