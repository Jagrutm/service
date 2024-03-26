using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CredEncryption.BusinessService
{

    /// <author>Rikunj Suthar</author>
    /// <created>24-Nov-2020</created>
    /// <summary>
    /// HSM result code for decision based on error.
    /// </summary>
    public enum EnumHSMResultCode
    {
        Success = 0,
        InvalidPIN = 1,
        PINFormatError = 2,
        ShortPIN = 3
    }

    public class HSMResult
    {
        bool _success = false;
        string _message = string.Empty;
        string _clearPIN = string.Empty;
        string _pinBlock = string.Empty;
        string _pvv = string.Empty;
        string _iwk1 = string.Empty;
        string _iwk2 = string.Empty;
        string _checkDigit = string.Empty;
        string _cvv = string.Empty;
        string _cvv2 = string.Empty;
        string _icvv = string.Empty;
        string _mac = string.Empty;
        string _clearValue = string.Empty;
        string _encValue = string.Empty;
        string _request = string.Empty;
        string _response = string.Empty;
        string _arpc = string.Empty;
        bool _isUnSafePin = false;
        string _iccMasterKey = string.Empty;
        string _kcv = string.Empty;
        string _cavv = string.Empty;
        string _cavvCvv2 = string.Empty;
        string _dcvv = string.Empty;
        //string _ivCBCEncDec = "0000000000000000";
        string _value = string.Empty;
        string _randomKey = string.Empty; // RS#128208
        string _newEncWorkingKey = string.Empty; // RS#128208
        string _newWorkingKeyCheckDigits = string.Empty; // RS#128208
        EnumHSMResultCode _hsmResultCode = EnumHSMResultCode.Success; // RS#128208
        string _aav = string.Empty;

        /// <author>Nidhi Thakrar</author>
        /// <created>17-Apr-17</created>
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        ///// <author>Nidhi Thakrar</author>
        ///// <created>14-Apr-17</created>
        ///// <summary>Gets or sets the IV for Encrypt Decrypt using CBC.</summary>
        ///// <value>The IV.</value>
        //public string IVCBCEncDec
        //{
        //    get { return _ivCBCEncDec; }
        //    set { _ivCBCEncDec = value; }
        //}

        public bool IsSuccess
        {
            get { return _success; }
            set { _success = value; }
        }

        public string ErrorMessage
        {
            get { return _message; }
            set { _message = value; }
        }

        public string PinBlock
        {
            get { return _pinBlock; }
            set { _pinBlock = value; }
        }

        public string ClearPIN
        {
            get { return _clearPIN; }
            set { _clearPIN = value; }
        }

        public string PVV
        {
            get { return _pvv; }
            set { _pvv = value; }
        }

        public string IWK1
        {
            get { return _iwk1; }
            set { _iwk1 = value; }
        }

        public string IWK2
        {
            get { return _iwk2; }
            set { _iwk2 = value; }

        }

        public string CheckDigit
        {
            get { return _checkDigit; }
            set { _checkDigit = value; }
        }

        public string CVV
        {
            get { return _cvv; }
            set { _cvv = value; }
        }

        public string CVV2
        {
            get { return _cvv2; }
            set { _cvv2 = value; }
        }

        public string ICVV
        {
            get { return _icvv; }
            set { _icvv = value; }
        }

        public string DCVV
        {
            get { return _dcvv; }
            set { _dcvv = value; }
        }

        public string MAC
        {
            get { return _mac; }
            set { _mac = value; }
        }

        public string ClearValue
        {
            get { return _clearValue; }
            set { _clearValue = value; }
        }

        public string EncryptedValue
        {
            get { return _encValue; }
            set { _encValue = value; }
        }

        public string ARPC
        {
            get { return _arpc; }
            set { _arpc = value; }
        }

        public string Request
        {
            get { return _request; }
            set { _request = value; }
        }

        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }

        public bool IsUnSafePin
        {
            get { return _isUnSafePin; }
            set { _isUnSafePin = value; }
        }

        public string ICCMasterKey
        {
            get { return _iccMasterKey; }
            set { _iccMasterKey = value; }
        }

        public string KCV
        {
            get { return _kcv; }
            set { _kcv = value; }
        }

        public string CAVV
        {
            set { _cavv = value; }
            get { return _cavv; }
        }

        public string CavvCVV2
        {
            set { _cavvCvv2 = value; }
            get { return _cavvCvv2; }
        }

        /// <author>Rikunj Suthar</author>
        /// <created>31-Aug-2020</created>
        /// <summary>
        /// New generated working key b y HSM.
        /// </summary>
        public string RandomKey
        {
            get { return _randomKey; }
            set { _randomKey = value; }
        }

        /// <author>Rikunj Suthar</author>
        /// <created>24-Nov-2020</created>
        /// <summary>
        /// HSM Result code.
        /// </summary>
        public EnumHSMResultCode HSMResultCode
        {
            get { return _hsmResultCode; }
            set { _hsmResultCode = value; }
        }
    }
}
