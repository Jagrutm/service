using System;
using System.ComponentModel;

namespace Common.CBGEnums
{
    public enum EnumBACSTransactionCode : short
    {
        [Description("0")]
        None = 0,

        [Description("01")]
        Direct_Debit_First_Collection = 1,

        [Description("17")]
        Direct_Debit_Regular_Collection = 2,

        [Description("18")]
        Direct_Debit_Represented = 3,

        [Description("19")]
        Direct_Debit_Final_Collection = 4,

        [Description("U1")]
        Automated_Return_Of_Unpaid_Direct_Debit_First_Collection = 5,

        [Description("U7")]
        Automated_Return_Of_Unpaid_Direct_Debit_Regular_Collection = 6,

        [Description("U8")]
        Automated_Return_Of_Unpaid_Direct_Debit_Represented = 7,

        [Description("U9")]
        Automated_Return_Of_Unpaid_Direct_Debit_Final_Collection = 8,

        [Description("99")]
        Direct_Credit_Debit_Contra = 9, //(a credit record to balance debit records)	9

        [Description("Z4")]
        Interest_Payments = 10,

        [Description("Z5")]
        Dividend_Payments = 11,

        [Description("07")]
        Automated_Teller_Collection = 12,

        [Description("13")]
        Claims_For_Unpaid_Cheques = 13,

        [Description("E1")]
        Credit_Card_Debit = 14,

        [Description("E2")]
        Credit_Card_Refund = 15,

        [Description("0N")]
        Direct_Debit_Instruction_New_Instruction = 16,

        [Description("0C")]
        Direct_Debit_Instruction_Cancellation_Instruction = 17,

        [Description("0S")]
        Direct_Debit_Instruction_Conversion_Instruction = 18,

        [Description("86")]
        Automated_Settlement_Credits = 19,

        [Description("RA")]
        Automated_Return_Of_Unapplied_Credits = 20
    }


    public static class EnumExtensionMethods
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
    }

}
