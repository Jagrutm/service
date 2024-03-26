using System;
using CredECard.Common.BusinessService;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Sapan Patibandha</author>
    /// <created>30-Aug-2011</created>
    /// <summary>
    /// </summary>
    [Serializable()]
    public class TransactionCode : DataItem
    {
        internal int _transactionCodeID = 0;
        internal string _code = string.Empty;
        internal string _defination = string.Empty;
        internal bool _isMonetary = false;

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public int TransactionCodeID
        {
            get { return _transactionCodeID; }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public string Defination
        {
            get { return _defination; }
            set { _defination = value; }
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>21-Mar-2013</created>
        /// <summary>Gets or sets a value indicating whether this TransactionCode is monetary.</summary>
        /// <value>
        /// 	<c>true</c> if this TransactionCode is monetary; otherwise, <c>false</c>.
        /// </value>
        public bool IsMonetary
        {
            get { return _isMonetary; }
            set { _isMonetary = value; }
        }
    }
}
