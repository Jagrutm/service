using System.ComponentModel;
using System.Xml.Serialization;

namespace Common.CBGEnums
{
    /// <summary>
    /// Account Type Enum
    /// </summary>
    public enum EnumAccountType : byte
    {
        [XmlEnum("1"), Description("Unknown")]
        None = 0,

        [XmlEnum("1"), Description("Customer")]
        Customer = 1,

        [XmlEnum("2"), Description("Institutional")]
        Institutional = 2, //Institutional holding accounts

        [XmlEnum("3"), Description("Holding")]
        Holding = 3
    }

    public enum HoldingTypes
    {
        Withdrawal = 1,
        Credit = 2,
        InstitutionWithdrawal = 3,
        InstitutionCredit = 4
    }
}
