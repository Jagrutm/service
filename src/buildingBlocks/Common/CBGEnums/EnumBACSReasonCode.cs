using System.ComponentModel;

namespace Common.CBGEnums
{
    public enum EnumBACSReasonCode : short
    {
        [Description("-1")]
        Unknown = -1,

        /// <summary>
        /// For ADDACS OR AWACS only
        /// </summary>
        [Description("0")]
        Instruction_Cancelled_Refer_To_payer_Invalid_Details = 1,

        /// <summary>
        /// For AUDDIS OR ADDACS only
        /// </summary>
        [Description("1")]
        Instruction_Cancelled_By_Payer = 2,

        /// <summary>
        /// For AUDDIS OR ADDACS only
        /// </summary>
        [Description("2")]
        Payer_Deceased = 3,

        /// <summary>
        /// For AUDDIS OR ADDACS, OR AWACS
        /// </summary>
        [Description("3")]
        Account_Transferred_To_Another_Bank = 4,

        /// <summary>
        /// AUDDIS Only
        /// </summary>
        [Description("5")]
        No_account = 5,

        /// <summary>
        /// AUDDIS Only
        /// </summary>
        [Description("6")]
        No_instruction = 6,

        /// <summary>
        /// For AUDDIS OR ADDACS
        /// </summary>
        [Description("B")]
        Account_Closed = 7,

        /// <summary>
        /// For AUDDIS OR ADDACS only
        /// </summary>
        [Description("C")]
        Account_Transferred_To_A_Different_Account_branch_of_The_bank_building_society = 8,
        
        /// <summary>
        /// ADDACS Only
        /// </summary>
        [Description("D")]
        Advance_Notice_Disputed = 9,

        /// <summary>
        /// ADDACS only
        /// </summary>
        [Description("E")]
        Instruction_Amended = 10,

        /// <summary>
        /// AUDDIS only
        /// </summary>
        [Description("F")]
        Invalid_AccountType = 11,

        /// <summary>
        /// AUDDIS only
        /// </summary>
        [Description("G")]
        Bank_Will_Not_Accept_DirectDebits_On_Account = 12,

        /// <summary>
        /// AUDDIS only
        /// </summary>
        [Description("H")]
        InstructionExpired = 13,

        /// <summary>
        /// AUDDIS only
        /// </summary>
        [Description("I")]
        PayerReferenceNotUnique = 14,

        /// <summary>
        /// AUDDIS only
        /// </summary>
        [Description("K")]
        InstructionCancelled_By_Bank = 15,

        /// <summary>
        /// ADDACS only
        /// </summary>
        [Description("R")]
        Instruction_Reinstated = 16
    }

    public enum EnumBACSARUDDReasonCode : short
    {
        [Description("-1")]
        None = 0,

        [Description("0")]
        Refer_to_payer = 1,

        [Description("1")]
        Instruction_cancelled = 2,

        [Description("2")]
        Payer_deceased = 3,

        [Description("3")]
        Account_transferred = 4,

        [Description("4")]
        Advance_notice_disputed = 5,

        [Description("5")]
        No_account = 6,

        [Description("6")]
        No_instruction = 7,

        [Description("7")]
        Amount_differs = 8,

        [Description("8")]
        Amount_not_due = 9,

        [Description("9")]
        Presentation_overdue = 10,

        [Description("A")]
        Originator_differs = 11,

        [Description("B")]
        Account_closed = 12
    }

    public enum EnumBACSARUCSReasonCode : short
    {
        [Description("-1")]
        None = 0,

        [Description("0")]
        Invalid_Details = 1,
        
        [Description("2")]
        Beneficiary_Deceased = 2,

        [Description("3")]
        Account_Transferred = 3,

        [Description("5")]
        No_Account = 4,

        [Description("B")]
        Account_Closed = 5,

        [Description("C")]
        Requested_by_originator = 6,
    }
}
