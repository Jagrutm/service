using System.ComponentModel;
using System.Xml.Serialization;

namespace Common.CBGEnums
{
    public enum EnumTypeOfBusiness
    {
        [Description("None")]
        [XmlEnum("0")]
        None = 0,

        [Description("Sole Trader")]
        [XmlEnum("1")]
        SoleTrader = 1,

        [Description("Partnership")]
        [XmlEnum("2")]
        Partnership = 2,

        [Description("Limited Company")]
        [XmlEnum("3")]
        Limited_Company = 3,

        [Description("Basic Business")]
        [XmlEnum("4")]
        Basic_Business = 4,

        [Description("Advanced Business")]
        [XmlEnum("5")]
        Advanced_Business = 5,

        [Description("Reseller Business")]
        [XmlEnum("6")]
        Reseller_Business = 6
    }

    public enum EnumSignUpType
    {
        NormalSignup = 1,
        BusinessSignup = 2,
        BulkBusinessEmployeeSignup = 3,  //Dharati Metra: Case 29713
        SecondaryLead = 4 //AP : C#154668
    }

    public enum EnumIdentityType
    {
        None = 0,
        UKPassport = 1,
        InternationalPassport = 2,
        DrivingLicence = 3,
        NationalIDCard = 4,
        SocialSecurityNumbers = 5
    }

    public enum EnumCardProgramCategory
    {
        Normal = 1,
        Business = 2
    }

    public enum EnumDatabaseType : short
    {
        Transaction = 1,
        Repository = 2,
        DataWarehouse = 3 //31652 
    }

    public enum EnumBusinessSignupBreadCrumbs
    {
        None = 0,
        AboutBusiness = 1,
        OfficerDetails = 2,
        TermsConditions = 3,
        DisableAll = 4,
    }

    public enum EnumTitle
    {
        [XmlEnum("0"), Description("Select")]
        Select = 0,

        [XmlEnum("1"), Description("Mr")]
        Mr = 1,

        [XmlEnum("2"), Description("Mrs")]
        Mrs = 2,

        [XmlEnum("3"), Description("Miss")]
        Miss = 3,

        [XmlEnum("4"), Description("Ms")]
        Ms = 4,

        [XmlEnum("5"), Description("Dr")]
        Dr = 5,

        [XmlEnum("6"), Description("Prof")]
        Prof = 6,

        [XmlEnum("7"), Description("Rev")]
        Rev = 7,

        [XmlEnum("8"), Description("Sir")]
        Sir = 8,

        [XmlEnum("9"), Description("Other")]
        Other = 9,

        [XmlEnum("10"), Description("Master")]
        Master = 10 //Viral Prajapati : Case 64056

    }

    public enum DirectDebitProviderFileDirection
    {
        None = 0,
        In = 1,
        Out = 2
    }

    public enum LimitActionMode
    {
        NewAgreement = 0,
        AmendAgreement = 1,
        //Closed = 2,
        ProposeAgreement = 3,
        StandardAgreement = 4,
        SwitchAgreement = 5
    }

    public enum EnumValidate
    {
        //33642 : Nirali : 08-FEB-2017

        IsInteger,

        IsLongInteger,

        IsDouble,

        IsDate,

        IsBoolean,

        IsNumeric,

        IsAlphaNumeric,

        IsAlphabets,

        IsAlphaNumericWithSpace,

        IsAplhaNumericWithSpclCharWithSpace,

        IsAplhabetsWithSpclChar,

        IsAplhabetsWithSpclCharWithSpace,

        IsMatchWithFormat,

        None

    }

    public enum EnumPreferences
    {
        GroupLayout = 1
    }

    public enum EnumValueTypes
    {
        AUDDIS_SubmissionSerialNumber = 1,
        ADDACS_SubmissionSerialNumber = 2,
        ARUDD_SubmissionSerialNumber = 3,
        ARUCS_SubmissionSerialNumber = 4,
        AWACS_SubmissionSerialNumber = 5,
        MIDEP_SubmissionSerialNumber = 24,
        LastAutoClientLogReadDateAndValue = 42
    }

    public enum EnumTransactionCodeForBACSFile
    {
        U1 = 01,
        U7 = 17,
        U8 = 18,
        U9 = 19
    }

    public enum EnumCreditTransactionType
    {
        None = 0,
        Paypoint = 1,
        IBAN = 2
    }

    public enum EnumMessageType
    {
        Transfers = 1,
        StandingOrder = 2
    }

