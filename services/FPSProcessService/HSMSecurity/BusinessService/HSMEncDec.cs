using CredECard.CardProduction.BusinessService;
using CredECard.Common.BusinessService;
using CredEncryption.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CredECard.HSMSecurity.BusinessService
{
    public class HSMEncDec : HSMCommon
    {
        ///// <author>Nidhi Thakrar</author>
        ///// <created>11-Apr-17</created>
        ///// <summary>Encypts the specified data to encrypt.</summary>
        ///// <param name="DataToEncrypt">The data to encrypt.</param>
        ///// <returns></returns>
        //public string Encypt(string dataToEncrypt)
        //{
        //    string encryptedData = string.Empty;


        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    string strHexString = HexEncoding.ToString(Encoding.Default.GetBytes(dataToEncrypt));
        //    int len = strHexString.Length - (strHexString.Length % 16);
        //    List<string> inputList = split(strHexString, len);

        //    for (int i = 0; i < inputList.Count; i++)
        //    {
        //        objResult = encrypt(inputList[i].PadRight(len, 'F'), objResult.IVCBCEncDec);

        //        if (!objResult.IsSuccess)
        //            break;
        //        else
        //            encryptedData += objResult.Value;
        //    }
        //    return encryptedData;
        //}

        public string VDSPDecrypt(string encryptedData)
        {
            string decryptedData = string.Empty;

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            int len = encryptedData.Length - (encryptedData.Length % 16);
            List<string> inputList = split(encryptedData, len);

            for (int i = 0; i < inputList.Count; i++)
            {
                objResult = decrypt(inputList[i].PadRight(len, 'F'));

                if (!objResult.IsSuccess)
                    break;
                else
                    decryptedData += objResult.Value;
            }

            string strBlock1 = string.Empty;
            string strBlock2 = string.Empty;

            decryptedData = HexEncoding.ConvertHexToAscii(decryptedData);

            if (decryptedData.Length > 8)
            {

                strBlock1 = decryptedData.Substring(6, 2);
                int dataLength = 0;
                int.TryParse(strBlock1, out dataLength);

                strBlock2 = decryptedData.Substring(8, dataLength);
            }

            return strBlock2;

        }

        public string VDSPEncrypt(string dataToEncrypt)
        {
            string encryptedData = string.Empty;

            string strLength = dataToEncrypt.Length.ToString("00");
            string strBlock1 = "  " + getRandomString(4) + strLength;
            string strPadding = "".PadRight(8 - ((strBlock1.Length + dataToEncrypt.Length) % 8), '!');

            StringBuilder sbFinalBlock = new StringBuilder();
            sbFinalBlock.Append(strBlock1);
            sbFinalBlock.Append(dataToEncrypt);
            sbFinalBlock.Append(strPadding);

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;


            string strHexString = HexEncoding.ToString(Encoding.Default.GetBytes(sbFinalBlock.ToString()));
            int len = strHexString.Length - (strHexString.Length % 16);
            List<string> inputList = split(strHexString, len);

            for (int i = 0; i < inputList.Count; i++)
            {
                objResult = encrypt(inputList[i].PadRight(len, 'F'));

                if (!objResult.IsSuccess)
                    break;
                else
                    encryptedData += objResult.Value;
            }

            return encryptedData;
        }

        private string getRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        ///// <author>Nidhi Thakrar</author>
        ///// <created>11-Apr-17</created>
        ///// <summary>Decrypts the specified encrypted data.</summary>
        ///// <param name="EncryptedData">The encrypted data.</param>
        ///// <returns></returns>
        //public string Decrypt(string EncryptedData)
        //{
        //    string decryptedData = string.Empty;

        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    int len = EncryptedData.Length - (EncryptedData.Length % 16);
        //    List<string> inputList = split(EncryptedData, len);

        //    for (int i = 0; i < inputList.Count; i++)
        //    {
        //        objResult = decrypt(inputList[i].PadRight(len, 'F'));

        //        if (!objResult.IsSuccess)
        //            break;
        //        else
        //            decryptedData += objResult.Value;
        //    }

        //    return HexEncoding.ConvertHexToAscii(decryptedData).TrimEnd('?');
        //}

        /// <author>Nidhi Thakrar</author>
        /// <created>17-Apr-17</created>
        /// <summary>Decrypts the specified hexadecimal data.</summary>
        /// <param name="hexData">The hexadecimal data.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <returns></returns>
        private HSMResult decrypt(string hexData)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string commandParam = "[AOCBCD;GGBD{0};GHBD{1};BO{2};BF1;]";
            string cmd = string.Format(commandParam,
                                VIS_VTS_WSD, //{0} Key Slot for Visa VTS, CBC encypt decrypt
                                VIS_VTS_WSD_IV,  //{1} IV
                                hexData); //{2} Hexdata to be decrypted

            string response = executeExcrypt(cmd);


            string functionID = "";

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            string message = string.Empty;
            string endingIV = string.Empty;

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BP":
                        message = data;
                        break;
                    case "GH":
                        endingIV = data;
                        break;
                }
            }

            if (functionID == "ERRO")
            {
                objResult.IsSuccess = false;
                objResult.ErrorMessage = message;
            }
            else if (functionID == "CBCD")
            {
                objResult.IsSuccess = true;
                objResult.Value = message;
            }

            return objResult;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>17-Apr-17</created>
        /// <summary>Encrypts the specified hexadecimal data.</summary>
        /// <param name="hexData">The hexadecimal data.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <returns></returns>
        private HSMResult encrypt(string hexData)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string commandParam = "[AOCBCE;GGBD{0};GHBD{1};BO{2};BF1;]";
            string cmd = string.Format(commandParam,
                                VIS_VTS_WSD, //{0} Key Slot for Visa VTS, CBC encypt decrypt
                                VIS_VTS_WSD_IV,  //{1} IV
                                hexData); //{2} Hexdata to be encrypted

            string response = executeExcrypt(cmd);


            string functionID = "";

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            string message = string.Empty;
            string endingIV = string.Empty;

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BP":
                        message = data;
                        break;
                    case "GH":
                        endingIV = data;
                        break;
                }
            }

            if (functionID == "ERRO")
            {
                objResult.IsSuccess = false;
                objResult.ErrorMessage = message;
            }
            else if (functionID == "CBCE")
            {
                objResult.IsSuccess = true;
                objResult.Value = message;
            }

            return objResult;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>17-Apr-17</created>
        /// <summary>Splits the specified string.</summary>
        /// <param name="str">The string.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <returns></returns>
        private List<string> split(string str, int chunkSize)
        {
            List<string> listArray = new List<string>();
            int remaining = 0;

            IEnumerable<string> splitArray = Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));

            if (splitArray != null)
            {
                listArray = splitArray.ToList();
                remaining = listArray.Count;
            }

            if (remaining * chunkSize < str.Length)
                listArray.Add(str.Substring(remaining * chunkSize));

            return listArray;
        }

        ///// <author>Rikunj Suthar</author>
        ///// <created>16-10-2020</created>
        ///// <summary>
        ///// Translate key.
        ///// </summary>
        ///// <param name="keyToTranslate">key to translate</param>
        ///// <returns></returns>
        //public HSMResult TranslateKey(string keyToTranslate)
        //{
        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    string functionID = "";

        //    string cmd = $"[AOTWKA;AS1;APBD{FPS_ZMK_SLOT_NO};BG{keyToTranslate};]";

        //    string response = executeExcrypt(cmd);

        //    response = response.Remove(0, 1);
        //    response = response.Remove(response.Length - 1, 1);
        //    string[] resultArray = response.Split(';');

        //    int _arrayLength = resultArray.Length;

        //    for (int i = 0; i < _arrayLength - 1; i++)
        //    {
        //        string str = resultArray[i].Substring(0, 2);
        //        string data = resultArray[i].Substring(2);

        //        switch (str)
        //        {
        //            case "AO":
        //                functionID = data;
        //                break;
        //            case "BH":
        //                objResult.EncryptedValue = data;
        //                break;
        //            case "AE":
        //                objResult.CheckDigit = data;
        //                break;
        //        }
        //    }

        //    if (functionID == "TWKA" && objResult.EncryptedValue.Length > 0 && objResult.CheckDigit.Length > 0)
        //    {
        //        objResult.IsSuccess = true;
        //        objResult.RandomKey = keyToTranslate;
        //    }   
        //    else
        //    {
        //        objResult.EncryptedValue = string.Empty;
        //        objResult.CheckDigit = string.Empty;
        //    }

        //    return objResult;
        //}


        /// <summary>
        /// Translate Working Key without a Modifier
        /// </summary>
        /// <param name="keyToTranslate"></param>
        /// <returns></returns>
        /// <created>Sapan Paibandha,11-10-2021</created>
        public HSMResult TranslateKeyWithoutModifier(string keyToTranslate, bool isOut = false)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            int zmk = isOut ? FPS_ZMK_SLOT_OUT_2 : FPS_ZMK_SLOT_IN_2;

            string cmd = $"[AOTWKM;AS3;APBD{zmk};BH{keyToTranslate};OFC;]";

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            int _arrayLength = resultArray.Length;

            for (int i = 0; i < _arrayLength - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);

                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BG":
                        objResult.EncryptedValue = data;
                        break;
                    case "AE":
                        objResult.CheckDigit = data;
                        break;
                }
            }

            if (functionID == "TWKM" && objResult.EncryptedValue.Length > 0 && objResult.CheckDigit.Length > 0)
            {
                objResult.IsSuccess = true;
                objResult.RandomKey = keyToTranslate;
            }
            else
            {
                objResult.EncryptedValue = string.Empty;
                objResult.CheckDigit = string.Empty;
            }

            return objResult;
        }


        ///// <author>Rikunj Suthar</author>
        ///// <created>08-Dec-2020</created>
        ///// <summary>
        ///// Translate key for storage.
        ///// </summary>
        ///// <param name="keyToTranslate"></param>
        ///// <returns></returns>
        //public HSMResult TranslateKeyForStorage(string keyToTranslate)
        //{
        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    string functionID = "";

        //    string cmd = $"[AOTWKS;AS1;APBD{FPS_ZMK_SLOT_NO};BH{keyToTranslate};]";

        //    string response = executeExcrypt(cmd);

        //    response = response.Remove(0, 1);
        //    response = response.Remove(response.Length - 1, 1);
        //    string[] resultArray = response.Split(';');

        //    int _arrayLength = resultArray.Length;

        //    for (int i = 0; i < _arrayLength - 1; i++)
        //    {
        //        string str = resultArray[i].Substring(0, 2);
        //        string data = resultArray[i].Substring(2);

        //        switch (str)
        //        {
        //            case "AO":
        //                functionID = data;
        //                break;
        //            case "BG":
        //                objResult.EncryptedValue = data;
        //                break;
        //            case "AE":
        //                objResult.CheckDigit = data;
        //                break;
        //        }
        //    }

        //    if (functionID == "TWKS" && objResult.EncryptedValue.Length > 0 && objResult.CheckDigit.Length > 0)
        //    {
        //        objResult.IsSuccess = true;
        //    }
        //    else
        //    {
        //        objResult.EncryptedValue = string.Empty;
        //        objResult.CheckDigit = string.Empty;
        //    }

        //    return objResult;
        //}

        /// <author>Rikunj Suthar</author>
        /// <created>02-Nov-2020</created>
        /// <summary>
        /// Get next zmk for verification.
        /// </summary>
        /// <returns>Result of HSM</returns>
        public HSMResult GetNextZMK()
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            string cmd = $"[AOTWKA;AS0;APBD{FPS_ZMK_SLOT_IN};BGBD{FPS_NEXT_ZMK_SLOT_NO};]";

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            int _arrayLength = resultArray.Length;

            for (int i = 0; i < _arrayLength - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);

                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BH":
                        objResult.EncryptedValue = data;
                        break;
                    case "AE":
                        objResult.CheckDigit = data;
                        break;
                }
            }

            if (functionID == "TWKA" && objResult.EncryptedValue.Length > 0 && objResult.CheckDigit.Length > 0)
            {
                objResult.IsSuccess = true;
            }
            else
            {
                objResult.EncryptedValue = string.Empty;
                objResult.CheckDigit = string.Empty;
            }

            return objResult;
        }
    }
}
