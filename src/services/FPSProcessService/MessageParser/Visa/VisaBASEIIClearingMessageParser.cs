using System;
using System.Text;
using ContisGroup.MessageParser.ISO8586Parser;
using ContisGroup.MessageParser.ISO8586Parser.Interface;
using CredECard.Common.Enums.Authorization;

namespace MastercardParser.Visa
{
    /// <author>Sapan Patibandha</author>
    /// <created>30-Aug-2011</created>
    /// <summary>
    /// </summary>
    [Serializable()]
    public class VisaBASEIIClearingMessageParser : IMessageFormatter
    {
        private ClearingDataElementsList _dataelementList = null;
        private SubElementList _subElementList = null;
        private TransactionCode _transactionCode = null;
        private TransactionComponentRecordsList _transactionComponentRecordsList = null;
        private byte[] _binaryMessage = null;
        private string _receivedMessage = string.Empty;
        BitMap _bitmap = null;
        int _tcr = 0;

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public int TCR
        {
            get { return _tcr; }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public ClearingDataElementsList ClearingDataElements
        {
            get { return _dataelementList; }
            set { _dataelementList = value; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>01-Mar-2013</created>
        /// <summary>Gets or sets the clearing sub element list.</summary>
        public SubElementList ClearingSubElementList
        {
            get { return _subElementList; }
            set { _subElementList = value; }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public TransactionCode TransactionCode
        {
            get { return _transactionCode; }
            set { _transactionCode = value; }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public TransactionComponentRecordsList TCRList
        {
            get { return _transactionComponentRecordsList; }
            set { _transactionComponentRecordsList = value; }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public byte[] BinaryDataMessage
        {
            get { return _binaryMessage; }
            set
            {
                _binaryMessage = value;
                if (_binaryMessage != null) _receivedMessage = Encoding.Default.GetString(_binaryMessage);
            }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        public void ParseMessage(string reportSubGroupNo = "")
        {
            _bitmap = new BitMap(_binaryMessage);

            int iTC = TransactionCode._transactionCodeID;

            if (iTC == (int)EnumTransactionCode.File_Header_90
                || iTC == (int)EnumTransactionCode.Batch_Trailer_91
                || iTC == (int)EnumTransactionCode.File_Trailer_92)
                _dataelementList = ClearingDataElementsList.GetElementListByTC(_dataelementList.Clone(), iTC).Clone();
            else if (iTC == (int)EnumTransactionCode.Member_Settlement_Data_46)
            {
                string TCRCode = _receivedMessage.Substring(1, 1);
                //string reportSubGroupNo = _receivedMessage.Substring(57, 1); // FIX for TC46
                short subGroupID = getReportSubGroupID(reportSubGroupNo);

                TransactionComponentRecords objTCR = TCRList[TCRCode];

                if (objTCR != null) _tcr = TCRList[TCRCode]._tcrID;

                // Get the element list based on ReportSubGroup and TCR.
                _dataelementList = ClearingDataElementsList.GetElementListByTCAndTCR(_dataelementList.Clone(), iTC, _tcr, subGroupID).Clone();

            }
            else
            {
                string sTCRCode = _receivedMessage.Substring(1, 1);

                TransactionComponentRecords objTCR = TCRList[sTCRCode];

                if (objTCR != null)
                    _tcr = TCRList[sTCRCode]._tcrID;

                _dataelementList = ClearingDataElementsList.GetElementListByTCAndTCR(_dataelementList.Clone(), iTC, _tcr).Clone();
            }

            if (_dataelementList != null)
            {
                for (int i = 0; i < _dataelementList.Count; i++)
                {
                    if (i >= _bitmap.Length) break;

                    ClearingDataElements actualElement = _dataelementList[i];

                    int startField = actualElement.StartPosition;

                    GetFieldInByte(actualElement, ref startField);

                    if (actualElement.HasSubElements && actualElement.FieldValue.Trim() != string.Empty)
                    {
                         actualElement.FullSubElementList = this.ClearingSubElementList;
                         actualElement.ParseElements();
                    }
                }
            }
        }

        private short getReportSubGroupID(string subGroupNo)
        {
            short subGroupID = 0;

            switch (subGroupNo)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    short.TryParse(subGroupNo, out subGroupID);

                    break;
                case "A":
                    subGroupID = 10;
                    break;                    
            }

            return subGroupID;
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <param name="actualElement">
        /// </param>
        /// <param name="startField">
        /// </param>
        private void GetFieldInByte(ClearingDataElements actualElement, ref int startField)
        {
            if (actualElement != null)
            {
                if (actualElement.DataRepresentation == EnumDataRepresentment.Fixed)
                {
                    //actualElement.FieldValue = _receivedMessage.Substring(startField, actualElement.ISODataElementLength);
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(_binaryMessage, startField, actualElement.ISODataElementLength);
                    startField += actualElement.ISODataElementLength;
                }
                else
                {
                    int actualLength = 0;
                    int.TryParse(_receivedMessage.Substring(startField, actualElement.LengthOfDELength), out actualLength);
                    startField += actualElement.LengthOfDELength; //get prefix length before message starts.
                    //actualElement.FieldValue = _receivedMessage.Substring(startField, actualLength);
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(_binaryMessage, startField, actualLength);
                    actualElement.ActualElementLength = actualLength;
                    startField += actualLength;
                }

                //actualElement.Validate();
            }
        }

    }
}
