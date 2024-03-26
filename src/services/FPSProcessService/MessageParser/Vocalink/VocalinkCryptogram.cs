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
    public class VocalinkCryptogram : VSDCChip
    {
        public VocalinkCryptogram() { }

        public int DerivationKeyIndex { get; set; } = 0;

        /// <author>Rikunj Suthar</author>
        /// <created>04-Aug-2020</created>
        /// <summary>
        /// Vocalink cryptogram constructor.
        /// </summary>
        /// <param name="dataElementList"></param>
        public VocalinkCryptogram(string cryptogram, DataElementList dataElementList)
        {
            ApplicationCryptogram = cryptogram;

            var deCryptogramAmount = dataElementList[EnumDataElement.Cryptogram_Amount];
            if (deCryptogramAmount != null && deCryptogramAmount.IsElementExist)
                this.AuthAmount = deCryptogramAmount.FieldValue;

            var deCryptogramCashbackAmount = dataElementList[EnumDataElement.Cryptogram_Cashback_Amount];
            if (deCryptogramCashbackAmount != null && deCryptogramCashbackAmount.IsElementExist)
                this.OtherAmount = deCryptogramCashbackAmount.FieldValue;
            else
                this.OtherAmount = DEFAULT_CRYPTOGRAMAMOUNT;

            var termCountrCode = dataElementList[EnumDataElement.Terminal_Country_Code];
            if (termCountrCode != null && termCountrCode.IsElementExist)
                this.TerminalCountryCode = termCountrCode.FieldValue.PadLeft(4, '0');

            var tvr = dataElementList[EnumDataElement.Terminal_Verification_Results_TVR];
            if (tvr != null && tvr.IsElementExist)
                this.TerminalVerificationResults = tvr.FieldValue;

            var cryptoCurCode = dataElementList[EnumDataElement.Cryptogram_Currency_Code];
            if (cryptoCurCode != null && cryptoCurCode.IsElementExist)
                this.TransactionCurrencycode = cryptoCurCode.FieldValue.PadLeft(4, '0');

            var termTxnDate = dataElementList[EnumDataElement.Terminal_Transaction_Date];
            if (termTxnDate != null && termTxnDate.IsElementExist)
                this.TransactionData = termTxnDate.FieldValue;

            var txnType = dataElementList[EnumDataElement.Cryptogram_Transaction_Type];
            if (txnType != null && txnType.IsElementExist)
                this.TransactionType = txnType.FieldValue;

            var unpredNo = dataElementList[EnumDataElement.Unpredictable_Number];
            if (unpredNo != null && unpredNo.IsElementExist)
                this.UnpredictableNumber = unpredNo.FieldValue;

            var appInterchangeProf = dataElementList[EnumDataElement.Application_Interchange_Profile];
            if (appInterchangeProf != null && appInterchangeProf.IsElementExist)
                this.ApplicationInterChangeProfile = appInterchangeProf.FieldValue;

            var atc = dataElementList[EnumDataElement.Application_Transaction_Counter];
            if (atc != null && atc.IsElementExist)
                this.ApplicationTransactionCounter = atc.FieldValue;

            var issuerAppData = dataElementList[EnumDataElement.Visa_Discretionary_Data];
            if (issuerAppData != null && issuerAppData.IsElementExist)
            {
                this.IssuerApplicationdata = issuerAppData.FieldValue;

                int.TryParse(this.IssuerApplicationdata.Substring(2, 2), out int derivationKeyIndex);

                this.DerivationKeyIndex = derivationKeyIndex;
                this.CVN = this.IssuerApplicationdata.Substring(4, 2);
                this.CVR = this.IssuerApplicationdata.Substring(6, 8);
            }   
        }

        /// <author>Rikunj Suthar</author>
        /// <created>04-Aug-2020</created>
        /// <summary>
        /// Get cryptogram data block
        /// </summary>
        /// <returns></returns>
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
