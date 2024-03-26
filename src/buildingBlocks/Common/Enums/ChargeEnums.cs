namespace CredECard.Common.Enums.Charge
{
    public enum EnumChargeType
    {
        Administration = 1,
        Interest,
        TransactionFee,
        Withdrawal,
        Additional = 5,
        TransferFee = 6,
        Local_ATM = 7,
        Topup = 8,      
        MaestroCardIssue = 9, 
        MaestroPINReissue = 10,
        SMS = 11,
        MonthlyFees = 12,        
        BalanceSMSCharge = 13,
        WithdrawSMSCharge = 14,
        LoadSMSCharge = 15,
        POSSMSCharge = 16,
        MaestroCardReissue = 17,
        MaestroAdditionalCardIssue = 18,
        Abroad_ATM = 19,
        Local_POS = 20,
        Abroad_POS = 21,
        Reset_Password_SMS = 22,
    }

}
