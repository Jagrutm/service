namespace CredECard.Common.Enums.Transaction
{
    public enum EnumDueType
    {
        Payment = 1,
        Withdraw = 2
    }

    //public enum EnumFileStatus
    //{
    //    Pending = 1,
    //    Processed = 2,
    //    Cancelled = 3,
    //    Converting = 4,
    //    Converted = 5,
    //    Processing = 6,
    //    Rejected = 7,
    //    Failed = 8,
    //    Downloading = 9,
    //    Downloaded = 10,
    //    ReDownload = 11,
    //    ToDownload = 12,
    //    DownloadAborted = 13,
    //    ProcessFailed = 14,
    //    ReProcess = 15,
    //    PartProcessed = 16,
    //    Importing = 17,
    //    Imported = 18,
    //    Notified = 19,
    //    NotificationNotRequired = 20,
    //    SentOut = 21,
    //    Verified = 22,
    //    Exported = 23,
    //    ToUpload = 24,
    //    Uploading = 25,
    //    Uploaded = 26
    //}

    //public enum EnumTransactionTypeOld
    //{
    //    Unknown = 0,        
    //    Transfer,
    //    Charge,
    //    Withdrawal,
    //    BillPayment,
    //    BillPaymentReversal,
    //    WithdrawalRequest,
    //    WithdrawalReversal,
    //    Credit,
    //    CreditReversal,
    //    CardPurchase,
    //    CardPurchaseReversal,
    //    CardWithdraw,
    //    CardWithdrawReversal,
    //    CardPurchaseRefund,
    //    CardPurchaseRefundReversal,
    //    CardChargebackRefund,
    //    CardChargebackRefundReversal,
    //    CardRepresentment,
    //    CardRepresentmentReversal,
    //    ChargeReversal

    //    //Interest,        
    //    //Refund,
    //    //Administration,
    //    //Return,
    //    //Transfer,
    //    //CashReturn,
    //    //Reversal,
    //    //Withdrawal,
    //    //TransactionFees,
    //    //BillPayment,
    //    //BillPaymentReversal,
    //    //RequestRefund,
    //    //RequestReturn,
    //    //RequestCashReturn,
    //    //RefundCancel,
    //    //ReturnCancel,
    //    //CashReturnCancel,
    //    //WithdrawalRequest,
    //    //InterestRefundRequest,
    //    //InterestRefund,
    //    //InterestRefundCancel,
    //    //WithdrawalReversal,
    //    //Credit,
    //    //CreditReversal,
    //    //CardPurchase,
    //    //CardPurchaseReversal,
    //    //CardWithdraw,
    //    //CardWithdrawReversal,
    //    //CardPurchaseRefund,
    //    //CardPurchaseRefundReversal,
    //    //CardChargebackRefund,
    //    //CardChargebackRefundReversal,
    //    //CardRepresentment,
    //    //CardRepresentmentReversal
    //}

    public enum EnumSMSTransferStatus
    {
        SentConfirmationCode = 1,
        Success = 2,
        Failure = 3
    }

    public enum EnumSMSRequestStatuses
    {
        Active = 1,
        Success = 2,
        Failed = 3,
        Expired = 4
    }


    public enum EnumAutoCardProcessStatus
    {
        None = 0,
        FreeRenew = 1,
        PaidRenew = 2,
        ExpiredAndCloseAccount = 3,
        ExpiredAndNewSchemeFreeCard = 4,
        ExpiredAndNewSchemePaidCard = 5,
        Processed = 6,
        LostReplacement = 7,
        StolenReplacement = 8,
        DamageReIssue = 9,
        CancelCardCloseAccount = 10,
        CancelAndNewSchemeFreeCard = 11
    }

    public enum EnumProcessActions
    {
        None=0,
        Pending=1,
    }

    public enum EnumAccountUpdaterQueueActions
    {
        None = 0,
        Initilize = 1,
        UpdateCard = 2,
        StopResumeAdvice = 3
    }
}
