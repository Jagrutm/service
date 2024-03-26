using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ContisGroup.MessageParser.ISO8586Parser;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;
//using CredECard.Common.BusinessService;
namespace ContisGroup.MessageParser.MasterCard
{
    public class MCISO8583MessageParser : ISO85831993MessageParser
    {
        public EnumProductType ProductType = EnumProductType.MasterCard;
        private  byte[] _binaryDataMessage = null;
        private SubElementList _fullPDSElementList = null;
        public string ErrorField = string.Empty;

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

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public byte[] BinaryDataMessage
        {
            get
            {
                return _binaryDataMessage;
            }
            set
            {
                _binaryDataMessage = value;                
                if (_binaryDataMessage != null) _receivedMessage = HexEncoding.ToString(_binaryDataMessage);
            }
        }

        protected override void ParseReceivedMessage()
        {
            //int startField = 4;

            int headerLength = 0;
            int msgtypeLength = 4;
            int startField = headerLength + msgtypeLength;
            // Thread.Sleep(50000);
            if (_receivedMessage.Length <= startField) throw new CredECard.Common.BusinessService.ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT));
            /*
            //extracting message type from received message.
            _messageType = _receivedMessage.Substring(0, startField);

            //bit1 = ExtractBitmap(ref startField);

            _bitmap = new BitMap(_binaryDataMessage);
            startField += _bitmap.BitmapSizeinByte;
            */
            _bitmap = null;
            _messageType = HexEncoding.ConvertEbcdicToAsciiString(GetByteFromArray(_binaryDataMessage, headerLength, msgtypeLength));
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
                if (i >= _bitmap.Length) break;
                DataElement actualElement = _dataElementList[i];
                try
                {                   
                    //check for field exist in bit
                    // if (_bitmap.IsSet(actualElement.FieldNo)) GetFieldInByte(actualElement, ref startField);
                    if (_bitmap.IsSet(actualElement.FieldNo))
                        GetFieldInByte(_binaryDataMessage, actualElement, ref startField, actualElement.ElementLength);
                }
                catch(Exception ex)
                {
                    ErrorField += actualElement.FieldNo.ToString();
                   // throw new CredECard.Common.BusinessService.ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, " Error in FieldNo :  " + actualElement.FieldNo + " : " + ex.ToString());
                }
               
            }
        }

        internal  void GetFieldInByte(byte[] receivedMessage, DataElement actualElement, ref int startField, int messageDataElementLength)
        {
            if (actualElement != null)
            {
                if (actualElement.DataRepresentation == EnumDataRepresentment.Fixed)
                {
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(receivedMessage, startField, messageDataElementLength);
                    startField += messageDataElementLength;
                }
                else
                {
                    int lengthofDELength = actualElement.LengthOfDELength, dataByteLength = 0;
                    //get message length from hex string.
                    byte[] hexString = actualElement.GetBytesFromMessage(receivedMessage, startField, lengthofDELength);
                    dataByteLength = Convert.ToInt32(HexEncoding.ConvertEbcdicToAsciiString(hexString));
                    //dataByteLength = GetDataByteLength(ref actualElement, hexString);

                    startField += lengthofDELength; //get prefix length before message starts.                
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(receivedMessage, startField, dataByteLength);
                    //actualElement.ActualElementLength = dataByteLength;
                    startField += dataByteLength;
                }

                this.SubElementParsing(actualElement);

                actualElement.Validate();
            }
        }

        internal static void GetFieldInByte(byte[] receivedMessage, Element actualElement, ref int startField, int messageDataElementLength)
        {
            if (actualElement != null)
            {
                if (actualElement.DataRepresentation == EnumDataRepresentment.Fixed)
                {
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(receivedMessage, startField, messageDataElementLength);
                    startField += messageDataElementLength;
                }
                else
                {
                    int lengthofDELength = actualElement.LengthOfDELength, dataByteLength = 0;
                    //get message length from hex string.
                    byte[] hexString = actualElement.GetBytesFromMessage(receivedMessage, startField, lengthofDELength);


                    dataByteLength = Convert.ToInt32(HexEncoding.ConvertEbcdicToAsciiString(hexString));
                    //dataByteLength = GetDataByteLength(ref actualElement, hexString);

                    startField += lengthofDELength; //get prefix length before message starts.                
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(receivedMessage, startField, dataByteLength);
                    //actualElement.ActualElementLength = dataByteLength;
                    startField += dataByteLength;
                }


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


        protected override void SubElementParsing(DataElement element)
        {
            //if (element.HasSubElements)
            //{
            //    element.FullSubElementList = this._subDataElementList;
            //    element.FullSubFieldList = this._subFieldList;
            //    element.ParseElements();
            //}

            if (element.HasSubElements)
            {
                if (element.DataRepresentation == EnumDataRepresentment.PrivateDataSubElement 
                     || element.DataRepresentation == EnumDataRepresentment.PDS_With_2_Byte_header_length
                     || element.DataRepresentation == EnumDataRepresentment.PDS_Elements_with_LLLt )
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
    }


}
