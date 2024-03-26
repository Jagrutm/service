using System;
using System.Security.Cryptography;
using System.Resources;
using System.Reflection;
using System.IO;
using System.Text;


namespace CredECard.Common.BusinessService
{
	public class HashCheck
	{
        /// <summary>
        /// Encrypts the given string using HMACSHA1
        /// </summary>
        /// <param name="data">string</param>
        /// <returns>string</returns>
		public static string GetHMACSHA1(string data)
		{
			// Get a private key from resource file.
			ResourceManager rm = new ResourceManager("Common.CommonResource",Assembly.GetExecutingAssembly());
			string key = rm.GetString("StrongKey");
			
			//get key array in bytes
			byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

			//get data byte array
			byte[] bytData = System.Text.Encoding.UTF8.GetBytes(data);
			
			// create object HMACSHA1 with private key 
			HMACSHA1 hmacSha1 = new HMACSHA1(keyBytes);

			// now calculate the keyed hash
			byte[] bytOut = hmacSha1.ComputeHash(bytData);			

			// Return bits as hex string (in lower case, dashes removed)
			return BitConverter.ToString(bytOut).Replace("-", string.Empty).ToLower();
		}

        /// <summary>
        /// Encrypts string using MACTripleDES
        /// </summary>
        /// <param name="data">string</param>
        /// <returns>string</returns>
		// TODO: Migration comment
        ////public static string GetMacTripleDES(string data)
		////{
		////	// Get a byte array for the md5 storage
		////	ResourceManager rm = new ResourceManager("Common.CommonResource",Assembly.GetExecutingAssembly());
		////	string key = rm.GetString("StrongKey");

		////	byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

		////	byte[] bytData = System.Text.Encoding.UTF8.GetBytes(data);
			
		////	// now calculate the keyed hash
		////	MACTripleDES macDes = new MACTripleDES(keyBytes);
		////	byte[] bytOut = macDes.ComputeHash(bytData);			
		////	return BitConverter.ToString(bytOut).Replace("-", string.Empty).ToLower();
		////}

        #region Get SHA256 hash value

        /// <author>Dharati Metra</author>
        /// <created>02-May-2019</created>
        /// <summary>
        /// Get SHA256 hashing for text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetSHA256(string text) //Case 86590
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] message = UE.GetBytes(text);

            SHA256CryptoServiceProvider a = new SHA256CryptoServiceProvider();
            byte[] hashValue1 = a.ComputeHash(message);
            string hex1 = string.Empty;
            foreach (byte x in hashValue1)
            {
                hex1 += String.Format("{0:x2}", x);
            }
            return hex1;
        }

        #endregion

        /// <author>Aarti Meswania</author>
        /// <created>12-Oct-2016</created>
        /// <summary>get hash</summary>
        /// <param name="input">input string need to be converted to hash</param>
        /// <returns>hash of supplied value</returns>
        public static string GetHashSHA1(string input)
        {
            using (SHA1Managed sha = new SHA1Managed())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                sb.Append(HexEncoding.ToString(hash));
                return sb.ToString();
            }
        }

        /// <author>Manthan Bhatti</author>
        /// <created>14-Aug-2019</created>
        /// <summary>get hash</summary>
        /// <param name="input">input string need to be converted to hash</param>
        /// <returns>hash of supplied value</returns>
        public static string GetHash256(string input)
        {
            return GetHash256(Encoding.UTF8.GetBytes(input));
        }

        public static string GetHash256(byte[] input)
        {
            using (SHA256Managed sha2 = new SHA256Managed())
            {
                var hash = sha2.ComputeHash(input);
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
