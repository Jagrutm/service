using System.ComponentModel;
using System.Xml.Serialization;

namespace Common.CBGEnums
{
    public enum EnumBACSRecordTypeCodes
    {
        [XmlEnum("1"), Description("ADDACS")]
        A = 1,
        [XmlEnum("2"), Description("AUDDIS_Return_of_0C")]
        D = 2,
        [XmlEnum("3"), Description("AUDDIS_Return_of_0N")]
        N = 3,
        [XmlEnum("4"), Description("AUDDIS_Return_of_0S")]
        S = 4,
        [XmlEnum("5"), Description("AWACS")]
        W = 5,
        [XmlEnum("6"), Description("ToDDaSO_message")]
        M = 6
    }
}
