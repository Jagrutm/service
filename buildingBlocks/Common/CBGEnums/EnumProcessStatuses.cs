using System.ComponentModel;
using System.Xml.Serialization;

namespace Common.CBGEnums
{
    public enum EnumProcessStatuses : byte
    {
        None = 0,
        Pending = 1,
        Processing = 2,
        ProcessedWithError = 3,
        Processed = 4,
        NotificationNotRequired = 5,
        Notified = 6,
        Failed = 7,
        Cancelled = 8,
        ToBeProcess = 9,
        Cleared = 10,
        Hold = 11,
        Requested = 12,
        OnHoldbecauseofduplicate = 13,
        ToValidate = 14,
        Raised = 15,
        Reported = 16,
        Accepted = 17,
        Warning = 18,
        Rejected = 19,
        Deleted = 20,
        ReRequested = 21,
        ResumeRequested = 22,
        Resumed = 23,
        ProcessingNotRequired = 24,
        Requesting = 25,
        Expired = 26
    }

    //public enum EnumReasonCodes
    //{
    //    //None = 0,
    //    //[XmlEnum("1"), Description("0")]
    //    //Code_0 = 1,
    //    [XmlEnum("8"), Description("C")]
    //    C = 8,
    //    [XmlEnum("10"), Description("E")]
    //    E = 10,
    //    [XmlEnum("7"), Description("B")]
    //    B = 7

    //}

    ///// <summary>
    ///// 
    ///// </summary>
    //public enum enumPaymentMethods
    //{
    //    BACS = 1,
    //    FasterPayments = 2
    //}
}
