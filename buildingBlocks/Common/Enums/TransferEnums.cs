namespace CredECard.Common.Enums.Transfer
{
    public enum EnumWithdrawalMethod
    {
        BankTransfer_BACS = 1,
        SameDayTransfer_CHAPS,
        Cheque,
        International_Regular,
        International_Urgent,
        Internal_Transfer = 99,
        ThirdParty_Transfer = 100
    }
}
