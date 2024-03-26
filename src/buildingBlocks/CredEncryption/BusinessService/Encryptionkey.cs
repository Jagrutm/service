using System;
using CredECard.Common.BusinessService;
using CredEcard.CredEncryption.DataService;
using Org.BouncyCastle.Bcpg;
using CredEcard.CredEncryption.DataObject;
using BaseDatabase;

namespace CredEcard.CredEncryption.BusinessService
{
    [Serializable]
    public class Encryptionkey : DataItem, IPersistableV2
    {

        #region Variables

        internal int _keyTypeID = 0;
        internal string _keyType = string.Empty;
        internal string _keyValue = string.Empty;
        internal string _description = string.Empty;
        internal string _phrase = string.Empty;
        internal DateTime _credatedOn = DateTime.MinValue;
        internal string _iV = string.Empty;
        internal short _encryptionMethodID = 0;
        internal short _encryptionFileFormatID = 0;
        internal long _secretKey = 0;
        internal short _symmetricKeyAlgorithmID = 0;
        internal bool _encrypt = false;
        internal int _signkeyTypeID = 0;
        internal string _fileExtension = string.Empty;
        internal string _encryptionFileSetupName = string.Empty;
        private Encryptionkey _signFileEncDecDetail = null;
        internal string _encryptedKeyValue = string.Empty;

        internal int _keyCode = 0;
        internal string _clearKeyValue = string.Empty;
        internal bool _checkIntegrity = false;

        #endregion

        #region Properties

        /// <author>Keyur</author>
        /// <created>29-Aug-2017</created>
        /// <summary>
        /// Get or Set Whether to Check Integrity or not
        /// </summary>
        public bool CheckIntegrity
        {
            get { return _checkIntegrity; }
            set { _checkIntegrity = true; }
        }

