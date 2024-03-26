using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContisGroup.MessageParser.ISO8586Parser;
using System.Collections;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;

namespace ContisGroup.MessageParser.Visa
{
    public enum EnumHeaderFields
    {
        HEADER_LENGTH=1,
        HEADER_FLAG_AND_FORMAT,
        TEXT_FORMAT,
        TOTAL_MESSAGE_LENGTH,
        DESTINATION_STATION_ID,
        SOURCE_STATION_ID,
        ROUND_TRIP_CONTROL_INFORMATION,
        BASE_I_FLAGS,
        MESSAGE_STATUS_FLAGS,
        BATCH_NUMBER,
        RESERVED,
        USER_INFORMATION,
        BITMAP,
        BITMAP_REJECT_DATA_GROUP

    }

    public class VisaISO8583MessageParser : ISO85831987MessageParser
    {
        private DataElementList _messageHeaderList = null;
        private DataElementList _rejectMessageHeaderList = null;
        private byte[] _headerMessage = null;
        private int _messageHeaderLength = 0;
        private const int HEADERLENGTHFIELD = 1;
        private const int REJECTHEADERLENGTH = 26;
        private const int FIRSTBITOFBITMAP = 1;
        private const string F60_6_TRANSACTION_INDICATOR = "6";
        private const string F63_3_ADVICE_REASON_CODE = "3";
        private const string F62_2_TRANSACTIONID = "2";
        
        private bool _parseConditionalHeaderField = false;

        public DataElementList MessageHeaderElementList
        {
            get
            {
                return _messageHeaderList;
            }
            set
            {
                _messageHeaderList = value;
            }
        }

        private SubElementList _fullPDSElementList = null;

        public SubElementList FullPDSElementList
        {
            get
            {
                return _fullPDSElementList;
            }
            set
            {
                _fullPDSElementList = value;
            }
        }

        public int MessageHeaderLength
        {
            get
            {
                return _messageHeaderLength;
            }
            set
            {
                _messageHeaderLength = value;
            }
        }

        public byte[] ReceivedHeaderMessage
        {
            get
            {
                return _headerMessage;
            }
            set
            {
                _headerMessage = value;
            }
        }

        public DataElementList RejectHeaderMessageList
        {
            get
            {
                return _rejectMessageHeaderList;
            }
        }
      

        protected override void ParseReceivedMessage()
        {
            int headerLength = 0;
            if (_messageHeaderList[HEADERLENGTHFIELD].IsElementExist)
            {
                if (_rejectMessageHeaderList != null)
                    headerLength = HexEncoding.HexToDecimal(_rejectMessageHeaderList[HEADERLENGTHFIELD.ToString()].FieldValue);

                headerLength += HexEncoding.HexToDecimal(_messageHeaderList[HEADERLENGTHFIELD.ToString()].FieldValue);
            }

            int msgtypeLength = 2;
            int startField = headerLength + msgtypeLength;

            

            if (_binaryDataMessage==null || _binaryDataMessage.Length <= startField) throw new CredECard.Common.BusinessService.ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT));

            //extracting message type 
            _bitmap = null;
            _messageType = HexEncoding.ToString(GetByteFromArray(_binaryDataMessage, headerLength, msgtypeLength));
            _primaryBitMap = new BitMap(_binaryDataMessage,startField, BITMAPLENGTHINBYTE);
            startField += BITMAPLENGTHINBYTE;
            _primaryBitMap.Copy(ref _bitmap);

            if (_primaryBitMap != null && _primaryBitMap.IsSet(1))
            {
                _secondaryBitMap = new BitMap(_binaryDataMessage, startField, BITMAPLENGTHINBYTE);
                startField += BITMAPLENGTHINBYTE;
                _secondaryBitMap.Copy(ref _bitmap);
            }
            if (_secondaryBitMap != null && _secondaryBitMap.IsSet(1))
            {
                _thirdBitMap = new BitMap(_binaryDataMessage, startField, BITMAPLENGTHINBYTE);
                startField += BITMAPLENGTHINBYTE;
                _thirdBitMap.Copy(ref _bitmap);
            }

           {
               int usageNumber = 0;
               int.TryParse(_messageHeaderList[((int)EnumHeaderFields.TEXT_FORMAT).ToString()].FieldValue, out usageNumber);
               _dataElementList = DataElementList.LoadUsagewiseDataElements(_dataElementList, usageNumber);
           }

            //_bitmap = new BitMap(_binaryDataMessage);

