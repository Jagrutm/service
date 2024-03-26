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
    public class SubField :Element, ICloneable
    {
        private int _subFieldsID = 0;
        private int _subDataElementID = 0;
        private int _subFieldNo = 0;
        private int _subFieldLength = 0;
        private string _tagNo = string.Empty;
        private int _subElementNumber = 0;
        private string _subFieldName =string.Empty;
        private const int LENGTHOFSFLENGTH = 2;

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>
        /// </summary>
        public SubField()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int SubFieldsID
        {
            get
            {
                return _subFieldsID;
            }
            set
            {
                _subFieldsID = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SubDataElementID
        {
            get
            {
                return _subDataElementID;
            }
            set
            {
                _subDataElementID = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
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

        ///<author>Tina</author>
        /// <created> 16-Aug-2010</created>
        /// <summary>
        ///Gets or sets the sub field description
        /// </summary>
        ///<value>string</value>
        public string SubFieldName
        {
            get
            {
                return _subFieldName;
            }
            set
            {
                _subFieldName = value;
            }
        }

        /// <summary>
        /// This is a SubElement's allowed Length
        /// </summary>
        public int SubFieldLength
        {
            get
            {
                return _subFieldLength;
            }
            set
            {
                _subFieldLength = value;
            }
        }
        
        /// <summary>This is a  subDataElements lenght of length.
        /// </summary>
        /// <value>int
        /// </value>
        public int SubFieldNumber
        {
            get
            {
                return _subFieldNo;
            }
            set
            {
                _subFieldNo = value;
            }
        }

        public int SubElementNumber
        {
            get
            {
                return _subElementNumber;
            }
            set
            {
                _subElementNumber = value;
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

            if (_elementFormat == string.Empty || _fieldValue == string.Empty) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo); ;
            if (_subFieldLength < _actualElementLength) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo); ;

            isValid = VerifyMessageFormat(_subFieldLength);
            if (!isValid) throw new ValidateException((int)EnumErrorNumber.INVALID_MESSAGE_FORMAT, MessageResource.GetMessage(EnumErrorNumber.INVALID_MESSAGE_FORMAT) + "-" + FieldNo);
        }

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
                else if(this._dataRepresentation == EnumDataRepresentment.LLVAR || this._dataRepresentation == EnumDataRepresentment.LLLVAR)
                {
                    dataelement.Append(this.SubFieldNumber.ToString().PadLeft(LENGTHOFSFLENGTH, '0'));
                    dataelement.Append(_fieldValue.Length.ToString().PadLeft(LENGTHOFSFLENGTH, '0'));
                    dataelement.Append(_fieldValue);
                }
            }
            return dataelement.ToString();
        }


        #region ICloneable Members

        /// <summary>
        /// clone AgreementLimit object
        /// </summary>
        /// <returns>AgreementLimit</returns>
        public SubField Clone()
        {
            return (SubField)((ICloneable)this).Clone();
        }

        #endregion

        #region ICloneable Members

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
            if (this.DataRepresentation == EnumDataRepresentment.Bit_only)
                _logString.Append(this.FieldValueInBit ? "1" : "0");
            else
            {
                _logString.Append("\t\t Sub Field : ");
                _logString.Append("[" + DataRepresentation.ToString() + " ");
                _logString.Append(this.ElementFormat + " " + this.ActualElementLength + "]");
                _logString.Append("\t");
                _logString.Append("[" + _subFieldNo.ToString().PadLeft(2, '0') + "]");
                _logString.Append("\t");
                _logString.Append("[" + this._fieldValue + "]");
                _logString.AppendLine();
            }


            return _logString.ToString();
        }
    }
}
