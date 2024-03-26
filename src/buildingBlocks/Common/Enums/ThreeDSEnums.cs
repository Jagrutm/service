namespace CredECard.Common.Enums.ThreeDSecure
{
    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// Invalid request codes
    /// </summary>
    public enum EnumInvalidRequestCodes
    {
        None = 0,
        Acquirer_not_participating_In_3DSecure = 50,
        Merchant_not_participating_In_3DSecure = 51,
        No_password_was_supplied = 52,
        Supplied_password_is_not_valid = 53,
        ISO_code_not_valid = 54,
        Transaction_data_not_valid = 55,
        VEReq_OR_PAReq_was_incorrectly_Routed = 56,
        Serial_Number_cannot_be_located = 57,
        Access_denied_invalid_endpoint = 58,

        Transient_system_failure = 98,
        Permanent_system_failure = 99
    }

    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// Payment Protocol
    /// </summary>
    public enum Enum3DSPaymentProtocol
    {
        ThreeDSecure_1_0_2 = 1
    }

    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// e-commerce transaction indicator
    /// </summary>
    public enum EnumECI
    {
        Secure_ECom_Tran = 5,
        CardHolder_Not_Enrolled = 6,
        Non_Authenticated_Security_Tran = 7,
        Non_Secure_Tran = 8
    }

    //public enum EnumCardMessageType
    //{
    //    None = 0,
    //    CRReq = 18,
    //    CRRes = 19,
    //    VEReq = 20,
    //    VERes = 21,
    //    PAReq = 22,
    //    PARes = 23,
    //    PATransReq = 24,
    //    PATransRes = 25,
    //    CPReq = 26,
    //    CPRes = 27,
    //    Error = 28,
    //}

    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// Message Extension formats
    /// </summary>
    public enum EnumMessageExtensionFormats
    {
        None = 0,
        XML_DATA = 1,
        BINARY_BASE64_ENCODED = 2,
    }

    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// Device categories used in VBV transactions
    /// </summary>
    public enum EnumDeviceCategory
    {
        PC = 0, //The client environment is such that the full size messages (PAReq/PARes) will be used and the core protocol specification governs. For example, PC (HTML). (Default value)
        WAP_Device = 1, //The client is a constrained device, such as WAP phone, where the condensed messages (CPRQ/CPRS) will be used and the Extension for Mobile Internet Devices must be followed.
        Two_Way_Messaging = 2 ,// The client uses two-way messaging (SMS or USSD) and the Extension for Voice and Messaging Channels must be followed.
        Voice_Channel_And_Extension = 3 //The client uses the voice channel and the Extension for Voice and Messaging Channels must be followed. 
    }

    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// CAAV calculation algorithm
    /// </summary>
    public enum EnumCavvAlgorithm
    {
        HMAC = 0,
        CVV = 1,
        CVV_WITH_ATN = 2,
        MASTERCARD_SPA = 3
    }

    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// Error codes
    /// </summary>
    public enum Enum3DSErrorCodes
    {
        None = 0,
        Root_Element_Invalid = 1,
        Message_Element_Not_A_Defined_Message = 2,
        Required_Field_Missing = 3,
        Critical_Element_Not_Recognized = 4,
        Format_Invalid = 5,
        Protocol_Version_Too_Old = 6,

        Transient_System_Failure = 98,
        Permanent_System_Failure = 99
    }

    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// Vendor Codes
    /// </summary>
    public enum EnumVendorCodes
    {
        None = 0,
        PAN_NOT_FOUND = 1,
        INVALID_MESSAGE_ID_LENGTH = 2,
    }


    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// Out wallet data being used for enrollment and forgot password
    /// functionality
    /// </summary>
    public enum Enum3DSOutWalletData
    {
        None = 0,
        DateOfBirth = 1,
        Mobile = 2,
        Email = 3,
    }

    /// <author>Keyur Parekh</author>
    /// <created>17-Jun-2013</created>
    /// <summary>
    /// 3D-Secure features
    /// </summary>
    public enum Enum3DSFeatures
    {
        None = 0,
        ADS = 1,    //Activation During shopping
        ActivationAnyTime = 2, //Activation any Time - implement the Activation Anytime Manager message extension for VEReq messages
        PC_Device = 3, //PC-browser enabled
        WAP_Device = 4, //WAP device enabled
    }

    /// <summary>
    /// Second Factor Authetication Result Code
    /// </summary>
    public enum EnumSecondFactorAuthenticationResult
    {
        None = 0, //No second factor authentication
        VSDC_Authentication_Cryptogram_Failed = 1,
        VSDC_Authentication_Cryptogram_Passed = 2,
    }

    public enum EnumAuthenticationAction
    {
        Standard_Authentication = 0, //0 – standard authentication performed (no ADS, no FYP performed)
        ADS = 1, // 1 – ADS registration/authentication performed
        ForgotPassword = 2, // 2 – “Forgot your password” (FYP) reregistration/re-authentication performed
    }
}
