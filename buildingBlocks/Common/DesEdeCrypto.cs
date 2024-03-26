using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Resources;
using System.Reflection;

namespace CredECard.Common.BusinessService
{
    public class AbstractCrypto
    {
        /**
        * Convert a HEX String to a series of bytes, e.g. '6161' to 97,97
        * @param hex
        * @return
        */
        public static byte[] FromHexString(String hex)
        {
            byte[] bts = new byte[hex.Length / 2];
            for (int i = 0; i < bts.Length; i++)
            {
                bts[i] = (byte)Convert.ToInt32(hex.Substring(2 * i, 2), 16);
            }
            return bts;
        }

        public static string ToHexString(byte[] plain)
        {
            string hex = BitConverter.ToString(plain);
            return hex.Replace("-", "");
        }
    }

    [Serializable()]
    public class AESCrypto : AbstractCrypto
    {
        private static string _securityKeyForEncDecForDB = null;
        private static string _securityIVForEncDecForDB = null;
        byte[] iv;
        byte[] key;

        public AESCrypto() { }

        /// <summary>
        /// Set IV and Key from SecurityKey for API and Portal Communication
        /// </summary>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        public string[] SetIVAndKeyFromSecurityKey(string securityKey)
        {
            string[] keys = new string[2];

            if (string.IsNullOrWhiteSpace(securityKey)) return keys;

            string iv = securityKey.Trim().Substring(0, 32); //First 32 -IV
            string key = securityKey.Trim().Substring(32); //Last 64 - Key
            keys[0] = iv;
            keys[1] = key;

            return keys;
        }

        internal static string SecurityKeyForEncDecForDB
        {
            get
            {
                return _securityKeyForEncDecForDB;
            }
            set
            {
                _securityKeyForEncDecForDB = value;
            }
        }

        public static string SecurityIVForEncDecForDB
        {
            get
            {
                if (_securityIVForEncDecForDB == null)
                {
                    ResourceManager rm = new ResourceManager("CredEncryption.CommonResource", Assembly.Load("CredEncryption"));
                    _securityIVForEncDecForDB = rm.GetString("CertificateIVForkeyEncryption").ToString();
                }
                return _securityIVForEncDecForDB;
            }
        }

        public AESCrypto(string iv, string key)
        {
            this.iv = FromHexString(iv);
            this.key = FromHexString(key);
        }

        public string Encrypt(string clearText)
        {
            try
            {
                AesCryptoServiceProvider aesCipher = new AesCryptoServiceProvider();
                byte[] plainText = System.Text.Encoding.Unicode.GetBytes(clearText);
                ICryptoTransform encryptor = aesCipher.CreateEncryptor(key, iv);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    //Defines a stream that links data streams to cryptographic transformations   
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainText, 0, plainText.Length);
                        //Writes the final state and clears the buffer   
                        csEncrypt.FlushFinalBlock();
                        byte[] cipherBytes = msEncrypt.ToArray();
                        string encryptedData = Convert.ToBase64String(cipherBytes);
                        return encryptedData;
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string Decrypt(string encrpytedText)
        {
            try
            {
                AesCryptoServiceProvider aesCipher = new AesCryptoServiceProvider();
                byte[] encryptedData = Convert.FromBase64String(encrpytedText);
                ICryptoTransform decryptor = aesCipher.CreateDecryptor(key, iv);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    //Defines the cryptographic stream for decryption.The stream contains decrypted data   
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainText = new byte[encryptedData.Length];
                        int decryptedCount = csDecrypt.Read(plainText, 0, plainText.Length);

                        string decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                        return decryptedData;
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }

}