    public enum EnumSubAccountPortalAccess
    {
        None = 0,
        SubacccoutViewTransaction = 1,
        SubaccountTransferSO = 2,
        SubacccountManageEnvelopes = 4,
        SubaccountViewTransaction_TransferSO = EnumSubAccountPortalAccess.SubacccoutViewTransaction | EnumSubAccountPortalAccess.SubaccountTransferSO,
        SubaccountViewTransaction_ManageEnvelopes = EnumSubAccountPortalAccess.SubacccoutViewTransaction | EnumSubAccountPortalAccess.SubacccountManageEnvelopes,
        SubaccountTransferSO_ManageEnvelopes = EnumSubAccountPortalAccess.SubaccountTransferSO | EnumSubAccountPortalAccess.SubacccountManageEnvelopes,
        SubaccountAllOptionSelect = EnumSubAccountPortalAccess.SubacccoutViewTransaction | EnumSubAccountPortalAccess.SubacccountManageEnvelopes | EnumSubAccountPortalAccess.SubaccountTransferSO
    }

    public enum EnumMarketSector
    {
        None = 0,
        [XmlEnum("1")]
        IT = 1,
        [XmlEnum("2")]
        Telecom = 2,
        [XmlEnum("3")]
        Pharmaceutical = 3,
        [XmlEnum("4")]
        Construction = 4,
        [XmlEnum("5")]
        Textiles = 5,
        [XmlEnum("6")]
        Cement = 6,
        [XmlEnum("7")]
        Paper = 7
    }

    public enum EnumAgreementLimits
    {
        TotalLoad
        , FlagLoad
        , FlagMonths
        , DailySendMoneyLimit
        , WeeklySpendLimitAmount
        , MonthlySpendLimitAmount
        , TotalSpendLimit
        , DailySpendLimitAmount
        , WeeklySendMoneyLimit
        , MonthlySendMoneyLimit
        , ATMTransactionValueLimit
        , YearlyATMValueLimit
        , YearlyTotalLimit
        , ATMWeeklySpendLimit
        , ATMMonthlySpendLimit
        , CashBackDailyLimit
        , CashBackWeeklyLimit
        , CashBackMonthlyLimit
        , CashBackYearlyLimit
        , BacsFbacsDailyLimit
        , BacsFbacsWeeklyLimit
        , BacsFbacsMonthlyLimit
        , POSTransactionValueLimit
        , POSWeeklyLimit
        , POSMonthlyLimit
        , YearlyPOSValueLimit
        , CardLimit
        , MaxBalance
        , FlagBalance
        , FlagYearlyATM
        , FlagIndividualPOS
        , DailyWorldpayLimit
        , WeeklyWorldpayLimit
        , MonthlyWorldpayLimit
        , LocalATMQualifyingLimit
        , EuropeanATMQualifyingLimit
        , AbroadATMQualifyingLimit
        , LocalPOSQualifyingLimit
        , EuropeanPOSQualifyingLimit
        , AbroadPOSQualifyingLimit
        , WorldpayDailyLoadCount
        , WorldpayWeeklyLoadCount
        , WorldpayMonthlyLoadCount
        , TotalLoadCount
        , TotalSpendCount
        , MonthlyLocalFreeATMCount
        //, MonthlyAbroadFreeATMCount Tejas Choksi 156798
        //, MonthlyEuropeanFreeATMCount Tejas Choksi 156798
        , SamedayFasterPaymentDailyLimit
        , SamedayFasterPaymentWeeklyLimit
        , SamedayFasterPaymentMonthlyLimit
        , SamedayFasterPaymentDailyCount
        , SamedayFasterPaymentWeeklyCount
        , SamedayFasterPaymentMonthlyCount
        , DailyIBANLimit
        , WeeklyIBANLimit
        , MonthlyIBANLimit
        , DailyIBANLoadCount
        , WeeklyIBANLoadCount
        , MonthlyIBANLoadCount
        , ECommDailyLimit//C:37681:Anita Chande
        , ECommWeeklyLimit//C:37681:Anita Chande
        , ECommMonthlyLimit//C:37681:Anita Chande
        , ECommYearlyLimit//C:37681:Anita Chande
        , ContactlessLimit //C:39295:Rikunj
        , IsIncludeCardChargesInSpendLimits
        , IsClientAuthorization
        , ClientAuthorizationDailyCount
        , ClientAuthorizationWeeklyCount
        , ClientAuthorizationMonthlyCount
        , ClientAuthorizationYearlyCount
        , ClientAuthorizationDailyLimit
        , ClientAuthorizationWeeklyLimit
        , ClientAuthorizationMonthlyLimit
        , ClientAuthorizationYearlyLimit
        , [Description("Daily Bank Withdrawal Limit")]
        DailyBankWithdrawalLimit
        , [Description("Weekly Bank Withdrawal Limit")]
        WeeklyBankWithdrawalLimit
        , [Description("Monthly Bank Withdrawal Limit")]
        MonthlyBankWithdrawalLimit
        , [Description("Daily Bank Withdrawal Count")]
        BankWithdrawalDailyCount
        , [System.ComponentModel.Description("Weekly Bank Withdrawal Count")]
        BankWithdrawalWeeklyCount
        , [System.ComponentModel.Description("Monthly Bank Withdrawal Count")]
        BankWithdrawalMonthlyCount
        , DailyAFDCount //Binal : 104703
        , DailyAFDLimit //Binal : 104703
        , WeeklyAFDCount //Binal : 104703
        , WeeklyAFDLimit //Binal : 104703
        , PostOfficeCashWithdrawQualifyingLimit // 128065
    }

