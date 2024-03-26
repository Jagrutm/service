using System;
using System.Collections.Generic;
using System.Text;

namespace CredECard.Common.Enums.NotificationActions
{
    /// <author>Ashka Modi</author>
    /// <created>25-May-2009</created>
    /// <summary>
    /// Notification action Enum
    /// </summary>
    public enum EnumNotificationActions
    {
        None = 0,
        PaymentReminder = 1,
        CardActivationReminder = 2,
        CloseAccount = 3,
        CardCancel = 4
    }

    public enum EnumSchemeAffiliateSentStatus
    {
        Pending=1,
        Success=2,
        Failed=3
    }
}
