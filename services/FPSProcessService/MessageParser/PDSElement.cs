using System;
using System.Linq;
using System.Text;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Prashant Soni</author>
    /// <created>26-Sep-2006</created>
    /// <summary>
    /// </summary>
    /// <exception cref="ValidateException">
    /// </exception>
    [Serializable()]
    public class PDSElement : SubElement, ICloneable
    {
        //intSubElementNo, intFieldNo, intDataRepresentmentID, intSubDataElementLength, strDataRepresentFormat, intMessageType       
        private string _tagNo = string.Empty;

        //private int _lengthOfSDELength = 0;//will get from Data Representation. value will be 2 or 3.
        private const int LENGTHOFPDSDATALENGTH = 3;
        private const int LENGTHOFTAGNO = 4;
        private string _tagName = string.Empty;

        private SubElementList _parsedPDSSubFieldList = null;

        //        ,intDataElementID
        //,intDataFormatID
        internal int _pdsElementID = 0;
        internal EnumPresenceNotation  _authPresenceNotationID = EnumPresenceNotation.Not_Required;
        int _parantPDSElementID = 0;
        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>
        /// </summary>
        public PDSElement()
        {
        }

        /// <author>Vipul</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This is a  subDataElements lenght of length.
        /// </summary>
        /// <value>int
        /// </value>
        public EnumPresenceNotation AuthPresenceNotationID
        {
            get
            {
                return _authPresenceNotationID;
            }
            set
            {
                _authPresenceNotationID = value;
            }
        }
        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This is a  subDataElements lenght of length.
        /// </summary>
        /// <value>int
        /// </value>
        public string TagNumber
        {
            get
            {
                return _tagNo;
            }
            set
            {
                _tagNo = value;
            }
        }

        /// <author>Tina</author>
        /// <created>16-Aug-2010</created>
        /// <summary>Descriptive name of the tag
        /// </summary>
        /// <value>string
        /// </value>
        public string TagName
        {
            get
            {
                return _tagName;
            }
            set
            {
                _tagName = value;
            }
        }

        public int ParantPDSElementID
        {
            get
            {
                return _parantPDSElementID;
            }
            set
            {
                _parantPDSElementID = value;
            }
        }

        public SubElementList ParsedPDSSubFieldList
        {
            get
            {
                return _parsedPDSSubFieldList;
            }
            set
            {
                _parsedPDSSubFieldList = value;
            }
        }

        public int PDSElementID
        {
            get
            {
                return _pdsElementID;
            }
        }

        protected override SubFieldList GetFilteredSubFieldList()
        {
            return SubFieldList.LoadPDSTagWiseSubFields(this.FullSubFieldList, this._tagNo).Clone();
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            StringBuilder dataelement = new StringBuilder();

            if (this._isElementExist)
            {
                if (this._dataRepresentation == EnumDataRepresentment.Fixed)
                    dataelement.Append(this._fieldValue);
                else if (this._dataRepresentation == EnumDataRepresentment.PrivateDataSubElement 
                    || this._dataRepresentation == EnumDataRepresentment.PDS_with_Reoccured
                    )
                {
                    //if (this.ProductType == EnumProductType.MasterCard)
                    //{
                    //    dataelement.Append(HexEncoding.ToString(HexEncoding.ConvertAsciiToEbcdic(this._tagNo.ToString().PadLeft(LENGTHOFTAGNO, '0'))));
                    //    dataelement.Append(HexEncoding.ToString(HexEncoding.ConvertAsciiToEbcdic(_fieldValue.Length.ToString().PadLeft(LENGTHOFPDSDATALENGTH, '0'))));
                    //    dataelement.Append(_fieldValue);
                    //}
                    //else
                    {
                        dataelement.Append(this._tagNo.ToString().PadLeft(LENGTHOFTAGNO, '0'));
                        dataelement.Append(_fieldValue.Length.ToString().PadLeft(LENGTHOFPDSDATALENGTH, '0'));
                        dataelement.Append(_fieldValue);
                    }
                }
                else if (this._dataRepresentation == EnumDataRepresentment.LLLVAR)
                {
                    dataelement.Append(this._tagNo.ToString().PadLeft(LENGTHOFTAGNO, '0'));
                    dataelement.Append(_fieldValue.Length.ToString().PadLeft(LENGTHOFPDSDATALENGTH, '0'));
                    dataelement.Append(_fieldValue);
                }
            }
            return dataelement.ToString();
        }

        public static PDSElement GetPDSElementsFromTagNumber(SubElementList list, string tag)
        {
            return GetPDSElementsFromTagNumber(list, tag, 0);
        }

        public static PDSElement GetPDSElementsFromTagNumber(SubElementList list, string tag, int parentPDSElementID)
        {
            var newList = from PDSElement selement in list
                          where (selement.TagNumber == tag && selement.ParantPDSElementID == parentPDSElementID)
                          select selement;

            if (newList == null) return null;

            PDSElement pdsElement = new PDSElement();
            foreach (PDSElement element in newList)
            {
                pdsElement = element;
                break;
            }
            return pdsElement;
        }

        public static PDSElement GetPDSElementsFromTagNumber(SubElementList list,int dataElementID, string tag, int parentPDSElementID)
        {
            var newList = from PDSElement selement in list
                          where (selement.TagNumber == tag && selement.DataElementID == dataElementID &&  selement.ParantPDSElementID == parentPDSElementID)
                          select selement;

            if (newList == null) return null;

            PDSElement pdsElement = new PDSElement();
            foreach (PDSElement element in newList)
            {
                pdsElement = element;
                break;
            }
            return pdsElement;
        }

        public static PDSElement GetPDSElementsFromTagNumber(SubElementList list, int dataElementID, string tag)
        {
            var newList = from PDSElement selement in list
                          where (selement.TagNumber == tag && selement.DataElementID == dataElementID)
                          select selement;

            if (newList == null) return null;

            PDSElement pdsElement = new PDSElement();
            foreach (PDSElement element in newList)
            {
                pdsElement = element;
                break;
            }
            return pdsElement;
        }
        //public static PDSElement GetPDSElementsFromTagNumber(SubElementList list, int tagNo)
        //{
        //    var newList = from PDSElement selement in list
        //                  where selement.TagNumber == tagNo
        //                  select selement;

        //    if (newList == null) return null;

        //    PDSElement pdsElement = new PDSElement();
        //    foreach (PDSElement element in newList)
        //    {
        //        pdsElement = element;
        //        break;
        //    }
        //    return pdsElement;
        //}

        public static PDSElement Specific(int tagNo)
        {
            return ReadPDSElement.GetTagWisePDSElement(tagNo);
        }

        #region ICloneable Members

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>AgreementLimit</returns>
        public PDSElement Clone()
        {
            return (PDSElement)((ICloneable)this).Clone();
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

        protected override string GetElementString()
        {
            StringBuilder _logString = new StringBuilder();
            _logString.Append("\t PDS : ");
            _logString.Append("[" + DataRepresentation.ToString() + " ");
            _logString.Append(this.ElementFormat + " " + this.ActualElementLength + "]");
            _logString.Append("\t");
            _logString.Append("[" + _tagNo.ToString().PadLeft(3, '0') + "]");
            _logString.AppendLine();
           
            if (this.HasSubElements)
            {
                if (_parsedPDSSubFieldList != null)
                {
                    foreach (Element element in _parsedPDSSubFieldList)
                    {
                        PDSElement pds = (PDSElement)element;
                        _logString.Append("\t");
                        _logString.Append("\t");
                        _logString.Append("[" + pds._tagNo.ToString().PadLeft(3, '0') + "]");
                        _logString.Append("\t");
                        _logString.Append("[");
                        //if (pds._dataRepresentation == EnumDataRepresentment.Bitmap_based_field_format)
                        //    _logString.Append(HexEncoding.ToString(pds.FieldValueinByte));
                        //else
                        _logString.Append(HexEncoding.ToString(pds.FieldValueinByte));
                        _logString.Append("]");
                        _logString.AppendLine();

                         if (element.HasSubElements && ((PDSElement)element).ParsedSubFieldList != null)
                        {
                            _logString.Append(((PDSElement)element).ParsedSubFieldList.LogData());
                            //foreach (SubField subField1 in ((PDSElement)element).ParsedSubFieldList)
                            //{
                            //    //subField1.GetElementString();
                            //    //_logString.Append("\t");
                            //    //_logString.Append("\t");
                            //    //_logString.Append("[" + subField1.FieldName.ToString() + "]");
                            //    //_logString.Append("\t");
                            //    //_logString.Append("[");
                            //    //if (pds._dataRepresentation == EnumDataRepresentment.Bit_only)
                            //    //    _logString.Append(HexEncoding.ToString(pds.FieldValueinByte));
                            //    //else
                            //    //    _logString.Append(HexEncoding.ToString(pds.FieldValueinByte));
                            //    //_logString.Append("]");
                            //    //_logString.AppendLine();
                            //}
                        }
                    }
                }
            }
            
           

            return _logString.ToString();
        }
    }
}
