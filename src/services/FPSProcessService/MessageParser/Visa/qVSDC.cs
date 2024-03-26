using CredECard.Common.Enums.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    public class qVSDC : VSDCChip
    {
        const string CVN10 = "0A";
        const string CVN17 = "11";
        const string CVN18 = "12";

        public qVSDC(SubElementList elementList )
            : base(elementList)
        {

        }

        public override string GetARQCDataBlock()
        {
            string paddadDatablock = string.Empty;

            switch (CVN)
            {
                case CVN10:
                    paddadDatablock = getCVN10DataBlock();
                    break;

                case CVN17:
                    paddadDatablock = getCVN17DataBlock();
                    break;
                case CVN18:
                    paddadDatablock = getCVN18DataBlock();
                    break;
            }

            return paddadDatablock;
        }

        private string getCVN17DataBlock()
        {
            string paddadDatablock = string.Empty;
            StringBuilder dataBlock = new StringBuilder();

            dataBlock.Append(this.AuthAmount);
            dataBlock.Append(this.UnpredictableNumber);
            dataBlock.Append(this.ApplicationTransactionCounter);
            dataBlock.Append(CVR_BYTE2);

            paddadDatablock = dataBlock.ToString();

            int padLength = paddadDatablock.Length % 16;
            if (padLength != 0) padLength = 16 - padLength;

            if (padLength > 0) paddadDatablock = paddadDatablock.PadRight(paddadDatablock.Length + padLength, '0');

            return paddadDatablock;

        }

        private string getCVN10DataBlock()
        {
            string paddadDatablock = string.Empty;
            StringBuilder dataBlock = new StringBuilder();

            string authAmt = this.AuthAmount != null ? this.AuthAmount : "000000000000";
            string othAmt = this.OtherAmount != null ? this.OtherAmount : "000000000000";
            string terCountryCd = this.TerminalCountryCode != null ? this.TerminalCountryCode : "0000";
            string tvr = this.TerminalVerificationResults != null ? this.TerminalVerificationResults : "0000000000";
            string tranCurCd = this.TransactionCurrencycode != null ? this.TransactionCurrencycode : "0000";

            string tranData = this.TransactionData != null ? this.TransactionData : "000000";
            string tranType = this.TransactionType != null ? this.TransactionType : "00";

            string unpredNum = this.UnpredictableNumber != null ? this.UnpredictableNumber : "00000000";
            string appIntCP = this.ApplicationInterChangeProfile != null ? this.ApplicationInterChangeProfile : "0000";
            string atc = this.ApplicationTransactionCounter != null ? this.ApplicationTransactionCounter : "0000";
            string cvr = this.CVR != null ? this.CVR : "00000000";


            dataBlock.Append(authAmt);
            dataBlock.Append(othAmt);
            dataBlock.Append(terCountryCd);
            dataBlock.Append(tvr);
            dataBlock.Append(tranCurCd);
            dataBlock.Append(tranData);
            dataBlock.Append(tranType);
            dataBlock.Append(unpredNum);
            dataBlock.Append(appIntCP);
            dataBlock.Append(atc);
            dataBlock.Append(cvr);

            paddadDatablock = dataBlock.ToString();

            int padLength = paddadDatablock.Length % 16;
            if (padLength != 0) padLength = 16 - padLength;

            if (padLength > 0) paddadDatablock = paddadDatablock.PadRight(paddadDatablock.Length + padLength, '0');

            return paddadDatablock;
        }

        private string getCVN18DataBlock()
        {
            string paddadDatablock = string.Empty;
            StringBuilder dataBlock = new StringBuilder();

            string authAmt = this.AuthAmount != null ? this.AuthAmount : "000000000000";
            string othAmt = this.OtherAmount != null ? this.OtherAmount : "000000000000";
            string terCountryCd = this.TerminalCountryCode != null ? this.TerminalCountryCode : "0000";
            string tvr = this.TerminalVerificationResults != null ? this.TerminalVerificationResults : "0000000000";
            string tranCurCd = this.TransactionCurrencycode != null ? this.TransactionCurrencycode : "0000";
            string tranData = this.TransactionData != null ? this.TransactionData : "000000";
            string tranType = this.TransactionType != null ? this.TransactionType : "00";
            string unpredNum = this.UnpredictableNumber != null ? this.UnpredictableNumber : "00000000";
            string appIntCP = this.ApplicationInterChangeProfile != null ? this.ApplicationInterChangeProfile : "0000";
            string atc = this.ApplicationTransactionCounter != null ? this.ApplicationTransactionCounter : "0000";
            string iad = this.IssuerApplicationdata != null ? this.IssuerApplicationdata : "00000000000000000000000000000000";


            dataBlock.Append(authAmt);
            dataBlock.Append(othAmt);
            dataBlock.Append(terCountryCd);
            dataBlock.Append(tvr);
            dataBlock.Append(tranCurCd);
            dataBlock.Append(tranData);
            dataBlock.Append(tranType);
            dataBlock.Append(unpredNum);
            dataBlock.Append(appIntCP);
            dataBlock.Append(atc);
            dataBlock.Append(iad);

            paddadDatablock = dataBlock.ToString();

            int padLength = paddadDatablock.Length % 16;
            if (padLength != 0) padLength = 16 - padLength;

            if (padLength > 0) paddadDatablock = paddadDatablock.PadRight(paddadDatablock.Length + padLength, '0');

            return paddadDatablock;
        }


    }
}
