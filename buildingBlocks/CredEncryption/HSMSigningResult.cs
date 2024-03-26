using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CredEncryption.BusinessService
{
    /// <author>Keyur Parekh</author>
    /// <created>05-Aug-2016</created>
    /// <summary>
    /// HSM Signing functionality result
    /// </summary>
    [Serializable()]
    internal class HSMSigningResult
    {
        #region Variables

        bool _success = false;
        string _errorMessage = string.Empty;
        bool _isContinue = false;
        string _signedData = string.Empty;
        bool _isVerified = false;
        string _CHCommand = string.Empty;

        #endregion

        #region Properties

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get or Set Is sign Verified or not
        /// </summary>
        internal bool IsVerified
        {
            get { return _isVerified; }
            set { _isVerified = value; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get or Set IsSuccess
        /// </summary>
        internal bool IsSuccess
        {
            get { return _success; }
            set { _success = value; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get or Set ErrorMessage
        /// </summary>
        internal string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get or Set whether response of continue request or not
        /// If false then final response
        /// </summary>
        internal bool IsContinue
        {
            get { return _isContinue; }
            set { _isContinue = value; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2016</created>
        /// <summary>
        /// Get or Set Signed data
        /// </summary>
        internal string SignedData
        {
            get { return _signedData; }
            set { _signedData = value; }
        }

        /// <author>Sapan</author>
        /// <created>31-Dec-2019</created>
        /// <summary>
        /// Get or set CH Command
        /// </summary>
        internal string CHCommand
        {
            get { return _CHCommand; }
            set { _CHCommand = value; }
        }

        #endregion

    }
}