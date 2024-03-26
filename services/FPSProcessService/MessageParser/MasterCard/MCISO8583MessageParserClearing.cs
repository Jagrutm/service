using ContisGroup.MessageParser.ISO8586Parser;
using CredECard.Common.BusinessService;
using System.Text;

namespace ContisGroup.MessageParser.MasterCard
{
    public class MCISO8583MessageParserClearing : ISO85831993MessageParser
    {
        protected override void ParseReceivedMessage()
        {
            int startField = 4;

            if (_receivedMessage.Length <= startField) throw new CredECard.Common.BusinessService.ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT));

            //extracting message type from received message.
            _messageType = _receivedMessage.Substring(0, startField);

            //if (_dataElementList != null && _dataElementList.Count > 0) //C#158440 Vishal S
            //{
            //    EnumDataFormat DataFormat = _dataElementList[0].DataFormat;
            //    byte[] _messageTypeByte = System.Text.Encoding.Default.GetBytes(_messageType);
            //    if (DataFormat == EnumDataFormat.EBCDIC)
            //        _messageType = HexEncoding.ConvertEbcdicToAsciiString(_messageTypeByte);
            //}

            //bit1 = ExtractBitmap(ref startField);

            //_bitmap = new BitMap(_binaryDataMessage);
            //startField += _bitmap.BitmapSizeinByte;

            //extracting message type
            _bitmap = null;
            //_messageType = HexEncoding.ToString(GetByteFromArray(_binaryDataMessage, headerLength, msgtypeLength));
            BitMap _primaryBitMap = new BitMap(_binaryDataMessage, startField, BITMAPLENGTHINBYTE);
            BitMap _secondaryBitMap = null;
            BitMap _thirdBitMap = null;
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

            //message Parse logic
            for (int i = 1; i < _dataElementList.Count; i++)
            {
                if (i >= _bitmap.Length - 1) break;

                DataElement actualElement = _dataElementList[i];

                if (actualElement.FieldNo > 128) break;

                //check for field exist in bit
                if (_bitmap.IsSet(actualElement.FieldNo)) GetFieldInByte(actualElement, ref startField);
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <param name="actualElement">
        /// </param>
        /// <param name="startField">
        /// </param>
        private void GetFieldInByte(DataElement actualElement, ref int startField)
        {
            if (actualElement != null)
            {
                if (actualElement.DataRepresentation == EnumDataRepresentment.Fixed)
                {
                    //actualElement.FieldValue = _receivedMessage.Substring(startField, actualElement.ISODataElementLength);
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(_binaryDataMessage, startField, actualElement.ISODataElementLength);
                    startField += actualElement.ISODataElementLength;
                }
                else
                {
                    int actualLength = 0;
                    //string stractualLength = string.Empty;

                    //if (actualElement.DataFormat == EnumDataFormat.EBCDIC) //C#158440 Vishal S
                    //{
                    //    byte[] _actualLengthByte = System.Text.Encoding.Default.GetBytes(_receivedMessage.Substring(startField, actualElement.LengthOfDELength));
                    //    stractualLength = HexEncoding.ConvertEbcdicToAsciiString(_actualLengthByte);
                    //}
                    //else if (actualElement.DataFormat == EnumDataFormat.ASCII)
                    //    stractualLength = _receivedMessage.Substring(startField, actualElement.LengthOfDELength);

                    //int.TryParse(stractualLength, out actualLength);
                    int.TryParse(_receivedMessage.Substring(startField, actualElement.LengthOfDELength), out actualLength);
                    startField += actualElement.LengthOfDELength; //get prefix length before message starts.
                    //actualElement.FieldValue = _receivedMessage.Substring(startField, actualLength);
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(_binaryDataMessage, startField, actualLength);
                    actualElement.ActualElementLength = actualLength;
                    startField += actualLength;
                }

                this.SubElementParsing(actualElement);

                actualElement.Validate();
            }
        }
    }
}