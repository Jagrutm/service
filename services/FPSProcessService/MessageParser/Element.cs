using System;
using System.Text;
using System.Threading;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;



namespace ContisGroup.MessageParser.ISO8586Parser
{

    public enum EnumISOVesion
    {
        None = 0,
        ISO8583_1987 = 1,
        ISO8583_1993 = 2,
        ISO8583_2003 = 3
    }

    public enum EnumDataRepresentment
    {
        None = 0,
        Fixed = 1,
        LLVAR = 2,
        LLLVAR = 3,
        Variable_Length_Reoccured = 4,
        LTV_With_SDE_ReOccured = 5,
        PrivateDataSubElement =6,
        PDS_with_Reoccured = 7,
        VISA_Variable_Length  = 8,
        Bitmap_based_field_format = 9,
        Shared_byte_field = 10,
        Bit_only = 11,
        TLV_Variable_Length = 12,
        PDS_With_2_Byte_header_length = 13,
        Bitmap_based_field_format_LLVAR = 14, // RS#140643
        Bitmap_based_field_format_LLLVAR = 15, // RS#140643
        PDS_Elements_with_LLLt = 16
    }

    public enum EnumElementType
    {
        None = 0,
        Header = 1,
        Data = 2,
        File = 3,
        Settlement = 4,
        RejectHeader=5,
        IssuerScripts = 6
    }

    public enum EnumDataFormat
    {
        None = 0,
        EBCDIC=1,
        BCD=2,
        BIT=3,
        BCD2 = 4,
        ASCII = 5,
        HEX= 6 // RS#140643
    }

    /// <author>Prashant Soni</author>
    /// <created>26-Sep-2006</created>
    /// <summary>
    /// </summary>
    /// <exception cref="ValidateException">
    /// </exception>
    [Serializable()]
    public abstract class Element 
    {
        protected int _fieldNo = 0; //string.Empty;
        protected string _fieldValue = string.Empty;
        protected EnumDataRepresentment _dataRepresentation = EnumDataRepresentment.None;
        protected string _elementFormat = string.Empty;
        protected string _messageType = string.Empty;
        protected int _actualElementLength = 0;
        protected int _elementLength = 0;
        protected bool _hasSubElements = false;
        protected EnumISOVesion _iso8583Version = EnumISOVesion.None;
        protected string _fieldName = string.Empty;
        protected bool _isClearingSingleElement = false;
        //protected StringBuilder _logString = null;

        protected int _lengthOfDELength = 0;


        protected EnumProductType _productType = EnumProductType.None;
        internal int _dataElementID = 0;
        protected EnumDataFormat _dataFormat = EnumDataFormat.None;
        protected EnumElementType _elementType = EnumElementType.None;

        protected bool _isElementExist = false;

        protected SubFieldList _subFieldList = null;

        protected byte[] _fieldValueInByte = null;

        protected BitMap _fieldValueinBitmap = null;

        public int DataElementID
        {
            get
            {
                return _dataElementID;
            }
        }

        public EnumISOVesion ISO8583Version
        {
            get
            {
                return _iso8583Version;
            }
            set
            {
                _iso8583Version = value;
            }
        }

        public EnumProductType ProductType
        {
            get
            {
                return _productType;
            }
            set
            {
                _productType = value;
            }
        }

        public EnumElementType ElementType
        {
            get
            {
                return _elementType;
            }
            set
            {
                _elementType = value;
            }
        }

        public EnumDataFormat DataFormat
        {
            get
            {
                return _dataFormat;
            }
            set
            {
                _dataFormat = value;
            }
        }