    public enum EnumCrossSchemeConfigurationType
    {
        None = 0,
        Transfer = 1
    }

    public enum EnumPackageType
    {
        [XmlEnum("1")]
        All = 1,
        [XmlEnum("2")]
        Available = 2,
        [XmlEnum("3")]
        Associated = 3
    }

    public enum EnumWizzit
    {
        None = 0,
        pin_set = 1,
        single_entry = 2
    }

    public enum EnumTokenTypes : byte
    {
        None = 0,
        Token_Global = 1,
        Token_2FALogin = 2,
        Token_ForgotPassword = 3,
        Token_PSD2Consent = 4
    }

    public enum EnumSchemeNotificationCategoryAdditionalFields  //Case 136216
    {
        None = 0,
        FeatureCode = 1     //i.e. CardFeatureCode
    }
    
    public enum EnumLogoutReasons
    {
        /// <summary>
        /// None
        /// </summary>
        [XmlEnum("0"), Description("None")]
        None = 0,

        /// <summary>
        /// Manually Logout
        /// </summary>
        [XmlEnum("1"), Description("Manually logout")]
        ManuallyLogout = 1,

        /// <summary>
        /// Session Timeout
        /// </summary>
        [XmlEnum("2"), Description("Session timeout")]
        SessionTimeout = 2,

        /// <summary>
        /// Logged Into Another Device
        /// </summary>
        [XmlEnum("3"), Description("Logged in to another device")]
        LoggedIntoAnotherDevice = 3,

        /// <summary>
        /// Lost Connection
        /// </summary>
        [XmlEnum("4"), Description("Lost connection")]
        LostConnection = 4,

        /// <summary>
        /// Force Close The App
        /// </summary>
        [XmlEnum("5"), Description("Force close the app")]
        ForceCloseTheApp = 5,

        /// <summary>
        /// Login Disabled
        /// </summary>
        [XmlEnum("6"), Description("Login disabled")]
        LoginDisabled = 6,
    }

    public enum EnumSecurityNumberActionType
    {
        None = 0,
        [XmlEnum("1"), Description("Login")]
        Login = 1,
        [XmlEnum("2"), Description("Change Security Number")]
        ChangeSecurityNumber = 2,
        [XmlEnum("3"), Description("Change Password")]
        ChangePassword = 3,
        [XmlEnum("4"), Description("Forgot Password")]
        ForgotPasswordConfirm = 4,
        [XmlEnum("5"), Description("Standing Order")]
        StandingOrderVerify = 5,
        [XmlEnum("6"), Description("Bank Transfer")]
        BankTransferVerify = 6,
        [XmlEnum("7"), Description("Local Transfer")]
        LocalTransferVerify = 7,
        [XmlEnum("8"), Description("International Transfer")]
        InternationalTransferVerify = 8,
        [XmlEnum("9"), Description("International Currency Cloud Transfer")]
        InternationalCCTransferVerify = 9,
        [XmlEnum("10"), Description("Bulk File Upload")]
        BulkFileUpload = 10,
        Validate_Reset_Pin = 11
    }

    public enum EnumSystemDocumentType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Privacy Policy
        /// </summary>
        [XmlEnum("1"), Description("Privacy Policy")]
        Privacy_Policy = 1,

        /// <summary>
        /// User Guide
        /// </summary>
        [XmlEnum("2"), Description("User Guide")]
        User_Guide = 2,

        /// <summary>
        /// Terms and Conditions
        /// </summary>
        [XmlEnum("3"), Description("Terms and Conditions")]
        Terms_Conditions = 3
    }

}
