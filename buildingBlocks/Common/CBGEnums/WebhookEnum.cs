namespace Common.CBGEnums
{
    public enum WebhookEnum
    {
        DirectDebitPaymentProcessed = 1,

        DirectDebitPaymentSettled = 2,

        DirectCreditPaymentProcessed = 3,

        DirectCreditPaymentSettled = 4,

        DirectDebitMandateInitiated = 5,

        DirectDebitMandateCancelled = 6,

        DirectDebitMandateMigrated = 7,

        DirectDebitPaymentReturned = 8,

        OutBoundPaymentRejected = 9,

        DirectCreditPaymentReturned = 10,

        InBoundFasterPayment = 11,

        OutBoundPaymentSettled = 12,
        
        FPSInboundPaymentReverse = 13,

        BACSDirectCreditRecalls = 14,

        BACSDirectCreditReversal = 15
    }
}
