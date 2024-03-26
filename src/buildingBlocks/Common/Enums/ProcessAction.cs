using System;

namespace CredECard.Common.Enums
{
    public enum ProcessAction
    {
        None = 0,
        Pending = 1,
        ToBeChargeback = 2,
        ChargebackFileGenerated = 3,
        Failed = 4
    }
}