        public bool IsClearingSingleElement
        {
            get
            {
                return _isClearingSingleElement;
            }
            set
            {
                _isClearingSingleElement = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public SubFieldList FullSubFieldList
        {
            get
            {
                return _subFieldList;
            }
            set
            {
                _subFieldList = value;
            }
        }

        /// <summary>it is a max length set by provider for specific field no.
        /// </summary>
        /// <value>
        /// </value>
        public EnumDataRepresentment DataRepresentation
        {
            get
            {
                return _dataRepresentation;
            }
            set
            {
                _dataRepresentation = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public bool HasSubElements
        {
            get
            {
                return _hasSubElements;
            }
            set
            {
                _hasSubElements = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>7-Jul-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public bool IsElementExist
        {
            get
            {
                return _isElementExist;
            }
        }

        /// <summary>it is a message format e.g "n","an" etc,
        /// </summary>
        /// <value>
        /// </value>
        public string ElementFormat
        {
            get
            {
                return _elementFormat;
            }
            set
            {
                _elementFormat = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>it is a message type, fixed or llvar or none.
        /// </summary>
        /// <value>
        /// </value>
        public string MessageType
        {
            get
            {
                return _messageType;
            }
            set
            {
                _messageType = value;
            }
        }

        public int ElementLength
        {
            get
            {
                return _elementLength;
            }
            set
            {
                _elementLength = value;
            }
        }

        public byte[] FieldValueinByte
        {
            get
            {
                return _fieldValueInByte;
            }
            set
            {
                _fieldValueInByte = value;
                _isElementExist = true;
                if (this.DataFormat == EnumDataFormat.EBCDIC)
                    this.FieldValue = HexEncoding.ConvertEbcdicToAsciiString(_fieldValueInByte);
                else if (this.DataFormat == EnumDataFormat.HEX && this.ProductType ==EnumProductType.Vocalink) // RS#140643
                {
                    this.FieldValue = Encoding.Default.GetString(_fieldValueInByte);
                    this._fieldValueInByte = HexEncoding.GetBytes(this.FieldValue);
                    _fieldValueinBitmap = new BitMap(_fieldValueInByte, this._fieldValueInByte.Length);
                }
                else if (this.DataFormat == EnumDataFormat.BCD || this.DataFormat == EnumDataFormat.BCD2|| this.DataFormat == EnumDataFormat.HEX)
                    this.FieldValue = HexEncoding.ToString(_fieldValueInByte);
                else if (this.DataFormat == EnumDataFormat.BIT)
                    _fieldValueinBitmap = new BitMap(_fieldValueInByte, this.ActualElementLength == 0 ? this.ElementLength : this.ActualElementLength); // Niken Shah Case 135732
                else if (this.DataFormat == EnumDataFormat.ASCII)
                    this.FieldValue = Encoding.Default.GetString(_fieldValueInByte);
               
                    
            }
        }

        public BitMap FieldValueinBitmap
        {
            get
            {
                return _fieldValueinBitmap;
            }
            set
            {
                _fieldValueinBitmap = value;
            }
        }

        private bool _fieldValueinBit = false;

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// This is a SubElement's allowed Length
        /// </summary>
        /// <value>
        /// </value>
        public bool FieldValueInBit
        {
            get
            {
                return _fieldValueinBit;
            }
            set
            {
                _fieldValueinBit = value;
                _isElementExist = true;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>it is a ISO8583 field no.
        /// </summary>
        /// <value>
        /// </value>
        public int FieldNo
        {
            get
            {
                return _fieldNo;
            }
            set
            {
                _fieldNo = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>it is a field value.
        /// </summary>
        /// <value>
        /// </value>
        public string FieldValue
        {
            get
            {
                return _fieldValue;
            }
            set
            {
                _fieldValue = value;
                 

            }
        }

        /// <author>Tina</author>
        /// <created>16-Aug-2010</created>
        /// <summary>it is a field name
        /// </summary>
        /// <value>string
        /// </value>
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
            }
        }

        /// <summary>it is a actual length in received message.
        /// </summary>
        /// <value>int.
        /// </value>
        public int ActualElementLength
        {
            get
            {
                return _actualElementLength;
            }
            set
            {
                _actualElementLength = value;
            }
        }

        public int LengthOfDELength
        {
            get
            {
                if (_lengthOfDELength == 0)
                {
                    if (EnumProductType.Vocalink == _productType || EnumProductType.FPS == _productType) // Changed from Visa to Vocalink and condition changed to equals
                    {
                        if (_dataRepresentation == EnumDataRepresentment.LLVAR 
                                || _dataRepresentation == EnumDataRepresentment.Bitmap_based_field_format_LLVAR)
                            _lengthOfDELength = 2;
                        else if (_dataRepresentation == EnumDataRepresentment.LLLVAR 
                                    || _dataRepresentation == EnumDataRepresentment.PrivateDataSubElement 
                                        || _dataRepresentation == EnumDataRepresentment.Bitmap_based_field_format_LLLVAR)
                            _lengthOfDELength = 3;
                        else
                            _lengthOfDELength = 0;
                    }
                    else if (EnumProductType.MasterCard == _productType)
                    {
                        if (_dataRepresentation == EnumDataRepresentment.PDS_Elements_with_LLLt)
                        {
                            _lengthOfDELength = 3;
                        }
                        else
                        {
                            _lengthOfDELength = (_dataRepresentation == EnumDataRepresentment.LLVAR ? 2 : ((_dataRepresentation == EnumDataRepresentment.LLLVAR || _dataRepresentation == EnumDataRepresentment.PrivateDataSubElement) ? 3 : 0));
                        }

                    }
                    else
                    {
                        _lengthOfDELength = ((_dataRepresentation == EnumDataRepresentment.PDS_With_2_Byte_header_length) ? 2 : 1);
                    }
                }
                return _lengthOfDELength;
            }
            set
            {
                _lengthOfDELength = value;
            }
        }

        public virtual void ParseElements()
        {
        }

        public abstract void Validate();

        public virtual byte[] GetBytesFromMessage(byte[] receivedBytes, int start, int length)
        {
            byte[] returnByte = new byte[length];
            for (int i = start,j=0; i < start + length; i++,j++)
            {
                returnByte[j] = receivedBytes[i];
            }
            return returnByte;
        }

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        protected bool VerifyMessageFormat(int elementLength)
        {
            
            bool isValid = false;
            string numericPattern = @"^(\d*)$";
            string alphaPattern = @"^([a-zA-Z]*)$";
            string alphanumericPattern = @"^([\w*])$*";
            string hexPattern = @"^([a-fA-F0-9])$*";

            //if
            //if (elementLength < _actualElementLength || _actualElementLength != _fieldValue.Length) return isValid;

            switch (_elementFormat.ToLower())
            {
                case "n":
                case "un":

                    if (this.DataFormat == EnumDataFormat.BCD || this.DataFormat == EnumDataFormat.BCD2)
                    {
                        isValid = System.Text.RegularExpressions.Regex.IsMatch(_fieldValue.Trim(), hexPattern);
                    }
                    else
                        isValid = System.Text.RegularExpressions.Regex.IsMatch(_fieldValue.Trim(), numericPattern);
                    break;
                case "a":
                    isValid = System.Text.RegularExpressions.Regex.IsMatch(_fieldValue.Trim(), alphaPattern);
                    break;
                case "an":
                case "an*":
                    isValid = System.Text.RegularExpressions.Regex.IsMatch(_fieldValue.Trim(), alphanumericPattern);
                    break;
                case "ans":
                case "b":
                case "z": // RS#135750 - For track data validation
                case "h": 
                    isValid = true; // System.Text.RegularExpressions.Regex.IsMatch(message, @"^([a-zA-Z0-9]|\s]");
                    break;
                case "x+n":
                    isValid = System.Text.RegularExpressions.Regex.IsMatch(_fieldValue.Trim(), @"^([C-D]\d*)$");
                    break;
                case "a/n":
                    isValid = System.Text.RegularExpressions.Regex.IsMatch(_fieldValue.Trim(), alphaPattern);
                    if (!isValid)
                        isValid = System.Text.RegularExpressions.Regex.IsMatch(_fieldValue.Trim(), numericPattern);
                    break;
                case "bit":
                    isValid = (_fieldValueinBit != null ? true : false);
                    break;
            }

            return isValid;
        }

        public string LogData()
        {
            return this.GetElementString();
        }

        protected virtual string GetElementString()
        {
            StringBuilder _logString = new StringBuilder();
            _logString.Append("[" + DataRepresentation.ToString() + " " );
            _logString.Append(this.ElementFormat + " " + this.ActualElementLength + "]");
            _logString.Append("\t");
            _logString.Append("[" + FieldNo.ToString().PadLeft(this.LengthOfDELength,'0') + "]");
            _logString.Append("\t");
            if (this.DataRepresentation == EnumDataRepresentment.Bit_only)
            {
                _logString.Append("[" + (this._fieldValueinBit ? "1" : "0") + "]");
            }
            else
                _logString.Append("[" + this._fieldValue + "]");
            _logString.Append("\t");
            _logString.Append("[" + this.DataFormat.ToString() + "]");
            _logString.AppendLine();

            return _logString.ToString();
           
        }

        internal static void GetFieldFromByte(byte[] receivedMessage, Element actualElement, ref int startField,int messageDataElementLength)
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
                    string hexString = HexEncoding.ToString(actualElement.GetBytesFromMessage(receivedMessage, startField, lengthofDELength));


                    //dataByteLength = HexEncoding.HexToDecimal(hexString);
                    dataByteLength =  GetDataByteLength(ref actualElement, hexString);

                    startField += lengthofDELength; //get prefix length before message starts.                
                    actualElement.FieldValueinByte = actualElement.GetBytesFromMessage(receivedMessage, startField, dataByteLength);
                    //actualElement.ActualElementLength = dataByteLength;
                    startField += dataByteLength;
                }
            }
        }

        private static int GetDataByteLength(ref Element element, string hexString)
        {

            int dataByteLength = HexEncoding.HexToDecimal(hexString);
            element.ActualElementLength = dataByteLength;

            if (element.DataFormat == EnumDataFormat.BCD)
            {
                dataByteLength += ((dataByteLength % 2 == 0) ? 0 : 1);

                dataByteLength = dataByteLength / 2;
            }

            return dataByteLength;
        }
        
        /// <summary>
        /// This method returns Element's value in Byte array. if element is fixed length then it will not append length other wise it 
        /// include length byte into returned byte array.
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ToByte()
        {
            if (this._fieldValueInByte == null) return null;

            byte[] message = null;
            if (this.DataRepresentation == EnumDataRepresentment.Fixed)
                message = this._fieldValueInByte;

            else if (this.DataRepresentation == EnumDataRepresentment.VISA_Variable_Length || this.ProductType == EnumProductType.Visa  )
            {
                int fieldLength = 0;
                

                if (this.DataFormat == EnumDataFormat.EBCDIC || this.DataFormat == EnumDataFormat.BCD2)
                {
                    fieldLength = this._fieldValueInByte.Length;
                    message = new byte[fieldLength + 1];
                    byte lengthinByte = HexEncoding.ByteToHexByte(fieldLength);
                    message[0] = lengthinByte;
                    //Array.Copy(this._fieldValueInByte, 0, message, 1, fieldLength);
                }
                else if (this.DataFormat == EnumDataFormat.BCD)
                {
                    fieldLength = this.ActualElementLength == 0 ? HexEncoding.ToString(this._fieldValueInByte).Length : this.ActualElementLength;
                    message = new byte[this._fieldValueInByte.Length + 1];
                    byte lengthinByte = HexEncoding.ByteToHexByte(fieldLength);
                    message[0] = lengthinByte;
                    //Array.Copy(this._fieldValueInByte, 0, message, 1, fieldLength);
                }
                else if (this.DataFormat == EnumDataFormat.BIT )
                {
                    string messageString = this._fieldValue.Length.ToString("00") + this._fieldValue;
                    message = Encoding.Default.GetBytes(messageString);
                    return message;
                }
                    Array.Copy(this._fieldValueInByte, 0, message, 1, this._fieldValueInByte.Length);

                //Array.Copy(lengthinByte, message, 1);

            } 
            else if (this.DataRepresentation == EnumDataRepresentment.LLVAR)
            {
                string messageString = this._fieldValue.Length.ToString("00") + this._fieldValue;                 
                message = HexEncoding.ConvertAsciiToEbcdic(messageString);
            }
            else if (this.DataRepresentation == EnumDataRepresentment.LLLVAR)
            {
                string messageString = this._fieldValue.Length.ToString("000") + this._fieldValue;
                message = HexEncoding.ConvertAsciiToEbcdic(messageString);
            }
            else if (this.DataRepresentation == EnumDataRepresentment.PrivateDataSubElement && this.ProductType == EnumProductType.MasterCard)
            {
                int len = LengthOfDELength;
                message = new byte[len + this._fieldValueInByte.Length];
                Array.Copy(HexEncoding.ConvertAsciiToEbcdic(this._fieldValueInByte.Length.ToString().PadLeft(len, '0')), 0, message, 0, len);
                Array.Copy(this._fieldValueInByte, 0, message, len, this._fieldValueInByte.Length);
            }
            else if ( this.DataRepresentation == EnumDataRepresentment.PDS_Elements_with_LLLt)
            {
                int len = LengthOfDELength;
                message = new byte[len + this._fieldValueInByte.Length];

                if (this is DataElement)
                {
                    Array.Copy(HexEncoding.ConvertAsciiToEbcdic(this._fieldValueInByte.Length.ToString().PadLeft(len, '0')), 0, message, 0, len);
                    //Array.Copy(HexEncoding.ConvertAsciiToEbcdic(((DataElement)this).TCCODE), 0, message, len, 1);
                    Array.Copy(this.FieldValueinByte, 0, message, len, this._fieldValueInByte.Length);
                }
            }

                return message;
        }
    }
}
