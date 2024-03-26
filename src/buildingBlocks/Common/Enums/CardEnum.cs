
namespace CredECard.Common.Enums.Card
{
    using System.ComponentModel;
   
    //Mehul Patel : 27 Nov 2006 : Mastreo- CredECardFileStatus addition
    /// <summary>
    /// credecard maestro file status enum
    /// </summary>
    public enum EnumCardFileStatuses
    {
        Pending = 1,
        Requested = 2,
        Issued = 3,
        Exception = 4
    }

    /// <summary>
    /// maestro card action enum
    /// </summary>
    public enum EnumCardAction
    {
        None = 0,
        New_Card = 1,
        Value_ReLoad = 2,
        Replacement = 3,
        ReIssue = 4,
        PIN_Replacement = 5,
        Amend_Card_Details = 6,
        StatusChange = 7,
        Expire = 8,
        PIN_Reminder = 9,
        Totals = 10
    }

    /// <summary>
    /// maestro card status enum
    /// </summary>
    public enum EnumCardStatus
    {
        [Description("")]
        None = 0,
        [Description("00")]
        Normal=1,
        [Description("01")]
        PIN_Tries_Exceeded=2,
        [Description("02")]
        Inactive = 3,
        [Description("03")]
        Card_Expired = 4,
        [Description("04")]
        Lost = 5,
        [Description("05")]
        Stolen = 6,
        [Description("06")]
        Cardholder_Closed = 7,
        [Description("07")]
        Issuer_Cancelled = 8,
        [Description("08")]
        Fraudulent_use = 9,
        [Description("77")]
        Block = 10,
        [Description("86")]
        Restricted_Card = 11,
        [Description("78")]
        Refer_To_issuer = 12,
        [Description("84")]
        Delinquent = 13,
        [Description("09")]
        Dormant = 14,
        [Description("89")]
        Pick_up = 15,
        [Description("10")]
        Pending_Breakage = 16,
        [Description("11")]
        Pending_Closure = 17,
        [Description("60")]
        Suspend = 18,
        [Description("61")]
        Card_Returned = 19,
        [Description("62")]
        Watch = 20,
        [Description("63")]
        Other_Returned = 21,
        [Description("64")]
        Fraud = 22,
        [Description("65")]
        Bankrupt = 23,
        [Description("66")]
        Cancelled = 24,
        [Description("67")]
        No_Renewal = 25,
        [Description("68")]
        Deceased = 26,
        [Description("69")]
        Damaged = 27,
        [Description("70")]
        Requested = 28,
        [Description("90")]
        CVV2_tries_exceeded = 29
    }

    /// <summary>
    /// maestro transaction code enum
    /// </summary>
    public enum EnumMaestroTransactionCodes
    {
        GoodsAndService = 0,
        CashWithdrawal = 1,
        PurchaseWithCashBack = 9,
        PurchaseRefund = 20
    }

    /// <summary>
    /// maestro card pin status
    /// </summary>
    public enum EnumMaestroCardPinStatuses
    {
        Pending = 1,
        Requested = 2,
        Issued = 3
    }

    /// <summary>
    /// maestro card event enum
    /// </summary>
    public enum EnumMaestroCardEvent
    {
        New_card = 1,
        Renew = 2,
        Replacement = 3,
        Reissue = 4,
        New_PIN = 5,
        PIN_Replacement = 6,
        StatusChange = 7,
        Expire = 8,
        PIN_Reminder = 9,
        Totals = 10
    }

    /// <author>Mantan Bhatti</author>
    /// <created>04-Mar-2020</created>
    /// <summary>
    /// Fraudulent Reasons code enum
    /// </summary>
    public enum EnumStatusChangeReasonCode
    {
          NONE = 0
        , BLOCKED_BY_CARD_HOLDER = 1
        , TEMP_BLOCKED_BY_SYSTEM_SUSPECTED_FRAUD = 2
        , BLOCKED_BY_SYSTEM_SUSPECTED_FRAUD = 3
        , BLOCKED_MANUALLY_SUSPECTED_FRAUD = 4
        , BLOCKED_MANUALLY_ON_CARD_HOLDER_REQUEST = 5       
        , BLOCKED_BY_SYSTEM_TO_PREVENT_FRAUD = 6
    }

    public enum EnumNotificationEvents
    {
        Card_Activation_Failed = 1
        ,PIN_Retrival_Failed = 2
        ,PIN_Change_Failed = 3
        ,Purchase_Failed = 4
        ,Withdrawal_Failed = 5 
        ,Card_Provisioning_Failed = 6
        ,Card_Shipment_Notification = 7//Manthan Bhatti : Case 107186
        ,Token_Device_Binding_Removed = 8//Niken Sah Case 128147
        ,Cross_Border_FX_Rate_Notification = 9 // Niken Shah Case 139421
    }
}
