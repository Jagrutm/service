using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CredECard.Common.BusinessService;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Sapan Patibandha</author>
    /// <created>30-Aug-2011</created>
    /// <summary>
    /// </summary>
    [Serializable()]
    public class TransactionComponentRecords : DataItem
    {
        internal int _tcrID = 0;
        internal string _tcrCode = string.Empty;
        internal string _description = string.Empty;

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public int TCRID
        {
            get { return _tcrID; }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public string TCRCode
        {
            get { return _tcrCode; }
            set { _tcrCode = value; }
        }

        /// <author>Sapan Patibandha</author>
        /// <created>30-Aug-2011</created>
        /// <summary>
        /// </summary>
        /// <value>
        /// </value>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
