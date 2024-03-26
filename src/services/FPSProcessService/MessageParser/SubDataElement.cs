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
    public class SubDataElement :SubElement, ICloneable
    {
        //intSubElementNo, intFieldNo, intDataRepresentmentID, intSubDataElementLength, strDataRepresentFormat, intMessageType
        private int _subElementNo = 0;
        private int _subDataElementID = 0;
        private bool _isFieldDependOnLength = false;

        private const int LENGTHOFSDELENGTH = 2;

        /// <author>Prashant Soni</author>
        /// <created>26-Sep-2006</created>
        /// <summary>
        /// </summary>
        public SubDataElement()
        {
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
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

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public bool IsFieldDependOnLength
        {
            get
            {
                return _isFieldDependOnLength;
            }
            set
            {
                _isFieldDependOnLength = value;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This is a  subDataElements lenght of length.
        /// </summary>
        /// <value>int
        /// </value>
        public int SubElementNumber
        {
            get
            {
                return _subElementNo;
            }
            set
            {
                _subElementNo = value;
            }
        }

        protected override SubFieldList GetFilteredSubFieldList()
        {
            return SubFieldList.LoadSubElementWiseSubFields(this.FullSubFieldList, this.SubDataElementID).Clone(); ;
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
                else if(this._dataRepresentation == EnumDataRepresentment.LLVAR || this._dataRepresentation == EnumDataRepresentment.LLLVAR)
                {
                    dataelement.Append(this.SubElementNumber.ToString().PadLeft(LENGTHOFSDELENGTH, '0'));
                    dataelement.Append(_fieldValue.Length.ToString().PadLeft(LENGTHOFSDELENGTH,'0'));
                    dataelement.Append(_fieldValue);
                }
                else if (this._dataRepresentation == EnumDataRepresentment.Variable_Length_Reoccured || this._dataRepresentation == EnumDataRepresentment.LTV_With_SDE_ReOccured)
                {
                    dataelement.Append(this.SubElementNumber.ToString().PadLeft(LENGTHOFSDELENGTH, '0'));

                    string reoccuredString = this.ParsedSubFieldList.ToString();
                    dataelement.Append(reoccuredString.Length.ToString().PadLeft(LENGTHOFSDELENGTH,'0'));
                    dataelement.Append(reoccuredString);

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
        public SubDataElement Clone()
        {
            return (SubDataElement)((ICloneable)this).Clone();
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
            _logString.Append("\t SubElement : ");
            _logString.Append("[" + DataRepresentation.ToString() + " ");
            _logString.Append(this.ElementFormat + " " + this.ActualElementLength + "]");
            _logString.Append("\t");
            _logString.Append("[" + _subElementNo.ToString().PadLeft(2, '0') + "]");
            _logString.Append("\t");
            _logString.Append("[" + this._fieldValue + "]");
            _logString.Append("\t");
            _logString.Append("[" + this.DataFormat.ToString() + "]");
            _logString.AppendLine();

            return _logString.ToString();
        }
    }
}
