using System;
using System.Collections.Generic;
using System.Text;
using CredECard.Common.BusinessService;



namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Prashant Soni</author>
    /// <created>26-Sep-2006</created>
    /// <summary>
    /// </summary>
    /// <exception cref="ValidateException">
    /// </exception>
    [Serializable()]
    public abstract class SubElement :Element
    {
        //protected int _subElementLength = 0;
        protected int _multipleOf = 0;
        protected string _subElementName = string.Empty;
        private SubFieldList _parsedSubFieldList = null;
       

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This is a  subDataElements lenght of length.
        /// </summary>
        /// <value>int
        /// </value>
        public int MultipleOf
        {
            get
            {
                return _multipleOf;
            }
            set
            {
                _multipleOf = value;
            }
        }

        /// <author>Tina</author>
        /// <created>16-Aug-2010</created>
        /// <summary>This is the description for the sub element
        /// </summary>
        /// <value>string 
        /// </value>
        public string SubElementName
        {
            get
            {
                return _subElementName;
            }
            set
            {
                _subElementName = value;
            }
        }  

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public SubFieldList ParsedSubFieldList
        {
            get
            {
                return _parsedSubFieldList;
            }
            set
            {
                _parsedSubFieldList = value;
            }
        }

        protected abstract SubFieldList GetFilteredSubFieldList();
        

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        public override void ParseElements()
        {
            if (!this.HasSubElements) return;

            //if (this._dataRepresentation == EnumDataRepresentment.PrivateDataSubElement)
            //{
            //      //newElement.Validate();
            //        //_parsedSubElementList.Add(newElement); //add parant element into list and parse its sub PDS element again

            //        int startField = 0;
            //        for (; startField < newElement.FieldValueinByte.Length; )
            //        {
            //            int tagLength = 0;
            //            string tag = string.Empty;

            //            //Get tag field length either 1 byte or 2 byte
            //            {
            //                byte[] tagByte = getTagByte(newElement.FieldValueinByte, startField);
            //                tag = HexEncoding.ToString(tagByte);
            //                startField += tagByte.Length;
            //            }
            //            //Get field length either in 1 byte or 2 byte
            //            {
            //                string hexTaglength = HexEncoding.ToString(getTagLength(newElement.FieldValueinByte, startField));
            //                tagLength = HexEncoding.HexToDecimal(hexTaglength);
            //                startField += (hexTaglength.Length) / 2;
            //            }
            //            ParsePdsElement(newElement.FieldValueinByte, tagLength, tag, ref startField);
            //    return;
            //}

            SubFieldList subFields = this.GetFilteredSubFieldList(); //SubFieldList.LoadSubElementWiseSubFields(this.FullSubFieldList, this.SubDataElementID).Clone();
            int startField = 0;
            int fieldLength = 2;
            int actualLength = 0;
            int subFieldNumber = 0;
            foreach (SubField subField in subFields)
            {
                if (subField.DataRepresentation == EnumDataRepresentment.Fixed)
                {
                    //If total length of field is lesser then total sub fields then return from loop.
                    if (startField >= this.FieldValueinByte.Length) break;

                    int newLength = 0;

                    if (this.FieldValueinByte.Length < (subField.SubFieldLength + startField))
                        newLength = (this.FieldValueinByte.Length - startField);
                    else
                    {
                        newLength = subField.SubFieldLength;
                    }

                    //subField.FieldValue = this.FieldValue.Substring(startField, subField.SubFieldLength);
                    subField.FieldValueinByte = this.GetBytesFromMessage(this.FieldValueinByte, startField, newLength);
                    startField += subField.SubFieldLength;
                }
                else if (subField.DataRepresentation == EnumDataRepresentment.VISA_Variable_Length)
                {
                    int lengthofDELength = 1, dataByteLength = 0;
                    //get message length from hex string.
                    string hexString = HexEncoding.ToString(subField.GetBytesFromMessage(this.FieldValueinByte, startField, lengthofDELength));
                    dataByteLength = HexEncoding.HexToDecimal(hexString);

                    startField += lengthofDELength; //get prefix length before message starts.                
                    subField.FieldValueinByte = subField.GetBytesFromMessage(this.FieldValueinByte, startField, dataByteLength);
                    subField.ActualElementLength = dataByteLength;
                    startField += dataByteLength;
                }
                else if (subField.DataRepresentation == EnumDataRepresentment.Bit_only)
                {
                    subField.FieldValueInBit = this.FieldValueinBitmap.IsSet(subField.SubFieldNumber);
                    subField.FieldValue = (subField.FieldValueInBit ? "1" : "0");
                }
                //else if (subField.DataRepresentation == EnumDataRepresentment.Shared_byte_field)
                //{
                //    //extract element based on hexadecimal value and single byte can be shared between 2 sub fields.

                //    int newLength = 0;

                //    if (this._fieldValue.Length < (subField.ElementLength + startField))
                //        newLength = (this._fieldValue.Length - startField);
                //    else
                //    {
                //        newLength = subField.ElementLength;
                //    }
                //    subField.FieldValue = this._fieldValue.Substring(startField, newLength);
                //    startField += newLength;
                //    subField.ActualElementLength = newLength;
                //}
                else
                {
                    int.TryParse(this._fieldValue.Substring(startField, fieldLength), out subFieldNumber);//fetch first 2 digit for subelement number.

                    if (subFieldNumber != subField.SubFieldNumber) continue; //element not found in string so try for next element.

                    //newElement.SubElementNumber = subElementNumber; 
                    startField += fieldLength;
                    int.TryParse(this._fieldValue.Substring(startField, fieldLength), out actualLength);
                    startField += fieldLength;
                    //subField.FieldValue = this._fieldValue.Substring(startField, actualLength);
                    subField.FieldValueinByte = this.GetBytesFromMessage(this.FieldValueinByte, startField, actualLength);
                    subField.ActualElementLength = actualLength;
                    startField += actualLength;
                }
                if (_parsedSubFieldList == null) _parsedSubFieldList = new SubFieldList();
                subField.Validate();
                _parsedSubFieldList.Add(subField);

            }
        }

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>this method validates field value with length and message format.
        /// </summary>
        /// <exception cref="ValidateException">
        /// </exception>
        public override void Validate()
        {
            bool isValid = false;

            //
            if (this.DataFormat != EnumDataFormat.BIT && this.DataFormat != EnumDataFormat.None)
            {
                //if (_elementFormat == string.Empty || _fieldValue == string.Empty) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo); ;
                //if (this.ElementLength < _actualElementLength) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo); ;

                isValid = VerifyMessageFormat(this.ElementLength);
            }
            else
                isValid = true;

            if (!isValid) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo);
        }
    }
}
