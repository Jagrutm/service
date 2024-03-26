using System;
using System.Text;

namespace CredECard.Common.BusinessService
{
    /// <summary>
    /// Summary description for HexEncoding.
    /// </summary>
    public class HexEncoding
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HexEncoding()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Gets the count of bytes in the string 
        /// </summary>
        /// <param name="hexString">string</param>
        /// <returns>int</returns>
        public static int GetByteCount(string hexString)
        {
            int numHexChars = 0;
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                    numHexChars++;
            }
            // if odd number of characters, discard last character
            if (numHexChars % 2 != 0)
            {
                numHexChars--;
            }
            return numHexChars / 2; // 2 characters per byte
        }

        public static byte[] GetBytes(string hexString)
        {
            int discard = 0;
            return GetBytes(hexString, out discard);
        }

        /// <author>Aarti Meswania</author>
        /// <created>12-Feb-2015</created>
        /// <summary>Read all bits from supplied byte and convert into boolean array.</summary>
        /// <param name="byteValue">byte</param>
        /// <returns>bool array</returns>
        public static bool[] GetBits(byte byteValue)
        {
            string eightBits = Convert.ToString(byteValue, 2).PadLeft(8, '0');
            bool[] bits = new bool[8];
            int counter = 0;
            foreach (char bit in eightBits)
            {
                bits[counter] = (bit == '1' ? true : false);
                counter++;
            }
            return bits;
        }

        /// <summary>
        /// Creates a byte array from the hexadecimal string. Each two characters are combined
        /// to create one byte. First two hexadecimal characters become first byte in returned array.
        /// Non-hexadecimal characters are ignored. 
        /// </summary>
        /// <param name="hexString">string to convert to byte array</param>
        /// <param name="discarded">number of characters in string ignored</param>
        /// <returns>byte array, in the same left-to-right order as the hexString</returns>
        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            string newString = "";
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                    newString += c;
                else
                    discarded++;
            }
            // if odd number of characters, discard last character
            if (newString.Length % 2 != 0)
            {
                discarded++;
                newString = newString.Substring(0, newString.Length - 1);
            }

            int byteLength = newString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new String(new Char[] { newString[j], newString[j + 1] });
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }

        #region public static byte[] ConvertAsciiToEbcdic(byte[] asciiData)

        public static byte[] ConvertAsciiToEbcdic(string asciiData)
        {
            byte[] asciiByte = Encoding.Default.GetBytes(asciiData);
            return ConvertAsciiToEbcdic(asciiByte);
        }

        public static byte[] ConvertAsciiToEbcdic(byte[] asciiData)
        {
            // Create two different encodings.          
            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding("IBM037");

            //Retutn Ebcdic Data 
            return Encoding.Convert(ascii, ebcdic, asciiData);
        }

        public static byte[] ConvertEbcdicToAsciiBytes(byte[] asciiData)
        {
            // Create two different encodings.          
            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding("IBM037");

            //Retutn Ebcdic Data 
            return Encoding.Convert(ebcdic, ascii, asciiData);
        }


        public static byte[] ConvertASCIIToHex(byte[] asciiData)
        {
            string str = Encoding.ASCII.GetString(asciiData);
            char[] charValues = str.ToCharArray();
            string hexOutput = "";
            foreach (char _eachChar in charValues)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(_eachChar);
                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput += String.Format("{0:X2}", value);
                // to make output as your eg 
                //  hexOutput +=" "+ String.Format("{0:X}", value);

            }

            return GetBytes(hexOutput);
        }

        #endregion


        #region public static byte[] ConvertEbcdicToAscii(byte[] ebcdicData)
        //public static byte[] ConvertEbcdicToAscii(byte[] ebcdicData)
        //{

        //    if (ebcdicData == null)
        //        return null;

        //    // Create two different encodings.       
        //    Encoding ascii = Encoding.ASCII;
        //    Encoding ebcdic = Encoding.GetEncoding("IBM037");

        //    //Retutn Ascii Data  

        //    return Encoding.Convert(ebcdic, ascii, ebcdicData);
        //}

        //private static string ConvertEBCDICtoASCII(string strEBCDICString)
        //{
        //    int[] e2a = new int[256]{0, 1, 2, 3,156, 9,134,127,151,141,142, 11, 12, 13, 14, 15,
        //                                16, 17, 18, 19,157,133, 8,135, 24, 25,146,143, 28, 29, 30, 31,
        //                                128,129,130,131,132, 10, 23, 27,136,137,138,139,140, 5, 6, 7,
        //                                144,145, 22,147,148,149,150, 4,152,153,154,155, 20, 21,158, 26,
        //                                32,160,161,162,163,164,165,166,167,168, 91, 46, 60, 40, 43, 33,
        //                                38,169,170,171,172,173,174,175,176,177, 93, 36, 42, 41, 59, 94,
        //                                45, 47,178,179,180,181,182,183,184,185,124, 44, 37, 95, 62, 63,
        //                                186,187,188,189,190,191,192,193,194, 96, 58, 35, 64, 39, 61, 34,
        //                                195, 97, 98, 99,100,101,102,103,104,105,196,197,198,199,200,201,
        //                                202,106,107,108,109,110,111,112,113,114,203,204,205,206,207,208,
        //                                209,126,115,116,117,118,119,120,121,122,210,211,212,213,214,215,
        //                                216,217,218,219,220,221,222,223,224,225,226,227,228,229,230,231,
        //                                123, 65, 66, 67, 68, 69, 70, 71, 72, 73,232,233,234,235,236,237,
        //                                125, 74, 75, 76, 77, 78, 79, 80, 81, 82,238,239,240,241,242,243,
        //                                92,159, 83, 84, 85, 86, 87, 88, 89, 90,244,245,246,247,248,249,
        //                                48, 49, 50, 51, 52, 53, 54, 55, 56, 57,250,251,252,253,254,255
        //                                };

        //    char chrItem = Convert.ToChar("0");
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < strEBCDICString.Length; i++)
        //    {
        //        try
        //        {
        //            chrItem = Convert.ToChar(strEBCDICString.Substring(i, 1));
        //            sb.Append(Convert.ToChar(e2a[(int)chrItem]));
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            return string.Empty;
        //        }

        //    }
        //    string result = sb.ToString();
        //    sb = null;
        //    return result;
        //}
        private static string ConvertEbcdicToAscii(byte[] ebcdicData)
        {
            //System.Text.Decoder EBCDICDecoder = Encoding.GetEncoding(37).GetDecoder();
            //int length = ebcdicData.Length;
            //char[] chars = new char[length];
            //EBCDICDecoder.GetChars(ebcdicData, 0, length, chars, 0);
            //return new string(chars);

            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding("IBM037");
            //Encoding ebcdic = Encoding.UTF32;
            byte[] convertedByte = Encoding.Convert(ebcdic, ascii, ebcdicData);
            return Encoding.ASCII.GetString(convertedByte);

        }

