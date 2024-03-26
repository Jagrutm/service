using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public class SymmCrypto
    {
        /// <summary>
        /// Supported .Net intrinsic SymmetricAlgorithm classes.
        /// </summary>
        public enum SymmProvEnum : int
        {
            DES, RC2, Rijndael
        }

        private SymmetricAlgorithm mobjCryptoService;
        private string _securityKey = string.Empty;

        /// <summary>
        /// Gets the security key
        /// </summary>
        /// <value>string</value>
		private string SecurityKey
        {
            get
            {
                if (string.IsNullOrEmpty(_securityKey))
                {
                    // Get a byte array for the md5 storage
                    ResourceManager rm = new ResourceManager("Common.CommonResource", Assembly.GetExecutingAssembly());
                    _securityKey = rm.GetString("SecurityKey");
                }
                return _securityKey;
            }
        }

        /// <summary>
        /// Constructor for using an intrinsic .Net SymmetricAlgorithm class.
        /// </summary>
        public SymmCrypto(SymmProvEnum NetSelected)
        {
            switch (NetSelected)
            {
                case SymmProvEnum.DES:
                    mobjCryptoService = new DESCryptoServiceProvider();
                    break;
                case SymmProvEnum.RC2:
                    mobjCryptoService = new RC2CryptoServiceProvider();
                    break;
                case SymmProvEnum.Rijndael:
                    mobjCryptoService = new RijndaelManaged();
                    break;
            }
        }

        /// <summary>
        /// Constructor for using a customized SymmetricAlgorithm class.
        /// </summary>
        public SymmCrypto()
        {
            mobjCryptoService = new DESCryptoServiceProvider();
        }

        /// <summary>
        /// Constructor for using a customized SymmetricAlgorithm class.
        /// </summary>
        public SymmCrypto(SymmetricAlgorithm ServiceProvider)
        {
            mobjCryptoService = ServiceProvider;
        }

        /// <summary>
        /// Depending on the legal key size limitations of a specific CryptoService provider
        /// and length of the private key provided, padding the secret key with space character
        /// to meet the legal size of the algorithm.
        /// </summary>
        /// <value>byte[]</value>
        private byte[] GetLegalKey(string Key)
        {
            string sTemp;
            if (mobjCryptoService.LegalKeySizes.Length > 0)
            {
                int lessSize = 0;
                const int value8 = 8;
                int moreSize = mobjCryptoService.LegalKeySizes[0].MinSize;

                // key sizes are in bits
                while (Key.Length * value8 > moreSize)
                {
                    lessSize = moreSize;
                    moreSize += mobjCryptoService.LegalKeySizes[0].SkipSize;
                }
                sTemp = Key.PadRight(moreSize / value8, ' ');
            }
            else
                sTemp = Key;

            // convert the secret key to byte array
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// Method to encrypt the source string using the security key of this instance
        /// </summary>
        /// <param name="source">strin</param>
        /// <returns>string</returns>g
		public string Encrypting(string source)
        {
            return this.Encrypting(source, this.SecurityKey);
        }

        /// <summary>
        /// Encrypts the source string with the passed security key
        /// </summary>
        /// <param name="Source">string</param>
        /// <param name="Key">string</param>
        /// <returns>string</returns>
		public string Encrypting(string Source, string Key)
        {
            byte[] bytIn = ASCIIEncoding.ASCII.GetBytes(Source);
            // create a MemoryStream so that the process can be done without I/O files
            byte[] bytOut;

            using (MemoryStream ms = new MemoryStream())
            {

                byte[] bytKey = GetLegalKey(Key);

                // set the private key
                mobjCryptoService.Key = bytKey;
                mobjCryptoService.IV = bytKey;

                // create an Encryptor from the Provider Service instance
                ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();

                // create Crypto Stream that transforms a stream using the encryption
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

                // write out encrypted content into MemoryStream
                cs.Write(bytIn, 0, bytIn.Length);
                cs.Close();
                bytOut = ms.ToArray();
            }

            // convert into Base64 so that the result can be used in xml
            return Convert.ToBase64String(bytOut);
        }

        /// <summary>
        /// Decrypts string with security key of this instance
        /// </summary>
        /// <param name="Source">string</param>
        /// <returns>string</returns>
		public string Decrypting(string Source)
        {
            return this.Decrypting(Source, this.SecurityKey);
        }

        /// <summary>
        /// Decrypts string with the passed security key
        /// </summary>
        /// <param name="Source">string</param>
        /// <param name="Key">string</param>
        /// <returns>string</returns>
		public string Decrypting(string Source, string Key)
        {
            // convert from Base64 to binary
            try
            {
                byte[] bytIn = Convert.FromBase64String(Source);
                StreamReader sr = null;
                string s = null;

                // create a MemoryStream with the input
                using (MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length))
                {

                    byte[] bytKey = GetLegalKey(Key);

                    // set the private key
                    mobjCryptoService.Key = bytKey;
                    mobjCryptoService.IV = bytKey;

                    // create a Decryptor from the Provider Service instance
                    ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();

                    // create Crypto Stream that transforms a stream using the decryption
                    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

                    // read out the result from the Crypto Stream
                    sr = new StreamReader(cs);
                    s = sr.ReadToEnd();
                }

                return s.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
