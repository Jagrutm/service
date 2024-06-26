using CredECard.Common.BusinessService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CredECard.Common.BusinessService
{
    /// <summary>
    /// Summary description for ValidateInput.
    /// </summary>
    public class ValidateInput
    {
        const int value2 = 2;
        const int value100 = 100;

        /// <summary>
        /// Method checkes if the passed string contains only numeric characters
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>bool</returns>
        public bool IsNumeric(string str)
        {
            int number = 0;
            try
            {
                number = Convert.ToInt32(Convert.ToDouble(str));
            }
            catch (System.OverflowException)
            {
                return true;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method checks if the string overflows ths size of a double data typed variable 
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>bool</returns>
        public bool IsNumericOverflow(string str)
        {
            int number = 0;
            try
            {
                number = Convert.ToInt32(Convert.ToDouble(str));
            }
            catch (System.OverflowException)
            {
                return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method checks if the given character is numeric 
        /// </summary>
        /// <param name="AsciiChar">char</param>
        /// <returns>bool</returns>
        public static bool IsNumeric(Char AsciiChar)
        {
            int asciiVal = Convert.ToInt32(AsciiChar);
            if ((asciiVal >= 48 && asciiVal <= 57) || (asciiVal == 8))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        ///<author>Sejal Maheshwari</author>
        /// <created>15-Jan-07</created>
        /// <summary>
        /// Function to check is entered value is of type double or not
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>bool</returns>
        public static bool IsDouble(string value)
        {
            bool isValid = false;
            double checkValue = 0.0;

            double.TryParse(value, out checkValue);

            if (checkValue > 0) isValid = true;

            return isValid;
        }

        /// <summary>
        /// Mehtod checks if passed string is integral number
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>bool</returns>
        public static bool IsInteger(string value)
        {
            bool isValid = false;
            int checkValue = 0;

            int.TryParse(value, out checkValue);
            if (checkValue > 0) isValid = true;

            return isValid;
        }

        /// <summary>
        /// Method checks if a given character is alphabet or number (not special character)
        /// </summary>
        /// <param name="AsciiChar">Char</param>
        /// <returns>bool </returns>
        public static bool IsAlphaNumeric(Char AsciiChar)
        {

            int asciiVal = Convert.ToInt32(AsciiChar);
            if ((asciiVal >= 65 && asciiVal <= 90) || (asciiVal >= 97 && asciiVal <= 122) || (asciiVal >= 48 && asciiVal <= 57) || (asciiVal == 8) || (asciiVal == 32))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <author>shrujal Shah</author>
        /// <created>22-Dec-2005</created>
        /// <summary>Method checks if the character is valid aplhanumeric character
        /// </summary>
        ///<returns>bool</returns>
        public static bool IsValidCode(Char AsciiChar)
        {

            int asciiVal = Convert.ToInt32(AsciiChar);
            if ((asciiVal >= 65 && asciiVal <= 90) || (asciiVal >= 97 && asciiVal <= 122) || (asciiVal >= 48 && asciiVal <= 57) || (asciiVal == 8))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method checks if the given string is valid alpha numeric string 
        /// </summary>
        /// <param name="strVal">string</param>
        /// <returns>bool</returns>
        public static bool IsAlphaNumeric(string strVal)
        {

            if (strVal == string.Empty) return false;
            int i = 0;
            int asciiVal = 0;
            for (i = 0; i <= strVal.Length - 1; i++)
            {
                asciiVal = Convert.ToInt32(Convert.ToChar(strVal.Substring(i, 1).ToString()));
                if ((asciiVal >= 65 && asciiVal <= 90) || (asciiVal >= 97 && asciiVal <= 122) || (asciiVal >= 48 && asciiVal <= 57) || (asciiVal == 8) || (asciiVal == 32))
                {
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validate Card Number
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns>True/False</returns>
        public static bool IsValidCardNumber(string strVal)
        {
            if (strVal == string.Empty || strVal.Length != 16) return false;

            int i = 0;
            int asciiVal = 0;
            for (i = 0; i <= strVal.Length - 1; i++)
            {
                asciiVal = Convert.ToInt32(Convert.ToChar(strVal.Substring(i, 1).ToString()));
                if ((asciiVal >= 48 && asciiVal <= 57))
                {

                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Method checks if the character is valid for phone number
        /// </summary>
        /// <param name="AsciiChar">char</param>
        /// <returns>bool</returns>
        public static bool IsValidPhone(Char AsciiChar)
        {

            int asciiVal = Convert.ToInt32(AsciiChar);
            if ((asciiVal >= 48 && asciiVal <= 57) || (asciiVal == 8) || (asciiVal == 43) || (asciiVal == 91) || (asciiVal == 93) || (asciiVal == 40) || (asciiVal == 41) || (asciiVal == 32))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method checks if the string overflows an long int64 data type variable
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>bool</returns>
        public bool IsNumberOverFlow(string str)
        {
            Int64 number = 0;
            try
            {
                number = Convert.ToInt64(str);
            }
            catch (System.OverflowException)
            {
                return false; //overflow
            }
            catch
            {
                return false; //not a number
            }
            if (number.ToString() != str) return false;
            else return true;
        }

        /// <summary>
        /// Method checks if the string is null or empty
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>bool</returns>
        public static bool IsEmpty(string str)
        {
            try
            {
                if ((str == null) || (str == string.Empty)) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method checks if the string length is between 8 and 16 characters
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>bool</returns>
        public bool Size(string str)
        {
            try
            {
                if ((str.Length >= 8) && (str.Length < 16)) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method checks if the string is a valid email address
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>bool</returns>
        public static bool IsEmail(string str)
        {
            string emailPattern = @"^[ ]*\w+([-+.!#$%&'*+-/=?^_`.{|}~\w]+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*[ ]*$"; //Mahesh Patel:17/12/2008:Fogbug Case:7635
            return System.Text.RegularExpressions.Regex.IsMatch(str, emailPattern);
        }

        /// <summary>
        /// Method checks is the string is a valid UK post code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>bool</returns>
        public static bool IsPostCode(string str)
        {
            string postPattern = "(GIR 0AA)|(^[A-z]{1,2}\\d{1,2}([A-z]{0,1})[\\x20]{0,1}([A-z]{0,1}){0,1}\\d{1,2}([A-z]{1,2}))";
            return System.Text.RegularExpressions.Regex.IsMatch(str, postPattern);
        }

        /// <summary>
        /// Method checks if the string is a valid sort code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>bool</returns>
        public static bool IsSortCode(string str)
        {
            string pattern = "[0-9\\s]*";
            return System.Text.RegularExpressions.Regex.IsMatch(str, pattern);
        }

        /// <author>Sejal Maheshwari</author>
        /// <created>23-Dec-2008</created>
        /// <summary>National ID card should allow only alphanumeric and angular bracket
        /// </summary>
        /// <param name="string">string
        /// <returns>bool
        /// </returns>
        public static bool IsNationalIDCard(string str)
        {
            bool isvalid = false;

            if (Regex.IsMatch(str, "^[a-zA-Z0-9<\\s]+$"))
                isvalid = true;

            return isvalid;
        }

        /// <author>Sapan Patibandha</author>
        /// <created>18-Dec-2006</created>
        /// <summary>get full phone number base on phone number and given code 
        /// </summary>
        /// <param name="phoneNumber">string
        /// </param>
        /// <param name="countryCode">string
        /// </param>
        /// <returns>string 
        /// </returns>
        public static string GetFullPhoneNumber(string phoneNumber, string countryCode)
        {
            string phoneNo = phoneNumber;

            while (phoneNo.StartsWith("0") || phoneNo.StartsWith("+"))
                phoneNo = phoneNo.Substring(1);
            if (!(phoneNo.StartsWith(countryCode))) phoneNo = countryCode + phoneNo;

            return phoneNo;
        }

        /// <summary>
        /// Method checks if the two strings are equal
        /// </summary>
        /// <param name="str1">string</param>
        /// <param name="str2">string</param>
        /// <returns>bool</returns>
        public bool IsEqual(string str1, string str2)
        {
            try
            {
                //if (str1.ToLower().Equals(str2.ToLower)) 
                if (str1.Equals(str2))
                //str1.CompareTo
                {
                    return true;
                }
                else
                { return false; }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method checks if the string is valid URL
        /// </summary>
        /// <param name="URL">string</param>
        /// <returns>bool</returns>
        public static bool IsURL(string URL)
        {
            if (URL == null || URL.Length == 0) return false;
            if (URL.IndexOf("http", 1) < 0) URL = "http://" + URL;
            string urlpattern = "^(http|ftp|https)://" + "(www\\.)?.+\\.(com|net|org)$";
            //			string urlpattern=("^[http]{1}(://){1}[_a-z0-9-]+(\\.){1}[a-z]+$");
            return System.Text.RegularExpressions.Regex.IsMatch(URL, urlpattern);

        }

        /// <summary>
        /// Method checks if the method is a valid URI
        /// </summary>
        /// <param name="URI">string</param>
        /// <returns>bool</returns>
        public static bool IsURI(string URI)
        {
            if (URI == string.Empty) return true;
            try
            {
                if (URI.IndexOf("//", 1) < 0) URI = "//" + URI;
                Uri myUri = new Uri(URI);
                return true;
            }
            catch (UriFormatException)
            {
                return false;
            }

        }

        /// <summary>
        /// Method checks if the string is valid password of length 8 to 50 and at has at least 2 numeric characters
        /// </summary>
        /// <param name="Password">string</param>
        /// <returns>bool</returns>
        public static bool IsPassword(string Password)
        {
            //check for empty
            if (Password == string.Empty) return false;
            //check for valid length (must be between 8 and 50)
            if (Password.Length < 8 || Password.Length > 50) return false;
            //check whether 2 numbers are included or not
            int i = 0;
            int occuranceCount = 0;
            int PrevPosition = 0;
            for (i = 0; i <= 9; i++)
            {
                PrevPosition = Password.IndexOf(i.ToString());
                if (PrevPosition != -1)
                {
                    occuranceCount = occuranceCount + 1;
                    if (Password.IndexOf(i.ToString(), PrevPosition + 1) != -1)
                    {
                        occuranceCount = occuranceCount + 1;
                    }
                    if (occuranceCount >= 2) return true;
                }
            }
            return false;
        }

        //		public bool IsSpecialChar(string str)
        //		{
        //			string[] specialChar={"~!@#$%^&*()_+=-`,./';:"};
        //			try
        //			{
        //				if(!((str==null) || (str==string.Empty))) 
        //				{
        //					//if (str.StartsWith(specialChar)) return true;  
        //					if (str.IndexOf(specialChar.ToString())== -1)
        //					{return true; }						
        //					else 
        //					{return false;}
        //				}
        //			}
        //			catch
        //			{
        //				return false;
        //			}
        //		}

        /// <summary>
        /// Method checks if the file name is valid
        /// </summary>
        /// <param name="fileName">string</param>
        /// <returns>bool</returns>
        public static bool IsValidFileName(string fileName)
        {

            bool result = true;

            char[] invalidChar = new char[9];

            invalidChar[0] = '/';
            invalidChar[1] = '\\';
            invalidChar[2] = ':';
            invalidChar[3] = '*';
            invalidChar[4] = '?';
            invalidChar[5] = '<';
            invalidChar[6] = '>';
            invalidChar[7] = '|';
            invalidChar[8] = '"';

            if (fileName.IndexOfAny(invalidChar) != -1)
                result = false;

            return result;
        }

        /// <summary>
        /// Method checks if the file name is valid
        /// </summary>
        /// <param name="fileName">string</param>
        /// <returns>bool</returns>
        public static bool IsValidFileFormat(string fileName)
        {

            bool result = true;

            char[] invalidChar = new char[9];

            invalidChar[0] = '/';
            invalidChar[1] = '\\';
            invalidChar[2] = ':';
            invalidChar[5] = '<';
            invalidChar[6] = '>';
            invalidChar[7] = '|';
            invalidChar[8] = '"';

            if (fileName.IndexOfAny(invalidChar) != -1)
                result = false;

            return result;
        }


        /// <author>Shrujal Shah</author>
        /// <created>26-May-2006</created>
        /// <summary>
        /// Replace space in code
        /// </summary>
        public static string RemoveSpace(string code)
        {
            code = code.Replace(" ", "");
            return code;
        }

        /// <author>Falguni Parikh</author>
        /// <created>26-May-2006</created>
        /// <summary>
        /// Remove hypen from code.        
        /// </summary>
        public static string RemoveHyphen(string code)
        {
            if (code == null || code == string.Empty) return code;
            code = code.Replace("-", "");
            return code;
        }

        /// <author>Shrujal Shah</author>
        /// <created>26-May-2006</created>
        /// <summary>
        /// Check for authorisation code is within range or not.
        /// </summary>        
        public static bool IsAuthorisationCodeValid(string strAuthorisationCode)
        {
            strAuthorisationCode = RemoveHyphen(strAuthorisationCode);
            strAuthorisationCode = RemoveSpace(strAuthorisationCode);

            if (strAuthorisationCode.Length == 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-Jun-2006</created>
        /// <summary>This will generate complate word for given days.
        /// </summary> 
        public static string DayOrDays(int noOfDay)
        {
            if (noOfDay > 1)
                return noOfDay.ToString() + " days";
            else
                return noOfDay.ToString() + " day";
        }

        /// <author>Arvind Ashapuri</author>
        /// <created>29-Jun-2006</created>
        /// <summary>This will generate complate word for given working days.
        /// </summary> 
        public static string WorkingDayOrDays(int noOfDay)
        {
            if (noOfDay > 1)
                return noOfDay.ToString() + " working days";
            else
                return noOfDay.ToString() + " working day";
        }

        /// <author>Shrujal Shah</author>
        /// <created>29-Jun-2006</created>
        /// <summary>This will generate formatted name
        /// </summary> 
        public static string AutoFormatName(string input)
        {
            string formattedName = string.Empty;
            formattedName = System.Text.RegularExpressions.Regex.Replace(input, @"\w+", new MatchEvaluator(FormatText));
            return formattedName;
        }

        /// <author>Shrujal Shah</author>
        /// <created>29-Jun-2006</created>
        /// <summary>This will generate formatted text
        /// </summary> 
        private static string FormatText(Match m)
        {
            // Get the matched string.
            string inputText = m.ToString();
            inputText = inputText.ToLower();
            // If the first char is lower case...
            if (char.IsLower(inputText[0]))
            {
                // Capitalize it.
                inputText = char.ToUpper(inputText[0]) + inputText.Substring(1, inputText.Length - 1);
            }
            // If string starts with Mac or Mc
            if (inputText.StartsWith("Mc") && inputText.Trim().Length > 2) //Sapan: length checking is added for Case 10531:
            {
                // Capitalize it.
                inputText = inputText.Substring(0, 2) + char.ToUpper(inputText[2]) + inputText.Substring(3);
            }
            // If string starts with Mac or Mc
            if (inputText.StartsWith("Mac") && inputText.Trim().Length > 3) //Sapan: length checking is added for Case 10531:
            {
                // Capitalize it.
                inputText = inputText.Substring(0, 3) + char.ToUpper(inputText[3]) + inputText.Substring(4);
            }
            return inputText;
        }

        /// <summary>
        /// Method generates string to disable a button with the given validation Group name
        /// </summary>
        /// <param name="validationGroupName">string</param>
        /// <returns>string</returns>
        public static string GetDisableButtonScript(string validationGroupName)
        {
            System.Text.StringBuilder sbValid = new System.Text.StringBuilder();
            sbValid.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sbValid.Append("if (Page_ClientValidate('" + validationGroupName + "' ) == false) { return false; }} ");
            sbValid.Append("this.disabled = true;");
            return sbValid.ToString();
        }

        /// <summary>
        /// MEthod get regular expression for email validation
        /// </summary>
        /// <returns></returns>
        public static string GetEmailRegExpression()
        {
            return @"^[ ]*\w+([-+.!#$%&'*+-/=?^_`.{|}~\w]+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*[ ]*$"; //Mahesh Patel:17/12/2008:Fogbug Case:7635
        }

        ///// <author>Sapan Patibandha</author>
        ///// <created>15-Nov-2006</created>
        ///// <summary>check for printer is installed or not
        ///// </summary>
        ///// <returns>true if printer is installed
        ///// </returns>
        //public static bool IsPrinterInstall()
        //{
        //    if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0)
        //        return true;
        //    else
        //        return false;
        //}

        /// <author>Rishit</author>
        /// <created>30/10/2006</created>
        /// <summary> This will get Obsecured Maestrocard Number in format eg. 1234-*****-****-1234
        /// </summary>
        /// <param name="cardNumber">
        /// string cardNumber
        /// </param>
        /// <returns>
        /// return string as Obsecured Maestrocard Number in format eg. 1234-*****-****-1234
        /// </returns>
        public static string ObscuredPANValue(string cardNumber)
        {
            int cardLength = cardNumber.Length;

            if (cardNumber.Length < 4) return cardNumber;

            StringBuilder errorList = new StringBuilder();

            errorList.Append(cardNumber.Substring(0, 4));
            errorList.Append("-****-****-");
            errorList.Append(cardNumber.Substring(cardLength - 4));

            return errorList.ToString();
        }

        /// <summary>
        /// Method removes special characters from the given string
        /// </summary>
        /// <param name="input">string</param>
        /// <returns>string</returns>
        public static string RemoveSpecialCharacters(string input)
        {
            return Regex.Replace(input, @"[^\w]", "");
        }

        /// <summary>
        /// MEthod obscures the center half of the passed string
        /// </summary>
        /// <param name="code">string</param>
        /// <returns>string</returns>
        public static string GetObscuredValue(string code)
        {

            string subText1 = string.Empty;
            string subText2 = string.Empty;


            subText1 = code.Substring(0, code.Length / 2);
            subText2 = code.Substring(code.Length / 2);


            if (subText1.Length >= 2)
            {
                subText1 = subText1.Substring(0, subText1.Length - 2) + "**";
            }
            else
            {
                subText1 = subText1.Remove(0, subText1.Length).PadRight(subText1.Length, '*');
            }

            if (subText2.Length >= 2)
            {
                subText2 = "**" + subText2.Substring(2);
            }
            else
            {
                subText2 = subText2.Remove(0, subText2.Length).PadLeft(subText2.Length, '*');
            }

            return subText1 + subText2;

        }

        /// <summary>
        /// Method gets Time elapsed in terms of days hours and minutes as string
        /// </summary>
        /// <param name="t">Timespan</param>
        /// <returns>string</returns>
        public static string GetTimeText(TimeSpan t)
        {
            string _days = string.Empty;
            string _hours = string.Empty;
            string _minutes = string.Empty;
            string _time = string.Empty;
            int _hour = 0;
            int _day = 0;
            int _minute = 0;
            _hour = t.Hours;
            _day = t.Days;
            _minute = t.Minutes;
            if (_minute > 0)
            {
                if (_minute > 30)
                {
                    _hour += 1;
                }
                else
                {
                    _minutes = _minute.ToString() + "m";
                }
            }

            if (_hour >= 24)
            {
                _day += 1;
                _hour -= 24;
            }
            if (_day > 0)
            {
                _days = _day.ToString() + "d";
            }
            if (_hour > 0)
            {
                _hours = _hour.ToString() + "h";
            }
            if (_days != string.Empty || _hours != string.Empty)
            {
                _time = _days + _hours;
            }
            else if (_minutes != string.Empty)
            {
                _time = _minutes;
            }
            else
            {
                _time = "<h";
            }
            return _time;
        }


        /// <author>Nimesh Panchal</author>
        /// <created>02 July 2007</created>
        /// <summary>Checks for input string is alphanumeric or not
        /// </summary>
        public static bool IsValidAlphaNumeric(string strVal)
        {
            if (Regex.IsMatch(strVal, "^[a-zA-Z0-9]+$"))
                return true;
            else
                return false;
        }

        /// <author>Nimesh Panchal</author>
        /// <created>06-Dec-2007</created>
        /// <summary>
        ///   Returns the fee charge display string
        /// </summary>
        /// <param name="credMoneyValue">long</param>
        /// <param name="credMoneyString">string</param>
        /// <param name="percent">double</param>
        /// <returns>string</returns>
        public static string GetFeeDisplayString(long credMoneyValue, string credMoneyString, double percent)
        {
            return GetFeeDisplayString(credMoneyValue, credMoneyString, percent, false);
        }

        /// <author>Tina Mori</author>
        /// <created>17-Jan-2008</created>
        /// <summary> fee charge display string</summary>
        /// <param name="credMoneyValue">long</param>
        /// <param name="credMoneyString">string</param>
        /// <param name="percent">double</param>
        /// <param name="credMoneyAndCharge">bool - whether both charge and percent are applied or either of the 2</param>
        /// <param name="IsDisplayWhicheverIsHigher">bool - whether the higher of the two (percent,value) is to be displayed</param>
        /// <returns>string</returns>
        public static string GetFeeDisplayString(long credMoneyValue, string credMoneyString, double percent, bool credMoneyAndCharge, ref bool IsDisplayWhicheverIsHigher)
        {
            string feeString = string.Empty;

            if (credMoneyValue != 0 && percent != 0)
            {
                if (credMoneyAndCharge)
                    feeString = credMoneyString + " plus " + percent.ToString("0.00") + "%";
                else
                {
                    feeString = credMoneyString + " or " + percent.ToString("0.00") + "% *";
                    IsDisplayWhicheverIsHigher = true;
                }
            }

            if (credMoneyValue != 0 && percent == 0)
                feeString = credMoneyString;

            if (credMoneyValue == 0 && percent != 0)
                feeString = percent.ToString("0.00") + "%";

            if (credMoneyValue == 0 && percent == 0)
                feeString = "FREE";

            return feeString;
        }

        /// <summary>
        /// MEthod gets display string for fees
        /// </summary>
        /// <param name="credMoneyValue">long</param>
        /// <param name="credMoneyString">string</param>
        /// <param name="percent">double</param>
        /// <param name="credMoneyAndCharge">bool</param>
        /// <returns>string</returns>
        public static string GetFeeDisplayString(long credMoneyValue, string credMoneyString, double percent, bool credMoneyAndCharge)
        {
            bool IsDisplay = false;
            return GetFeeDisplayString(credMoneyValue, credMoneyString, percent, credMoneyAndCharge, ref IsDisplay);
        }

        /// <author>Nimesh Panchal</author>
        /// <created>07-Dec-2007</created>
        /// <summary>
        ///   Returns the fee charge display string
        /// </summary>
        /// <param name="credMoneyValue">long</param>
        /// <param name="credMoneyString">string</param>
        /// <returns>string</returns>
        public static string GetFeeDisplayString(long credMoneyValue, string credMoneyString)
        {
            return GetFeeDisplayString(credMoneyValue, credMoneyString, 0);
        }

        /// <author>Nimesh Panchal</author>
        /// <created>12-March-2008</created>
        /// <summary>
        ///   Checks for input string is alphabets or not
        /// </summary>
        /// <param name="strVal">string</param>
        /// <returns>bool</returns>
        public static bool IsAlphabets(string strVal)
        {
            if (Regex.IsMatch(strVal, "^[a-zA-Z]+$"))
                return true;
            else
                return false;
        }

        /// <author>Gaurang Majithiya</author>
        /// <created>16-Jan-2009</created>
        /// <summary>
        /// To check password is valid or not
        /// </summary>
        /// <param name="password">password</param>
        /// <returns>true or false</returns>
        public static bool IsValidPassword(string password)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(password, PasswordRegularExpression);
        }

        /// <author>Gaurang Majithiya</author>
        /// <created>16-Jan-2009</created>
        /// <summary>
        /// Gets regular expression
        /// </summary>
        /// <returns>regular expression for password</returns>
        public static string PasswordRegularExpression
        {
            get
            {
                return @"^.*(?=.{8,20})(?=.*\d{2,})(?=.*[a-zA-Z]{2,})(?=.*[!@#$%^&\*+=\|\?]{0,}).*$";
            }
        }

        /// <author>Gaurang Majithiya</author>
        /// <created>1-Sep-2009</created>
        /// <summary>
        /// This function will check invalid characters, if found any invalid character it will return false. Case 10827
        /// </summary>
        /// <param name="cEcText">string</param>
        /// <returns>bool</returns>
        public static bool IsAlphaNumericWithSpecialCharacters(string cEcText)
        {
            /*Case 10827 : However, when a card is requested for the user with First / last name having special char, 
              few Special chars are getting changed(with '?') onto the CRI file on Card display Name Tag.*/

            //if (Regex.IsMatch(cEcText, @"^([A-Z0-9a-zÆØÅĄĆĘŁŃÓŚŹŻÄÖßÜÀÉ\.\,\/\&\-\'\s])+$"))

            if (Regex.IsMatch(cEcText, @"^([A-Z0-9a-z\.\,\/\&\-\'\s])+$"))
                return true;
            else
                return false;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>07-Dec-2010</created>
        /// <summary>Allow alpha numeric with special chars including space and hyphen </summary>
        /// <param name="cEcText">string</param>
        /// <returns>bool</returns>
        public static bool IsAlphabetsWithSpecialCharactersWithSpace(string cEcText)
        {
            if (Regex.IsMatch(cEcText, @"^([a-zA-Z/&\-'.,\s])+$"))
                return true;
            else
                return false;
        }

        /// <author>Ashka Modi</author>
        /// <created>30-Aug-2010</created>
        /// <summary>
        /// get non numeric characters
        /// </summary>
        /// <param name="cEcText">string</param>
        /// <returns>bool</returns>
        public static bool IsAlphabetsWithSpecialCharacters(string cEcText)
        {
            if (Regex.IsMatch(cEcText, @"^([a-zA-Z/&-'.,])+$"))
                return true;
            else
                return false;
        }

        /// <author>Keyur Parekh</author>
        /// <created>22-Jan-2010</created>
        /// <summary>
        /// Validate IP Address
        /// </summary>
        /// <param name="str">String</param>
        /// <returns></returns>
        public static bool IsValidIPAddress(string str)
        {
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

            return Regex.IsMatch(str, pattern);
        }

        public static DateTime GetRegularDateFromJulianDate(string julianDate)
        {
            DateTime newDate = DateTime.MinValue;
            julianDate = julianDate.Trim();

            if (julianDate.Length == 4) //YDDD
                julianDate = DateTime.Now.Year.ToString().Substring(0, 3) + julianDate; // ADD CCY in date

            if (julianDate.Length == 5) //YYDDD
                julianDate = DateTime.Now.Year.ToString().Substring(0, 2) + julianDate; // ADD CC in date

            if (julianDate.Length == 7)  //CCYYDDD
            {
                newDate = new DateTime(Convert.ToInt32(julianDate.Substring(0, 4)), 01, 01);
                newDate = newDate.AddDays(Convert.ToInt32(julianDate) % 1000 - 1);
            }

            return newDate;
        }

        /// <author>Keyur Parekh</author>
        /// <created>25-Jun-2012</created>
        /// <summary>
        /// Validate Track data name
        /// </summary>
        /// <param name="str">String</param>
        /// <returns></returns>
        public static bool IsValidTrackDataName(string str)
        {
            string pattern = @"^([A-Z0-9\-\\\.\/\s]){2,26}$";
            return Regex.IsMatch(str, pattern);
        }

        ///<Author>Dharati Metra</Author>
        ///<CreatedDate>16-Dec-2014</CreatedDate>
        /// <summary>
        /// Validate Card Number with 8 digits
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns>True/False</returns>
        public static bool IsValid8DigitCardNumber(string strVal)   //Case: 20325
        {
            if (strVal == string.Empty || strVal.Length != 8) return false;

            int i = 0;
            int asciiVal = 0;
            for (i = 0; i <= strVal.Length - 1; i++)
            {
                asciiVal = Convert.ToInt32(Convert.ToChar(strVal.Substring(i, 1).ToString()));
                if ((asciiVal >= 48 && asciiVal <= 57))
                {

                }
                else
                    return false;
            }
            return true;
        }

        /// <author>Keyur Parekh</author>
        /// <created>22-Jul-2015</created>
        /// <summary>
        /// To check password is strong or not
        /// </summary>
        /// <param name="password">password</param>
        /// <returns>true or false</returns>
        public static bool IsValidStrongPassword(string password)
        {
            bool flag = false;

            HashSet<char> specialCharacters = new HashSet<char>() { '#', '!', '?', '^', '@' };
            if (
                password.Length > 7 &&
                password.Any(char.IsUpper) &&
                password.Any(char.IsLower) &&
                password.Any(char.IsDigit) &&
                password.Any(specialCharacters.Contains))
            {
                flag = true;
            }

            return flag;
        }

        /// <author>Aarti Meswania</author>
        /// <created>04-Jan-2019</created>
        /// <summary>Replace new line with space</summary>
        /// Sample : Regex.Replace("\raaa\r\nbbb\rnnn\nooo\r", @"\r\n|\r|\n", " "); --> output = " aaa bbb nnn ooo ";
        public static string ReplaceNewLineWithSpace(string text)
        {
            if (text != null)
                text = text = Regex.Replace(text, @"\r\n|\r|\n", " ");

            return text;
        }

        /// <author>Vipul patel</author>
        /// <created>19-Mar-2019</created>
        /// <summary>
        /// Method to GetEnumDescription
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>returns string</returns>
        public static string GetEnumDescription(Enum value)
        {
            //Case 33642
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string description = string.Empty;
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            else
            {
                description = value.ToString();
            }

            // Get enum description from resource file if specified. e.g. : resource key should be like : <Enum class name>_<Enum name>
            string[] classNames = (fi.FieldType).FullName.Split('.');
            string keyName = classNames[classNames.Length - 1] + "_" + value.ToString();
            try
            {
                string tmpValue = CommonMessage.GetMessage(keyName);
                if (!string.IsNullOrEmpty(tmpValue)) description = tmpValue;
            }
            catch
            {
                //DO NOTHING
            }

            return description;
        }

        /// <author>Aarti Meswania</author>
        /// <created>01-jul-2016</created>
        /// <summary>Method will split string by supplied character</summary>
        /// <example>
        /// -----------------------Input
        /// input = "123,345,,347"
        /// splitBy = ','
        /// -----------------------Output
        /// string[0] = 123
        /// string[1] = 345
        /// string[2] = 347
        /// Ignores empty values
        /// </example>
        public static List<string> SplitString(string input, char splitBy)
        {
            List<string> list = null;
            if (!string.IsNullOrEmpty(input))
            {
                string[] ipList = input.Split(splitBy);
                for (int i = 0; i <= ipList.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(ipList[i].Trim()))
                    {
                        if (list == null) list = new List<string>();
                        list.Add(ipList[i].Trim());
                    }
                }
            }
            return list;
        }

        /// <author>Aarti Meswania</author>
        /// <created>01-jul-2016</created>
        /// <summary>Method will split string by supplied character and will ignore splitting inside provided Group character</summary>
        /// <example>
        /// -----------------------Input
        /// input = "123,345,|456,789,22|,347"
        /// splitBy = ','
        /// groupChar = '|'
        /// -----------------------Output
        /// string[0] = 123
        /// string[1] = 345
        /// string[2] = |456,789,22|
        /// string[3] = 347
        /// </example>
        public static string[] SplitString(string input, char splitBy, char groupChar)
        {
            List<string> list = new List<string>();
            char[] specialCharacters = { '|', '$', '*', '+', '?', '[', ']', '^', '.', '(', ')', '"' };
            string splitChar = (specialCharacters.Contains(splitBy) ? @"\" + splitBy.ToString() : splitBy.ToString());

            string ignoreWithInGroupChar = (specialCharacters.Contains((char)groupChar) ? @"\" + groupChar.ToString() : groupChar.ToString());
            Regex csvSplit = new Regex(@"(?:^|" + splitChar + ")("
                                            + ignoreWithInGroupChar
                                            + "(?:[^" + ignoreWithInGroupChar + "]+|"
                                            + ignoreWithInGroupChar + "" + ignoreWithInGroupChar + ")*"
                                            + ignoreWithInGroupChar
                                            + "|[^" + splitChar + "]*)"
                                            , RegexOptions.Compiled);
            //split and set values in list
            string curr = null;
            foreach (Match match in csvSplit.Matches(input))
            {
                curr = match.Value;
                if (0 == curr.Length)
                {
                    list.Add("");
                }

                list.Add(curr.TrimStart(','));
            }
            return list.ToArray();
        }

        public static object GetEnumByDescription<T>(string desc)
        {
            foreach (Enum enumval in Enum.GetValues(typeof(T)))
            {
                if (GetAttribute<DescriptionAttribute>(enumval).Description == desc)
                {
                    return enumval;
                }
            }

            throw new ArgumentException("no such enum found for desc = " + desc + ", For " + typeof(T).ToString());
        }

        private static TAttribute GetAttribute<TAttribute>(Enum enumValue)
        where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }


        /// <summary>Logic for serial number in a such way that module of (srno+1) should not become zero (0) 
        /// if zero then will update +2 in count.
        /// </summary>
        public static Int64 GetNextSubmissionSerialNumber(Int64 currSubmissionSerialNumber)
        {
            Int64 nextSubmissionSerialNumber = 0;
            nextSubmissionSerialNumber = currSubmissionSerialNumber + 1;
            if (nextSubmissionSerialNumber % value100 == 0)
            {
                nextSubmissionSerialNumber = currSubmissionSerialNumber + value2;
            }
            return nextSubmissionSerialNumber;
        }
    }
}
