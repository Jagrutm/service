
namespace ContisGroup.MessageParser.ISO8586Parser
{
    using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;

    public class IssuerScriptCommand : DataItem
    {
        #region Variables

        internal int _issuerScriptCommandID = 0;
        internal string _commandName = string.Empty;
        internal string _class = string.Empty;
        internal string _instruction = string.Empty;
        internal string _parameter = string.Empty;
        internal string _subParameter = string.Empty;
        internal int _dataElementID = 0;
        internal int _fieldNo = 0;

        #endregion

        #region Properties

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Gets the issuer script command ID.</summary>
        public int IssuerScriptCommandID
        {
            get { return _issuerScriptCommandID; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Gets the name of the command.</summary>
        public string CommandName
        {
            get { return _commandName; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Gets the class.</summary>
        public string Class
        {
            get { return _class; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Gets the instruction.</summary>
        public string Instruction
        {
            get { return _instruction; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Gets the parameter.</summary>
        public string Parameter
        {
            get { return _parameter; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Gets the sub parameter.</summary>
        public string SubParameter
        {
            get { return _subParameter; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Gets the data element ID.</summary>
        public int DataElementID
        {
            get { return _dataElementID; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>23-Dec-2014</created>
        /// <summary>Gets the field no.</summary>
        public EnumIssuerScriptDataElement FieldNo
        {
            get { return (EnumIssuerScriptDataElement)_fieldNo; }
        }

        #endregion
    }
}
