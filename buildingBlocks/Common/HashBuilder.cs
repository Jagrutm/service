using System;
using System.Collections.Generic;
using System.Text;

namespace UserSecurity.Common
{
    public class HashBuilder
    {
        #region Variable

        readonly StringBuilder _sb = null;

        #endregion

        #region Constructor

        /// <author>Nidhi Thakrar</author>
        /// <created>13-Mar-18</created>
        /// <summary>
        /// HashStringBuilder Constructor
        /// </summary>
        public HashBuilder()
        {
            _sb = new StringBuilder();
        }

        #endregion

        #region Methods

        /// <author>Nidhi Thakrar</author>
        /// <created>13-Mar-18</created>
        /// <summary>Appends the specified string.</summary>
        /// <param name="str">The string.</param>
        public void Append(string str)
        {
            _sb.Append(str);
            _sb.Append("&");
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>13-Mar-18</created>
        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return _sb.ToString();
        }

        #endregion
    }
}