            //startField += _bitmap.BitmapSizeinByte;
            //message Parse logic 
            for (int i = 0; i < _dataElementList.Count; i++)
            {
                DataElement actualElement = _dataElementList[i];

                if (i >= _bitmap.Length || _bitmap.Length < (actualElement.FieldNo - 1)) break;

                //check for field exist in bit
                if (_bitmap.IsSet(actualElement.FieldNo))
                {
                    try
                    {
                        DataElement.GetFieldFromByte(_binaryDataMessage, actualElement, ref startField, actualElement.ElementLength);

                        this.SubElementParsing(actualElement);
                    }
                    catch (Exception ex)
                    {
                        throw new CredECard.Common.BusinessService.ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, " Error in FieldNo :  " + actualElement.FieldNo + " : " + ex.ToString());
                    }                    
                }
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>07-Apr-2017</created>
        /// <summary>
        /// This message set Message Reason code and Whether Token Transaction or not flag
        /// </summary>
        protected override void SetMessageIdentifier()
        {
            if (_dataElementList[EnumDataElement.VIP_Private_Use_Field] != null && _dataElementList[EnumDataElement.VIP_Private_Use_Field].ParsedSubElementList != null)
            {
                SubElementList objSubList = ((SubElementList)_dataElementList[EnumDataElement.VIP_Private_Use_Field].ParsedSubElementList);
                if ((SubDataElement)objSubList[F63_3_ADVICE_REASON_CODE] != null)
                {
                    int messageReasonCodeID = 0;
                    int.TryParse(((SubDataElement)objSubList[F63_3_ADVICE_REASON_CODE]).FieldValue, out messageReasonCodeID);
                    _messageReasonCode = messageReasonCodeID;
                }
            }

            if (_dataElementList[EnumDataElement.AdviceReasonCode_OR_Additional_POS_Information] != null && _dataElementList[EnumDataElement.AdviceReasonCode_OR_Additional_POS_Information].IsElementExist)
            {
                SubElementList objSubList = ((SubElementList)_dataElementList[EnumDataElement.AdviceReasonCode_OR_Additional_POS_Information].ParsedSubElementList);
                if (objSubList != null && (SubDataElement)objSubList[F60_6_TRANSACTION_INDICATOR] != null)
                {
                    int transactionindicator = 0;
                    int.TryParse(((SubDataElement)objSubList[F60_6_TRANSACTION_INDICATOR]).FieldValue, out transactionindicator);
                    if (transactionindicator == 4)
                        _isTokenTransaction = true;

                }
            }

            if (_dataElementList[EnumDataElement.EndToEndReference] != null && _dataElementList[EnumDataElement.EndToEndReference].IsElementExist)
            {
                SubElementList objSubList = ((SubElementList)_dataElementList[EnumDataElement.EndToEndReference].ParsedSubElementList);
                if (objSubList != null && (SubDataElement)objSubList[F62_2_TRANSACTIONID] != null)
                {
                    _tranID = ((SubDataElement)objSubList[F62_2_TRANSACTIONID]).FieldValue;
                }
            }

            if (_dataElementList[EnumDataElement.ProcessingCode] != null && _dataElementList[EnumDataElement.ProcessingCode].IsElementExist)
            {
                string ProcessingCode = _dataElementList[EnumDataElement.ProcessingCode].FieldValue;
                int processingCode = 0;
                int.TryParse(ProcessingCode.Substring(0, 2), out processingCode);
                if (processingCode == (int)EnumISOTransactionType.PurchaseRefund)
                {
                    _isMerchandiseReturns = true;
                }
                
            }
        }

        protected override void SubElementParsing(DataElement element)
        {
            if (element.HasSubElements)
            {
                if (element.DataRepresentation == EnumDataRepresentment.PrivateDataSubElement || element.DataRepresentation == EnumDataRepresentment.PDS_With_2_Byte_header_length)
                {
                    element.FullSubElementList = this._fullPDSElementList;
                }
                else
                {
                    element.FullSubElementList = this.FullSubElementList;
                }
                element.FullSubFieldList = this.FullSubFieldList;
                element.ParseElements();
            }
        }

        public void ParseHeaderMessage()
        {
            int startField = 0;
            //string length = HexEncoding.HexToDecimal(GetByteFromArray(_headerMessage, startField, HEADERLENGTHFIELD));

            int headerLength = HexEncoding.HexToDecimal(HexEncoding.ToString(GetByteFromArray(_headerMessage, startField, HEADERLENGTHFIELD)));
            //int.TryParse(length,out headerLength);

            if (headerLength >= REJECTHEADERLENGTH)
            {
                _rejectMessageHeaderList = DataElementList.GetHeaderElementList(EnumISOVesion.ISO8583_1987, EnumProductType.Visa, EnumElementType.RejectHeader);
                if (_rejectMessageHeaderList != null) ParseHeaderMessage(ref startField,ref _rejectMessageHeaderList);
            }
            
            ParseHeaderMessage(ref startField,ref _messageHeaderList);
        }

        public void ParseHeaderMessage(ref int startField,ref DataElementList headerElementList)
        {
            //int startField = 0;

            for (int i = 0; i < headerElementList.Count; i++)
            {

                DataElement actualHeaderElement = headerElementList[i];

                if (actualHeaderElement.IsConditionalHeaderField && !_parseConditionalHeaderField) continue;

                //check for field exist in bit
                DataElement.GetFieldFromByte(_headerMessage, actualHeaderElement, ref startField, actualHeaderElement.ElementLength);

                if (actualHeaderElement.FieldNo == (int)EnumHeaderFields.HEADER_FLAG_AND_FORMAT)
                {
                    BitMap bitmap = new BitMap(actualHeaderElement.FieldValueinByte, actualHeaderElement.ElementLength);
                    if (bitmap.IsSet(FIRSTBITOFBITMAP)) _parseConditionalHeaderField = true; 
                }                
            }

        }

        private void ParseRejectHeaderMessage()
        {

        }


    }
}
