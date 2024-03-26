using System.ComponentModel;

namespace Common.Enums
{
    public enum EnumProcessValidationResults : short
    {
        [Description("None")]
        None = 0,

        [Description("Normal")]
        Normal = 1,

        [Description("Institution Sortcode not found.")]
        Institution_Sortcode_Not_Found = 2,

        [Description("Customer account number not found.")]
        Customer_Account_Number_Not_Found = 3,

        [Description("Customer account status is not valid.")]
        Customer_Account_Status_Is_Not_Valid = 4,

        [Description("Direct Debit Instruction not found.")]
        Direct_Debit_Instruction_Not_Found = 5,

        [Description("Transaction code is not allowed.")]
        Transaction_Code_Not_ALlowed = 6,

        [Description("An automated recall of transaction.")]
        Automated_Recalls_Of_Transaction = 7,

        [Description("Return Settlement Transaction.")]
        Return_Settlement_Transaction = 8,

        [Description("ReProcess of DD Payment.")]
        INSUFFICIENT_FUND = 9,

        [Description("Duplicate AUDDIS.")]
        DUPLICATE_AUDDIS = 10,

        [Description("AUDDIS Already Cancelled.")]
        AUDDIS_ALREADY_CANCELLED = 11,

        [Description("BACS Transaction Settlement Date Should be Greater or Equal to Current Date.")]
        Settlement_Date_Should_be_Greater_OR_Equal_TO_Current_Date = 15,

        [Description("Originator Differ.")]
        ORIGINATOR_DIFFER = 16,

        [Description("Amount of the Direct Debit differs from the amount in the advance notice to payer.")]
        AMOUNT_DIFFER = 17,

        [Description("Duplicate DD Cancellation Request.")]
        Duplicate_DD_Cancellation_Request = 18,
        
        [Description("Invalid Recall Information")]
        Invalid_Recall_Information = 19,

        [Description("Recall is not allowed on settlement day (BACS 3rd Day)")]
        Recall_Not_Allowed_On_Settlement_Day = 20,

        [Description("Next Day Credit Reversal")]
        Next_Day_Credit_Reversal = 24,
    }
}
