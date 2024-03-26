using System;
using System.Collections.Generic;

namespace CredECard.Common.BusinessService
{
    public class CredEcardBarcode
    {
        /// <authoer>Arvind Ashapuri</authoer>
        /// <created>13-Jul-06</created>
        /// <summary>This will generate barcode number
        /// </summary>
        /// <returns>string</returns>
        public static string GenerateBarcode(Int64 number, string categoryCode, string barcodeIIN)
        {
            string barcodeNumber = string.Empty;

            //To prepare barcode.
            barcodeNumber = barcodeIIN + categoryCode + number.ToString("00000000");

            //To calculate and get luhn check digit and append on existing barcode number.
            barcodeNumber += luhnCheckDigit(barcodeNumber);

            return barcodeNumber;
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>13-Jul-06</created>
        /// <summary>This will generated luhn check digit
        /// </summary>
        /// <param name="barcodeNumber">string</param>
        /// <returns>int</returns>
        private static int luhnCheckDigit(string barcodeNumber)
        {
            //Sepearate out the barcode number into char array.
            char[] barcodeChar = barcodeNumber.ToCharArray();

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
