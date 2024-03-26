using CredECard.Common.Enums.EncryptionKey;
using Microsoft.SqlServer.Server;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CredEcard.CredEncryption.BusinessService
{
    /// <author>Arvind Ashapuri</author>
    /// <created>29-May-2008</created>
    /// <summary>This class will be used to decrypt master card data
    /// </summary>
    public class MasterCardDecrypt : MasterCardCrypto
    {
        private Encryptionkey _objEncryptionkey = null;
        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>constructor
        /// </summary>
        public MasterCardDecrypt()
        {
            objCryptoService = new RijndaelManaged();
        }

        /// <author>Vipul patel</author>
        /// <created>23-June-2014</created>
        /// <summary> get set ObjEncrypionKey
        /// </summary>
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
        //public MasterCardDecrypt(KMSconfig keyConfig)
        //{
        //    objCryptoService = new RijndaelManaged();
        //    objKMSconfig = keyConfig;
        //}

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>This will decrypt PAN and return decrypt value as string
        /// </summary>
        /// <param name="PAN">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string DecryptPANUsingKMS(string PAN)
        {
            //if (objKMSconfig == null) objKMSconfig = GetConfigValues();
            //return DecHelper.DecryptDataUsingKMS(this.objKMSconfig, PAN, CARD_PAN);

            if (objEncryptionkey == null)
                objEncryptionkey = Encryptionkey.Specific((int)EnumEncryptionKey.PAN_Key);

            return DecHelper.DecString(objCryptoService, PAN, Convert.FromBase64String(objEncryptionkey.DecryptdKeyValue), Convert.FromBase64String(objEncryptionkey.IV), Encoding.ASCII);
        }

              

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>This will decrypt card expiry date
        /// </summary>
        /// <param name="expiryDate">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string DecryptExpiryDateUsingKMS(string expiryDate)
        {
            //return DecHelper.DecryptDataUsingKMS(CurKMClient, expiryDate, CARD_EXPIRY); // use this for KMS decryption
            return DecHelper.DecString(objCryptoService, expiryDate, SecurityKeyForExpiryDate, SecurityIVForExpiryDate, Encoding.ASCII);
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>This will decrypt card holder name
        /// </summary>
        /// <param name="cardHolderName">string
        /// </param>
        /// <returns>string
        /// </returns>
        [SqlFunction]
        public string DecryptCardHolderUsingKMS(string cardHolderName)
        {
            //return DecHelper.DecryptDataUsingKMS(CurKMClient, cardHolderName, CARD_HOLDERNAME); // use this for KMS decryption
            return DecHelper.DecString(objCryptoService, cardHolderName, SecurityKeyForCardHolder, SecurityIVForCardHolder, Encoding.ASCII);
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-May-2008</created>
        /// <summary>This will decrypt file
        /// </summary>
        /// <param name="filePath">string
        /// </param>
        /// <returns>MemoryStream object
        /// </returns>
        public MemoryStream DecryptFileData(string filePath)
        {
            return DecHelper.DecFile(objCryptoService, filePath, SecurityKeyForFile, SecurityIVForFile);
        }

        /// <author>Keyur Parekh</author>
        /// <created>09-Nov-2011</created>
        /// <summary>This will decrypt Card Info
        /// </summary>
        /// <param name="CardInfo">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string DecryptCardInfoUsingKey(string cardInfo)
        {
            return DecHelper.DecString(objCryptoService, cardInfo, SecurityKeyForCardInfo, SecurityIVForPAN, Encoding.ASCII);
        }

         /// <author>Keyur Parekh</author>
        /// <created>09-Nov-2011</created>
        /// <summary>This will decrypt Card Info
        /// </summary>
        /// <param name="CardInfo">string
        /// </param>
        /// <returns>string
        /// </returns>
        public string DecryptEncryptionKeyUsingKey(string key)
        {
            return DecHelper.DecString(objCryptoService, key, SecurityKeyForKeyEncryption, SecurityIVForKeyEncryption, Encoding.ASCII);
        }

        /// <author>vipul Patel</author>
        /// <created>25-Aug-2014</created>
        /// <summary>This will decrypt Card Info
        /// </summary>
        /// <param name="key">string
        /// </param>
        /// <param name="setting key">string
        /// </param
        /// <returns>string
        /// </returns>
        public string DecryptEncryptionKeyUsingKey(string key,string encryptionkey)
        {
            return DecHelper.DecString(objCryptoService, key,Convert.FromBase64String(encryptionkey), SecurityIVForKeyEncryption, Encoding.ASCII);
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>12-Mar-2015</created>
        /// <summary>Decrypts the data using key.</summary>
        public string DecryptDataUsingKey(string LogData)
        {
            if (objEncryptionkey == null)
                objEncryptionkey = Encryptionkey.Specific((int)EnumEncryptionKey.Log_Key);

            return DecHelper.DecString(objCryptoService, LogData, Convert.FromBase64String(objEncryptionkey.DecryptdKeyValue), Convert.FromBase64String(objEncryptionkey.IV), Encoding.ASCII);
        }

        public string Decrypt3DSPasswordUsingKey(string password)
        {
            return getDecryptedString(password, EnumEncryptionKey.ThreeDSecure_Key);
        }

        /// <author>Keyur Parekh</author>
        /// <created>24-Jun-2015</created>
        /// <summary>Decrypts the data using key.</summary>
        private string getDecryptedString(string data,EnumEncryptionKey key)
        {
            if (objEncryptionkey == null)
                objEncryptionkey = Encryptionkey.SpecificByCode((int)key);

            return DecHelper.DecString(objCryptoService, data, Convert.FromBase64String(objEncryptionkey.DecryptdKeyValue), Convert.FromBase64String(objEncryptionkey.IV), Encoding.ASCII);
        }
    }
}
