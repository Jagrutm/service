﻿
namespace CredECard.Common.Enums.Authorization
{
    public enum EnumClearingDataElement
    {
        Processing_BIN = 1,
        Processing_Date = 2,
        Settlement_Date = 3,
        Release_Number = 4,
        Test_Option = 5,
        Security_Code = 6,
        Incoming_File_ID = 7,
        BIN = 8,
        Destination_Amount = 10,
        Number_of_Monetary_Transactions = 11,
        Batch_Number = 12,
        Number_of_TCRs = 13,
        Center_Batch_ID = 14,
        Number_of_Transactions = 15,
        Source_Amount = 16,
        Account_Number = 17,
        Account_Number_Extension = 18,
        Floor_Limit_Indicator = 19,
        CRBException_File_Indicator = 20,
        Positive_Cardholder_Authorization_Service_Indicator = 21,
        Acquirer_Reference_Number = 22,
        Acquirers_Business_ID = 23,
        Purchase_Date = 24,
        Destination_Amount_12 = 25,
        Destination_Currency_Code = 26,
        Source_Amount_12 = 27,
        Source_Currency_Code = 28,
        Merchant_Name = 29,
        Merchant_City = 30,
        Merchant_Country_Code = 31,
        Merchant_Category_Code = 32,
        Merchant_ZIP_Code = 33,
        Merchant_StateProvince_Code = 34,
        Requested_Payment_Service = 35,
        Usage_Code = 36,
        Reason_Code = 37,
        Settlement_Flag = 38,
        Authorization_Characteristics_Indicator = 39,
        Authorization_Code = 40,
        POS_Terminal_Capability = 41,
        International_Fee_Indicator = 42,
        Cardholder_ID_Method = 43,
        CollectionOnly_Flag = 44,
        POS_Entry_Mode = 45,
        Central_Processing_Date = 46,
        Reimbursement_Attribute = 47,
        Chargeback_Reference_Number = 48,
        TerminalID = 49,
        Authorization_Source_Code = 50,
        Transaction_Identifier = 51,
        Authorized_Amount = 52,
        Authorization_Currency_Code = 53,
        Authorization_Response_Code = 54,
        Validation_Code = 55,
        Excluded_Transaction_Identifier_Reason = 56,
        CRS_Processing_Code = 57,
        Chargeback_Rights_Indicator = 58,
        Multiple_Clearing_Sequence_Number = 59,
        Multiple_Clearing_Sequence_Count = 60,
        Market_Specific_Authorization_Data_Indicator = 61,
        Total_Authorized_Amount = 62,
        Information_Indicator = 63,
        Merchant_Telephone_Number = 64,
        Additional_Data_Indicator = 65,
        Merchant_Volume_Indicator = 66,
        Electronic_Commerce_Goods_Indicator = 67,
        Merchant_Verification_Value = 68,
        Interchange_Fee_Amount = 69,
        Interchange_Fee_Sign = 70,
        Source_Currency_to_Base_Currency_Exchange_Rate = 71,
        Base_Currency_to_Destination_Currency_Exchange_Rate = 72,
        Optional_Issuer_ISA_Amount = 73,
        Product_ID = 74,
        Program_ID = 75,
        Dynamic_Currency_Conversion_Indicator = 76,
        CVV2_Result_Code = 77,
        Local_Tax = 78,
        Local_Tax_Included = 79,
        National_Tax = 80,
        National_Tax_Included = 81,
        Merchant_VAT_Registration_Number = 82,
        Customer_VAT_Registration_Number = 83,
        Summary_Commodity_Code = 84,
        Other_Tax = 85,
        Message_Identifier = 86,
        Time_of_Purchase = 87,
        Customer_Code_CRI = 88,
        Non_Fuel_Product_Code_1 = 89,
        Non_Fuel_Product_Code_2 = 90,
        Non_Fuel_Product_Code_3 = 91,
        Non_Fuel_Product_Code_4 = 92,
        Non_Fuel_Product_Code_5 = 93,
        Non_Fuel_Product_Code_6 = 94,
        Non_Fuel_Product_Code_7 = 95,
        Non_Fuel_Product_Code_8 = 96,
        Merchant_Postal_Code = 97,
        Transaction_Type = 98,
        Card_Sequence_Number = 99,
        Terminal_Transaction_Date = 100,
        Terminal_Capability_Profile = 101,
        Terminal_Country_Code = 102,
        Terminal_Serial_Number = 103,
        Unpredictable_Number = 104,
        Application_Transaction_Counter = 105,
        Application_Interchange_Profile = 106,
        Cryptogram = 107,
        Issuer_Application_Data_Byte_2 = 108,
        Issuer_Application_Data_Byte_3 = 109,
        Terminal_Verification_Results = 110,
        Issuer_Application_Data_Byte_4_7 = 111,
        Cryptogram_Amount = 112,
        Issuer_Application_Data_Byte_8 = 113,
        Issuer_Application_Data_Byte_9_16 = 114,
        Issuer_Application_Data_Byte_1 = 115,
        Issuer_Application_Data_Byte_17 = 116,
        Issuer_Application_Data_Byte_18_32 = 117,
        Form_Factor_Indicator = 118,
        Issuer_Script_1_Results = 119,
        CashBack = 120,
        MailPhoneECPaymentIndicator = 121,
        Interface_Trace_Number = 125,
        Business_Format_Code = 126,
        Agent_Unique_ID = 127,
        Additional_Authentication_Method = 128,
        Additional_Authentication_Reason_Code = 129,
        Card_Acceptor_ID = 130,
        CardHolder_Activated_Terminal_Indicator = 131,
        Account_Number_Extension_Fraud = 132,
        Issuer_Generated_Authorization = 133,
        Notification_Code = 134,
        Account_Sequence_Number = 135,
        Reserved_convenience_check = 136,
        Fraud_Type = 137,
        Card_Expiration_Date = 138,
        Merchant_Postal_Code_Fraud = 139,
        Fraud_Investigative_Status = 140,
        Travel_Agency_ID = 141,
        Cashback_Indicator = 142,
        Card_Capability = 143,
        Reserved_6 = 144,
        Reserved_86 = 145,
        Transaction_Code = 146,
        Transaction_Code_Qualifier = 147,
        Transaction_Component_Sequence_Number = 148,
        Destination_BIN = 149,
        Source_BIN = 150,
        Response_Code = 151,
        Reserved_52 = 157,
        Reserved_18 = 156,
        Outgoing_File_ID = 155,
        Reserved_89 = 154,
        Reserved_29 = 153,
        Reserved_16 = 152,
        Reject_Warning_Codes = 158,
        Reserved_1 = 160,
        Issuer_Workstation_BIN = 161,
        Acquirer_Workstation_BIN = 162,
        Documentation_Indicator = 163,
        Member_Message_Text = 164,
        Special_Condition_Indicators = 165,
        Fee_Program_Indicator = 166,
        Issuer_Charge = 167,
        National_Reimbursement_Fee = 169,
        Special_Chargeback_Indicator = 170,
        Unattended_Acceptance_Terminal_Indicator = 172,
        Prepaid_Card_Indicator = 173,
        Service_Development_Field = 174,
        AVS_Response_Code = 175,
        Purchase_Identifier_Format = 177,
        Account_Selection = 178,
        Installment_Payment_Count = 179,
        Purchase_Identifier = 180,
        Chip_Condition_Code = 182,
        POSEnvironment = 122,
        Reserved_23 = 183,
        Retrieval_Request_ID = 184,
        Issuer_Control_Number = 185,
        Original_Transaction_Code = 186,
        Original_Transaction_Code_Qualifier = 187,
        Original_Transaction_Component_Sequence_Number = 188,
        Source_Batch_Date = 189,
        Source_Batch_Number = 190,
        Item_Sequence_Number = 191,
        Original_Source_Amount = 192,
        Original_Source_Currency = 193,
        Original_Settlement_Flag = 194,
        Chargeback_Reduction_Service_Return_Flag = 196,
        Return_Reason_Code = 197,
        TokenAssuranceLevel =241,
        Token =242,
        TokenRequestorID=243,
        PaymentAccountReference=244,
        Reserved_63 = 268

    }

