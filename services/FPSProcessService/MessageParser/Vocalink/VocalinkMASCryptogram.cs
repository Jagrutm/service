using ContisGroup.MessageParser.ISO8586Parser;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contis.MessageParser.Vocalink
{
    /// <author>Rikunj Suthar</author>
    /// <created>04-Aug-2020</created>
    /// <summary>
    /// Vocalink cryptogram logic related class.
    /// </summary>
    public class VocalinkMASCryptogram
    {
        public VocalinkMASCryptogram() { }

        private const string CardAcceptor_Name = "1";
        private const string CardAcceptor_CityName = "2";
        private const string CardAcceptor_CountryName = "3";

        private string Pan { get; set; } = string.Empty;
        private string ProcessingCode { get; set; } = string.Empty;
        private string TransactionAmount { get; set; } = string.Empty;
        private string TraceNumber { get; set; } = string.Empty;
        private string LocalTime { get; set; } = string.Empty;
        private string LocalDate { get; set; } = string.Empty;
        private string CardExpiryDate { get; set; } = string.Empty;
        private string PosEntryMode { get; set; } = string.Empty;
        private string PosConditionCode { get; set; } = string.Empty;
        private string AcquireInstitutionNumber { get; set; } = string.Empty;
        private string RetrievalRefereneNumber { get; set; } = string.Empty;
        private string CardAcceptorTerminalId { get; set; } = string.Empty;
        private string CardAcceptorIdCode { get; set; } = string.Empty;
        private string CardAcceptorLocation1 { get; set; } = string.Empty;
        private string CardAcceptorLocation2 { get; set; } = string.Empty;
        private string CardAcceptorLocation3 { get; set; } = string.Empty;
        private string TransactionCurrency { get; set; } = string.Empty;
        private string PosData { get; set; } = string.Empty;
        private string AccountIdentifier1 { get; set; } = string.Empty;
        private string AccountIdentifier2 { get; set; } = string.Empty;
        public string Cryptogram { get; set; } = string.Empty;
        public string KeyIndex { get; set; } = string.Empty;
        public string CVN { get; set; } = string.Empty;
        public string ApplicationTransactionCounter { get; set; } = string.Empty;

        public VocalinkMASCryptogram(string cryptogram, DataElementList dataElementList)
        {
            KeyIndex = cryptogram.Substring(0, 3);
            Cryptogram = cryptogram.Substring(3, 16);

            setOtherFields(dataElementList);
        }

        /// <author>Rikunj Suthar</author>
        /// <created>01-Sep-2020</created>
        /// <summary>
        /// Add leading zero if field value is less then required.
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        private string LeadingZero(DataElement dataElement)
        {
            string fieldValue = string.Empty;
            if (dataElement != null && dataElement.IsElementExist) fieldValue = dataElement.FieldValue;
            return fieldValue.PadLeft(dataElement.ISODataElementLength, '0');
        }

        /// <author>Rikunj Suthar</author>
        /// <created>01-Sep-2020</created>
        /// <summary>
        /// Add trailing space if field lenght is less then requied.
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        private string TrailingSpace(Element dataElement)
        {
            string fieldValue = string.Empty;
            if (dataElement != null && dataElement.IsElementExist) fieldValue = dataElement.FieldValue;
            int lenght = dataElement.ElementLength;

            if (dataElement is DataElement) lenght = ((DataElement)dataElement).ISODataElementLength;

            return fieldValue.PadRight(lenght, ' ');
        }

        private void setOtherFields(DataElementList dataElementList)
        {
            this.Pan = LeadingZero(dataElementList[EnumDataElement.PAN]);

            DataElement processingCodeElement = dataElementList[EnumDataElement.ProcessingCode];
            if (processingCodeElement != null && processingCodeElement.IsElementExist) this.ProcessingCode = processingCodeElement.FieldValue;

            DataElement deTraceNumber = dataElementList[EnumDataElement.MessageID];
            if (deTraceNumber != null && deTraceNumber.IsElementExist) this.TraceNumber = deTraceNumber.FieldValue;

            DataElement deLocalTime = dataElementList[EnumDataElement.Date_Sent];
            if (deLocalTime != null && deLocalTime.IsElementExist) this.LocalTime = deLocalTime.FieldValue;

            DataElement deLocalDate = dataElementList[EnumDataElement.LocalDate];
            if (deLocalDate != null && deLocalDate.IsElementExist) this.LocalDate = deLocalDate.FieldValue;

            DataElement deCardExpDate = dataElementList[EnumDataElement.Date_Expiration];
            if (deCardExpDate != null && deCardExpDate.IsElementExist) this.CardExpiryDate = deCardExpDate.FieldValue;

            DataElement dePOSEntryMode = dataElementList[EnumDataElement.POSEntryMode];
            if (dePOSEntryMode != null && dePOSEntryMode.IsElementExist) this.PosEntryMode = dePOSEntryMode.FieldValue;

            DataElement dePOSConditionCode = dataElementList[EnumDataElement.POSConditionCode];
            if (dePOSConditionCode != null && dePOSConditionCode.IsElementExist) this.PosConditionCode = dePOSConditionCode.FieldValue;

            this.AcquireInstitutionNumber = LeadingZero(dataElementList[EnumDataElement.SubmitingMamber]);

            DataElement deRRN = dataElementList[EnumDataElement.RRN]; 
            if (deRRN != null && deRRN.IsElementExist) this.RetrievalRefereneNumber = deRRN.FieldValue;

            this.CardAcceptorTerminalId = TrailingSpace(dataElementList[EnumDataElement.CountOfCreditIetms]);

            DataElement deCardAcceptorLocation = dataElementList[EnumDataElement.OriginatingCustomerAccountNumber];

            if (deCardAcceptorLocation != null && deCardAcceptorLocation.IsElementExist)
            {
                SubElementList objSubList = (SubElementList)deCardAcceptorLocation.ParsedSubElementList;

                if (objSubList != null)
                {
                    this.CardAcceptorLocation1 = TrailingSpace((SubDataElement)objSubList[CardAcceptor_Name]);
                    this.CardAcceptorLocation2 = TrailingSpace((SubDataElement)objSubList[CardAcceptor_CityName]);
                    this.CardAcceptorLocation3 = TrailingSpace((SubDataElement)objSubList[CardAcceptor_CountryName]);
                }
            }

            DataElement deTransactionCurrency = dataElementList[EnumDataElement.TransactionCurrencyCode];
            if (deTransactionCurrency != null && deTransactionCurrency.IsElementExist) this.TransactionCurrency = deTransactionCurrency.FieldValue;

            this.PosData = TrailingSpace(dataElementList[EnumDataElement.PaymentData]);
            this.AccountIdentifier1 = TrailingSpace(dataElementList[EnumDataElement.Account_Identification_1]);            

            DataElement deAppTransactionCounter = dataElementList[EnumDataElement.Application_Transaction_Counter];
            if (deAppTransactionCounter != null && deAppTransactionCounter.IsElementExist) this.ApplicationTransactionCounter = deAppTransactionCounter.FieldValue;
        }

        /// <author>Rikunj Suthar</author>
        /// <created>04-Aug-2020</created>
        /// <summary>
        /// Get cryptogram data block
        /// </summary>
        /// <returns></returns>
        public string GetARQCCryptogramDataBlock()
        {
            var fields = new StringBuilder();

            fields.Append(this.Pan);
            fields.Append(this.ProcessingCode);            
            fields.Append(this.TraceNumber);
            fields.Append(this.LocalTime);
            fields.Append(this.LocalDate);
            fields.Append(this.CardExpiryDate);
            fields.Append(this.PosEntryMode);
            fields.Append(this.PosConditionCode);
            fields.Append(this.AcquireInstitutionNumber);
            fields.Append(this.RetrievalRefereneNumber);
            fields.Append(this.CardAcceptorTerminalId);
            fields.Append(this.CardAcceptorLocation1);
            fields.Append(this.CardAcceptorLocation2);
            fields.Append(this.CardAcceptorLocation3);
            fields.Append(this.TransactionCurrency);
            fields.Append(this.PosData);
            fields.Append(this.AccountIdentifier1);

            string messageBlock = fields.ToString();

            int messageLength = messageBlock.Length % 16;
            if (messageLength != 0) messageLength = 16 - messageLength;

            if (messageLength > 0) messageBlock = messageBlock.PadRight(messageBlock.Length + messageLength, '0');

            return messageBlock;
        }
    }
}
