using System.ComponentModel;
using System.Xml.Serialization;

namespace Common.CBGEnums
{
    public enum EnumTransactionType : int
    {
        Unknown = 0,
        [XmlEnum("1"), Description("FPS Credit")]
        FPSCredit = 1,
        [XmlEnum("2"), Description("Withdrawal")]
        Withdrawal = 2,
        [XmlEnum("3"), Description("DirectDebit Payment")]
        DirectDebitPayment = 3,
        [XmlEnum("4"), Description("DirectDebit Return")]
        DirectDebitReturn = 4,
        [XmlEnum("5"), Description("DirectDebit Indemnity Claim")]
        DirectDebitIndemnityClaim = 5,
        [XmlEnum("6"), Description("DirectCredit Return")]
        DirectCreditReturn = 6,
        [XmlEnum("7"), Description("Direct Credit")]
        DirectCredit = 7,
        [XmlEnum("8"), Description("FPS Credit Return")]
        FPSCredit_Return = 8,
    }
}
