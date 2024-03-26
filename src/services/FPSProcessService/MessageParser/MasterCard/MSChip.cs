using System.Text;
using ContisGroup.MessageParser.ISO8586Parser;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;

namespace ContisGroup.MessageParser.MasterCard
{
    public class MSChip : VSDCChip
    {     

        public MSChip(SubElementList elementList)
        {
            setValues(elementList);
            
        }
 
        private void setValues(SubElementList elementList)
        {
            SubElementList pdsList = elementList;
            if (pdsList != null)
            {
                if (pdsList[MASTERCARDFIELD, CARDHOLDER_VERIFICATION_METHOD] != null && ((PDSElement)pdsList[VSDSCHIPDATA, CARDHOLDER_VERIFICATION_METHOD]).IsElementExist)
                {
                    this.CVMResult = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, CARDHOLDER_VERIFICATION_METHOD]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, TERMINALTYPE] != null && ((PDSElement)pdsList[VSDSCHIPDATA, TERMINALTYPE]).IsElementExist)
                {
                    this.TerminalType = HexEncoding.ToString(((PDSElement)pdsList[VSDSCHIPDATA, TERMINALTYPE]).FieldValueinByte);
                }

                if (pdsList[MASTERCARDFIELD, ISSUER_SCRIPT_TEMPLATE_1] != null && ((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_SCRIPT_TEMPLATE_1]).IsElementExist)
                {
                    this.IssuerScript1 = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_SCRIPT_TEMPLATE_1]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, ISSUER_SCRIPT_TEMPLATE_2] != null && ((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_SCRIPT_TEMPLATE_2]).IsElementExist)
                {
                    this.IssuerScript2 = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_SCRIPT_TEMPLATE_2]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, APPLICATION_INTERCHANGE_PROFILE] != null && ((PDSElement)pdsList[MASTERCARDFIELD, APPLICATION_INTERCHANGE_PROFILE]).IsElementExist)
                {
                    this.ApplicationInterChangeProfile = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, APPLICATION_INTERCHANGE_PROFILE]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, ISSUER_AUTHENTICATION_DATA] != null && ((PDSElement)pdsList[VSDSCHIPDATA, ISSUER_AUTHENTICATION_DATA]).IsElementExist)
                {
                    this.IssuerAuthData = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_AUTHENTICATION_DATA]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, TERMINAL_VERIFICATION_RESULTS] != null && ((PDSElement)pdsList[MASTERCARDFIELD, TERMINAL_VERIFICATION_RESULTS]).IsElementExist)
                {
                    this.TerminalVerificationResults = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, TERMINAL_VERIFICATION_RESULTS]).FieldValueinByte);

                    //SubFieldList list = ((PDSElement)pdsList[MASTERCARDFIELD, TERMINAL_VERIFICATION_RESULTS]).ParsedSubFieldList;
                    //if (list != null)
                    //{
                    //    this.IsCardVerificationUnSuccessful = list[CARD_VERIFICATION_UNSUCCESSFUL].FieldValueInBit;
                    //    this.IsOfflineTryExceeded = list[OFFLINE_PIN_TRY_EXCEED].FieldValueInBit;
                    //    this.IsOnlinePinEntered = list[ONLINE_PIN_ENTERED].FieldValueInBit;
                    //}
                }
                if (pdsList[MASTERCARDFIELD, TRANSACTION_DATA] != null && ((PDSElement)pdsList[MASTERCARDFIELD, TRANSACTION_DATA]).IsElementExist)
                {
                    this.TransactionData = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, TRANSACTION_DATA]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, TRANSACTION_TYPE] != null && ((PDSElement)pdsList[MASTERCARDFIELD, TRANSACTION_TYPE]).IsElementExist)
                {
                    this.TransactionType = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, TRANSACTION_TYPE]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, SECONDARY_PIN_BLOCK] != null && ((PDSElement)pdsList[MASTERCARDFIELD, SECONDARY_PIN_BLOCK]).IsElementExist)
                {
                    this.SecondaryPinBlock = ((PDSElement)pdsList[MASTERCARDFIELD, SECONDARY_PIN_BLOCK]).FieldValue;
                    SecondaryPinBlockByte = ((PDSElement)pdsList[MASTERCARDFIELD, SECONDARY_PIN_BLOCK]).FieldValueinByte;
                }
                if (pdsList[MASTERCARDFIELD, TRANSACTION_CURRENCY_CODE] != null && ((PDSElement)pdsList[MASTERCARDFIELD, TRANSACTION_CURRENCY_CODE]).IsElementExist)
                {
                    this.TransactionCurrencycode = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, TRANSACTION_CURRENCY_CODE]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, AMOUNTAUTHORIZED] != null && ((PDSElement)pdsList[MASTERCARDFIELD, AMOUNTAUTHORIZED]).IsElementExist)
                {

                    this.AuthAmount = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, AMOUNTAUTHORIZED]).FieldValueinByte);
                }
                else
                {
                    this.AuthAmount = DEFAULT_CRYPTOGRAMAMOUNT;
                }
                if (pdsList[MASTERCARDFIELD, AMOUNT_OTHER] != null && ((PDSElement)pdsList[MASTERCARDFIELD, AMOUNT_OTHER]).IsElementExist)
                {
                    this.OtherAmount = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, AMOUNT_OTHER]).FieldValueinByte);
                }
                else
                {
                    this.OtherAmount = DEFAULT_CRYPTOGRAMAMOUNT;
                }
                if (pdsList[MASTERCARDFIELD, ISSUER_APPLICATION_DATA] != null && ((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_APPLICATION_DATA]).IsElementExist)
                {
                    this.IssuerApplicationdata = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_APPLICATION_DATA]).FieldValueinByte);

                    ////////ADDED BY KEYUR START
                    SubFieldList list = ((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_APPLICATION_DATA]).ParsedSubFieldList;

                                       
                    byte[] iadBytes = ((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_APPLICATION_DATA]).FieldValueinByte;
                    if (iadBytes.Length > CVR_BYTE2_POSITION)
                        this.CVR_BYTE2 = iadBytes[CVR_BYTE2_POSITION].ToString("X2");

                    if (list != null)
                    {
                        if (list[CVRDATA] != null) this.CVR = HexEncoding.ToString(list[CVRDATA].FieldValueinByte).Trim();
                        if (list[CVNDATA] != null) this.CVN = HexEncoding.ToString(list[CVNDATA].FieldValueinByte).Trim();
                        if(list[COUNTER1] !=null)this.Counter1 = HexEncoding.ToString(list[COUNTER1].FieldValueinByte).Trim();
                        if (list[Counter2] != null) this.Counter2 = HexEncoding.ToString(list[COUNTER2].FieldValueinByte).Trim();
                        if (list[LastATC] != null) this.LastATC  = HexEncoding.ToString(list[LASTATC].FieldValueinByte).Trim();                        
                    }


                    ////////ADDED BY KEYUR END

                }
                if (pdsList[MASTERCARDFIELD, TERMINAL_COUNTRY_CODE] != null && ((PDSElement)pdsList[MASTERCARDFIELD, TERMINAL_COUNTRY_CODE]).IsElementExist)
                {
                    this.TerminalCountryCode = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, TERMINAL_COUNTRY_CODE]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, IFD_SERIAL_NUMBER] != null && ((PDSElement)pdsList[MASTERCARDFIELD, IFD_SERIAL_NUMBER]).IsElementExist)
                {
                    this.IFDSerialNumber = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, IFD_SERIAL_NUMBER]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, APPLICATION_CRYPTOGRAM] != null && ((PDSElement)pdsList[MASTERCARDFIELD, APPLICATION_CRYPTOGRAM]).IsElementExist)
                {
                    this.ApplicationCryptogram = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, APPLICATION_CRYPTOGRAM]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, TERMINAL_CAPABILITY_PROFILE] != null && ((PDSElement)pdsList[MASTERCARDFIELD, TERMINAL_CAPABILITY_PROFILE]).IsElementExist)
                {
                    this.TerminalCapabilityProfile = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, TERMINAL_CAPABILITY_PROFILE]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, APPLICATION_TRANSACTION_COUNTER] != null && ((PDSElement)pdsList[MASTERCARDFIELD, APPLICATION_TRANSACTION_COUNTER]).IsElementExist)
                {
                    this.ApplicationTransactionCounter = (HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, APPLICATION_TRANSACTION_COUNTER]).FieldValueinByte));

                }
                if (pdsList[MASTERCARDFIELD, UNPREDICTABLE_NUMBER] != null && ((PDSElement)pdsList[MASTERCARDFIELD, UNPREDICTABLE_NUMBER]).IsElementExist)
                {
                    this.UnpredictableNumber = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, UNPREDICTABLE_NUMBER]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, ISSUER_SCRIPT_RESULTS] != null && ((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_SCRIPT_RESULTS]).IsElementExist)
                {
                    this.IssuerScriptResults = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, ISSUER_SCRIPT_RESULTS]).FieldValueinByte);
                }
                if (pdsList[MASTERCARDFIELD, FORM_FACTOR_INDICATOR] != null && ((PDSElement)pdsList[MASTERCARDFIELD, FORM_FACTOR_INDICATOR]).IsElementExist)
                {
                    this.FormFactorIndicator = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, FORM_FACTOR_INDICATOR]).FieldValueinByte);

                     
                }
                if (pdsList[MASTERCARDFIELD, CUSTOMER_EXCLUSIVE_DATA] != null && ((PDSElement)pdsList[MASTERCARDFIELD, CUSTOMER_EXCLUSIVE_DATA]).IsElementExist)
                {
                    this.CustomerExclusiveData = HexEncoding.ToString(((PDSElement)pdsList[MASTERCARDFIELD, CUSTOMER_EXCLUSIVE_DATA]).FieldValueinByte);
                }

            }
        }
        
        public override string GetARQCDataBlock()
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
            dataBlock.Append(this.CVR);
            //if (this.Counter1  != string.Empty) dataBlock.Append(this.Counter1);
            //if (this.Counter2 != string.Empty) dataBlock.Append(this.Counter2);
            //if (this.LastATC != string.Empty) dataBlock.Append(this.LastATC);
            //dataBlock.Append(tempLastATC);

            paddadDatablock = dataBlock.ToString();

            return paddadDatablock;
        }
    }
}
