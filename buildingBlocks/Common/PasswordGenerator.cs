using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace CredECard.Common.BusinessService
{
    ///Added by Hetal: 22-Sep-06 : 4268
    enum EnumArrayType
    {
        ALPHANUMERIC = 0,
        ALPHABETS = 1,
        NUMERIC = 2
    }

	/// <author>Chirag Khilosia</author>
	/// <created>9/20/2005</created>
	/// <summary>
	/// This class generates Random Password which includes only alphanumeric values
	/// </summary>
	public class PasswordGenerator
	{
		private const int DefaultMaximum = 10;
		private const int DefaultMinimum = 6;
        ///Added by Hetal: 22-Sep-06 : 4268
        private const int FixCharSize = 6;
        private const int FixNumberSize = 2;
        private const int UserNameMax = 20;
        private const int UserNameMin = 8;

        public const int SALT_BYTE_SIZE = 24;
        public const int PBKDF2_ITERATIONS = 1000;
        public const int HASH_BYTE_SIZE = 24;
        
		private string exclusionSet;
		private int maxSize;
		private int minSize;
        private char[] pwdAlphaNumericArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        ///Added by Hetal: 22-Sep-06 : 4268
        private char[] pwdCharArray = "abcdefghijkmnpqrstuvwxyz".ToCharArray();
        private char[] pwdNumberArray = "23456789".ToCharArray();


		/// <author>Chirag Khilosia</author>
		/// <created>9/20/2005</created>
		/// <summary>
		/// Constructor with Min and Max as default
		/// </summary>
		public PasswordGenerator()
		{
			Minimum = DefaultMinimum;
			Maximum = DefaultMaximum;
			Exclusions = null;
		}

        /// <author>Nidhi Thakrar</author>
        /// <created>16-Jul-2015</created>
        /// <summary>Generates the password with salt.</summary>
        public static string GeneratePasswordWithSalt(string password, out string salt)
        {
            //Generate saltBytes using RNGCryptoServiceProvider 
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[SALT_BYTE_SIZE]; //24 byte in size
            cryptoProvider.GetBytes(saltBytes);

            salt = Convert.ToBase64String(saltBytes);
            return PBKDF2(password, saltBytes);
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>16-Jul-2015</created>
        /// <summary>provides hash password with help of pbkdf2.</summary>
        public static string PBKDF2(string password, byte[] salt)
        {
            //Generate Hash using Rfc2898DeriveBytes pbkdf2
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = PBKDF2_ITERATIONS;
            byte[] passwordBytes = pbkdf2.GetBytes(HASH_BYTE_SIZE);
            return Convert.ToBase64String(passwordBytes);
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>16-Jul-2015</created>
        /// <summary>Validates the password.</summary>
        public static bool ValidatePassword(string password, string salt, string passwordHash)
        {
            return (PBKDF2(password, Convert.FromBase64String(salt)) == passwordHash);
        }

        /// <author>Ashka Modi</author>
        /// <created>07-Jul-2010</created>
        /// <summary>
        /// Generate 8-20 alphanumeric username
        /// </summary>
        /// <returns>string</returns>
        public string GenerateUserName()
        {
            int size = UserNameMax;
            byte[] data = new byte[1];

            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

            crypto.GetNonZeroBytes(data);

            size = (data[0] % (UserNameMax - UserNameMin)) + UserNameMin;

            data = new byte[size];
            crypto.GetNonZeroBytes(data);

            StringBuilder result = new StringBuilder(size);

            foreach (byte datum in data)
            {
                result.Append(pwdAlphaNumericArray[datum % (pwdAlphaNumericArray.Length - 1)]);
            }

            return result.ToString();

        }

		/// <author>Chirag Khilosia</author>
		/// <created>9/20/2005</created>
		/// <summary>
		/// Provides the allowed character set
		/// </summary>
		/// <returns>
		/// char[]
		/// </returns>
        private char[] GetAvailableChars(EnumArrayType objArrayType)
		{
            ///Added by Hetal: 22-Sep-06 : 4268
            ArrayList result = null;
            switch (objArrayType)
            {
                case EnumArrayType.ALPHANUMERIC:
                    result = new ArrayList(pwdAlphaNumericArray);
                    break;
                case EnumArrayType.ALPHABETS:
                    result = new ArrayList(pwdCharArray);
                    break;
                case EnumArrayType.NUMERIC:
                    result = new ArrayList(pwdNumberArray);
                    break;
            }

			char[] exclusions = String.Empty.ToCharArray();
			if (exclusionSet != null)
			{
				exclusions = exclusionSet.ToCharArray();
			}
			foreach (char exclusion in exclusions)
			{
				result.Remove(exclusion);
			}

			return (char[]) result.ToArray(typeof(char));
		}

		/// <author>Chirag Khilosia</author>
		/// <created>9/20/2005</created>
		/// <summary>Generates a string using allowed characters
		/// </summary>
		/// <returns>string
		/// </returns>
        public string Generate(bool isAlphaNumeric)
		{
			int size = maxSize;
			byte[] data = new byte[1];
            char[] availableChars = GetAvailableChars(EnumArrayType.ALPHANUMERIC);

			RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
			crypto.GetNonZeroBytes(data);
			size = (data[0] % (maxSize - minSize)) + minSize;
			data = new byte[size];
			crypto.GetNonZeroBytes(data);
			StringBuilder result = new StringBuilder(size);

			foreach(byte datum in data)
			{
				result.Append(availableChars[datum % (availableChars.Length - 1)]);
			}

			return result.ToString();
		}

        /// <author>Hetal Thaker</author>
        /// <created>25-Sep-06</created>
        /// <summary>Generate password of fix length 8,with 6 char and 2 numbers. 4268
        /// </summary>
        /// <returns>string
        /// </returns>
        public string Generate()
        {
            int size = maxSize;
            byte[] data = new byte[1];
            char[] availableChars = GetAvailableChars(EnumArrayType.ALPHABETS);
            char[] availableNumbers = GetAvailableChars(EnumArrayType.NUMERIC);

            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);

            size = FixCharSize + FixNumberSize;

            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);

            foreach (byte datum in data)
            {
                if (result.Length >= FixCharSize)
                    result.Append(availableNumbers[datum % (availableNumbers.Length - 1)]);
                else
                    result.Append(availableChars[datum % (availableChars.Length - 1)]);
            }

            return result.ToString();
        }

		/// <author>Chirag Khilosia</author>
		/// <created>9/20/2005</created>
        /// <summary>Gets or sets the exclusion set</summary>
		/// <value>string
		/// </value>
		public string Exclusions
		{
			get { return exclusionSet; }

			set { exclusionSet = value; }

		}

		/// <author>Chirag Khilosia</author>
		/// <created>9/20/2005</created>
        /// <summary>Gets or sets the Max size</summary>
        /// <value>int
        /// </value>
		public int Maximum
		{
			get { return this.maxSize; }

			set
			{
				this.maxSize = value;
				if (this.minSize >= this.maxSize)
				{
					this.maxSize = PasswordGenerator.DefaultMaximum;
				}
			}

		}

		/// <author>Chirag Khilosia</author>
		/// <created>9/20/2005</created>
        /// <summary>Gets or sets the minimum size</summary>
		/// <value>int
		/// </value>
		public int Minimum
		{
			get { return this.minSize; }

			set
			{
				this.minSize = value;
				if (PasswordGenerator.DefaultMinimum > this.minSize)
				{
					this.minSize = PasswordGenerator.DefaultMinimum;
				}
			}

		}
	} 
}
