namespace CredECard.Common.Enums
{
    /// <author>Nidhi Thakrar</author>
    /// <created>26-Apr-2016</created>
    /// <summary>Enum for notification types</summary>
    public enum EnumNotificationType
    {
        None = 0,
        VBV = 1,
        TOKEN = 2,
        TOKEN_OTP = 3,
        Token_Reminder =4,
        VBV_OTP =5,
        CardHolder_Notification = 6
    }

    /// <author>Nidhi Thakrar</author>
    /// <created>26-Apr-2016</created>
    /// <summary>enum for notification methods</summary>
    public enum EnumNotificationMethod
    {
        Http_Post = 1,
        API = 2
    }
}
