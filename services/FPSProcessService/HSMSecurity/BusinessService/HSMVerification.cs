using CredECard.CardProduction.BusinessService;
//using CredECard.CardProduction.BusinessService;
using CredECard.Common.Enums.Authorization;
using CredEncryption.BusinessService;
using System;

namespace CredECard.HSMSecurity.BusinessService
{
    public class HSMVerification : HSMCommon
    {
        public HSMVerification() { }
        public HSMVerification(string cardNumber, short index) : base(cardNumber, index) { }
        public HSMVerification(string cardNumber, short pvki, short zoneKeyIndex) : base(cardNumber, pvki, zoneKeyIndex) { }

        public HSMResult VerifyPin(string cardNumber, string encryptedPinBlock, string pvv)
        {
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            string functionID = "";
            string BY_pvki = Index.ToString();  

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string bzCardNumber = cardNumber.Substring(4, 11);
            string pvr = string.Empty;//Pin Verification result

            string commandParam = "[AOVPIN;BF{0};AW{1};AL{2};AXBD{3};CABD{4};CBBD{5};BX{6};BY{7};BZ{8};AK{9};]";

            string cmd = string.Format(commandParam,
                BF_pinGenerationMethod,         //{0} Verification Method (VISA)
                AW_pinBlockFormat,              //{1} Input PIN block format: 1 = ANSI, 2 = IBM 3624, 3 = PIN Pad
                encryptedPinBlock,              //{2} Encrypted PIN block
                BT_PEK_SLOT_NO,                 //{3} PEK encrypted under modifier 1 of the MFK - IWK
                PVKA_SLOT_NO,                   //{4} Cryptogram of left VISA Key Pair encrypted under modifier 4 of the MFK
                PVKB_SLOT_NO,                   //{5} Cryptogram of right VISA Key Pair encrypted under modifier 4 of the MFK
                pvv,                            //{6} PIN Verification Value (PVV)
                BY_pvki,                        //{7} PIN Verification Key Index (PVKI)
                bzCardNumber,                   //{8} Partial PAN, 11 rightmost digits excluding check digit
                AK_additionalPinBlock);         //{9} Additional PIN block data


            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        pvr = data;
                        break;
                }
            }

            if (functionID == "VPIN" && pvr.Length > 0)
            {
                switch (pvr.ToUpper())
                {
                    case "Y":// = verified
                        objResult.IsSuccess = true;
                        break;
                    case "N":// = not verified
                        objResult.ErrorMessage = "Pin not verified";
                        break;
                    case "S":// = 
                        objResult.ErrorMessage = "Format error (sanity)";
                        break;
                    case "L":// = 
                        objResult.ErrorMessage = "Short PIN entered";
                        break;
                }
            }
            else if (functionID == "EERO")
            {
                objResult.ErrorMessage = pvr;
            }


