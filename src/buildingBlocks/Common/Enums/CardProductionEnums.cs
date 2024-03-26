
namespace CredECard.CardProduction.Enums
{
    public enum EnumProductTypes
    { 
        ALL = 0,
        MasterCard = 1,
        Visa = 2,
        Vocalink = 3 // RS#128206
    }

    public enum EnumCardTypes
    {
        None = 0,
        Magstripe = 1,
        Chip = 2,
        VirtualCard = 3,
        Contactless = 4
    }

    public enum EnumCardVerificationMethods
    {
        None = 0,
        Offline_Online = 1,
        OnlineOnly = 2
    }

    public enum EnumCardActions
    {
        None = 0,
        NewCard = 1,
        NewPIN = 2,
        ReplacementCard = 3,
        PINReplacement = 4,
        ReissueCard = 5,
        PINReset = 6,
        RenewCard = 7
    }

    public enum EnumCardDesignPreferences
    {
        PAN = 1,
        VALID_FROM_DATE_TO_DATE = 2,
        LAST_NAME_FIRST_NAME = 3,
        BLANK = 4
    }

}
