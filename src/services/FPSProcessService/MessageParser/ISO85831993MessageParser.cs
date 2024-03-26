using System;
using System.Collections;
using System.Text;
using System.IO;

using CredECard.Common.BusinessService;

using CredECard.Common.Enums.Transaction;
using ContisGroup.MessageParser.ISO8586Parser;
using ContisGroup.MessageParser.ISO8586Parser.Interface;


namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Prashant Soni</author>
    /// <created>26-Sep-2006</created>
    /// <summary>
    /// </summary>
    /// <exception cref="ValidateException">
    /// </exception>
    [Serializable()]
    public class ISO85831993MessageParser : ISO8583MessageParser, IMessageFormatter
    {
        #region Private declaration

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

        public ISO85831993MessageParser()
        {
            _iso8583Version = EnumISOVesion.ISO8583_1993;
        }

        protected override void SubElementParsing(DataElement element)
        {
            if (element.HasSubElements)
            {
                if (element.DataRepresentation == EnumDataRepresentment.PrivateDataSubElement)
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

        

        #endregion

    }

 
}