            return objResult;
        }

        /// <author>Rikunj Suthar</author>
        /// <created>01-Sep-2020</created>
        /// <summary>
        /// Method to verify pin with key.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="encryptedPinBlock"></param>
        /// <param name="pvv"></param>
        /// <param name="workingKey"></param>
        /// <returns></returns>
        public HSMResult VerifyPinDynamicKey(string cardNumber, string encryptedPinBlock, string pvv, string workingKey)
        {
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            string functionID = "";
            string BY_pvki = Index.ToString();

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string bzCardNumber = cardNumber.Substring(4, 11);
            string pvr = string.Empty; //Pin Verification result

            string commandParam = "[AOVPIN;BF{0};AW{1};AL{2};AX{3};CABD{4};CBBD{5};BX{6};BY{7};BZ{8};AK{9};]";
                        
            string cmd = string.Format(commandParam,
                BF_pinGenerationMethod,         //{0} Verification Method (VISA)
                AW_pinBlockFormat,              //{1} Input PIN block format: 1 = ANSI, 2 = IBM 3624, 3 = PIN Pad
                encryptedPinBlock,              //{2} Encrypted PIN block
                workingKey,                     //{3} PEK encrypted under modifier 1 of the MFK - IWK
                PVKA_SLOT_NO,                   //{4} Cryptogram of left VISA Key Pair encrypted under modifier 4 of the MFK
                PVKB_SLOT_NO,                   //{5} Cryptogram of right VISA Key Pair encrypted under modifier 4 of the MFK
                pvv,                            //{6} PIN Verification Value (PVV)
                BY_pvki,                        //{7} PIN Verification Key Index (PVKI)
                bzCardNumber,                   //{8} Partial PAN, 11 rightmost digits excluding check digit
                AK_additionalPinBlock);         //{9} Additional PIN block data


            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            int _arrayLength = resultArray.Length; // RS#128208 review changes.

            for (int i = 0; i < _arrayLength - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        pvr = data;
                        break;
                }
            }

            if (functionID == "VPIN" && pvr.Length > 0)
            {
                switch (pvr.ToUpper())
                {
                    case "Y":// = verified
                        objResult.IsSuccess = true;
                        objResult.HSMResultCode = EnumHSMResultCode.Success;
                        break;
                    case "N":// = not verified
                        objResult.ErrorMessage = "Pin not verified";
                        objResult.HSMResultCode = EnumHSMResultCode.InvalidPIN;
                        break;
                    case "S":// = 
                        objResult.ErrorMessage = "Format error (sanity)";
                        objResult.HSMResultCode = EnumHSMResultCode.PINFormatError;
                        break;
                    case "L":// = 
                        objResult.ErrorMessage = "Short PIN entered";
                        objResult.HSMResultCode = EnumHSMResultCode.ShortPIN;
                        break;
                }
            }
            else if (functionID == "EERO")
            {
                objResult.ErrorMessage = pvr;
            }


            return objResult;
        }

        public HSMResult VerifyCVV(string cardNumber, DateTime expiryDate, int serviceCode, string cvv)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            string faExpDate = expiryDate.ToString("yyMM");

            string commandParam = "[AOVCVV;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FC{5};]"; //For CVV only
            string cmd = string.Format(commandParam,
                    cardNumber,     //{0}
                    CVKA_SLOT_NO,   //{1}
                    CVKB_SLOT_NO,   //{2}
                    faExpDate,      //{3}
                    serviceCode,    //{4}
                    cvv);           //{5}

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                }
            }

            return objResult;
        }

        public HSMResult VerifyCVC(string cardNumber, DateTime expiryDate, int serviceCode, string cvv) // Niken Shah Case 128255 MasterCard
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            string faExpDate = expiryDate.ToString("yyMM");

            string commandParam = "[AOVCVC;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FC{5};]"; //For CVV only
            string cmd = string.Format(commandParam,
                    cardNumber,     //{0}
                    CVKA_SLOT_NO,   //{1}
                    CVKB_SLOT_NO,   //{2}
                    faExpDate,      //{3}
                    serviceCode,    //{4}
                    cvv);           //{5}

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                }
            }

            return objResult;
        }

        public HSMResult VerifyCVV2(string cardNumber, DateTime expiryDate, int serviceCode, string cvv2)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            string faExpDate = expiryDate.ToString("yyMM");

            string commandParam = "[AOVCVV;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FT{5};]"; // For CVV2 only
            string cmd = string.Format(commandParam,
                    cardNumber,     //{0}
                    CVKA_SLOT_NO,   //{1}
                    CVKB_SLOT_NO,   //{2}
                    faExpDate,      //{3}
                    serviceCode,    //{4}
                    cvv2);          //{5}

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                }
            }

            return objResult;
        }

        public HSMResult VerifyCVC2(string cardNumber, DateTime expiryDate, int serviceCode, string cvv2) // Niken Shah Case 128255 MasterCard
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            string faExpDate = expiryDate.ToString("yyMM");

            string commandParam = "[AOVCVC;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FT{5};]"; // For CVV2 only
            string cmd = string.Format(commandParam,
                    cardNumber,     //{0}
                    CVKA_SLOT_NO,   //{1}
                    CVKB_SLOT_NO,   //{2}
                    faExpDate,      //{3}
                    serviceCode,    //{4}
                    cvv2);          //{5}

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                }
            }

            return objResult;
        }

        public HSMResult GenerateCvvAndCvv2(string cardNumber, DateTime expiryDate, int serviceCode)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string avCardNumber = cardNumber;
            string faExpDate = expiryDate.ToString("yyMM");

            string commandParam = "[AOGCVV;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FT;]";
            string cmd = string.Format(commandParam,
                cardNumber,     //{0} Card Number
                CVKA_SLOT_NO,   //{1} Card Verificaion Key Part 1
                CVKB_SLOT_NO,   //{2} Card Verification Key Part 2
                faExpDate,      //{3} Expiry Date
                serviceCode     //{4} Service Code
                );

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "FC":
                        objResult.CVV = data;
                        break;
                    case "FT":
                        objResult.CVV2 = data;
                        break;
                }
            }

            if (functionID == "GCVV" && objResult.CVV.Length > 0 && objResult.CVV2.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.CVV = string.Empty;
                objResult.CVV2 = string.Empty;
            }


            return objResult;
        }

        public HSMResult VerifyICVV(string cardNumber, DateTime expiryDate, int serviceCode, string icvv)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            string faExpDate = expiryDate.ToString("yyMM");

            string commandParam = "[AOVCVV;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FC{5};]"; //For iCVV only
            string cmd = string.Format(commandParam,
                    cardNumber,     //{0}
                    CVKA_SLOT_NO,   //{1}
                    CVKB_SLOT_NO,   //{2}
                    faExpDate,     //{3}
                    serviceCode,   //{4}
                    icvv);          //{5}

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                }
            }

            return objResult;
        }

        public HSMResult VerifyICVC(string cardNumber, DateTime expiryDate, int serviceCode, string icvv) // Niken Shah Case 128255 MasterCard
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            string faExpDate = expiryDate.ToString("yyMM");

            string commandParam = "[AOVCVC;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FC{5};]"; //For iCVV only
            string cmd = string.Format(commandParam,
                    cardNumber,     //{0}
                    CVKA_SLOT_NO,   //{1}
                    CVKB_SLOT_NO,   //{2}
                    faExpDate,     //{3}
                    serviceCode,   //{4}
                    icvv);          //{5}

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                }
            }

            return objResult;
        }

        public HSMResult VerifyARQC(string cardNumber, string sequenceNumber, string atc, string dataBlock, string ARQC, string responseCode)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            sequenceNumber = (sequenceNumber != string.Empty) ? sequenceNumber : "00";

            string functionID = "";
            string commandParam = "<350#1#T{0}#{1}#{2}##{3}#{4}#{5}#>"; //For ARQC verification.
            string cmd = string.Format(commandParam,
                    MDK_SLOT_NO,          //{0}
                    cardNumber,         //{1}
                    sequenceNumber,     //{2}
                    ARQC,               //{3}
                    dataBlock,          //{4}
                    responseCode);      //{5}

            string response = executeExcrypt(cmd, ">");

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split('#');

            functionID = resultArray[0];
            string data = resultArray[1];

            if (functionID == "450" && data.Length > 0)
            {
                objResult.IsSuccess = true;
                objResult.ARPC = data;
            }
            else
                objResult.IsSuccess = false;

            return objResult;
        }

        /// <author>Keyur Parekh</author>
        /// <created>05-Mar-2019</created>
        /// <summary>
        /// Verify ARQC
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="sequenceNumber"></param>
        /// <param name="atc"></param>
        /// <param name="data"></param>
        /// <param name="ARQC"></param>
        /// <param name="responseCode"></param>
        /// <returns>True/False</returns>
        public HSMResult VerifyEMVAARQC(string cardNumber, string sequenceNumber, string atc, string dataBlock, string ARQC, string responseCode, EnumKeyDerivationMethod keyDerivationMethod, string CSU = "00860000") //Prashant Soni:7-Apr-20 : updated function from ARQC verify only to ARPC return with method 2
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;
            sequenceNumber = (sequenceNumber != string.Empty) ? sequenceNumber : "00";
            int km = (int)keyDerivationMethod;

            string functionID = "";

            // string commandParam = "[AOEMVA;FS0;KM1;KPBD{0};KQ{1};KR{2};KS{3};KT{4};BO{5};BJ{6};]"; //Command verifies ARQC only
            //string commandParam = "[AOEMVA;FS2;KM1;KPBD{0};KQ{1};KR{2};KS{3};KT{4};BO{5};CA{6};]";
            string commandParam = "[AOEMVA;FS2;KM{0};KPBD{1};KQ{2};KR{3};KS{4};KT{5};BO{6};CA{7};]";
            string cmd = string.Format(commandParam,
                     km.ToString(),
                     MDK_SLOT_NO,       //{0} - IMK-AC encrypted under modifier 9 of the MFK)
                     cardNumber,        //{1} - PAN
                     sequenceNumber,    //{2} - Pan Seq. Number
                     atc,               //{3} - ATC
                     dataBlock,         //{4} - padded datablock
                     ARQC,              //{5} - ARQC
                                        //responseCode,       //{6} - ARC Authorization Response Code -- not require for method 2 //Prashant Soni:1-4-20 : commented as ARC not require for method 2 ARPC
                     CSU                //{7} - CSU data //Prashant Soni:1-4-20 : new field added
                    );


            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                    case "BG": //Prashant soni : 1-Apr-20 : Added ARPC data to return in code
                        objResult.ARPC = data;
                        break;
                }
            }

            return objResult;
        }

        /// <author>Keyur Parekh</author>
        /// <created>05-Mar-2019</created>
        /// <summary>
        /// Verify ARQC
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="sequenceNumber"></param>
        /// <param name="atc"></param>
        /// <param name="data"></param>
        /// <param name="ARQC"></param>
        /// <param name="responseCode"></param>
        /// <returns>True/False</returns>
        public HSMResult VerifyEMVAARQC_MC(string cardNumber, string sequenceNumber, string atc, string dataBlock, string ARQC, string responseCode, EnumKeyDerivationMethod keyDerivationMethod,string UniqueNumber , string CSU = "00860000") //Prashant Soni:7-Apr-20 : updated function from ARQC verify only to ARPC return with method 2
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;
            sequenceNumber = (sequenceNumber != string.Empty) ? sequenceNumber : "00";
            int km = (int)keyDerivationMethod;

            string functionID = "";

            string commandParam = "[AOEMVA;FS1;KM{0};KPBD{1};KQ{2};KR{3};KS{4};KT{5};BO{6};BJ{7};KU{8};]";
            string cmd = string.Format(commandParam,
                     km.ToString(),
                     MDK_SLOT_NO,       //{0} - IMK-AC encrypted under modifier 9 of the MFK)
                     cardNumber,        //{1} - PAN
                     sequenceNumber,    //{2} - Pan Seq. Number
                     atc,               //{3} - ATC
                     dataBlock,         //{4} - padded datablock
                     ARQC,              //{5} - ARQC
                     responseCode,       //{6} - ARC Authorization Response Code -- not require for method 2 //Prashant Soni:1-4-20 : commented as ARC not require for method 2 ARPC
                     //CSU,                //{7} - CSU data //Prashant Soni:1-4-20 : new field added
                     UniqueNumber      
                    );


            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                    case "BG": //Prashant soni : 1-Apr-20 : Added ARPC data to return in code
                        objResult.ARPC = data;
                        break;
                }
            }

            return objResult;
        }
        public HSMResult ChangeEMVPin(string cardNumber, string sequenceNumber, string newEncPinBlock_IWK, string atc, string dataBlock, EnumDerivationType enumDerivationType)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            int dt = (int)EnumDerivationType.EMV_VISA;//(int)enumDerivationType; // Niken Shah Case 128255

            sequenceNumber = (sequenceNumber != string.Empty) ? sequenceNumber : "00";

            string functionID = "";
            string commandParam = "<351#{0}#1##T{1}#T{2}#T{3}#T{4}#{5}##{6}#{7}#{8}#{9}#{10}##>";


            string cmd = string.Format(commandParam,
                dt.ToString(),          //{0} EMV Derivation Type (1=Visa, 0=MC/Euro)
                BT_PEK_SLOT_NO,         //{1} PIN Encryption Key (PEK) encrypted under modifier 1 of the MFK
                MDK_ENC_SLOT_NO,        //{2} EMV ENC Master Key encrypted under modifier 14 of the MFK
                MDK_MAC_SLOT_NO,        //{3} EMV Message Authentication Master Key encrypted under modifier 13 of the MFK
                MDK_SLOT_NO,            //{4} MDK encrypted under modifier 9 of the MFK
                newEncPinBlock_IWK,     //{5} Pinblock encrypted under IWK
                cardNumber,             //{6} Card Number
                sequenceNumber,         //{7} Sequence Number
                atc,                    //{8} ATC
                dataBlock,              //{9} Padded Data Block
                AK_additionalPinBlock); //{10} Additioanl Pin Block Data


            string response = executeExcrypt(cmd, ">");

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split('#');

            functionID = resultArray[0];
            string sanityCheck = resultArray[1];
            string encPinBlock = resultArray[2];
            string mac = resultArray[3];

            if (functionID == "451" && sanityCheck.ToUpper() == "Y")
            {
                objResult.IsSuccess = true;
                objResult.MAC = mac;
                objResult.PinBlock = encPinBlock;
            }
            else
                objResult.IsSuccess = false;

            return objResult;
        }


        public HSMResult VerifyCAVV(string cardNumber, string merchantName, long tranSeqID, string cavv)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string merchantSH1Hash = GetSha1HashData(merchantName);
            string tranSeq = tranSeqID.ToString("X2").PadLeft(8, '0');

            string commandParam = "[AOVAAV;KC{0};KD{1};KE{2};KF{3};KG{4};KH{5};KI{6};KJBD{7};KK{8};]";
            string cmd = string.Format(commandParam,
                cardNumber,             //{0} Card Number
                KD_controlByte,         //{1} AAV Control Byte
                merchantSH1Hash,        //{2} SHA-1 Hash of the Merchant name.
                KF_acsIdentifier,       //{3} Access Control Server Identifier.
                KG_authenticationMethod,//{4} Authentication Method 0 = No Cardholder Authentication,1 = Password,2 = Secret Key
                Index.ToString(),       //{5} Key Identifier.
                tranSeq,                //{6} Transaction Sequence Number.
                CAK_SLOT_NO,            //{7} AAV Hash Key
                cavv                    //{8} The already calculated AAV, Base64 encoded.  
                );

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                }
            }

            return objResult;
        }

        public HSMResult VerifyCavvCvv2(string cardNumber, string unPredictableNumber, string serviceCode, string cavvCvv2)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string faExpDate = unPredictableNumber;

            string commandParam = "[AOVCVV;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FC{5};]"; //For iCVV only
            string cmd = string.Format(commandParam,
                    cardNumber,     //{0}
                    CAKA_SLOT_NO,   //{1}
                    CAKB_SLOT_NO,   //{2}
                    faExpDate,     //{3}
                    serviceCode,   //{4}
                    cavvCvv2);          //{5}

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                }
            }

            return objResult;
        }

        public HSMResult VerifyAAV(string cardNumber, string unPredictableNumber, string serviceCode, string cavvCvv2) // Niken Shah Case 128255 MasterCard
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string faExpDate = unPredictableNumber;

            string commandParam = "[AOVCVC;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FC{5};]"; //For iCVV only
            string cmd = string.Format(commandParam,
                    cardNumber,     //{0}
                    CAKA_SLOT_NO,   //{1}
                    CAKB_SLOT_NO,   //{2}
                    faExpDate,     //{3}
                    serviceCode,   //{4}
                    cavvCvv2);          //{5}

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                }
            }

            return objResult;
        }
              

        public HSMResult VerifyDCVV(string cardNumber, string panSeqNum, string unPredNum, string atc, string trackData, string dcvv)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";


            string commandParam = "[AOVDCV;CABD{0};BZ{1};KR{2};FY{3};KS{4};KT{5};FC{6}]";
            string cmd = string.Format(commandParam,
                    MDK_SLOT_NO,    //{0} Cryptogram of Issuer Master CVC Key encrypted under modifier 4 of the MFK
                    cardNumber,     //{1}
                    panSeqNum,   //{2}
                    unPredNum,   //{3}
                    atc,         //{4}
                    trackData,              //{5}
                    dcvv);                  //{6}

            string response = executeExcrypt(cmd);

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO":
                        functionID = data;
                        break;
                    case "BB":
                        if (data.ToUpper() == "Y")
                            objResult.IsSuccess = true;
                        else
                        {
                            objResult.IsSuccess = false;
                            objResult.ErrorMessage = "Not verified";
                        }
                        break;
                    case "AE": //Check Digit of Derived CVC Key (default is 4 digit check digit)
                        //objResult.CVV2 = data;
                        break;
                    case "KX":
                        //Check Digit of Issuer Master CVC Key (default is 4 digit check digit) 
                        break;
                }
            }

            return objResult;
        }

    }
}