    /// <author>Nidhi Thakrar</author>
    /// <created>23-Dec-2014</created>
    /// <summary>Data elements used in issuer script</summary>
    public enum EnumIssuerScriptDataElement
    {
        None = 0,
        PIN_CHANGE = 204 ,
        PIN_UNBLOCK = 205,
        VLP_FUNDS_LIMIT = 206,
        CONTACTLESS_TRANSACTION_COUNTER_LOWER_LIMIT = 207,
        CONSECUTIVE_TRANSACTION_COUNTER_LIMIT = 208
    }

    public enum EnumTransactionCode
    {
        Short_Block_00 = 1,
        Returned_Credit_01 = 2,
        Returned_Debit_02 = 3,
        Returned_Nonfinancial_03 = 4,
        Reclassification_Advice_Transaction_04 = 5,
        Sales_Draft_or_Representment_05 = 6,
        Credit_Voucher_06 = 7,
        Cash_Disbursement_07 = 8,
        Money_Transfer_08 = 9,
        Fee_Collection_10 = 10,
        Sales_Draft_Chargeback_15 = 11,
        Credit_Voucher_Chargeback_16 = 12,
        Cash_Disbursement_Chargeback_17 = 13,
        Money_Transfer_19 = 14,
        Funds_Disbursement_20 = 15,
        Sales_Draft_Reversal_25 = 16,
        Credit_Voucher_Reversal_26 = 17,
        Cash_Disbursement_Reversal_27 = 18,
        ICS_Input_30 = 19,
        ICS_Response_Transaction_File_31 = 20,
        NMAS_Requests_and_Responses_32 = 21,
        Multipurpose_Message_Transactions_33 = 22,
        Sales_Draft_Chargeback_Reversal_35 = 23,
        Credit_Voucher_Chargeback_Reversal_36 = 24,
        Cash_Disbursement_Chargeback_Reversal_37 = 25,
        Copy_Request_Service_Chargeback_Documentation_Automation_Service_Message_38 = 26,
        Automated_Copy_Fulfillment_39 = 27,
        Fraud_Advice_40 = 28,
        Merchant_File_Update_42 = 29,
        Merchant_File_Update_43 = 30,
        Collection_Batch_Acknowledgment_44 = 31,
        General_Delivery_Report_45 = 32,
        Member_Settlement_Data_46 = 33,
        Report_Generation_47 = 34,
        BASE_I_Advice_48 = 35,
        Free_Text_Message_50 = 36,
        Request_for_Original_Paper_51 = 37,
        Request_for_Photocopy_or_Substitute_Draft_52 = 38,
        Photocopy_Original_Mailing_Confirmation_53 = 39,
        Table_Update_Data_54 = 40,
        RCRF_Update_55 = 41,
        Currency_Conversion_Rate_Update_56 = 42,
        Data_Capture_Transaction_Advice_57 = 43,
        National_Settlement_Advice_58 = 44,
        Interface_Transaction_Advice_59 = 45,
        File_Header_90 = 46,
        Batch_Trailer_91 = 47,
        File_Trailer_92 = 48
        , File_Header_1644_697 = 49
        , Text_Messages_1644_693 = 50
        , Currency_Updates_1644_640 = 51
        , First_Presentments_1240_200 = 52
        , Financial_Detail_Addendum_1644_696 = 53
        , Retrieval_Requests_1644_603 = 54
        , Fee_Collections_1740_700 = 55
        , First_Chargebacks_Full_1442_450 = 56
        , First_Chargebacks_Partial_1442_453 = 57
        , Second_Presentments_full_1240_205 = 58
        , Second_Presentments_Partial_1240_282 = 59
        , Arbitration_Chargebacks_Full_1442_451 = 60
        , Arbitration_Chargebacks_Partial_1442_454 = 61
        , Fee_Collections_1740_700_Customergenerated = 62
        , Returns_1740_780 = 63
        , Resubmissions_1740_781 = 64
        , Arbitration_Returns_1740_782 = 65
        , those_generated_by_Mastercard_1740_783 = 66
        , Message_Exception_1644_691 = 67
        , File_Reject_1644_699 = 68
        , File_Currency_Summary_1644_680 = 69
        , Financial_Position_Detail_1644_685 = 70
        , Settlement_Position_Detail_1644_688 = 71
        , File_Trailer_1644_695 = 72
        , Fee_Collections_Retrieval_Fee_Billing_1740_700 = 73 //C#161830 Vishal S
        , Fee_Collections_Funds_Transfer_1740_790 = 74 //C#161830 Vishal S
    }

    public enum EnumTransactionComponentRecords
    {
        TCR_0 = 1,
        TCR_1 = 2,
        TCR_5 = 3,
        TCR_6 = 4,
        TCR_7 = 5,
        TCR_E = 6,
        TCR_2 = 7
    }
}
