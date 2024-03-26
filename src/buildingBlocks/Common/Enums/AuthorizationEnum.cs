namespace CredECard.Common.Enums.Authorization
{

    public enum EnumAmountType
    {
        Ledger_Balance = 1,
        Available_Balance = 2,
        Amount_Owing = 3,
        Amount_Due = 4,
        Healthcare_Eligibility = 10,
        Prescription_Eligibility = 11,
        Amount_CashBack = 40,
        Original_Amount = 57
    }

    public enum EnumKeyType
    {
        PEK = 1,
        PVK = 2,
        CVK1 = 3,
        CVK2 = 4
    }

    public enum EnumPaymentTypeCode
    {
        Bulk_Authorisation_00 = 0
        , Single_Immediate_Payment_10 = 10
        , Return_Payment_20 = 20
        , Scheme_Return_Payment_25 = 25
        , Standing_Order_30 = 30
        , Forward_Dated_Payment_40 = 40
        , Corporate_Bulk_Payment_50 = 50
        , none = 99
    }



    public enum EnumISOTransactionType
    {
        Purchase = 0,
        Withdrawal = 1,
        Check_Acceptance = 3,
        PurchaseCashBack = 9,
        Account_Funding = 10, // GenericDebitTransaction (Vocalink)
        Quasi_Cash_Transaction = 11,
        Cash_Disbursement = 12,
        E_cash_Load_Transaction = 13, // RS#128206


        Debits_Cash_advances_using_credit_cards = 17, // RS#128206 (MC_Cash_Disbursement)
        Unique_Transaction = 18,
        Fee_Collection_Debit = 19,
        PurchaseRefund = 20,
        Deposit_Cash_only = 21, // RS#128206
        Deposit_Cheque_only_or_Cash_and_Cheque = 24, // RS#128206
        Generic_Credit_Transaction = 25, // RS#128206
        Original_Credit = 26,
        Automated_Deposit = 27, // RS#128206
        Prepaid_Activation_Load = 28,
        Fee_Collection_Credit = 29,
        BalanceInquiry = 30,
        Balance_Enquiry = 31, // RS#128206
        Eligibility_Inquiry = 39,
        Cardholder_Account_Transfer = 40,
        Bill_Payment = 50,
        Payment = 53,
        Pre_pay_Payment = 59, // RS#128206
        PIN_Change = 70,
        PIN_Unblock_Prepaid_Activation = 72,
        PIN_Management_PIN_Change = 90, // RS#128206
        //PIN_Management_PIN_Unlock = 91, // RS#128206
        MC_PIN_Unblock = 91,
        MC_PIN_Change = 92
        //PIN_Change = 92
    }

    public enum EnumPOSEntryMode
    {
        Unknown = 0,
        Manual_key_entry = 1,
        Magnetic_stripe_read_CVV_Check_not_Possible = 2,
        Barcode_read = 3,
        ICC_read_CVV_Check_Possible = 5,
        Contactless_payment_USING_VSDC_chip_rules = 7,
        Card_On_File = 10,
        Magnetic_stripe_read_CVV_Check_Possible = 90,
        Contactless_payment_USING_Magstrip_data_rule = 91,
        ICC_read_CVV_Check_not_Possible = 95,
        PAN_Token_entry_via_E_Comm = 81,  // MC_vipul
        Hybrid_Terminal_With_An_Online_Connection = 79,
        International_Card_Scheme_Gateway_acquired_transaction = 99  // RS#128206
    }
    public enum EnumPOSConditionCode
    {
        Normal_transaction = 0,
        Cardholder_not_present = 1,
        Unattended_acceptance_terminal = 2,
        Merchant_suspicious_of_transaction_or_card = 3,
        Standing_order_recurring_transactions = 4, //MC vipul
        Cardholder_present_card_not_present = 5,
        Completion_advice = 6,
        Telephone_Device_Request = 7, // RS#128206
        Mail_phone_order_recurring_transaction = 8,
        Customer_identity_verified = 10,
        account_verification_request_without_authorization = 51,
        Non_ICC_Capable_Branch_ATM_or_Terminal = 54, // RS#128206
        ICC_Capable_Branch_ATM_or_Terminal = 55, // RS#128206
        Non_ICC_Capable_Remote_Branch_ATM_or_Terminal = 56, // RS#128206
        ICC_Capable_Remote_Branch_ATM_or_Terminal = 57, // RS#128206
        E_commerce_request = 59,
        Card_present_magstripe_cannot_read = 71,
        International_Card_Scheme_Gateway_Acquired_Transaction = 99, // RS#128206
    }

    public enum EnumPOSCardholderPresence
    {
        Cardholder_Present = 0,
        Cardholder_Not_Present = 1,
        Cardholder_Not_Present_mail_Facsimile_Order = 2,
        Cardholder_Not_Present_Phone = 3,
        Standing_Order_Recurring_Transactions = 4,
        Cardholder_Not_Present_Electronic_Order = 5
    }
    public enum EnumISOMessageType
    {
        AuthRequest = 100,
        RepeatAuthRequest = 101,
        AuthResponse = 110,
        AuthAdvice = 120,
        AuthAdviceResponse = 130,
        AuthNegativeAdvice = 190,
        FinancialTransactionRequest = 200, // RS#128206
        ReversalRequest = 400,
        RepeatReversalRequest = 401,
        ReversalResponse = 410,
        ReversalAdvice = 420,
        RepeatReversalAdvice = 421, // RS#128206
        ReversalAdviceResponse = 430,
        NetworkManagementRequest = 800,
        NetworkManagementResponse = 810,
        NetworkManagementAdvice = 820,
        Presentment = 1240,
        Chargeback = 1442,
        File_Message = 1644,
        Fee_Collection = 1740,
        Local_Network_MessageRequest = 9000,
        Local_Network_MessageResponse = 9100,
        TokenNotificationAdviceRequest = 620,
        TokenNotificationAdviceResponse = 630,
        FileMaintenanceAdviceRequest = 302,
        FileMaintenanceAdviceResponse = 312,

        AGENCY_PAYMENT_REQUEST_REPEAT = 9101
        , AGENCY_PAYMENT_REQUEST = 9100
        , AGENCY_PAYMENT_RESPONSE = 9110
        , DCAFIM_AUTHORISATION_REQUEST = 9100
        , THIRD_PARTY_PAYMENT_REQUEST = 9100
        , THIRD_PARTY_PAYMENT_REQUEST_REPEAT = 9101
        , DCAFIM_AUTHORISATION_RESPONSE = 9110
        , THIRD_PARTY_PAYMENT_RESPONSE = 9110
        , DCAFIM_AUTHORISATION_ADJUSTMENT_REQUEST = 9120
        , THIRD_PARTY_NOTIFICATION = 9120
        , DCAFIM_AUTHORISATION_ADJUSTMENT_REQUEST_REPEAT = 9121
        , DCAFIM_AUTHORISATION_ADJUSTMENT_RESPONSE = 9130
        , AGENCY_PAYMENT_CREDIT_REQUEST = 9200
        , AGENCY_PAYMENT_CREDIT_REQUEST_REPEAT = 9201
        , AUTHORISATION_REQUEST = 9200
        , MEMBER_PAYMENT_REQUEST = 9200
        , MEMBER_PAYMENT_REQUEST_REPEAT = 9201
        , AGENCY_PAYMENT_CREDIT_RESPONSE = 9210
        , AUTHORISATION_RESPONSE = 9210
        , MEMBER_PAYMENT_RESPONSE = 9210
        , AGENCY_PAYMENT_CREDIT_REVERSAL_REQUEST = 9420
        , AUTHORISATION_REVERSAL_REQUEST = 9420
        , DCAFIM_AUTHORISATION_REVERSAL_REQUEST = 9420
        , PAYMENT_REVERSAL_REQUEST = 9420
        , THIRD_PARTY_REVERSAL_REQUEST = 9420
        , AGENCY_PAYMENT_CREDIT_REVERSAL_REQUEST_REPEAT = 9421
        , AUTHORISATION_REVERSAL_REQUEST_REPEAT = 9421
        , DCAFIM_AUTHORISATION_REVERSAL_REQUEST_REPEAT = 9421
        , PAYMENT_REVERSAL_REQUEST_REPEAT = 9421
        , THIRD_PARTY_REVERSAL_REQUEST_REPEAT = 9421
        , AGENCY_PAYMENT_CREDIT_REVERSAL_RESPONSE = 9430
        , AUTHORISATION_REVERSAL_RESPONSE = 9430
        , DCAFIM_AUTHORISATION_REVERSAL_RESPONSE = 9430
        , PAYMENT_REVERSAL_RESPONSE = 9430
        , THIRD_PARTY_REVERSAL_RESPONSE9430
        , ADMINISTRATION_ADVICE = 9624
        , NETWORK_MANAGEMENT_REQUEST = 9804
        , NETWORK_MANAGEMENT_REQUEST_RESPONSE = 9814
        , UNSOLICITED_MESSAGE = 9824
        , UNSOLICITED_MESSAGE_ACKNOWLEDGEMENT = 9834
    }

    public enum EnumCardMessageType
    {
        None = 0,
        Authorization_Request = 1,
        Authorization_Response = 2,
        Authorization_Advice = 3,
        Authorization_Advice_Response = 4,
        Authorization_Response_Acknowledgement = 5,
        Authorization_Negative_Acknowledgement = 6,
        Reversal_Request = 7,
        Reversal_Request_Response = 8,
        Reversal_Advice = 9,
        Reversal_Advice_Response = 10,
        Network_Management_Request = 11,
        Network_Management_Response = 12,
        Network_Management_Advice = 13,

        CPRQ = 18,
        CPRS = 19,
        VEReq = 20,
        VERes = 21,
        PAReq = 22,
        PARes = 23,
        PATransReq = 24,
        PATransRes = 25,
        Error = 26,

        GetCVMReq = 27,
        GetCVMRes = 28,
        SendPasscodeReq = 29,
        SendPassCodeRes = 30,
        CheckEligibilityReq = 31,
        CheckEligibilityRes = 32,
        LifeCycleMgmtReq = 33,
        LifeCycleMgmtRes = 34,
        TokenInqReq = 35,
        TokenInqRes = 36,
        UpdateMetaDataReq = 37,
        UpdateMetaDataRes = 38,
        GeneralMsgReq = 39,
        GeneralMsgRes = 40,

        TokenNotificationAdviceRequest = 41,
        TokenNotificationAdviceResponse = 42,
        FileMaintenanceMessageRequest = 43,
        FileMaintenanceMessageResponse = 44,
        FinancialTransactionRequest = 45, // RS#128206
        FinancialTransactionRequestResponse = 46, // RS#128206


        AGENCY_PAYMENT_REQUEST_REPEAT = 47,
        AGENCY_PAYMENT_REQUEST = 48,
        AGENCY_PAYMENT_RESPONSE = 49,
        DCA_FIM_AUTHORISATION_REQUEST = 50,
        THIRD_PARTY_PAYMENT_REQUEST = 51,
        THIRD_PARTY_PAYMENT_REQUEST_REPEAT = 52,
        DCA_FIM_AUTHORISATION_RESPONSE = 53,
        THIRD_PARTY_PAYMENT_RESPONSE = 54,
        DCA_FIM_AUTHORISATION_ADJUSTMENT_REQUEST = 55,
        THIRD_PARTY_NOTIFICATION = 56,
        DCA_FIM_AUTHORISATION_ADJUSTMENT_REQUEST_REPEAT = 57,
        DCA_FIM_AUTHORISATION_ADJUSTMENT_RESPONSE = 58,
        AGENCY_PAYMENT_CREDIT_REQUEST = 59,
        AGENCY_PAYMENT_CREDIT_REQUEST_REPEAT = 60,
        AUTHORISATION_REQUEST = 61,
        MEMBER_PAYMENT_REQUEST = 62,
        MEMBER_PAYMENT_REQUEST_REPEAT = 63,
        AGENCY_PAYMENT_CREDIT_RESPONSE = 64,
        AUTHORISATION_RESPONSE = 65,
        MEMBER_PAYMENT_RESPONSE = 66,
        AGENCY_PAYMENT_CREDIT_REVERSAL_REQUEST = 67,
        AUTHORISATION_REVERSAL_REQUEST = 68,
        DCA_FIM_AUTHORISATION_REVERSAL_REQUEST = 69,
        PAYMENT_REVERSAL_REQUEST = 70,
        THIRD_PARTY_REVERSAL_REQUEST = 71,
        AGENCY_PAYMENT_CREDIT_REVERSAL_REQUEST_REPEAT = 72,
        AUTHORISATION_REVERSAL_REQUEST_REPEAT = 73,
        DCA_FIM_AUTHORISATION_REVERSAL_REQUEST_REPEAT = 74,
        PAYMENT_REVERSAL_REQUEST_REPEAT = 75,
        THIRD_PARTY_REVERSAL_REQUEST_REPEAT = 76,
        AGENCY_PAYMENT_CREDIT_REVERSAL_RESPONSE = 77,
        AUTHORISATION_REVERSAL_RESPONSE = 78,
        DCA_FIM_AUTHORISATION_REVERSAL_RESPONSE = 79,
        PAYMENT_REVERSAL_RESPONSE = 80,
        THIRD_PARTY_REVERSAL_RESPONSE = 81,
        ADMINISTRATION_ADVICE = 82,
        NETWORK_MANAGEMENT_REQUEST = 83,
        NETWORK_MANAGEMENT_REQUEST_RESPONSE = 84,
        UNSOLICITED_MESSAGE = 85,
        UNSOLICITED_MESSAGE_ACKNOWLEDGEMENT = 86
    }

    public enum EnumSubMessageType
    {
        None = 0,
        Auth_Request_0100 = 1,
        Auth_Request_Response_0110 = 2,
        Auth_Advice_Acquirer_generated_0120 = 3,
        Auth_Advice_System_generated_0120 = 5,
        Auth_Advice_Response_Acquirer_generated_0130 = 6,
        Auth_Advice_Response_System_generated_0130 = 7,
        Auth_Response_Acknowledgement_0180 = 9,
        Auth_Response_Negative_Acknowledgement_0190 = 10,
        Reversal_Request_0400 = 11,
        Reversal_Request_Response_0410 = 12,
        Reversal_Advice_0420 = 13,
        Reversal_Advice_Response_0430 = 14,
        Sign_On_Sign_Off_0800 = 15,
        Network_Connection_Status_Member_generated_0800 = 16,
        Network_Connection_Status_System_generated_0800 = 17,
        Host_Session_Activation_Deactivation_0800 = 18,
        PEK_Exchange_0800 = 19,
        PEK_Exchange_On_Demand_0800 = 20,
        Response_Sign_On_Sign_Off_0810 = 21,
        Network_Connection_Status_Member_generated_0810 = 22,
        Network_Connection_Status_System_generated_0810 = 23,
        Host_Session_Activation_Deactivation_0810 = 24,
        PEK_Exchange_0810 = 25,
        PEK_Exchange_On_Demand_0810 = 26,
        Advice_PEK_Exchange_0820 = 27,
        First_Presentment = 28,
        Second_Presentment = 29,
        First_Chargeback = 30,
        Arbitration_Chargeback = 31,
        Retrieval_Request = 32,
        File_Header = 33,
        File_Trailer = 34,
        Text_Message = 35,
        Reconciliation_Messages = 36,
        Fee_Messages = 37,
        NonCPS_Standard_Purchase_PIN_No_PIN_E_Commerce_110 = 38,
        Non_CPS_Standard_Purchase_Voice_Authorization_110 = 39,
        CPS_Retail_Purchase_Passenger_Transport_and_Hotel_Auto_Rental_110 = 40,
        CPS_Card_Not_Present_Direct_Marketing_Hotel_and_Auto_Rental_E_Commerce_110 = 41,
        CPS_Card_Present_Automated_Fuel_Dispenser_110 = 42,
        Completion_Advice_for_BASE_I_130 = 43,
        Authorization_Advice_and_Response_for_Base_I_130 = 44,
        Non_CPS_ATM_Cash_Transaction_With_PIN_110 = 45,
        Manual_Cash_or_Quasi_Cash_Electronic_Terminal_No_PIN_110 = 46,
        Manual_Cash_or_Quasi_Cash_Voice_Authorization_110 = 47,
        CPS_ATM_Visa_Card_With_PIN_110 = 48,
        ATM_Balance_Inquiry_110 = 49,
        POS_Balance_Inquiry_110 = 50,
        Prepaid_Load_and_Activate_110 = 51,
        Standard_Non_CPS_Purchase_Manual_Cash_or_Quasi_Cash_Reversal_Electronic_Terminal_410 = 52,
        Standard_Non_CPS_Purchase_Manual_Cash_or_Quasi_Cash_Reversal_Voice_Authorization_410 = 53,
        Automated_POS_Purchase_Reversal_With_PIN_410 = 54,
        CPS_Card_Present_POS_Authorization_Reversal_Retail_Purchase_Passenger_Transport_and_Hotel_and_Auto_Rental_410 = 55,
        CPS_Card_Not_Present_Reversal_Passenger_Transport_Hotel_and_Auto_Rental_Direct_Marketing_and_E_Commerce_410 = 56,
        CPS_Automated_Fuel_Dispenser_Reversal_410 = 57,
        POS_Partial_Reversal_Non_CPS_and_CPS_410 = 58,
        ATM_Full_and_Partial_Reversal_410 = 59,
        Prepaid_Load_and_Activate_Reversal_of_0100_410 = 60,
        Reversal_Advice_and_Response_for_Base_I_Issuers_410 = 61,
        POS_Partial_Reversal_Advice_and_Response_for_BASE_I_Issuers_430 = 62,
        VSDC_Non_CPS_Card_Present_Request_Standard_Purchase_Electronic_Terminal_PIN_or_No_PIN_ECommerce_110 = 63,
        VSDC_CPS_Card_Present_Request_Retail_Purchase_No_PIN_Passenger_Transport_and_Hotel_and_Auto_Rental_110 = 64,
        Incremental_BASE_I_VIP_Hotel_Auto_Rental_Authorizations_110 = 65,
        VSDC_CPS_Card_Present_Request_Automated_Fuel_Dispenser_110 = 66,
        VSDC_Prepaid_Load_and_Activate_0100_ATM_Requests_110 = 67,
        VSDC_Non_CPS_ATM_Authorization_Request_110 = 68,
        VSDC_CPS_ATM_Request_Visa_Card_With_PIN_110 = 69,
        VSDC_Non_CPS_ATM_Balance_Inquiry_Request_110 = 70,
        PIN_Change_Unblock_Request_110 = 71,
        Authorization_Advice_and_Response_for_Base_I_Issuers_130 = 72,
        VSDC_Non_CPS_Purchase_Manual_Cash_or_Quasi_Cash_Reversal_Electronic_Terminal_410 = 73,
        VSDC_CPS_Card_Present_POS_Authorization_Reversal_Retail_Purchase_Passenger_Transport_and_Hotel_and_Auto_Rental_410 = 74,
        VSDC_CPS_Automated_Fuel_Dispenser_Reversal_410 = 75,
        VSDC_Non_CPS_and_CPS_POS_Partial_Authorization_Reversal_410 = 76,
        VSDC_Prepaid_Load_and_Activate_Reversal_of_0100_410 = 77,
        VSDC_Non_CPS_ATM_Authorization_Reversal_410 = 78,
        VSDC_CPS_ATM_Authorization_Reversal_410 = 79,
        PIN_Change_Unblock_Request_Reversal_410 = 80,
        Reversal_Advice_and_Response_for_Base_I_Issuers_430 = 81,
        VSDC_POS_Partial_Reversal_Advice_and_Response_for_BASE_I_Issuers_430 = 82,
        Network_Management_Messages_810 = 83,
        Local_Network_In_Management_Messages_9000 = 84,
        Local_Network_Out_Management_Messages_9100 = 85,

        CPRQ = 86,
        CPRS = 87,
        VEReq = 88,
        VERes = 89,
        PAReq = 90,
        PARes = 91,
        PATransReq = 92,
        PATransRes = 93,
        Error = 94,

        GetCVMReq = 95,
        GetCVMRes = 96,
        SendPasscodeReq = 97,
        SendPassCodeRes = 98,
        CheckEligibilityReq = 99,
        CheckEligibilityRes = 100,
        LifeCycleMgmtReq = 101,
        LifeCycleMgmtRes = 102,
        TokenInqReq = 103,
        TokenInqRes = 104,
        UpdateMetaDataReq = 105,
        UpdateMetaDataRes = 106,
        GeneralMsgReq = 107,
        GeneralMsgRes = 108

        , TokenNotificationAdvice_620_3700_TokenCreation = 109
        , TokenNotificationAdvice_620_3701_TokenDeactivation = 110
        , TokenNotificationAdvice_620_3702_TokenSuspend = 111
        , TokenNotificationAdvice_620_3703_TokenResume = 112
        , TokenNotificationAdvice_620_3711_DeviceProvisioningResult = 113
        , TokenNotificationAdvice_620_3712_OtpVerificationResult = 114
        , TokenNotificationAdvice_620_3713_CallCentreActivation = 115
        , TokenNotificationAdvice_620_3714_MobileBankingAppActivation = 116
        , TokenNotificationAdvice_620_3715_LukReplenishment = 117
        , TokenNotificationAdvice_620_3720_PanExpiryDateUpdate = 118
        , TokenNotificationAdvice_620_3721_PanReplacement = 119

        , TokenNotificationAdvice_0630_Response = 120

        , File_Maintenance_0302_3713_Call_centre_Activation = 121
        , File_Maintenance_0302_3702_Token_Suspend = 122
        , File_Maintenance_0302_3703_Token_Resume = 123
        , File_Maintenance_0302_3701_Token_Deactivation = 124
        , File_Maintenance_0302_PAN_Expiration_date_update = 125
        , File_Maintenance_0302_PAN_Replacement_update = 126
        , File_Maintenance_0302_All_Tokens_for_PAN = 127
        , File_Maintenance_0302_Token_Details = 128
        , File_Maintenance_0312_RESPONSE = 129

        , Token_Activation_Requst_Response_110 = 130
        , Token_Activation_Request_Advice_Response_130 = 131
        , Financial_Transaction_Request_Response_0210 = 132 // RS#128206
        , MasterCard_Authorization_Request_Response_110 = 133
        , MasterCard_Authorization_Request_Response_120 = 134
        , MasterCard_Authorization_Request_Response_130 = 135
        , MasterCard_Authorization_Request_Response_410 = 136
        , MasterCard_Authorization_Request_Response_430 = 137

        , FPS_NETWORK_MANAGEMENT_REQUEST_9804 = 138
        , FPS_NETWORK_MANAGEMENT_REQUEST_RESPONSE_9814 = 139
        , FPS_UNSOLICITED_MESSAGE_9824 = 140
        , FPS_UNSOLICITED_MESSAGE_ACKNOWLEDGEMENT_9834 = 141
        , FPS_MEMBER_PAYMENT_REQUEST_9200 = 142
        , FPS_MEMBER_PAYMENT_REQUEST_REPEAT_9201 = 143
        , FPS_MEMBER_PAYMENT_RESPONSE_9210 = 144
        , FPS_PAYMENT_REVERSAL_REQUEST_9420 = 145
        , FPS_PAYMENT_REVERSAL_REQUEST_REPEAT_9421 = 146
        , FPS_PAYMENT_REVERSAL_RESPONSE_9430 = 147
        , FPS_ADMINISTRATION_ADVICE_9624 = 148
    }

    public enum EnumPresenceNotation
    {
        Mandatory = 1,
        Conditional = 2,
        Optional = 3,
        Authorization = 4,
        Mandatory_Echo = 5,
        Conditional_Echo = 6,
        Authorization_Echo = 7,
        Not_Required = 8
    }

    public enum EnumDataElement
    {
        ProcessingCode = 3,                                     //FPS 003	PROCESSING CODE
        OriginalAmount = 4,                                     //FPS 004	ORIGINAL AMOUNT
        RejectedAmount = 5,                                     //FPS 005	REJECTED AMOUNT
        Amount = 6,                                             //FPS 006	AMOUNT
        DateNTimeTransaction = 7,                               //FPS 007	DATE AND TIME TRANSMISSION
        ExchangeRate = 10,                                      //FPS 010	EXCHANGE RATE
        MessageID = 11,                                         //FPS 011	MESSAGE ID
        DateSent = 12,                                          //FPS 012	DATE SENT
        SettlementDate = 15,                                    //FPS 015	SETTLEMENT DATE
        FunctionCode = 24,                                      //FPS 024	FUNCTION CODE
        ActionCode = 26,                                        //FPS 026	ACTION CODE for FPS
        ProcessedAsynchronously = 27,                           //FPS 027	PROCESSED ASYNCHRONOUSLY
        SettlementCycleID = 29,                                 //FPS 029	SETTLEMENT CYCLE ID
        TransactionReferenceNumber = 31,                        //FPS 031	TRANSACTION REFERENCE NUMBER
        SubmitingMamber = 32,                                   //FPS 032	SUBMITTING MAMBER
        BeneficiaryCustomerAccountNumber = 35,                  //FPS 035	BENEFICIARY CUSTOMER ACCOUNT NUMBER
        ServiceStatus = 40,                                     //FPS 040	SERVICE STATUS
        CountOfCreditIetms = 41,                                //FPS 041	COUNT OF CREDIT ITEMS
        OriginatingCreditInstitution = 42,                      //FPS 042	ORIGINATING CREDIT INSTITUTION
        OriginatingCustomerAccountNumber = 43,                  //FPS 043	ORIGINATING CUSTOMER ACCOUNT NMBER
        SyntexErrorData = 44,                                   //FPS 044	SYNTEX ERROR DATA
        ResponseTime = 45,                                      //FPS 045	RESPONSE TIME
        ChargingIformation = 46,                                //FPS 046	CHARGING INFORATION
        AgencySortCodeWithNumber = 47,                          //FPS 047	AGENCY SORT CODE WITH NUMBER
        AgencyAccountWithMember = 48,                           //FPS 048	AGENCY ACCOUNT WITH MEMBER
        OrigionalCurrency = 49,                                 //FPS 049	ORIGINAL CURRENCY
        Currency = 51,                                          //FPS 051	CURRENCY
        RedirectedBeneficaryCreditInformation = 57,             //FPS 057	REDIRECTED BENEFICARY CREDIT INFORMATION
        RedirectedBeneficaryCustomerAccountNumber = 58,         //FPS 058	REDIRECTED BENEFICARY CUSTOMER ACCOUNT NUMBER
        PaymentData = 61,                                       //FPs 061	PAYMENT DATA
        EndToEndReference = 62,                                 //FPS 062	END TO END REFERENCE
        SuspensionStatus = 67,                                  //FPS 067	SUSPENSION STATUS
        PreviousSuspensionStatus = 68,                          //FPS 068	PREVIOUS SUSPENSION STATUS
        PaymentPriorityGroupAvailablity = 69,                   //FPS 069	PAYMENT PRIORITY GROUP AVAILABLITY
        NumbericReference = 71,                                 //FPS 071	NUMBERIC REFERENCE
        FileId = 72,                                            //FPS 072	FILE ID
        SettlementIndividualTransactionLimitStatus = 85,        //FPS 085	SETTLEMENT INDIVIDUAL TRANSACTION LIMIT STATUS
        NewSettlementIndividualTransactionLimit = 86,           //FPS 086	NEW SETTLEMENT INDIVIDUAL TRANSACTION LIMIT
        ProeviousSettlementIndividualTransactionLimit = 87,     //FPS 087	PROEVIOUS SETTLEMENT INDIVIDUAL TRANSACTION LIMIT
        NewSecurityTransactionLimit = 88,                       //FPS 088	NEW SECURITY TRANSACTION LIMIT
        PreviousSecurityTransactionLimit = 89,                  //FPS 089	PREVIOUS SECURITY TRANSACTION LIMIT
        SettlementProcessAndStatus = 91,                        //FPS 091	SETTLEMENT PROCESS AND STATUS
        BeneficaryCreditInstitution = 95,                       //FPS 095	BENEFICIARY CREDIT INSTITUTION
        SendingFPSInstitution = 98,                             //FPS 098	SENDING FPS INSTITUTION
        ReceivingNumber = 99,                                   //FPS 099	RECEIVING NUMBER
        FPSInstitutionOrThirdPartyBeneficaryID = 100,           //FPS 100	FPS INSTITUTION/THIRD PARTY BENEFICARY ID
        ReceivingFPSInstitutioID = 101,                         //FPS 101	RECEIVING FPS INSTITUION / THIRD PARTY BENEFICARY ID
        NameOfFPSInstitition = 104,                             //FPS 104	NAME OF FPS INSTITUTION
        NetSenderCAP = 105,                                     //FPS 105	NET SENDER CAP
        PreviousNetSenderCAP = 106,                             //FPS 106	PREVIOUS NET SENDER CAP
        NETSenderCAPStatus = 107,                               //FPS 107	NET SENDER CAP STATUS
        SettlementManagement = 109,                             //FPS 109	SETTLEMENT MANAGEMENT
        NameOfReceivingFPSInstitution = 110,                    //FPS 110	NAME OF RECEIVING FPS INSTITUTION
        SettlementRiskManaement = 111,                          //FPS 111	SETTLEMENT RISK MANAGEMENT
        OrginatingCustomerAccountName = 116,                    //FPS 116	ORIGINATING CUSTOMER ACCOUNT NAME
        OrginatingCstomerAccountAddress = 117,                  //FPS 117	ORIGINATING CUSTOMER ACCOUNT ADDRESS
        BeneficiaryCustomerAccountName = 118,                   //FPS 118	BENEFICIARY CUSTOMER ACCOUNT NAME
        BeneficiaryCustomerAccountAddress = 119,                //FPS 119	BENEFICARY CUSTOMER ACCOUNT ADDRESS
        ReferenceInformation = 120,                             //FPS 120	REFERENCE INFORMATION
        RemittanceInformation = 121,                            //FPS 121	REMITTANCE INFORMATION
        RegulatoryReporting = 122,                              //FPS 122	REGULATORY REPORTING
        ReservedForFPSParticipantInternalUse = 123,             //FPS 123	RESERVED FOR FPS PARTICIPANT INTERNAL USE
        InfoText = 124,                                         //FPS 124	INFO TEXT
        NetworkManagementInformation = 125,                     //FPS 125	NETWORK MANAGEMENT INFORMATION
        PaymentReturnCode = 126,                                //FPS 126	PAYMENT RETURN CODE
        ReturnedPaymentFPID = 127,                              //FPS 127	RETURNED PAYMENT FPID
        MessageAuthenticationCode_128 = 128                     //FPS 128	MESSAGE AUTHENTICATION CODE
    }

    public enum EnumVisaSignStatuses
    {
        None = 0,
        Sign_On = 1,
        Sign_Off = 2,
        Test_Message = 3,
        Start_advices = 4,
        Stop_advices = 5,
        Handshake = 6,
        KeyExchange_Request = 7, // RS#128208
        KeyExchange = 8, // RS#128208
        KeyVerification = 9, // RS#128208
        CutOver = 10, // RS#128208        
        Group_Sign_On = 11,
        Group_Sign_Off = 12,
        Sign_on_submit = 13,
        MAC_verification_failure = 14,
        Sign_on_receive = 16
    }

    public enum EnumIPMDataElement
    {
        PAN = 2,
        ProcessingCode = 3,
        TransactionAmount = 4,
        SettlementAmount = 5,
        CardholderAmount = 6,
        TransactionDate = 7,
        ConversionRate = 9,
        LocalDateTime = 12,
        POSData = 22,
        CardSequenceNumber = 23,
        FunctionCode = 24,
        MessageReasonCode = 25,
        MCC = 26,
        OriginalAmount = 30,
        AcquirerReferenceData = 31,
        AcquiringInstitutionID = 32,
        ForwardingInstitutionID = 33,
        RRN = 37,
        ApprovalCode = 38,
        ServiceCode = 40,
        AcceptorTerminalID = 41,
        AcceptorIDCode = 42,
        AcceptorNameLocation = 43,
        AdditionalData = 48,
        TransactionCurrencyCode = 49,
        SettlementCurrencyCode = 50,
        CardholderCurrencyCode = 51,
        AdditionAmountData = 54,
        AdditionalData2 = 62,
        TransactionLifeCycleID = 63,
        MessageNumber = 71,
        DataRecord = 72,
        CardIssuerReferenceData = 95,
        AdditionalData3 = 123,
        AdditionalData4 = 124,
        AdditionalData5 = 125
    }

    public enum EnumResponseCode
    {
        Approved = 0,
        //Refer_card_issuer = 1,
        //Refer_card_issuer_special_condition = 2,
        Invalid_merchant = 3,
        Capture_card = 4,
        Do_not_honor = 5,
        Error = 6,
        Pickup_card_special_condition = 7,
        //Honor_with_ID = 8,
        Partial_Approval = 10,
        VIP_approval = 11,
        Invalid_transaction = 12,
        Invalid_amount = 13,
        Invalid_card_number = 14,
        Invalid_issuer = 15,
        Re_enter_transaction = 19,
        No_action_taken = 21,
        Unable_to_locate_record_account_number_missing_from_inquiry = 25,
        File_is_temporarily_unavailable = 28,
        Format_error = 30,
        Lost = 41,
        Stolen = 43,
        Closed_Account = 46,
        Insufficient_funds_over_credit_limit = 51,
        No_checking_account = 52,
        No_savings_account = 53,
        Expired_card = 54,
        Invalid_PIN = 55,
        Transaction_not_permitted_to_issuer_cardholder = 57,
        Transaction_not_permitted_to_acquirer_terminal = 58,
        Suspected_fraud = 59,
        Exceeds_withdrawal_amount_limit = 61,
        Restricted_card = 62,
        Security_violation = 63,
        Transaction_does_not_fulfill_AML_requirement = 64,
        Exceeds_withdrawal_count_limit = 65,
        Contact_Card_Issuer = 70,
        PIN_Not_Changed = 71,
        Allowable_number_of_PIN_tries_exceeded = 75,
        Non_existent_To_Account_specified = 76, // Key Synchronisation Error in vocalink
        Non_existent_From_Account_specified = 77,
        Non_existent_account_specified = 78,
        PIN_cryptographic_error_found = 81,
        Negative_CAM_dCVV_iCVV_or_CVV_results = 82,
        //Invalid_Authorization_Life_Cycle = 84,
        Not_declined = 85,
        PIN_Validation_not_possible = 86,
        Purchase_Amount_Only_No_Cash_Back_Allowed = 87,
        Cryptographic_failure = 88,
        Unacceptable_PIN_Transaction_Declined_Retry = 89,
        Authorization_System_or_issuer_system_inoperative = 91,
        Unable_to_route_transaction = 92,
        Transaction_cannot_be_completed_violation_of_law = 93,
        //Duplicate_transmission_detected = 94,
        System_error_Decline = 96,
        Surcharge_amount_not_permitted_on_Visa_cards = 97,
        Force_STIP = 98,
        Cash_service_not_available = 99,
        Cashback_request_exceeds_issuer_limit = 100,
        Decline_for_CVV2_failure = 102,
        Invalid_biller_information = 103,
        PIN_Change_Unblock_request_declined = 104,
        Unsafe_PIN = 105,
        Stop_Payment_Order = 106,
        Revocation_of_Authorization_Order = 107,
        Revocation_of_All_Authorizations_Order = 108,
        Unable_to_go_online_declined = 109,
        Forward_to_issuer = 110,
        Card_Authentication_failed = 112,
        VISA_Auth_Reversal = 113,

        VBV_Root_Element_Invalid = 114,
        VBV_Message_Element_Not_A_Defined_Message = 115,
        VBV_Required_Element_Missing = 116,
        VBV_Critical_Element_Not_Recognized = 117,
        VBV_Format_Of_One_Or_More_Element_Invalid = 118,
        VBV_Protocol_Version_Too_Old = 119,

        VBV_Acquirer_Not_Participating_In_3DSecure = 120,
        VBV_Merchant_Not_Participating_In_3DSecure = 121,
        VBV_No_Password_Was_Supplied = 122,
        VBV_Supplied_Password_Is_Not_Valid = 123,
        VBV_ISO_Code_Not_Valid = 124,
        VBV_Transaction_Data_Not_Valid = 125,
        VBV_VEReq_OR_PAReq_was_incorrectly_Routed = 126,
        VBV_Serial_Number_Can_Not_Be_Located = 127,
        VBV_Access_Denied_Invalid_Endpoint = 128,

        VBV_Transient_System_Failure = 129,
        VBV_Permanent_System_Failure = 130, // Manthan Bhatti : Case : 91983
        SCA_Require = 131, // Manthan Bhatti : Case : 91983
        PIN_Data_Require = 132, // Manthan Bhatti : Case : 91983
        Fallback_Not_Allowed = 152 // RS#109965
    }

    public enum EnumResponseCodeAction
    {
        Approved = 1,
        Valid = 2,
        Call_Issuer = 3,
        Decline = 4,
        Capture = 5,
    }

    public enum EnumCardFeature
    {
        None = 0,
        Purchase = 1,
        E_commerce = 2,
        Withdrawal = 3,
        Purchase_Cashback = 4,
        Refund_Correction = 5,
        Balance_Inquiry = 6,
        PIN_change = 7,
        Autofuel_dispense = 8,
        Bill_Payment = 9,
        E_commerce_Without_CVV2 = 10,
        Mail_Phone_Order = 11,
        Recurring_Transaction = 12,
        Quasi_Cash_Transaction = 13,
        VPP = 14,
        FIP = 15, //Financial Institution Payment
        Account_Verification = 16,
        Estimated = 17,
        EstimatedPartial = 18,
        COF = 19,
        // RS#128206 - Start
        PostOffice_Withdrawal = 20,
        PostOffice_BalanceEnquiry = 21,
        PostOffice_Deposit = 22,
        // RS#128206 - End
        Cash_Disbursement = 23, // MC
        Purchase_Manual_With_No_CVM = 24, // MC
        Account_Verification_No_CVM = 25



    }

    public enum EnumNetworkManagementCode
    {
        Sign_on_by_prefix = 1,
        Sign_off_by_prefix = 2,
        Group_sign_on_by_MasterCard_group = 61,
        Group_sign_off_by_MasterCard_group = 62,
        Group_sign_on_alternate_issuer_route = 63,
        Group_sign_off_alternate_issuer_route = 64,
        Prefix_sign_on_by_Group_primary_route = 65,
        Prefix_sign_off_by_Group_primary_route = 66,
        Prefix_sign_on_by_Group_alternate_route = 67,
        Prefix_sign_off_by_Group_alternate_route = 68,
        Sign_on_RiskFinder_by_Prefix_request = 70,
        Sign_off_RiskFinder_by_Prefix_not_request = 71,
        Member_by_Prefix_RiskFinder_from_SAF = 72,
        Host_session_activation = 81,
        Host_session_deactivation = 82,
        Encryption_key_exchange = 161,
        Solicitation_key_exchange = 162,
        Network_connection_echo_test = 270,

    }

    //public enum EnumFunctionCode
    //{
    //    First_Chargeback_Full_Amount = 450,
    //    First_Chargeback_Partial_Amount = 453,
    //    Arbitration_Chargeback_Full_Amount = 451,
    //    Arbitration_Chargeback_Partial_Amount = 454
    //}

    public enum EnumProductType
    {
        None = 0,
        MasterCard = 1,
        Visa = 2,
        Vocalink = 3,
        FPS = 4
    }

    /// <author>Keyur Parekh</author>
    /// <created>07-Mar-2012</created>
    /// <summary>
    /// Transaction Currency Enum 
    /// </summary>
    public enum EnumTransactionCurrencies
    {
        ALL = 8,
        DZD = 12,
        ADP = 20,
        AZM = 31,
        ARS = 32,
        AUD = 36,
        ATS = 40,
        BSD = 44,
        BHD = 48,
        BDT = 50,
        AMD = 51,
        BBD = 52,
        BEF = 56,
        BMD = 60,
        BTN = 64,
        BOB = 68,
        BWP = 72,
        BZD = 84,
        SBD = 90,
        BND = 96,
        BGL = 100,
        MMK = 104,
        BIF = 108,
        KHR = 116,
        CAD = 124,
        CVE = 132,
        KYD = 136,
        LKR = 144,
        CLP = 152,
        CNY = 156,
        COP = 170,
        KMF = 174,
        CRC = 188,
        HRK = 191,
        CUP = 192,
        CYP = 196,
        CZK = 203,
        DKK = 208,
        DOP = 214,
        SVC = 222,
        ETB = 230,
        ERN = 232,
        EEK = 233,
        FKP = 238,
        FJD = 242,
        FIM = 246,
        FRF = 250,
        DJF = 262,
        GMD = 270,
        DEM = 280,
        GHC = 288,
        GIP = 292,
        GRD = 300,
        GTQ = 320,
        GNF = 324,
        GYD = 328,
        HTG = 332,
        HNL = 340,
        HKD = 344,
        HUF = 348,
        ISK = 352,
        INR = 356,
        IDR = 360,
        IRR = 364,
        IQD = 368,
        IEP = 372,
        ILS = 376,
        ITL = 380,
        JMD = 388,
        JPY = 392,
        KZT = 398,
        JOD = 400,
        KES = 404,
        KPW = 408,
        KRW = 410,
        KWD = 414,
        KGS = 417,
        LAK = 418,
        LBP = 422,
        LVL = 428,
        LRD = 430,
        LYD = 434,
        LTL = 440,
        LUF = 442,
        MOP = 446,
        MGF = 450,
        MWK = 454,
        MYR = 458,
        MVR = 462,
        MTL = 470,
        MRO = 478,
        MUR = 480,
        MXN = 484,
        MNT = 496,
        MDL = 498,
        MAD = 504,
        MZM = 508,
        OMR = 512,
        NAD = 516,
        NPR = 524,
        NLG = 528,
        ANG = 532,
        AWG = 533,
        VUV = 548,
        NZD = 554,
        NIO = 558,
        NGN = 566,
        NOK = 578,
        PKR = 586,
        PAB = 590,
        PGK = 598,
        PYG = 600,
        PEN = 604,
        PHP = 608,
        PTE = 620,
        GWP = 624,
        QAR = 634,
        ROL = 642,
        RUB = 643,
        RWF = 646,
        SHP = 654,
        STD = 678,
        SAR = 682,
        SCR = 690,
        SLL = 694,
        SGD = 702,
        SKK = 703,
        VND = 704,
        SIT = 705,
        SOS = 706,
        ZAR = 710,
        ZWD = 716,
        ESP = 724,
        SDD = 736,
        SRG = 740,
        SZL = 748,
        SEK = 752,
        CHF = 756,
        SYP = 760,
        THB = 764,
        TOP = 776,
        TTD = 780,
        AED = 784,
        TND = 788,
        TMM = 795,
        UGX = 800,
        MKD = 807,
        RUR = 810,
        EGP = 818,
        GBP = 826,
        TZS = 834,
        USD = 840,
        UYU = 858,
        UZS = 860,
        VEB = 862,
        WST = 882,
        YER = 886,
        CSD = 891,
        ZMK = 894,
        TWD = 901,
        BYN = 933,
        ZWR = 935,
        GHS = 936,
        VEF = 937,
        SDG = 938,
        RSD = 941,
        MZN = 943,
        AZN = 944,
        RON = 946,
        TRY = 949,
        XAF = 950,
        XCD = 951,
        XOF = 952,
        XPF = 953,
        XDR = 960,
        SRD = 968,
        MGA = 969,
        AFN = 971,
        TJS = 972,
        AOA = 973,
        BYR = 974,
        BGN = 975,
        CDF = 976,
        BAM = 977,
        EUR = 978,
        MXV = 979,
        UAH = 980,
        GEL = 981,
        BOV = 984,
        PLN = 985,
        BRL = 986,
        CLF = 990,
        USN = 997,
        USS = 998,
    }

    /// <author>Nidhi Thakrar</author>
    /// <created>09-Feb-2015</created>
    /// <summary>EnumModeOfFund</summary>
    public enum EnumModeOfFund
    {
        None = 0,
        Cash = 1,
        Check = 2,
        Card = 3
    }

    /// <author>Nidhi Thakrar</author>
    /// <created>30-Sep-16</created>
    /// <summary>Message Reason code enum</summary>
    public enum EnumMessageReasonCode
    {
        None = 0,
        Transaction_Voided_by_customer = 2501,
        Transaction_not_completed = 2502,
        No_confirmation_from_point_of_service = 2503,
        Partial_dispense_by_ATM_misdispense_or_POS_partial_reversal = 2504,
        Token_create = 3700,
        Token_deactivate = 3701,
        Token_suspend = 3702,
        Token_resume = 3703,
        Device_provisioning_result = 3711,
        OTP_verification_result = 3712,
        Call_centre_activation_result = 3713,
        Mobile_banking_app_activation = 3714,
        Replenishment_confirmation_of_limited_use_keys = 3715,
        PAN_expiry_update = 3720,
        PAN_update = 3721,
        Device_Provisioning_Update_Results = 3730,
        Incremental_Authorization = 3900,
        Resubmission = 3901,
        Delayed_Charges = 3902,
        Reauthorization = 3903,
        No_Show = 3904,
        Account_TopUp = 3905,
        AFD_completion_Advice = 3906,// vipul 104445
        Device_Binding = 3740, //Manthan Bhatti : Case : 91715 
        Device_Binding_Results = 3741, //Manthan Bhatti : Case : 91715 
        OTP_Verification_Result_Device_Binding = 3742, //Manthan Bhatti : Case : 91715 
        Call_Center_Step_Up_Device_Binding = 3743, //Manthan Bhatti : Case : 91715 
        Mobile_Banking_App_Device_Binding = 3744, //Manthan Bhatti : Case : 91715 
        Device_Binding_Removed = 3745, //Manthan Bhatti : Case : 91715 
        Cardholder_Verification_Results = 3751, //Manthan Bhatti : Case : 91715 
        OTP_Verification_Result_Cardholder_Verification = 3752, //Manthan Bhatti : Case : 91715 
        Call_Center_Step_Up_Cardholder_Verification = 3753, //Manthan Bhatti : Case : 91715 
        Mobile_Banking_App_Step_Up_Cardholder_Verification = 3754 //Manthan Bhatti : Case : 91715 
    }

    /// <author>Vipul Patel</author>
    /// <created>26-Nov-2018</created>
    /// <summary>BusinessApplicationIdentifier enum</summary>
    public enum BusinessApplicationIdentifier
    {
        AA = 1,
        PP = 2,
        BI = 3,
        TU = 4,
        WT = 5
    }

    /// <author>Vipul Patel</author>
    /// <created>26-Nov-2018</created>
    /// <summary>VPP Types</summary>
    public enum VPPTypes
    {
        Standard_Account_Funding = 1,
        Immediate_Account_Funding = 2,
        Original_Credit = 3,
        Fast_Funding = 4
    }

    /// <author>Manthan Bhatti</author>
    /// <created>29-July-2019</created>
    /// <summary>VPP Types</summary>
    //public enum EnumSCAVerificationType // Manthan Bhatti : Case : 91983
    //{
    //    CHECK_ISSUER_EXEMPTION = 1,
    //    CHECK_ISSUER_CONTACTLESS_TRANSACTION_EXEMPTION = 2,
    //    SCA_PERFORMED_3DSECURE_TRANSACTION = 3,
    //    SCA_EXEMPTION_3DSECURE_RBA = 4,
    //    SCA_BYDEFINITION_TOKEN_TRANSACTION = 5,
    //    SCA_BYDEFINITION_CHIP_AND_PIN_TRANSACTION = 6,
    //    SCA_BYDEFINITION_CHIP_TRANSACTION = 7,
    //    ACQUIRER_TRUSTED_BENEFICIARY_MERCHANT_EXEMPTION = 8,
    //    ACQUIRER_LOW_VALUE_EXEMPTION = 9,
    //    ACQUIRER_TRANSACTION_RISK_ANALYISS_EXEMPTION = 10,
    //    ACQUIRER_SECURE_CORPORATE_EXEMPTION = 11,
    //    OOS_MIT_EXEMPTION = 12,
    //    OOS_MCC_EXCLUSION = 13,
    //    OOS_MOTO_EXEMPTION = 14,
    //    OOS_ONE_LEG_OUT_EXEMPTION = 15,
    //    OOS_ANONYMUS_PAYMENT_EXEMPTION = 16,
    //    NON_FINANCIAL_TRANSACTION = 17,
    //    CREDIT_TRANSACTION_EXEMPTION = 18
    //}

    public enum EnumSCAType
    {
        NONE = 0
        , SCA_OUT_OF_SCOPE = 1
        , SCA_DONE = 2
        , SCA_EXEMPTIONS = 3
        , SCA_LIMIT = 4
        , SCA_TRA = 5
        , SCA_REQUIRED_BUT_NOT_DONE = 6
        , SCA_3DS_102 = 7
        , SOFT_DECLINED = 8
        , HARD_DECLINED = 9
    }

    public enum EnumSCASubType
    {
        NONE = 0
       , NON_FINANCIAL_TRANSACTION = 1
       , CREDIT_OR_REFUND_TRANSACTION = 2
       , ACCOUNT_VERIFICATION_TRANSACTION = 3
       , MERCHANT_INITIATED_TRANSACTION = 4
       , UN_ATTENDED_PARKING_TRANSACTION = 5
       , MOTO_TRANSACTION = 6
       , RECURRING_OR_INSTALMENT_TRANSACTION = 7
       , ONE_LEG_OUT = 8
       , TOKEN_TRANSACTION = 9
       , CHIP_ONLY_TRANSACTION = 10
        , CHIP_AND_PIN_TRANSACTION = 11
       , ATM_TRANSACTION = 12
       , POS_TRANSACTION = 13
       //  , THREEDSECURE_ONE_TIME_PASSCODE_TRANSACTION = 14
       , THREEDSECURE_102 = 14
       , CONTACTLESS_TRANSACTION = 15
       , LOW_VALUE_TRANSACTION = 16
       , ISSUER_RISK_BASED_TRANSACTION = 17
       , ACQUIRER_TRA = 18
       , SCA_REQUIRE = 19
       , THREEDSECURE_BIOMETRIC_AUTHENTICATION = 20
       , THREEDSECURE_OTP_AUTHENTICATION = 21
       , ECOM_WITH_CVV2 = 22
       , ECOM_WITHOUT_CVV2 = 23
        , THREEDSECURE_ATTEMPT = 24
        , THREEDSECURE_OTHER_AUTHENTICATION = 25
        , THREEDSECURE_AUTHENTCATION_METHOD_MISSING = 26
        , COF_TRANSACTION = 27
        , ECOM_Transaction = 28
        , NON_3DS_TRANSACTION = 29
    }


    /// <author>Vipul Patel</author>
    /// <created>20-July-2020</created>
    /// <summary>Key Derivation method for HSM CMD
    /// </summary>
    public enum EnumKeyDerivationMethod
    {
        EMV_2000 = 0,
        EMV_CSK = 1,
        EMV_CSK_With_Unique_Number = 2,
        MasterCard_MChip_SKD_Derivation = 3,
        VISA_VIS_15 = 4,
        AMEX = 5
    }


    /// <author>Niken Shah</author>
    /// <created>05-Aug-2020</created>
    /// <summary>Derivation types for HSM CMD
    /// </summary>
    public enum EnumDerivationType
    {
        EMV_MC_EU = 0,
        EMV_VISA = 1
    }

    public enum EnumFunctionCodeField24
    {
        Cannot_parse_message_650 = 1
        , Unable_to_generate_Scheme_Return_Payment_670 = 2
        , Action_code_not_valid_for_Return_Payment_or_Scheme_Return_Payment_671 = 3
        , Action_code_not_valid_for_Reversa_l672 = 4
        , MAC_verification_failure_690 = 5
        , Signon_to_submit_881 = 6
        , Signon_to_receive_891 = 7
        , Signoff_to_submit_882 = 8
        , Signoff_to_receive_892 = 9
        , Key_change_811 = 10
        , Request_new_key_885 = 11
        , Echo_test_831 = 12
        , Key_Verification_886 = 13
        , Service_Status_Change_971 = 14
        , FPS_Institutions_and_Third_Party_Beneficiary_Status_Change_972 = 15
        , Settlement_Risk_Net_Sender_Threshold_Status_Change_973 = 16
        , Net_Sender_Cap_Status_974 = 17
        , Settlement_Individual_Transaction_Limit_Change_975 = 18
        , Security_Transaction_Limit_Change_976 = 19
        , Net_Sender_Threshold_Change_977 = 20
        , Net_Sender_Cap_Change_978 = 21
        , Settlement_Status_979 = 22
        , Scheme_Return_Payment_Failure_980 = 23
        , ASPM_alert_message_981 = 24
    }
}

