        /// <author>Keyur</author>
        /// <created>16-Jun-2017</created>
        /// <summary>
        /// Get or Set Clear Key
        /// </summary>
        public string ClearKeyValue
        {
            get { return _clearKeyValue; }
            set { _clearKeyValue = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set KeyTypeID
        /// </summary>
        public int KeyTypeID
        {
            get { return _keyTypeID; }
            set { _keyTypeID = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set KeyType
        /// </summary>
        public string KeyType
        {
            get { return _keyType; 
            
            }
            set { _keyType = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set KeyValue
        /// </summary>
        public string KeyValue
        {
            get { return _keyValue; }
            set { _keyValue = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set Phrase
        /// </summary>
        public string Phrase
        {
            get { return _phrase; }
            set { _phrase = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set CredatedOn
        /// </summary>
        public DateTime CredatedOn
        {
            get { return _credatedOn; }
            set { _credatedOn = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set IV
        /// </summary>
        public string IV
        {
            get { return _iV; }
            set { _iV = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set EncryptionMethodID
        /// </summary>
        public short EncryptionMethodID
        {
            get { return _encryptionMethodID; }
            set { _encryptionMethodID = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set EncryptionFileFormatID
        /// </summary>
        public short EncryptionFileFormatID
        {
            get { return _encryptionFileFormatID; }
            set { _encryptionFileFormatID = value; }
        }


        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>
        /// Get File Encryption Method
        /// </summary>
        public EnumFileEncryptionMethod FileEncryptionMethod
        {
            get
            {
                return (EnumFileEncryptionMethod)_encryptionMethodID;
            }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>
        /// Get SymmetricKeyAlgorithmTag
        /// </summary>
        public SymmetricKeyAlgorithmTag SymmetricKeyAlgorithm
        {
            get
            {
                return (SymmetricKeyAlgorithmTag)_symmetricKeyAlgorithmID;
            }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>
        /// Get EncryptFile Format
        /// </summary>
        public EnumEncryptFileFormat EncryptFileFormat
        {
            get
            {
                return (EnumEncryptFileFormat)_encryptionFileFormatID;
            }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set SecretKey
        /// </summary>
        public long SecretKey
        {
            get { return _secretKey; }
            set { _secretKey = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set SymmetricKeyAlgorithmID
        /// </summary>
        public short SymmetricKeyAlgorithmID
        {
            get { return _symmetricKeyAlgorithmID; }
            set { _symmetricKeyAlgorithmID = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set Encrypt
        /// </summary>
        public bool IsEncrypt
        {
            get { return _encrypt; }
            set { _encrypt = value; }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set SignEncryptionFileSetupID
        /// </summary>
        public int SignkeyTypeID
        {
            get { return _signkeyTypeID; }
            set { _signkeyTypeID = value; }
        }

        /// <author>Keyur Parekh</author>
        /// <created>2-Oct-2012</created>
        /// <summary>
        /// Get Sign File Enc Dec Setup Detail
        /// </summary>
        public Encryptionkey SignkeyTypeDetail
        {
            get
            {
                if (_signkeyTypeID  > 0 && _signFileEncDecDetail == null)
                    _signFileEncDecDetail = Encryptionkey.Specific(_signkeyTypeID);

                return _signFileEncDecDetail;
            }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set FileExtension
        /// </summary>
        public string FileExtension
        {
            get { return _fileExtension; }
            set { _fileExtension = value; }
        }


        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set EncryptedKeyValue
        /// </summary>
        internal string EncryptedKeyValue
        {
            get 
            {
                MasterCardEncrypt objEncrypt = new MasterCardEncrypt();
                return objEncrypt.EncryptEncryptionKeyUsingKey(_keyValue);
            }
        }

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get or Set DecryptdKeyValue
        /// </summary>
        public string DecryptdKeyValue
        {            
            get
            {
                MasterCardDecrypt objDecrypt = new MasterCardDecrypt();
                return objDecrypt.DecryptEncryptionKeyUsingKey(_keyValue);                
            }

        }

        /// <Author>Dharati Metra</Author>
        /// <CreatedDate>03-Jul-2015</CreatedDate>
        /// <summary>
        /// Get/Set KeyCode
        /// </summary>
        public int KeyCode
        {
            get
            {
                return _keyCode;
            }
            set
            {
                _keyCode = value;
            }
        }

        #endregion

        #region Method

        #region IPersistableV2 Members

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Save Data 
        /// </summary>
        /// <param name="conn">DataController</param>
        public void Save(DataController conn)
        {
            WriteEncryptionkey wr = new WriteEncryptionkey(conn);
            wr.WriteData(this);
        }

        #endregion

        /// <author>vipul patel</author>
        /// <created>19-Jun-2014</created>
        /// <summary>Get specific object
        /// </summary>
        public static Encryptionkey Specific(int id)
        {
            if (id == 0) return null;

            return ReadEncryptionkey.ReadSpecific(id);
        }

        /// <author>vipul patel</author>
        /// <created>24-Jun-2015</created>
        /// <summary>
        /// Get Encryption Key By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Encryptionkey</returns>
        public static Encryptionkey SpecificByCode(int code)
        {
            if (code == 0) return null;

            return ReadEncryptionkey.ReadSpecificByCode(code);
        }


        public override ValidateResult Validate()
        {
            FieldErrorList newErrors = new FieldErrorList();
            ValidateResult result = new ValidateResult(newErrors);

            if (this._keyType == string.Empty) newErrors.Add(new FieldError("Key Type", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));
            if (this._keyValue == string.Empty) newErrors.Add(new FieldError("Key", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));
            if (this._symmetricKeyAlgorithmID == 0) newErrors.Add(new FieldError("Algorithm", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));
            if (this.EncryptionFileFormatID == 0) newErrors.Add(new FieldError("File Format", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));
            if (this.EncryptionMethodID == 0) newErrors.Add(new FieldError("Encryption Method", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));
            if (this._fileExtension == string.Empty) newErrors.Add(new FieldError("File Extension", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));
            if (this.IsEncrypt != true)
            {
                if (this._phrase == string.Empty) newErrors.Add(new FieldError("Pass Phrase", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));
               // if (this._secretKey == 0) newErrors.Add(new FieldError("Key ID", CommonMessage.GetMessage(EnumErrorConstants.MUST_BE_SPECIFIED)));
            }


            return result;
        }

        #endregion
    }
}
