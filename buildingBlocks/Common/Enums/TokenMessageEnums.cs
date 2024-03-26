using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CredECard.Common.Enums
{
    /// <author>Nidhi Thakrar</author>
    /// <created>11-Apr-17</created>
    /// <summary></summary>
    public enum EnumTokenMessageErrorCode
    {
        None,
        [Description("Invalid Request Mandatory Data Missing")]
        ISE40000,
        [Description("Invalid Pan")]
        ISE40001,
        [Description("Invalid Card Type")]
        ISE40003,
        [Description("Invalid Request, Invalid Field Length")]
        ISE40005,
        [Description("Invalid Request, Invalid Field Type")]
        ISE40006,
        [Description("Cryptography Error")]
        ISE40010,
        [Description("Invalid Request, Invalid Field Value")]
        ISE40011,
        [Description("Pan Ineligible")]
        ISE40101,
        [Description("Authorization Failure")]
        ISE40300,
        [Description("Unknown Token value")]
        ISE40402,
        [Description("Unknown Token Reference ID Value")]
        ISE40500,
        [Description("Invalid Resolution Method ID")]
        ISE40501,
        [Description("Consumer tenured data channel not available.")]
        ISE40502,
        [Description("Consumer tenured data not available.")]
        ISE40503,
        [Description("Internal system error")]
        ISE40504,
        [Description("Suspected Fraud")]
        ISE40505,
        [Description("Expired Card")]
        ISE40506,
        [Description("New Card issued")]
        ISE40507,
    }

    /// <author>Nidhi Thakrar</author>
    /// <created>11-Apr-17</created>
    /// <summary></summary>
    public enum TokenMessageCardHolderVerificationMethod
    {
        cell_phone = 1,
        email = 2,
        bank_app = 3,
        customer_service = 4,
        outbound_call = 5
    }
}