//        Private EBCDICDecoder As System.Text.Decoder = System.Text.Encoding.GetEncoding(37).GetDecoder
//Private Function ConvertEbcdicToAscii(ByVal ebsidic_bytes As Byte()) As String
//Dim length As Integer = ebsidic_bytes.Count
//Dim chars() As Char = New Char(length - 1) {}
//EBCDICDecoder.GetChars(ebsidic_bytes, 0, length, chars, 0)
//Return New String(chars)
//End Function

        
        public static string ConvertEbcdicToAsciiString(byte[] ebcdicData)
        {
            //byte[] asciiByte = ConvertEbcdicToAscii(ebcdicData);

            //if (asciiByte != null)
            //    return Encoding.ASCII.GetString(asciiByte);
            //else
            //    return string.Empty;


            return ConvertEbcdicToAscii(ebcdicData);
        }

        #endregion
        /// <summary>
        /// Converts byte array to string
        /// </summary>
        /// <param name="bytes">byte[]</param>
        /// <returns>string</returns>
        public static string ToString(byte[] bytes)
        {
            string hexString = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                hexString += bytes[i].ToString("X2");
            }
            return hexString;
        }

        public static string HexToString(string hexstring)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hexstring.Length; i += 2)
            {
                string hs = hexstring.Substring(i, 2);
                sb.Append(Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }
            return sb.ToString();

        }

        public static string ByteArrayToHexString(byte[] Bytes)
        {
            StringBuilder Result = new StringBuilder(Bytes.Length * 2);
            // string HexAlphabet = "0123456789ABCDEF";
            string HexAlphabet = HexEncoding.ToString(Bytes);
            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }
            return Result.ToString();
        }

        public string ASCIItoHex(string Value)
        {
            StringBuilder sb = new StringBuilder();

            byte[] inputByte = Encoding.UTF8.GetBytes(Value);

            foreach (byte b in inputByte)
            {
                sb.Append(string.Format("{0:x2}", b));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines if given string is in proper hexadecimal string format
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static bool InHexFormat(string hexString)
        {
            bool hexFormat = true;

            foreach (char digit in hexString)
            {
                if (!IsHexDigit(digit))
                {
                    hexFormat = false;
                    break;
                }
            }
            return hexFormat;
        }

        /// <summary>
        /// Returns true is c is a hexadecimal digit (A-F, a-f, 0-9)
        /// </summary>
        /// <param name="c">Character to test</param>
        /// <returns>true if hex digit, false if not</returns>
        public static bool IsHexDigit(Char c)
        {
            int numChar;
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            c = Char.ToUpper(c);
            numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6))
                return true;
            if (numChar >= num1 && numChar < (num1 + 10))
                return true;
            return false;
        }
        /// <summary>
        /// Converts 1 or 2 character string into equivalant byte value
        /// </summary>
        /// <param name="hex">1 or 2 character string</param>
        /// <returns>byte</returns>
        private static byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return newByte;
        }
        /// <summary>
        /// Convert hex value into decimal value.
        /// </summary>
        /// <param name="hex"> hex string</param>
        /// <returns>integer decimal value</returns>
        public static int HexToDecimal(string hex)
        {
            int x = 0;
            int.TryParse(int.Parse(hex, System.Globalization.NumberStyles.HexNumber).ToString(), out x);
            return x;
        }

        /// <summary>
        /// Converts int to Hex byte
        /// </summary>
        /// <param name="val">int</param>
        /// <returns>byte[]</returns>
        public static byte ByteToHexByte(int val)
        {
            if (val > 255) throw new ArgumentException("val must be not more then 255");
            string hex = val.ToString("X");
            return HexToByte(hex);
        }
        
        /// <summary>
        /// Converts int to Hex byte array
        /// </summary>
        /// <param name="val">int</param>
        /// <returns>byte[]</returns>
        public static byte[] Int16toHexBytes(int val)
        {
            byte[] inby = BitConverter.GetBytes(Convert.ToInt16(val));
            byte[] hxby = new byte[inby.Length];
            for (int i = inby.Length, j = 0; i > 0; i--, j++) hxby[j] = inby[i - 1];
            return hxby;
        }

        /// <summary>
        /// Converts int to Hex byte array
        /// </summary>
        /// <param name="val">int</param>
        /// <returns>byte[]</returns>
        public static byte[] Int32toHexBytes(int val)
        {
            byte[] inby = BitConverter.GetBytes(Convert.ToInt32(val));
            byte[] hxby = new byte[inby.Length];
            for (int i = inby.Length, j = 0; i > 0; i--, j++) hxby[j] = inby[i - 1];
            return hxby;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>17-Apr-17</created>
        /// <summary>Converts the hexadecimal to ASCII.</summary>
        /// <param name="hexInput">The hexadecimal input.</param>
        /// <returns></returns>
        public static string ConvertHexToAscii(string hexInput)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return Encoding.ASCII.GetString(bytes);
        }

    }
}