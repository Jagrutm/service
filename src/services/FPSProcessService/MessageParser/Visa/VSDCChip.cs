using System.Text;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    public class VSDCChip
    {

        internal const string ISSUER_SCRIPT_TEMPLATE_1 = "71";
        internal const string ISSUER_SCRIPT_TEMPLATE_2 = "72";
        internal const string APPLICATION_INTERCHANGE_PROFILE = "82";
        internal const string ISSUER_AUTHENTICATION_DATA = "91";
        internal const string TERMINAL_VERIFICATION_RESULTS = "95";
        internal const string TRANSACTION_DATA = "9A";
        internal const string TRANSACTION_TYPE = "9C";
        internal const string SECONDARY_PIN_BLOCK = "C0";
        internal const string TRANSACTION_CURRENCY_CODE = "5F2A";
        internal const string AMOUNTAUTHORIZED = "9F02";
        internal const string AMOUNT_OTHER = "9F03";
        internal const string ISSUER_APPLICATION_DATA = "9F10";
        internal const string TERMINAL_COUNTRY_CODE = "9F1A";
        internal const string IFD_SERIAL_NUMBER = "9F1E";
        internal const string APPLICATION_CRYPTOGRAM = "9F26";
        internal const string TERMINAL_CAPABILITY_PROFILE = "9F33";
        internal const string APPLICATION_TRANSACTION_COUNTER = "9F36";
        internal const string UNPREDICTABLE_NUMBER = "9F37";
        internal const string ISSUER_SCRIPT_RESULTS = "9F5B";
        internal const string FORM_FACTOR_INDICATOR = "9F6E";
        internal const string CUSTOMER_EXCLUSIVE_DATA = "9F7C";
        internal const string TERMINALTYPE = "9F35";
        internal const string CARDHOLDER_VERIFICATION_METHOD = "9F34";
        
        internal const string VSDSCHIPDATA = "01";
        internal const string CVRDATA = "3";
        internal const string CARD_VERIFICATION_UNSUCCESSFUL = "17";
        internal const string OFFLINE_PIN_TRY_EXCEED = "19";
        internal const string ONLINE_PIN_ENTERED = "22";
        internal const string MASTERCARDFIELD = "0";



        protected const string DEFAULT_CRYPTOGRAMAMOUNT = "000000000000";
        internal const string CVNDATA = "2";
        protected const string CVN18 = "12";
        internal const int CVR_BYTE2_POSITION = 4; //IAD byte 5 is CVR byte 2, used in CVN 17
        private const int IAD_MIN_LENGTH = 4;
        EnumProductType _productType = EnumProductType.Visa;
        internal const string COUNTER1 = "5";
        internal const string COUNTER2 = "6";
        internal const string LASTATC = "7";
        //71

        //SubElementList _subList = null;

        public VSDCChip()
        {
        }

        public VSDCChip(SubElementList elementList)
        {          
            //setValues(elementList);
        }

        private void setValues(SubElementList elementList)
        {
            //SubElementList _subList = elementList;
            //if (_subList != null)
            //{
            //    //PDSElement pdsElement = ((PDSElement)_subList[((int)EnumDataElement.Integrated_Circuit_Card_Related_Data).ToString(), VSDSCHIPDATA]);
            //    if (pdsElement != null && pdsElement.IsElementExist)
            //    {
            //        SubElementList pdsList = ((PDSElement)_subList[((int)EnumDataElement.Integrated_Circuit_Card_Related_Data).ToString(), VSDSCHIPDATA]).ParsedPDSSubFieldList;
            //        if (pdsList != null)
            //        {
                        
            //            if (pdsList[VSDSCHIPDATA, ISSUER_SCRIPT_TEMPLATE_1] != null && ((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_SCRIPT_TEMPLATE_1]).IsElementExist)
            //            {
            //                this.IssuerScript1 = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_SCRIPT_TEMPLATE_1]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, ISSUER_SCRIPT_TEMPLATE_2] != null && ((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_SCRIPT_TEMPLATE_2]).IsElementExist)
            //            {
            //                this.IssuerScript2 = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_SCRIPT_TEMPLATE_2]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, APPLICATION_INTERCHANGE_PROFILE] != null && ((PDSElement)pdsList[VSDSCHIPDATA, APPLICATION_INTERCHANGE_PROFILE]).IsElementExist)
            //            {
            //                this.ApplicationInterChangeProfile = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, APPLICATION_INTERCHANGE_PROFILE]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, ISSUER_AUTHENTICATION_DATA] != null && ((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_AUTHENTICATION_DATA]).IsElementExist)
            //            {
            //                this.IssuerAuthData = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_AUTHENTICATION_DATA]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, TERMINAL_VERIFICATION_RESULTS] != null && ((PDSElement)pdsList[VSDSCHIPDATA, TERMINAL_VERIFICATION_RESULTS]).IsElementExist)
            //            {
            //                this.TerminalVerificationResults = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, TERMINAL_VERIFICATION_RESULTS]).FieldValueinByte);

            //                SubFieldList list = ((PDSElement)pdsList[VSDSCHIPDATA, TERMINAL_VERIFICATION_RESULTS]).ParsedSubFieldList;
            //                if (list != null)
            //                {
            //                    this.IsCardVerificationUnSuccessful = list[CARD_VERIFICATION_UNSUCCESSFUL].FieldValueInBit;
            //                    this.IsOfflineTryExceeded = list[OFFLINE_PIN_TRY_EXCEED].FieldValueInBit;
            //                    this.IsOnlinePinEntered = list[ONLINE_PIN_ENTERED].FieldValueInBit;
            //                }
            //            }
            //            if (pdsList[VSDSCHIPDATA, TRANSACTION_DATA] != null && ((PDSElement)pdsList[VSDSCHIPDATA, TRANSACTION_DATA]).IsElementExist)
            //            {
            //                this.TransactionData = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, TRANSACTION_DATA]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, TRANSACTION_TYPE] != null && ((PDSElement)pdsList[VSDSCHIPDATA, TRANSACTION_TYPE]).IsElementExist)
            //            {
            //                this.TransactionType = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, TRANSACTION_TYPE]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, SECONDARY_PIN_BLOCK] != null && ((PDSElement)pdsList[VSDSCHIPDATA, SECONDARY_PIN_BLOCK]).IsElementExist)
            //            {
            //                this.SecondaryPinBlock = ((PDSElement)pdsList[VSDSCHIPDATA, SECONDARY_PIN_BLOCK]).FieldValue;
            //                SecondaryPinBlockByte = ((PDSElement)pdsList[VSDSCHIPDATA, SECONDARY_PIN_BLOCK]).FieldValueinByte;
            //            }
            //            if (pdsList[VSDSCHIPDATA, TRANSACTION_CURRENCY_CODE] != null && ((PDSElement)pdsList[VSDSCHIPDATA, TRANSACTION_CURRENCY_CODE]).IsElementExist)
            //            {
            //                this.TransactionCurrencycode = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, TRANSACTION_CURRENCY_CODE]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, AMOUNTAUTHORIZED] != null && ((PDSElement)pdsList[VSDSCHIPDATA, AMOUNTAUTHORIZED]).IsElementExist)
            //            {

            //                this.AuthAmount = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, AMOUNTAUTHORIZED]).FieldValueinByte);
            //            }
            //            else
            //            {
            //                this.AuthAmount = DEFAULT_CRYPTOGRAMAMOUNT;
            //            }
            //            if (pdsList[VSDSCHIPDATA, AMOUNT_OTHER] != null && ((PDSElement)pdsList[VSDSCHIPDATA, AMOUNT_OTHER]).IsElementExist)
            //            {
            //                this.OtherAmount = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, AMOUNT_OTHER]).FieldValueinByte);
            //            }
            //            else
            //            {
            //                this.OtherAmount = DEFAULT_CRYPTOGRAMAMOUNT;
            //            }
            //            if (pdsList[VSDSCHIPDATA, ISSUER_APPLICATION_DATA] != null && ((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_APPLICATION_DATA]).IsElementExist)
            //            {
            //                this.IssuerApplicationdata = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_APPLICATION_DATA]).FieldValueinByte);

            //                SubFieldList list = ((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_APPLICATION_DATA]).ParsedSubFieldList;

            //                byte[] iadBytes = ((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_APPLICATION_DATA]).FieldValueinByte;
            //                if (iadBytes.Length > CVR_BYTE2_POSITION)
            //                    this.CVR_BYTE2 = iadBytes[CVR_BYTE2_POSITION].ToString("X2");

            //                if (list != null && list.Count >= IAD_MIN_LENGTH)
            //                {
            //                    this.CVR = HexEncoding.ToString(list[CVRDATA].FieldValueinByte).Trim();
            //                    this.CVN = HexEncoding.ToString(list[CVNDATA].FieldValueinByte).Trim();
            //                }

            //            }
            //            if (pdsList[VSDSCHIPDATA, TERMINAL_COUNTRY_CODE] != null && ((PDSElement)pdsList[VSDSCHIPDATA, TERMINAL_COUNTRY_CODE]).IsElementExist)
            //            {
            //                this.TerminalCountryCode = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, TERMINAL_COUNTRY_CODE]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, IFD_SERIAL_NUMBER] != null && ((PDSElement)pdsList[VSDSCHIPDATA, IFD_SERIAL_NUMBER]).IsElementExist)
            //            {
            //                this.IFDSerialNumber = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, IFD_SERIAL_NUMBER]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, APPLICATION_CRYPTOGRAM] != null && ((PDSElement)pdsList[VSDSCHIPDATA, APPLICATION_CRYPTOGRAM]).IsElementExist)
            //            {
            //                this.ApplicationCryptogram = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, APPLICATION_CRYPTOGRAM]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, TERMINAL_CAPABILITY_PROFILE] != null && ((PDSElement)pdsList[VSDSCHIPDATA, TERMINAL_CAPABILITY_PROFILE]).IsElementExist)
            //            {
            //                this.TerminalCapabilityProfile = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, TERMINAL_CAPABILITY_PROFILE]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, APPLICATION_TRANSACTION_COUNTER] != null && ((PDSElement)pdsList[VSDSCHIPDATA, APPLICATION_TRANSACTION_COUNTER]).IsElementExist)
            //            {
            //                this.ApplicationTransactionCounter = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, APPLICATION_TRANSACTION_COUNTER]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, UNPREDICTABLE_NUMBER] != null && ((PDSElement)pdsList[VSDSCHIPDATA, UNPREDICTABLE_NUMBER]).IsElementExist)
            //            {
            //                this.UnpredictableNumber = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, UNPREDICTABLE_NUMBER]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, ISSUER_SCRIPT_RESULTS] != null && ((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_SCRIPT_RESULTS]).IsElementExist)
            //            {
            //                this.IssuerScriptResults = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_SCRIPT_RESULTS]).FieldValueinByte);
            //            }
            //            if (pdsList[VSDSCHIPDATA, FORM_FACTOR_INDICATOR] != null && ((PDSElement)pdsList[VSDSCHIPDATA, FORM_FACTOR_INDICATOR]).IsElementExist)
            //            {
            //                this.FormFactorIndicator = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, FORM_FACTOR_INDICATOR]).FieldValueinByte);

            //                //SubFieldList list = ((PDSElement)pdsList[VSDSCHIPDATA, FORM_FACTOR_INDICATOR]).ParsedSubFieldList;
            //                //if (list != null)
            //                //{
            //                //    StringBuilder sbConsumerDevice = new StringBuilder();

            //                //    sbConsumerDevice.Append(list["1"].FieldValueInBit ? "1" : "0");
            //                //    sbConsumerDevice.Append(list["2"].FieldValueInBit ? "1" : "0");
            //                //    sbConsumerDevice.Append(list["3"].FieldValueInBit ? "1" : "0");
            //                //    sbConsumerDevice.Append(list["4"].FieldValueInBit ? "1" : "0");
            //                //    sbConsumerDevice.Append(list["5"].FieldValueInBit ? "1" : "0");
            //                //}
            //            }
            //            if (pdsList[VSDSCHIPDATA, CUSTOMER_EXCLUSIVE_DATA] != null && ((PDSElement)pdsList[VSDSCHIPDATA, CUSTOMER_EXCLUSIVE_DATA]).IsElementExist)
            //            {
            //                this.CustomerExclusiveData = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, CUSTOMER_EXCLUSIVE_DATA]).FieldValueinByte);
            //            }
                                                
                                                 
            //        }
            //    }
            //}
        }
               
        public string IssuerScript1 { get; set; }
        public string IssuerScript2 { get; set; }
        public string ApplicationInterChangeProfile { get; set; }
        public string IssuerAuthData { get; set; }
        public string TerminalVerificationResults { get; set; }
        public string TransactionData { get; set; }
        public string TransactionType { get; set; }
        public string SecondaryPinBlock { get; set; }
        public string TransactionCurrencycode { get; set; }
        public string AuthAmount { get; set; }
        public string OtherAmount { get; set; }
        public string IssuerApplicationdata { get; set; }
        public string TerminalCountryCode { get; set; }
        public string IFDSerialNumber { get; set; }
        public string ApplicationCryptogram { get; set; }
        public string TerminalCapabilityProfile { get; set; }
        public string ApplicationTransactionCounter { get; set; }
        public string UnpredictableNumber { get; set; }
        public string IssuerScriptResults { get; set; }
        public string FormFactorIndicator { get; set; }
        public string CustomerExclusiveData { get; set; }
        public byte[] SecondaryPinBlockByte { get; set; }
        public string CVR { get; set; }
        public string CVN { get; set; }
        public bool IsCardVerificationUnSuccessful { get; set; }
        public bool IsOfflineTryExceeded { get; set; }
        public bool IsOnlinePinEntered { get; set; }
        public string CVR_BYTE2 { get; set; }
        public string TerminalType { get; set;}
        public string CVMResult { get; set; }

        public string Counter1 { get; set; }

        public string Counter2 { get; set; }

        public string LastATC { get; set; }

  
        public virtual string GetARQCDataBlock()
        {
            string paddadDatablock = string.Empty;
            StringBuilder dataBlock = new StringBuilder();

            dataBlock.Append(this.AuthAmount);
            dataBlock.Append(this.OtherAmount);
            dataBlock.Append(this.TerminalCountryCode);
            dataBlock.Append(this.TerminalVerificationResults);
            dataBlock.Append(this.TransactionCurrencycode);
            dataBlock.Append(this.TransactionData);
            dataBlock.Append(this.TransactionType);

            dataBlock.Append(this.UnpredictableNumber);
            dataBlock.Append(this.ApplicationInterChangeProfile);
            dataBlock.Append(this.ApplicationTransactionCounter);
            if (this.CVN == CVN18)
                dataBlock.Append(this.IssuerApplicationdata);
            else
                dataBlock.Append(this.CVR); 
                
            paddadDatablock = dataBlock.ToString();

            int padLength = paddadDatablock.Length % 16;
            if (padLength != 0) padLength = 16 - padLength;

            if (padLength > 0) paddadDatablock = paddadDatablock.PadRight(paddadDatablock.Length + padLength, '0');

            return paddadDatablock;
        }
                
    }
}
