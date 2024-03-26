using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AccountProcessService.Domain.Enum
{
    /// <summary>
    /// Account Type Enum
    /// </summary>
    internal enum EnumAccountType : byte
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
