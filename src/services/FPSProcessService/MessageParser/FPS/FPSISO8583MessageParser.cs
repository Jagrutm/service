using ContisGroup.MessageParser.ISO8586Parser;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;
using DataLogging.LogWriters;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ContisGroup.MessageParser.FPS
{
    public class FPSISO8583MessageParser : ISO85831987MessageParser
    {
        private const string F60_6_TRANSACTION_INDICATOR = "6";
        private const string F63_3_ADVICE_REASON_CODE = "3";
        private const string F62_2_TRANSACTIONID = "2";
        private new const int BITMAPLENGTHINBYTE = 16;


        private SubElementList _fullPDSElementList = null;

        public SubElementList FullPDSElementList
        {
            get { return _fullPDSElementList; }
            set { _fullPDSElementList = value; }
        }

        private void StoreServiceWatchLogInFile(string messageType)
        {
            string logFilePath = Path.Combine(SimpleLogWriter.DetailLogPath, "WatchLog", "ServiceUp.log");
            var write = new SimpleLogWriter(logFilePath);
            write.LogEntry(new BasicLog("Message type: " + MessageType));
        }

        private void StoreLog(string messageLog)
        {
            if (ConfigurationManager.AppSettings.Get("logdata") == "1")
            {
                string logFilePath = $"{SimpleLogWriter.DetailLogPath}_FormattedMessages_{DateTime.Today.ToString("ddMMyyyy")}.log";
                var write = new SimpleLogWriter(logFilePath);
                write.LogEntry(new BasicLog(messageLog));
            }
        }

        protected override void ParseReceivedMessage()
        {
            StringBuilder stringBulder = new StringBuilder();

            int startField = 4;

            if (_receivedMessage.Length <= startField) throw new CredECard.Common.BusinessService.ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT));

            _messageType = _receivedMessage.Substring(0, startField);

            stringBulder.Append($"Incoming Message <-- MTI: {_messageType}");

            StoreServiceWatchLogInFile(_messageType);

            byte[] primarybitmapbyte = HexEncoding.GetBytes(_receivedMessage.Substring(startField, BITMAPLENGTHINBYTE));

            _bitmap = null;

            BitMap _primaryBitMap = new BitMap(primarybitmapbyte, 0, primarybitmapbyte.Length);
            startField += BITMAPLENGTHINBYTE;
            _primaryBitMap.Copy(ref _bitmap);

            if (_primaryBitMap != null && _primaryBitMap.IsSet(1))
            {
                byte[] secondarybitmapbyte = HexEncoding.GetBytes(_receivedMessage.Substring(startField, BITMAPLENGTHINBYTE));
                BitMap _secondaryBitMap = new BitMap(secondarybitmapbyte, 0, secondarybitmapbyte.Length);
                startField += BITMAPLENGTHINBYTE;
                _secondaryBitMap.Copy(ref _bitmap);
            }

            int _deCount = _dataElementList.Count;

            stringBulder.Append($", BitMap: {_bitmap}");

            for (int i = 0; i < _deCount; i++)
            {
                DataElement actualElement = _dataElementList[i];

                if (actualElement.FieldNo > _bitmap.Length) break;

                if (_bitmap.IsSet(actualElement.FieldNo))
                {
                    if (actualElement.FieldNo == 65) // If third bit map is set..
                    {
                        ExtractThirdBitmap(actualElement, ref startField, _bitmap);
                        stringBulder.Append($", F{actualElement.FieldNo}: {actualElement.FieldValue}");
                    }
                    else
                    {
                        GetFieldInByte(actualElement, ref startField);
                        stringBulder.Append($", F{actualElement.FieldNo}: {actualElement.FieldValue}");
                    }
                }
            }

            StoreLog(stringBulder.ToString());
        }

        private void ExtractThirdBitmap(DataElement actualElement, ref int startField, BitMap _secondaryBitMap)
        {
            byte[] thirdbitmapbyte = HexEncoding.GetBytes(_receivedMessage.Substring(startField, BITMAPLENGTHINBYTE));
            BitMap _thirdBitMap = new BitMap(thirdbitmapbyte, 0, thirdbitmapbyte.Length);
            actualElement.FieldValueinByte = thirdbitmapbyte;
            startField += BITMAPLENGTHINBYTE;
            _thirdBitMap.Copy(ref _bitmap);
        }

        private void GetFieldInByte(DataElement actualElement, ref int startField)
        {
            if (actualElement != null)
            {
                if (actualElement.DataRepresentation == EnumDataRepresentment.Fixed)
                {
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(_binaryDataMessage, startField, actualElement.ISODataElementLength);
                    startField += actualElement.ISODataElementLength;
                }
                else
                {
                    int actualLength = 0;
                    int.TryParse(_receivedMessage.Substring(startField, actualElement.LengthOfDELength), out actualLength);
                    startField += actualElement.LengthOfDELength;
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(_binaryDataMessage, startField, actualLength);
                    actualElement.ActualElementLength = actualLength;
                    startField += actualLength;
                }

                this.SubElementParsing(actualElement);

                if (actualElement.DataRepresentation == EnumDataRepresentment.Fixed || actualElement.ActualElementLength > 0)
                    actualElement.Validate();
            }
        }


        protected override void SetMessageIdentifier()
        {
            //if (_dataElementList[EnumDataElement.VIP_Private_Use_Field] != null && _dataElementList[EnumDataElement.VIP_Private_Use_Field].ParsedSubElementList != null)
            //{
            //    SubElementList objSubList = ((SubElementList)_dataElementList[EnumDataElement.VIP_Private_Use_Field].ParsedSubElementList);
            //    if ((SubDataElement)objSubList[F63_3_ADVICE_REASON_CODE] != null)
            //    {
            //        int messageReasonCodeID = 0;
            //        int.TryParse(((SubDataElement)objSubList[F63_3_ADVICE_REASON_CODE]).FieldValue, out messageReasonCodeID);
            //        _messageReasonCode = messageReasonCodeID;
            //    }
            //}

            //if (_dataElementList[EnumDataElement.AdviceReasonCode_OR_Additional_POS_Information] != null && _dataElementList[EnumDataElement.AdviceReasonCode_OR_Additional_POS_Information].IsElementExist)
            //{
            //    SubElementList objSubList = ((SubElementList)_dataElementList[EnumDataElement.AdviceReasonCode_OR_Additional_POS_Information].ParsedSubElementList);
            //    if (objSubList != null && (SubDataElement)objSubList[F60_6_TRANSACTION_INDICATOR] != null)
            //    {
            //        int transactionindicator = 0;
            //        int.TryParse(((SubDataElement)objSubList[F60_6_TRANSACTION_INDICATOR]).FieldValue, out transactionindicator);
            //        if (transactionindicator == 4)
            //            _isTokenTransaction = true;

            //    }
            //}

            if (_dataElementList[EnumDataElement.EndToEndReference] != null && _dataElementList[EnumDataElement.EndToEndReference].IsElementExist)
            {
                SubElementList objSubList = ((SubElementList)_dataElementList[EnumDataElement.EndToEndReference].ParsedSubElementList);
                if (objSubList != null && (SubDataElement)objSubList[F62_2_TRANSACTIONID] != null)
                {
                    _tranID = ((SubDataElement)objSubList[F62_2_TRANSACTIONID]).FieldValue;
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

        //public void ParseHeaderMessage()
        //{
        //    int startField = 0;
        //    //string length = HexEncoding.HexToDecimal(GetByteFromArray(_headerMessage, startField, HEADERLENGTHFIELD));

        //    int headerLength = HexEncoding.HexToDecimal(HexEncoding.ToString(GetByteFromArray(_headerMessage, startField, HEADERLENGTHFIELD)));
        //    //int.TryParse(length,out headerLength);

        //    if (headerLength >= REJECTHEADERLENGTH)
        //    {
        //        _rejectMessageHeaderList = DataElementList.GetHeaderElementList(EnumISOVesion.ISO8583_1987, EnumProductType.Vocalink, EnumElementType.RejectHeader);
        //        if (_rejectMessageHeaderList != null) ParseHeaderMessage(ref startField,ref _rejectMessageHeaderList);
        //    }

        //    ParseHeaderMessage(ref startField,ref _messageHeaderList);
        //}

        //public void ParseHeaderMessage(ref int startField,ref DataElementList headerElementList)
        //{
        //    //int startField = 0;

        //    for (int i = 0; i < headerElementList.Count; i++)
        //    {

        //        DataElement actualHeaderElement = headerElementList[i];

        //        if (actualHeaderElement.IsConditionalHeaderField && !_parseConditionalHeaderField) continue;

        //        //check for field exist in bit
        //        DataElement.GetFieldFromByte(_headerMessage, actualHeaderElement, ref startField, actualHeaderElement.ElementLength);

        //        // RS#TODO
        //        //if (actualHeaderElement.FieldNo == (int)EnumHeaderFields.HEADER_FLAG_AND_FORMAT)
        //        //{
        //        //    BitMap bitmap = new BitMap(actualHeaderElement.FieldValueinByte, actualHeaderElement.ElementLength);
        //        //    if (bitmap.IsSet(FIRSTBITOFBITMAP)) _parseConditionalHeaderField = true; 
        //        //}                
        //    }

        //}

        private void ParseRejectHeaderMessage()
        {

        }

        private static byte[] GetByteFromArray(byte[] sourceArray, int dataLength)
        {
            byte[] returnByte = new byte[dataLength];
            Array.Copy(sourceArray, returnByte, dataLength);

            return returnByte;
        }

        private static byte[] GetByteFromArray(byte[] sourceArray, int startIndex, int dataLength)
        {
            byte[] returnByte = new byte[dataLength];
            Array.Copy(sourceArray, startIndex, returnByte, 0, dataLength);

            return returnByte;
        }
    }
}
