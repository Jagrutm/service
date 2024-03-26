using System;
using System.Collections.Generic;
using System.Text;

namespace CredECard.Common.BusinessService
{
    public class LuhnCheckDigit
    {
        private const int padZeros = 7;

        /// <author>Rishit Rajput</author>
        /// <created>19-Sep-07</created>
        /// <summary>
        /// return account number with luhncheck digit check sum
        /// </summary>
        /// <param name="barcodeIIN">string</param>
        /// <param name="accountID">integer</param>
        /// <returns>string</returns>
        public static string GenerateAccountNumberWithLuhnCheckDigit(string barcodeIIN, int accountID)
        {
            string accountnumber = accountID.ToString().PadLeft(padZeros, '0');
            string barcodeNumber = barcodeIIN + accountnumber;

            //To calculate and get luhn check digit and append on accountID
            return accountnumber += luhnCheckDigit(barcodeNumber);
        }

        /// <summary>
        /// return account number with specific format like eg. 00001234
        /// </summary>
        /// <param name="accountnumer">string</param>
        /// <returns>string</returns>
        public static string ConvertToNumberFormat(string accountnumer)
	    {
            if (!string.IsNullOrEmpty(accountnumer))
            {
                return accountnumer.PadLeft(padZeros + 1, '0');
            }
            else
            {
                return string.Empty;
            }
    	}

        /// <author>Arvind Ashapuri</author>
        /// <created>13-Jul-06</created>
        /// <summary>This will generated luhn check digit
        /// </summary>
        /// <param name="barcodeWithAccountNumber">string</param>
        /// <returns>integer</returns>
        private static int luhnCheckDigit(string barcodeWithAccountNumber)
        {
            //Sepearate out the barcode number into char array.
            char[] barcodeChar = barcodeWithAccountNumber.ToCharArray();

            //Now to pick the alternate number from right-n-side of barcode string, 
            //start from very first digit and mutiple that number by 2.
            //Append each digit and get the string.
            string stringOfMultipleByTwo = string.Empty;
            for (int i = barcodeChar.Length - 1; i >= 0; i -= 2)
            {
                int muitpliedValue = Convert.ToInt32(barcodeChar[i].ToString()) * 2;
                stringOfMultipleByTwo += muitpliedValue.ToString();
            }

            //Now to pick the alternate number from right-n-side of barcode string,
            //start from second digit of string.
            //Sum each digit.
            int sumOfWithOutMultipleByTwo = 0;

            for (int i = barcodeChar.Length - 2; i >= 0; i -= 2)
            {
                sumOfWithOutMultipleByTwo += Convert.ToInt32(barcodeChar[i].ToString());
            }


            //variable to for total sum.
            int totalSum = 0;

            //Now sum all digit, that mutiplied by 2 earlier.
            char[] multipleByTwoChar = stringOfMultipleByTwo.ToCharArray();
            
            for (int i = 0; i < multipleByTwoChar.Length; i++)
            {
                totalSum += Convert.ToInt32(multipleByTwoChar[i].ToString());
            }

            //To sum all the digits.
            totalSum += sumOfWithOutMultipleByTwo;

            //To take the modulus.
            int modResult = totalSum % 10;
            if (modResult != 0)
                modResult = 10 - modResult;

            return modResult;
        }
    }
}
