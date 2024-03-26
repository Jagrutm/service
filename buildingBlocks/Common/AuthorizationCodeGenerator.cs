using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace CredECard.Common.BusinessService
{
	/// <author>Kalpendu</author>
	/// <created>17/02/2005</created>
	/// <summary>
	/// This class generates random authorization code which includes only alphanumeric values
	/// </summary>
	public class AuthorizationCodeGenerator
	{
		/// <author>Kalpendu</author>
		/// <created>17/02/2005</created>
		/// <summary>
		/// Constructor
		/// </summary>
		public AuthorizationCodeGenerator()
		{
			
		}
		/// <author>Kalpendu</author>
		/// <created>17/02/2005</created>
		/// <summary>
		/// Genereate random code of exact eight character which includes only alphanumeric values.
		/// seperated by "-" by every two character.
		/// </summary>
        /// <returns>string
		/// </returns>
		public string GenerateCode()
		{
			
				
			int size = 8;
			byte[] data = new byte[1];
			char[] availableChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

			RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
			crypto.GetNonZeroBytes(data);

			data = new byte[size];
			crypto.GetNonZeroBytes(data);
			StringBuilder result = new StringBuilder(size);

			foreach(byte datum in data)
			{
				result.Append(availableChars[datum % (availableChars.Length - 1)]);
			}

			string finalcode = result.ToString();
			finalcode =finalcode.Substring(0,2) + "-" + finalcode.Substring(2,2)+ "-" + finalcode.Substring(4,2)+ "-" + finalcode.Substring(6,2);
		
			return finalcode;
		}

        /// <summary>
        /// generate 3 digit access code
        /// </summary>
        /// <returns>
        /// return 3 digit accesscode as a string
        /// </returns>
        public string GenerateAccessCode()
        {
            int randomNum;
            Random tempRndnum = new System.Random(unchecked((int)DateTime.Now.Ticks));
            randomNum = tempRndnum.Next(100, 999);
            return randomNum.ToString();
        }

        /// <author>Hetal Thaker</author>
        /// <created>12/09/2006</created>
        /// <summary>
        /// Genereate random code of particular characters which includes only alphanumeric values.
        /// </summary>
        /// <returns>string
        /// </returns>
        public string GenerateCode(int size)
        {
            byte[] data = new byte[1];
            char[] availableChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);

            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);

            foreach (byte datum in data)
            {
                result.Append(availableChars[datum % (availableChars.Length - 1)]);
            }

            string finalcode = result.ToString();

            return finalcode;
        }

	
	} 
}
