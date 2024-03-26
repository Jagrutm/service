using System.ComponentModel;
using System.Xml.Serialization;

/// <summary>
/// 
/// </summary>
namespace Common.CBGEnums
{
    public enum EnumAccountStatus : byte
    {
        None = 0,
        [XmlEnum("1"), Description("Active")]
        Active = 1,
        [XmlEnum("2"), Description("Inactive")]
        Inactive = 2,
        [XmlEnum("3"), Description("Closed")]
        Closed = 3
    }

    //public enum EnumUserAccountType
    //{
    //    None = 0,
    //    [XmlEnum("1"), Description("Virtual")]
    //    Virtual = 1,
    //    [XmlEnum("2"), Description("Personal Topup")]
    //    PersonalTopup = 2,
    //    [XmlEnum("3"), Description("Business")]
    //    Business = 3,//25475 renamed
    //    [XmlEnum("4"), Description("Merchant Business")]
    //    MerchantBusiness = 4,
    //    [XmlEnum("6"), Description("Pay On Order")]
    //    PayOnOrder = 6,
    //    [XmlEnum("7"), Description("Junior")]
    //    Junior = 7,
    //    [XmlEnum("8"), Description("Nominal Account")]
    //    NominalAccount = 8,
    //    [XmlEnum("9"), Description("Direct Debit")]
    //    DirectDebit = 9
    //}

    public enum EnumAlertActions
    {
        SMSPurchaseOverAmount = 1,
        SMSBalanceOnWithdraw = 2,
        SMSOnPayments = 3,
        SMSPurchasePaymentOverAmount = 4,
        SMSOnLowBalance = 5,
        SMSEnvelope = 6,
        DebitNotification = 7 //Darshit Desai :  37086 - DAF V0863 - Additional SMS and Email triggers (For push notifications) - Mobile Development
        , BankTransferRejection = 8
        , RequestMoneyNotification = 9 // case : 73265 : Denish Makwana
        , CrossBorderFXRateNotification = 10 // Niken Shah Case 139421
    }

    public enum EnumAccountTotalType
    {
        Payment = 1,
        Return = 2,
        Refund = 3,
        Charge = 4,
        Interest = 5,
        Purchase = 6,
        Withdrawal = 7,
        TransactionFees = 8,
        CashReturn = 9,
        RefundCancel = 10,
        ReturnCancel = 11,
        CashReturnCancel = 12,
        WithdrawRequest = 13,
        InterestRefund = 14,
        InterestRefundCancel = 15,
        BillPayment = 16
    }

    public enum EnumHoldingType
    {
        MainAccount = 1
        , Investment = 2
        , DebtAccount = 3
        , Interest = 4
        , Refund = 5
        , Cash = 6
        , UnallocatedCash = 7
        , Return = 8
        , Withdrawal = 9
        , SendReceiveMoney = 10
        , ChargebackLossAccount = 11
        , BACSTopup = 12
        , AdministrationCharge = 13
        , TransactionFee = 14
        , WithdrawalCharge = 15
        , AdditionalCharge = 16
        , TransferFee = 17
        , MaestroTransactionCharge = 18
        , TopupCharge = 19
        , SendReceiveCharge = 20
        , MaestrocardIssueReissueCharge = 21
        , MaestroPINReissueCharge = 22
        , SMScharge = 23
        , InternalCredits = 24
        , MonthlyFees
        , WithdrawalAgency = 27//case 27787 Aarti Meswania - equvivalent to 9
        , CashAgency = 28//case 27787 Aarti Meswania - equvivalent to 6
        , CommissionPayment = 62//case 25060 Aarti Meswania

        , DDICControlAccount = 151  //Dharati Metra: Case 28582
        , DDICLossAccount = 152  //Dharati Metra: Case 28582
        , DDICBACSAccount = 153    //Dharati Metra: Case 28582

        , Post_Office_Settlement_Account_GBP = 300 //128890 // Incremented series as environment entries are different.
    }

    public enum EnumRelationship
    {
        None = 0,
        [XmlEnum("1")]
        Self = 1,
        [XmlEnum("2")]
        Family = 2,
        [XmlEnum("3")]
        Friend = 3,
        [XmlEnum("4")]
        Colleague = 4,
        Managed = 5 //Mahesh Vala : Case 31745
    }

    public enum EnumSuspenseType
    {
        SchemeSuspense = 1
        , SchemeSuspenseRevenue = 2
        , LoyaltyRevenue = 3
        , CardCreditSuspense = 4
        , WorldpaySuspense = 5
        , CardCreditToCloseAccount = 8 // Krishnan Prajapati : CASE: 117599
    }

    public enum EnumCustomerType
    {
        Junior = 1 //Mayur : Case 22476
    }

    public enum EnumAccountClosureReason
    {
        ACCOUNT_HOLDER_REQUEST = 447,
        AUTOMATED_CLOSURE = 448,
        FRAUD = 449,
        MANUAL_CLOSURE_BY_TECH_SUPPORT_TEAM = 450
    }


    public enum EnumAccountCreationStatus
    {
        [Description("000")]
        Created = 0,
        [Description("999")]
        CreationFailed = 999
    }

    //public enum EnumAccountType
    //{
    //    None = 0,
    //    Customer = 1,
    //    Nominal = 2,
    //    Holding = 3
    //}
}
