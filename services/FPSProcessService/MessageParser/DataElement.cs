using System;
using System.Text;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;



namespace ContisGroup.MessageParser.ISO8586Parser
{

    /// <author>Prashant Soni</author>
    /// <created>26-Sep-2006</created>
    /// <summary>This object contains information about each data element exist into message. e.g. field no: 2 PAN, 3 - processing code etc...
    /// </summary>
    /// <exception cref="ValidateException">It validates data against its actual expected data.
    /// </exception>
    [Serializable()]
    public class DataElement : Element,ICloneable
    {

        private int _isoDELength = 0; 
        //private int _messageDEMaxLength = 0;
        private string _dataElementProperties = string.Empty;

        private SubElementList _subElementlist = null;       
        private SubElementList _parsedSubElementList = null;

        private const int PDSTAGLENGTH = 4;
        private const int PDSLENGTHOFDATALENGTH = 3;

        private bool _isLoggable = false;
        private bool _isConditionalField = false;

        private BitMap _fieldBitmap = null;

        internal int  _usageNumber = 0;
        protected internal string _tCCode = string.Empty;

               
        SubElementList parsedPDSSubElements = null;

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>
        /// </summary>
        public DataElement()
        {
            _elementType = EnumElementType.Data;
        }


        /// <author>Vipul</author>
        /// <created>23-Oct-2020</created>
        /// <summary>Get TCCODE
        /// </summary>
        /// <value>int
        /// </value>
        public string TCCODE
        {
            get
            {
                return _tCCode;
            }
             
        }
        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This is a ISO8583 DataElement. max length.
        /// </summary>
        /// <value>int
        /// </value>
        public int ISODataElementLength
        {
            get
            {
                return _isoDELength;
            }
            set
            {
                _isoDELength = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>This property contains information about which version of usage will be used for datafield. e.g. VSDC Chip data or Bitmap based field
        /// </summary>
        /// <value>int
        /// </value>
        public int UsageNumber
        {
            get
            {
                return _usageNumber;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This property provide information as if field is conditional or not
        /// Note: it is specially related to VISA header field where no such bitmap for field exist or not.
        /// </summary>
        /// <value>int
        /// </value>
        public bool IsConditionalHeaderField
        {
            get
            {
                return _isConditionalField;
            }
            set
            {
                _isConditionalField = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>it is a max length set by mastercard for specific field no.
        /// </summary>
        /// <value>
        /// </value>
        //public int MessageDataElementMaxLength
        //{
        //    get
        //    {
        //        return _messageDEMaxLength;
        //    }
        //    set
        //    {
        //        _messageDEMaxLength = value;
        //    }
        //}

        /// <author>Prashant Soni</author>
        /// <created>7-Jul-2010</created>
        /// <summary>It is a check for element loggable or not.
        /// </summary>
        /// <value>
        /// </value>
        public bool IsLoggableElement
        {
            get
            {
                return _isLoggable;
            }
            set
            {
                _isLoggable = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>It is a Full list of available subelement into system.
        /// </summary>
        /// <value>
        /// </value>
        public SubElementList FullSubElementList
        {
            get
            {
                return _subElementlist;
            }
            set
            {
                _subElementlist = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>Get Parsed subelement list.
        /// </summary>
        /// <value>
        /// </value>
        public SubElementList ParsedSubElementList
        {
            get
            {
                return _parsedSubElementList;
            }
            set
            {
                _parsedSubElementList = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This method parse subelement/pdselement from data element.
        /// </summary>
        public override void ParseElements()
        {
            if (!HasSubElements) return;

            if (_productType == EnumProductType.MasterCard && this._dataRepresentation == EnumDataRepresentment.PrivateDataSubElement)
            {
                if (this.DataFormat == EnumDataFormat.EBCDIC)
                    this.ParseMCISOVersion1PDSElement();               
                else
                    ParseMasterCardPDSElement();               
               
            }
            else if(this._dataRepresentation == EnumDataRepresentment.PDS_Elements_with_LLLt )
            {
                ParseMasterCardPDSElement_48();
            }                    
            else if (_productType == EnumProductType.Visa && (this._dataRepresentation == EnumDataRepresentment.PrivateDataSubElement || this._dataRepresentation == EnumDataRepresentment.PDS_With_2_Byte_header_length))
            {
                ParseVisaPDSElement();
            }
            else if (this.DataRepresentation == EnumDataRepresentment.Bitmap_based_field_format 
                        || this.DataRepresentation== EnumDataRepresentment.Bitmap_based_field_format_LLLVAR 
                            || this.DataRepresentation==EnumDataRepresentment.Bitmap_based_field_format_LLVAR) // RS#140643 - Introduced new for bimap+veriable lenght.
            {
                this.ParseBitmapBasedSubElements();
            }
            else
            {
                this.ParseSubDataElements();
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>This method parse the element based on a bitmap. it checks subelement existance based on bit value in bitmap.
        /// </summary>
        private void ParseBitmapBasedSubElements()
        {
            SubElementList fieldSubElements = SubElementList.LoadFieldwiseSubDataElements(_subElementlist, _dataElementID).Clone();

            if (fieldSubElements == null) return;

            SubDataElement bitmapElement =((SubDataElement)fieldSubElements[0]);
            if (bitmapElement.DataFormat == EnumDataFormat.HEX)
            {
                byte[] bitmapByte= HexEncoding.GetBytes(this.FieldValue.Substring(0,bitmapElement.ElementLength)); //first field will be  bitmap only in case of bitmap wise subfields
                _fieldBitmap = new BitMap(bitmapByte, bitmapByte.Length);
            }
            else
            {
                 //first field will be  bitmap only in case of bitmap wise subfields
                _fieldBitmap = new BitMap(this.FieldValueinByte, bitmapElement.ElementLength);
            }
                

            

            int startField = bitmapElement.ElementLength;
            for(int i=1; i < fieldSubElements.Count;i++)
            {       
                SubDataElement element = (fieldSubElements[i] as SubDataElement).Clone();
                if (_fieldBitmap.IsSet(element.SubElementNumber))
                {
                    GetFieldFromByte(this.FieldValueinByte, element, ref startField, element.ElementLength);
                    AddParsedElement(element);
                }
                
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>This method parse the subdataelements value in each elements.
        /// </summary>
        private void ParseSubDataElements()
        {
            int previousElementNo = 0;

            //Filter subelement list based on fieldno.
            SubElementList fieldSubElements = SubElementList.LoadFieldwiseSubDataElements(_subElementlist, _dataElementID).Clone();
            int startField = 0;
            foreach (SubDataElement subElement in fieldSubElements)
            {
                if (startField >= _fieldValue.Length) break;
                subElement.ProductType = this.ProductType;
                int fieldLength = 2;
                int actualLength = 0;
                int subElementNumber = 0;


                //this logic handles the variable length record which reoccured into data element.
                if (subElement.DataRepresentation == EnumDataRepresentment.Variable_Length_Reoccured || subElement.DataRepresentation == EnumDataRepresentment.LTV_With_SDE_ReOccured)
                {
                    byte[] dataValue = null;

                    //this logic handle sub data element have LTV format and it is reoccured into element.
                    if (subElement.DataRepresentation == EnumDataRepresentment.LTV_With_SDE_ReOccured)
                    {
                        int.TryParse(this._fieldValue.Substring(startField, fieldLength), out subElementNumber);

                        //check if the subelement that exist in message is same with the sub element in the loop
                        if (subElementNumber != subElement.SubElementNumber) continue;

                        startField += fieldLength;
                        //get length from TLV and extract data.
                        int.TryParse(this._fieldValue.Substring(startField, fieldLength), out actualLength);
                        startField += fieldLength;

                        dataValue = this.GetBytesFromMessage(this._fieldValueInByte, startField, actualLength);
                    }
                    else
                        dataValue = this._fieldValueInByte;

                    int counter = 0;
                    //Now extract each sub element reoccured into data 
                    for (int subStartField = 0; subStartField < dataValue.Length; )
                    {
                        SubDataElement newElement = subElement.Clone();
                        newElement.SubElementNumber += counter;
                        newElement.FieldValueinByte = this.GetBytesFromMessage(dataValue, subStartField, newElement.MultipleOf);
                        newElement.ActualElementLength = newElement.MultipleOf;

                        AddParsedElement(newElement);
                        counter++;
                        subStartField += newElement.MultipleOf;
                        startField += newElement.MultipleOf;
                    }
                }
                else
                {
                    //extract element based on fixed length format.
                    SubDataElement newElement = subElement.Clone();
                    if (subElement.DataRepresentation == EnumDataRepresentment.Fixed)
                    {
                        int newLength = 0;

                        if (this.FieldValueinByte.Length < (newElement.ElementLength + startField))
                            newLength = (this.FieldValueinByte.Length - startField);
                        else
                        {
                            newLength = newElement.ElementLength;
                        }
                        //newElement.FieldValue = this._fieldValue.Substring(startField, newElement.SubElementLength);
                        newElement.FieldValueinByte = this.GetBytesFromMessage(this.FieldValueinByte, startField, newLength);
                        startField += newLength;
                        newElement.ActualElementLength = newLength;
                    }
                    else if (subElement.DataRepresentation == EnumDataRepresentment.Shared_byte_field) 
                    {
                        //extract element based on hexadecimal value and single byte can be shared between 2 sub fields.

                        int newLength = 0;

                        if (this._fieldValue.Length < (newElement.ElementLength + startField))
                            newLength = (this._fieldValue.Length - startField);
                        else
                        {
                            newLength = newElement.ElementLength;
                        }
                        newElement.FieldValue = this._fieldValue.Substring(startField, newLength);
                        startField += newLength;
                        newElement.ActualElementLength = newLength;
                    }
                    else if (subElement.DataRepresentation == EnumDataRepresentment.LLVAR)
                    {
                        //extract element based on variable length with 2 digit length header.

                        //newElement.FieldNo = _fieldNo;
                        int.TryParse(this._fieldValue.Substring(startField, fieldLength), out subElementNumber);//fetch first 2 digit for subelement number.

                        //if subelement that parsed and element in list is not same then iterate next.
                        if (subElementNumber != newElement.SubElementNumber) continue; //element not found in string so try for next element.

                        //if same element exist into template data and length will be different
                        //in this case we need to find which element length is matched with received element to find out subelement details.
                        if (subElement.IsFieldDependOnLength)
                        {
                            int lenstartField = startField + fieldLength;
                            int.TryParse(this._fieldValue.Substring(lenstartField, fieldLength), out actualLength);

                            //we have not received matched element so if same element again... need to move next element.
                            if (subElement.SubElementNumber == previousElementNo) continue;

                            // if list element's length match with received element lentgh then parse else move to next element.
                            if (subElement.ElementLength != actualLength) continue;

                            startField += fieldLength;
                            previousElementNo = subElement.SubElementNumber;
                        }
                        else
                        {
                            startField += fieldLength;
                            int.TryParse(this._fieldValue.Substring(startField, fieldLength), out actualLength);
                        }

                        startField += fieldLength;
                        //newElement.FieldValue = this._fieldValue.Substring(startField, actualLength);
                        newElement.FieldValueinByte = this.GetBytesFromMessage(this.FieldValueinByte, startField, actualLength);
                        newElement.ActualElementLength = actualLength;
                        startField += actualLength;
                    }                   
                    else if(subElement.DataRepresentation == EnumDataRepresentment.Bit_only)
                    {
                        newElement.FieldValueInBit = this.FieldValueinBitmap.IsSet(newElement.SubElementNumber);
                    }
                    AddParsedElement(newElement);
                }
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Oct-2010</created>
        /// <summary>This method parse PDS Elements from received message and fields.
        /// </summary>
        /// <exception cref="ValidateException">
        /// </exception>
        private void ParseMasterCardPDSElement()
        {
            int actualLength = 0;
            string pdsElementNumber = string.Empty;
            int startField = 0;

            for (; startField < this.FieldValueinByte.Length; )
            {
                pdsElementNumber = this.FieldValue.Substring(startField, PDSTAGLENGTH);

                startField += PDSTAGLENGTH;

                PDSElement pdsElement = PDSElement.GetPDSElementsFromTagNumber(this._subElementlist, this.DataElementID, pdsElementNumber,0);//C#158440 Vishal S Added DataElementID
                if (pdsElement == null) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo);

                int.TryParse(this._fieldValue.Substring(startField, PDSLENGTHOFDATALENGTH), out actualLength);
                startField += PDSLENGTHOFDATALENGTH;

                byte[] dataValue = this.GetBytesFromMessage(this.FieldValueinByte, startField, actualLength);
                startField += actualLength;

                if (pdsElement.DataRepresentation == EnumDataRepresentment.PDS_with_Reoccured)
                {
                    for (int subStartField = 0; subStartField < actualLength; )
                    {
                        PDSElement newpdsElement = pdsElement.Clone();
                        newpdsElement.FieldValueinByte = this.GetBytesFromMessage(dataValue, subStartField, newpdsElement.MultipleOf);
                        newpdsElement.ActualElementLength = newpdsElement.MultipleOf;

                        subStartField += newpdsElement.MultipleOf;
                        AddParsedElement(newpdsElement);
                    }
                }
                else 
                {
                    pdsElement.ActualElementLength = actualLength;
                    pdsElement.FieldValueinByte = dataValue; 
                    AddParsedElement(pdsElement);
                }
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Oct-2010</created>
        /// <summary>This method parse PDS Elements from received message and fields.
        /// </summary>
        /// <exception cref="ValidateException">
        /// </exception>
        private void ParseMasterCardPDSElement_48()
        {
            int actualLength = 0;
            string pdsElementNumber = string.Empty;
            int startField =1;
            int PDSTAGLENGTH_48 = 2;
            
            this._tCCode = HexEncoding.ConvertEbcdicToAsciiString(this.GetBytesFromMessage(this.FieldValueinByte, 0, 1));
            
            for (; startField < this.FieldValueinByte.Length;)
            {
                pdsElementNumber = this.FieldValue.Substring(startField, PDSTAGLENGTH_48);

                startField += PDSTAGLENGTH_48;

                PDSElement pdsElement = PDSElement.GetPDSElementsFromTagNumber(_subElementlist, this._dataElementID, pdsElementNumber);

                if (pdsElement == null) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo);

                int.TryParse(this._fieldValue.Substring(startField, PDSTAGLENGTH_48), out actualLength);
                startField += PDSTAGLENGTH_48;

                byte[] dataValue = this.GetBytesFromMessage(this.FieldValueinByte, startField, actualLength);
                startField += actualLength;

                if (pdsElement.DataRepresentation == EnumDataRepresentment.PDS_with_Reoccured)
                {
                    for (int subStartField = 0; subStartField < actualLength;)
                    {
                        PDSElement newpdsElement = pdsElement.Clone();                        
                        newpdsElement.FieldValueinByte = this.GetBytesFromMessage(dataValue, subStartField, newpdsElement.MultipleOf);
                        newpdsElement.ActualElementLength = newpdsElement.MultipleOf;
                        newpdsElement.ProductType = this.ProductType;
                        subStartField += newpdsElement.MultipleOf;
                        AddParsedElement(newpdsElement);
                    }
                }
                else
                {
                    pdsElement.ActualElementLength = actualLength;
                    pdsElement.ProductType = this.ProductType;
                    pdsElement.FieldValueinByte = dataValue;
                    AddParsedElement(pdsElement);
                }
            }
        }

        protected void ParseMCISOVersion1PDSElement()
        {

            string pdsElementNumber = string.Empty;
            int startField = 0;

            for (; startField < this.FieldValueinByte.Length;)
            {
                int tagLength = 0;
                string tag = string.Empty;

                //Get tag field length either 1 byte or 2 byte
                {
                    //tag = HexEncoding.ToString(this.GetBytesFromMessage(this.FieldValueinByte, startField, 1));
                    ////tag = HexEncoding.ConvertEbcdicToAsciiString(this.GetBytesFromMessage(this.FieldValueinByte, startField, 1));
                    //startField += 1;

                    byte[] tagByte = getTagByte(this.FieldValueinByte, startField); //C#159461 Vishal S
                    tag = HexEncoding.ToString(tagByte);
                    startField += tagByte.Length;
                }
                //Get field length either in 1 byte or 2 byte
                {
                    string hexLength = HexEncoding.ToString(this.GetBytesFromMessage(this.FieldValueinByte, startField, 1)); //C#159461 Vishal S
                    tagLength = HexEncoding.HexToDecimal(hexLength);
                    startField += 1;
                }
                ParsePdsElement(this.FieldValueinByte, tagLength, tag, ref startField);

            }
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary> This method parse dataelement which contains TLV data format.
        /// </summary>
        protected void ParseVisaPDSElement()
        {
           
            string pdsElementNumber = string.Empty;
            int startField = 0;

            for (; startField < this.FieldValueinByte.Length; )
            {
                int tagLength = 0;
                string tag = string.Empty;

                //Get tag field length either 1 byte or 2 byte
                {
                    tag = HexEncoding.ToString(this.GetBytesFromMessage(this.FieldValueinByte, startField, 1));
                    startField += 1;
                }
                //Get field length either in 1 byte or 2 byte
                {
                    string hexLength = HexEncoding.ToString(this.GetBytesFromMessage(this.FieldValueinByte, startField, 2));
                    tagLength = HexEncoding.HexToDecimal(hexLength);
                    startField += 2;
                }
                ParsePdsElement(this.FieldValueinByte, tagLength, tag,ref startField);
              
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary> This method parse dataelements which contains TLV format subdatalements.
        /// </summary>
        /// <param name="messegeByte"> it is a byte array or elements value.
        /// </param>
        /// <param name="tagLength"> it is a taglength which available into message.
        /// </param>
        /// <param name="tag"> it is a tag number.
        /// </param>
        /// <param name="startField"> it is a running length field from which subelement will be start.
        /// </param>
        /// <exception cref="ValidateException">This execption will be raised if data format will not match with the defined schema.
        /// </exception>
        private void ParsePdsElement(byte[] messegeByte,int tagLength,string tag, ref int startField)
        {
            ParsePdsElement(messegeByte, tagLength, tag, ref startField, null, 0);
        }

        private void ParsePdsElement(byte[] messegeByte, int tagLength, string tag, ref int startField,SubElementList parsedPDSSubElementList, int parentPDSElementID)
        {
            PDSElement pdsElement = PDSElement.GetPDSElementsFromTagNumber(this._subElementlist, this.DataElementID, tag, parentPDSElementID);
            if (pdsElement == null) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo);

            byte[] dataValue = this.GetBytesFromMessage(messegeByte, startField, tagLength);
            startField += tagLength;
            if (pdsElement.DataRepresentation == EnumDataRepresentment.PDS_with_Reoccured)
            {
                for (int subStartField = 0; subStartField < tagLength; )
                {
                    PDSElement newpdsElement = pdsElement.Clone();
                    newpdsElement.FieldValueinByte = this.GetBytesFromMessage(dataValue, subStartField, newpdsElement.MultipleOf);
                    newpdsElement.ActualElementLength = newpdsElement.MultipleOf;
                    newpdsElement.ProductType = this.ProductType;
                    subStartField += newpdsElement.MultipleOf;
                    if (parsedPDSSubElements != null)
                    {
                        AddParsedElement(newpdsElement, parsedPDSSubElementList);
                    }
                    else
                        AddParsedElement(newpdsElement);
                }
            }
            else
            {
                pdsElement.ActualElementLength = tagLength;
                pdsElement.FieldValueinByte = dataValue;
                pdsElement.ProductType = this.ProductType;
                if (parsedPDSSubElementList != null)
                {
                    AddParsedElement(pdsElement, parsedPDSSubElementList);
                }
                else
                    AddParsedElement(pdsElement);
            }
        }


        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>This method return byte array from message element for given tag 
        /// </summary>
        /// <param name="message"> it is a message byte from which bytes of tag should be extract.
        /// </param>
        /// <param name="startIndex">it is a start index for fetching data from this index number.
        /// </param>
        /// <returns>it reurns byte array.
        /// </returns>
        private byte[] getTagByte(byte[] message,int startIndex)
        {
            //BitArray bitArr = new BitArray(this.GetBytesFromMessage(message, startIndex, 1));
            BitMap bitMap = new BitMap(this.GetBytesFromMessage(message, startIndex, 1), 1);
            byte[] tagByte = null;
            bool is2Byte = true;

            for (int i = 4; i <= bitMap.Length; i++)
            {
                if (!bitMap.IsSet(i))
                {
                    is2Byte = false;
                    break;
                }
            }
            if (is2Byte)
                tagByte = this.GetBytesFromMessage(message, startIndex, 2);
            else
                tagByte = this.GetBytesFromMessage(message, startIndex, 1);

            return tagByte;
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>This method returns length contains by a tag. length is depend on a first byte's 
        /// first bit if it's a 1 then it contains 2 byte length other wise 1 byte length.
        /// </summary>
        /// <param name="message">it is a message byte from which length of tag should be extract.
        /// </param>
        /// <param name="startIndex">it is a start index for fetching data from this index number.
        /// </param>
        /// <returns>it reurns length of tag.
        /// </returns>
        private byte[] getTagLength(byte[] message, int startIndex)
        {
            //BitArray bitArr = new BitArray(this.GetBytesFromMessage(message, startIndex, 1));
            BitMap bitMap = new BitMap(this.GetBytesFromMessage(message, startIndex, 1), 1);

            byte[] tagByte = null;

            if (bitMap.IsSet(1))
                tagByte = this.GetBytesFromMessage(message, startIndex, 2);
            else
                tagByte = this.GetBytesFromMessage(message, startIndex, 1);

            return tagByte;
        }

        private void AddParsedElement(Element newElement)
        {
            AddParsedElement(newElement, null);
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This method creates a list of parsed sub elements.
        /// </summary>
        /// <param name="newElement">this is a element which needs to add into list object.
        /// </param>
        private void AddParsedElement(Element newElement, SubElementList parsedPDSSubElementList)
        {
            if (_parsedSubElementList == null) _parsedSubElementList = new SubElementList();

            if (newElement.HasSubElements)
            {
                if (newElement.DataRepresentation == EnumDataRepresentment.PrivateDataSubElement)
                {
                    PDSElement newPDSElement = (PDSElement)newElement;
                    newPDSElement.ProductType = this.ProductType;
                    if (newPDSElement.ParsedPDSSubFieldList == null) newPDSElement.ParsedPDSSubFieldList = new SubElementList();

                    //newElement.Validate();

                    //_parsedSubElementList.Add(newElement); //add parant element into list and parse its sub PDS element again

                    int startField = 0;
                    for (; startField < newElement.FieldValueinByte.Length; )
                    {
                        int tagLength = 0;
                        string tag = string.Empty;

                        //Get tag field length either 1 byte or 2 byte
                        {
                            byte[] tagByte = getTagByte(newElement.FieldValueinByte, startField);
                            tag = HexEncoding.ToString(tagByte);
                            startField += tagByte.Length;
                        }
                        //Get field length either in 1 byte or 2 byte
                        {
                            string hexTaglength = HexEncoding.ToString(getTagLength(newElement.FieldValueinByte, startField));
                            tagLength = HexEncoding.HexToDecimal(hexTaglength);
                            startField += (hexTaglength.Length) / 2;
                        }
                        ParsePdsElement(newElement.FieldValueinByte, tagLength, tag, ref startField, newPDSElement.ParsedPDSSubFieldList, newPDSElement.PDSElementID);
                    }
                }
                else if (newElement.DataRepresentation == EnumDataRepresentment.PDS_Elements_with_LLLt)
                {
                    PDSElement newPDSElement = (PDSElement)newElement;
                    newPDSElement.ProductType = this.ProductType;

                    if (newPDSElement.ParsedPDSSubFieldList == null) newPDSElement.ParsedPDSSubFieldList = new SubElementList();

                    //newElement.Validate();

                    //_parsedSubElementList.Add(newElement); //add parant element into list and parse its sub PDS element again

                    int startField = 0;
                    for (; startField < newElement.FieldValueinByte.Length;)
                    {
                        int tagLength = 0;
                        string tag = string.Empty;


                        //Get tag field length either 1 byte or 2 byte
                        {
                            tag = HexEncoding.ConvertEbcdicToAsciiString(this.GetBytesFromMessage(newElement.FieldValueinByte, startField, 2));
                            startField += 2;
                        }
                        //Get field length either in 1 byte or 2 byte
                        {
                            string hexLength = HexEncoding.ConvertEbcdicToAsciiString(this.GetBytesFromMessage(newElement.FieldValueinByte, startField, 2));
                            tagLength = Convert.ToInt32(hexLength);// HexEncoding.HexToDecimal(hexLength);
                            startField += 2;
                        }

                        ////Get tag field length either 1 byte or 2 byte
                        //{

                        //    byte[] tagByte = getTagByte(newElement.FieldValueinByte, startField);
                        //    tag = HexEncoding.ToString(tagByte);
                        //    startField += tagByte.Length;
                        //}
                        ////Get field length either in 1 byte or 2 byte
                        //{
                        //    string hexTaglength = HexEncoding.ToString(getTagLength(newElement.FieldValueinByte, startField));
                        //    tagLength = HexEncoding.HexToDecimal(hexTaglength);
                        //    startField += (hexTaglength.Length) / 2;
                        //}
                        ParsePdsElement(newElement.FieldValueinByte, tagLength, tag, ref startField, newPDSElement.ParsedPDSSubFieldList, newPDSElement.PDSElementID);
                    }
                }
                else
                {
                    newElement.FullSubFieldList = this.FullSubFieldList;
                    newElement.ParseElements();
                }
            }
            newElement.Validate();
           
            if (parsedPDSSubElementList != null) 
                parsedPDSSubElementList.Add(newElement);
            else
                _parsedSubElementList.Add(newElement);
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

            //if (_elementFormat == string.Empty || _fieldValue == string.Empty) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo); ;
            //if (_elementLength < _actualElementLength) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo); ;

            isValid = VerifyMessageFormat(_elementLength);
            if (!isValid) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo);
        }

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>This method returns string of this object into message format.
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            StringBuilder dataelement = new StringBuilder();

            if (this._isElementExist)
            {
                //if fixed element, then add string without length header.
                if (this._dataRepresentation == EnumDataRepresentment.Fixed)
                {
                    if (_elementFormat.ToLower() == "n" && this._fieldValue.Length < this._elementLength)
                    {
                        this._fieldValue = this._fieldValue.PadLeft(this._elementLength, '0');
                    }
                    else if ((_elementFormat.ToLower() == "ans" || _elementFormat.ToLower() == "an") && this._fieldValue.Trim().Length > 0 && this._fieldValue.Length < this.ElementLength)
                    {
                        this._fieldValue = this._fieldValue.PadRight(this._elementLength, ' ');
                    }

                    if (this._hasSubElements && !this._isClearingSingleElement)
                    {
                        this._fieldValue = this._fieldValue.PadRight(this._elementLength, ' ');

                        if (this._parsedSubElementList != null && _parsedSubElementList[0] is PDSElement)
                        {
                            dataelement.Append(this._parsedSubElementList.ToString().Length.ToString().PadLeft(LengthOfDELength, '0'));
                            dataelement.Append(this._parsedSubElementList.ToString());
                        }
                        else
                        {
                            dataelement.Append(this._fieldValue);
                        }
                    }
                    else
                    {
                        dataelement.Append(this._fieldValue);
                    }
                }
                else
                {
                    //if variable length element, then add length header and then add data into string.

                    if (this._hasSubElements && !this._isClearingSingleElement)
                    {
                        if (this._parsedSubElementList != null && _parsedSubElementList[0] is PDSElement)
                        {

                            string val = this._parsedSubElementList.ToString();

                            //if (this.DataFormat == EnumDataFormat.EBCDIC)
                            //    val = HexEncoding.ToString(HexEncoding.ConvertAsciiToEbcdic(val));

                            dataelement.Append(val.Length.ToString().PadLeft(LengthOfDELength, '0'));
                            dataelement.Append(val);
                        }
                        else
                        {
                            dataelement.Append(_fieldValue.Length.ToString().PadLeft(LengthOfDELength, '0'));
                            dataelement.Append(_fieldValue);
                        }
                    }
                    else
                    {
                        dataelement.Append(_fieldValue.Length.ToString().PadLeft(LengthOfDELength, '0'));
                        dataelement.Append(_fieldValue);
                    }
                }
            }

            return dataelement.ToString();
        }


        #region ICloneable Members

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>AgreementLimit</returns>
        public DataElement Clone()
        {
            return (DataElement)((ICloneable)this).Clone();
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>This method set subelement data value for TLV format element.
        /// </summary>
        /// <param name="tagNo">it is a tagno for which value should be set.
        /// </param>
        /// <param name="value">it is a value to be set.
        /// </param>
        public void SetSubElementValue(int tagNo, byte[] value)
        {
            this._isElementExist = true;
            this.ParsedSubElementList[this.FieldNo.ToString(), tagNo.ToString()].FieldValueinByte = value;
        }


        #endregion

        #region ICloneable Members

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>object</returns>
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>This method returns string data in a message received format.
        /// </summary>
        /// <returns>
        /// </returns>
        protected override string GetElementString()
        {
            StringBuilder _logString = new StringBuilder();
            _logString.Append("[" + DataRepresentation.ToString() + " ");
            if (DataRepresentation == EnumDataRepresentment.Fixed)
                _logString.Append(this.ElementFormat + " " + this.ActualElementLength + "]\t");
            else
            {
                _logString.Append(this.ElementFormat + ".." + this.ISODataElementLength.ToString().PadLeft(this.LengthOfDELength, '0') + " " + this.ActualElementLength.ToString().PadLeft(this.LengthOfDELength, '0') + "]");
            }
            _logString.Append("\t");
            _logString.Append("[" + FieldNo.ToString().PadLeft(3, '0') + "]");
            _logString.Append("\t");
            _logString.Append("[" + ((_isLoggable)? this._fieldValue: "******") + "]");
            _logString.Append("\t");
            _logString.Append("[" + this.DataFormat.ToString() + "]");
            _logString.AppendLine();

            return _logString.ToString();
        }
    }
}
