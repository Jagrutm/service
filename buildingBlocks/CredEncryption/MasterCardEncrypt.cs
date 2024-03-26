using CredECard.Common.BusinessService;
using CredECard.Common.Enums.EncryptionKey;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace CredEcard.CredEncryption.BusinessService
{
    /// <author>Arvind Ashapuri</author>
    /// <created>29-May-2008</created>
    /// <summary>This class will be used to encrypt master card data
    /// </summary>    
    /// 
    [Serializable()]
    public class MasterCardEncrypt : MasterCardCrypto
    {
        private Encryptionkey _objEncryptionkey = null;
        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>constructor
        /// </summary>
        public MasterCardEncrypt()
        {
            objCryptoService = new RijndaelManaged();
        }

        public MasterCardEncrypt(Encryptionkey encryptionkey)
        {
            objCryptoService = new RijndaelManaged();
            _objEncryptionkey = encryptionkey;
        }

        private Encryptionkey objEncryptionkey
        {
            get
            {
                return _objEncryptionkey;

            }
            set
            {
                _objEncryptionkey = value;
            }
        }
        ///// <author>Arvind Ashapuri</author>
        ///// <created>24-Oct-2008</created>
        ///// <summary>constructor
        ///// </summary>
        ///// <param name="keyConfig">KMSconfig object</param>
        //public MasterCardEncrypt(KMSconfig keyConfig)
        //{
        //    objCryptoService = new RijndaelManaged();
        //    objKMSconfig = keyConfig;
        //}



        /// <author>Vipul patel</author>
        /// <created>19-June-2014</created>
        /// <summary>This will encrypt EncryptionKey
        /// </summary>
        /// <param name="PAN">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string EncryptEncryptionKeyUsingKey(string key)
        {
            return EncHelper.EncString(objCryptoService, key, SecurityKeyForKeyEncryption, SecurityIVForKeyEncryption, Encoding.ASCII);

        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>This will encrypt PAN
        /// </summary>
        /// <param name="PAN">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string EncryptPANUsingKey(string PAN)
        {
            if (objEncryptionkey == null)
                objEncryptionkey = Encryptionkey.Specific((int)EnumEncryptionKey.PAN_Key);

            return EncHelper.EncString(objCryptoService, PAN, Convert.FromBase64String(objEncryptionkey.DecryptdKeyValue), Convert.FromBase64String(objEncryptionkey.IV), Encoding.ASCII);
        }

        /// <author>Keyur</author>
        /// <created>16-Jun-2017</created>
        /// <summary>
        /// Encrypt Pan with Clear Pan Encryption Key
        /// </summary>
        /// <param name="PAN">Pan</param>
        /// <param name="objEncryptionkey">Encryption Key</param>
        /// <returns>Encrypted Pan</returns>
        public string EncryptPANUsingKey(string PAN, Encryptionkey objEncryptionkey)
        {
            if (objEncryptionkey == null)
                objEncryptionkey = Encryptionkey.Specific((int)EnumEncryptionKey.PAN_Key);

            if (objEncryptionkey.ClearKeyValue == string.Empty)
                return EncHelper.EncString(objCryptoService, PAN, Convert.FromBase64String(objEncryptionkey.DecryptdKeyValue), Convert.FromBase64String(objEncryptionkey.IV), Encoding.ASCII);
            else
                return EncHelper.EncString(objCryptoService, PAN, Convert.FromBase64String(objEncryptionkey.ClearKeyValue), Convert.FromBase64String(objEncryptionkey.IV), Encoding.ASCII);
        }

        /// <author>Vipul Patel</author>
        /// <created>25-Aug-2014</created>
        /// <summary>This will encrypt PAN
        /// </summary>
        /// <param name="PAN">string
        /// </param>
        /// <param name="key">string
        /// </param>
        /// <param name="keyEncryptionkey">string
        /// </param>
        /// <param name="IV">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string EncryptPANUsingKey(string PAN, string key, string keyEncryptionkey, string IV)
        {
            MasterCardDecrypt objDecrypt = new MasterCardDecrypt();
            string decryptedKey = objDecrypt.DecryptEncryptionKeyUsingKey(key, keyEncryptionkey);

            return EncHelper.EncString(objCryptoService, PAN, Convert.FromBase64String(decryptedKey), Convert.FromBase64String(IV), Encoding.ASCII);
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>This will encrypt card expiry date
        /// </summary>
        /// <param name="expDate">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string EncryptExpiryDateUsingKMS(string expDate)
        {
            //return EncHelper.EncDataUsingKMS(CurKMClient, expDate, CARD_EXPIRY); // use this for KMS encryption
            return EncHelper.EncString(objCryptoService, expDate, SecurityKeyForExpiryDate, SecurityIVForExpiryDate, Encoding.ASCII);
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>This will encrypt card holder name
        /// </summary>
        /// <param name="cardHolderName">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string EncryptCardHolderUsingKMS(string cardHolderName)
        {
            //return EncHelper.EncDataUsingKMS(CurKMClient, cardHolderName, CARD_HOLDERNAME); // use this for KMS encryption
            return EncHelper.EncString(objCryptoService, cardHolderName, SecurityKeyForCardHolder, SecurityIVForCardHolder, Encoding.ASCII);
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>This will encrypt file
        /// </summary>
        /// <param name="filePath">string
        /// </param>
        /// <returns>MemoryStream
        /// </returns>
        public MemoryStream EncryptFileData(string filePath)
        {
            return EncHelper.EncFile(objCryptoService, filePath, SecurityKeyForFile, SecurityIVForFile);
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>06-Apr-2015</created>
        /// <summary>Decrypts the connection string.</summary>
        public string DecryptConnectionString(string ConnectionString)
        {
            SymmCrypto objCrypto = new SymmCrypto();
            return objCrypto.Decrypting(ConnectionString);
        }

        /// <author>Keyur Parekh</author>
        /// <created>09-Nov-2011</created>
        /// <summary>This will encrypt Card Info
        /// </summary>
        /// <param name="CardInfo">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string EncryptCardInfoUsingKey(string cardInfo)
        {
            return EncHelper.EncString(objCryptoService, cardInfo, SecurityKeyForCardInfo, SecurityIVForPAN, Encoding.ASCII);
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>12-Mar-2015</created>
        /// <summary>Encrypts the log data using key.</summary>
        public string EncryptDataUsingKey(string LogData)
        {
            if (objEncryptionkey == null)
                objEncryptionkey = Encryptionkey.Specific((int)EnumEncryptionKey.Log_Key);

            if (objEncryptionkey.ClearKeyValue == string.Empty)
                return EncHelper.EncString(objCryptoService, LogData, Convert.FromBase64String(objEncryptionkey.DecryptdKeyValue), Convert.FromBase64String(objEncryptionkey.IV), Encoding.ASCII);
            else
                return EncHelper.EncString(objCryptoService, LogData, Convert.FromBase64String(objEncryptionkey.ClearKeyValue), Convert.FromBase64String(objEncryptionkey.IV), Encoding.ASCII);
        }

        public string Encrypt3DSPasswordUsingKey(string password)
        {
            return getEncryptedString(password, EnumEncryptionKey.ThreeDSecure_Key);
        }

        private string getEncryptedString(string data, EnumEncryptionKey key)
        {
            if (objEncryptionkey == null)
                objEncryptionkey = Encryptionkey.SpecificByCode((int)key);

            return EncHelper.EncString(objCryptoService, data, Convert.FromBase64String(objEncryptionkey.DecryptdKeyValue), Convert.FromBase64String(objEncryptionkey.IV), Encoding.ASCII);
        }
    }
}
