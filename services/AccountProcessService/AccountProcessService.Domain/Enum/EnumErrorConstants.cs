﻿namespace AccountProcessService.Domain.Enum
{
    public enum EnumErrorConstants
    {
        SUCCESS = 0,
        INTERNAL_SERVER_ERROR = 500,
        AMOUNT_SHOULD_BE_GREATER_THAN_ZERO = 1001,
        VALID_ACCOUNT_MUST_BE_SPECIFIED = 1002,
        TRANSACTION_IS_NOT_ALLOWED = 1003,
        TRANSACTION_NOT_ALLOWED_MULTIPLE_CURRENCY = 1004,
        ACCOUNT_MUST_BE_SPECIFIED = 1005,
        INVALID_ACCOUNT_TYPE = 1006,
        INVALID_ACCOUNT_STATUS = 1007,
        DUPLICATE_TRANSACTION = 1008,
        SPECIFIED_INDEX_NOT_FOUND = 1009,
        MUST_BE_SPECIFIED = 1010,
        FILE_ALREADY_EXISTS = 1011,
        SUCCESSFULL_ERROR_REPORT = 1012,
        INVALID_FOLDER_PATH = 1013,
        MUST_BE_CS_FILE = 1014,
        MUST_BE_LESS_THAN = 1015,
        MODULE_ALREADY_REGISTERED = 1016,
        ALREADY_REQUESTED_FOR_AUTHORISATION = 1017,
        USER_ROLE_CHANGES = 1018,
        PRIVILEGES_IN_ROLES_CHANGES = 1019,
        ALREADY_EXIST = 1020,
        INVALID = 1021,
        TRANSFER_FINISHED = 1022,
        MUST_CONTAIN_DATE = 1023,
        REFERENCE_ALREADY_USED = 1024,
        ALREADY_DELETED = 1025,
        FAILED = 1026,
        DUPLICATEINSTRUCTION = 1027,
        FUND_NOT_AVAILABLE = 1033,
        DOES_NOT_EXIST_OR_INVALID_STATUS = 1028,  //"does not exist or invalid status."
        PROBLEM_IN_PERFORMING_ACTION = 1029, //"There is some problem in performing the action."
        ALREADY_REJECTED = 1030,   //"already rejected."
        INVALID_REASONCODE = 1031,    //"invalid reasoncode."
        ALREADY_EXIST_WITH_SAME_ACCOUNTNUMBER = 1032,      //"already exists with same accountnumber-sortcode."
        DDI_REJECTION_CUTOFF_TIME_EXCEEDED = 1034,    //"Cutoff time is passed for rejection."
        INVALID_MODEL_STATE = 1035,
        INVALID_PAYMENTMETHOD = 1036,    //"Invalid paymentMethod"
        INVALID_DEBTOR_ACCOUNT = 1037,  //Invalid debtor account: 
        WITHDRAW_HOLDING_ACCOUNT__NOT_SETUP_FOR_SORTCODE = 1038,  //Withdraw holding account of the institution is not setup for sortcode:
        LIMITED_NUMBER_OF_RECORDS_ARE_ALLOWED = 1039,   //Limited number of records are allowed for the operation.
        FPS_WITHDRAWAL_TRANSACTION_DESCRIPTION = 1040,
        DOES_NOT_EXIST = 1041,
        USER_ALREADY_EXISTS = 1042,
        BAD_REQUEST = 1043,
        INVALID_REQUEST = 1044,
        INVALID_SORT_CODE = 1045,
        ACCOUNT_NUMBER_EXIST = 1046,
        MISSING_INSTITUTE_ACCOUNT = 1047,
        INVALID_INSTITUTE_ACCOUNT = 1048,
        INVALID_PUBLIC_KEY = 1049,
        INVALID_THUMB_PRINT = 1050,
        INVALID_SIGNATURE = 1051,
        MISSING_SIGNATURE = 1052,
        PAYMENT_NOT_FOUND = 1053,
        INVALID_INSTITUTION = 1054,
        INVALID_ACCOUNT = 1055,
        INVALID_AMOUNT = 1056,
        INVALID_TRANSACTION_REFERENCE_NUMBER = 1057,
        INVALID_SENT_DATE = 1058,

        PAYMENT_ALREADY_RETURN = 1059,
        INVALID_PROCESS_DATE = 1060,
        DUPLICATE_RECALL_ID = 1061,
        DUPLICATE_DATE = 1062

    }
}
