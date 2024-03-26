using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CredECard.CardProduction.BusinessService;
using CredECard.Common.Enums.Authorization;
using CredEncryption.BusinessService;

namespace CredECard.HSMSecurity.BusinessService
{
    /// <summary>
    /// This class contain all the function required to generate Pin/CVV1/CVV2/iCVV etc.
    /// </summary>
    public class HSMGeneration : HSMCommon
    {
        public HSMGeneration() { }
        public HSMGeneration(short index) : base(index) { }
        public HSMGeneration(string cardNumber, short index) : base(cardNumber, index) { }
        public HSMGeneration(string cardNumber, short pvki, short zoneKeyIndex) : base(cardNumber, pvki, zoneKeyIndex) { }

        /// <summary>
        /// Generate PIN Number for Card. This method will generate PIN under Contis-Pin Encryption Key,
        /// which we are using to store the PIN BLOCK in database.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public HSMResult GeneratePin(string cardNumber)
        {
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            string BY_pvki = Index.ToString();

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string bzCardNumber = cardNumber.Substring(4, 11);

            string commandParam = "[AOGPIN;BF{0};BTBD{1};CABD{2};CBBD{3};BY{4};BZ{5};AK{6};CM{7};]";
            string cmd = string.Format(commandParam,
                BF_pinGenerationMethod,         //{0} Verification Method (VISA)
                CONTIS_PEK_SLOT_NO,             //{1} PEK encrypted under modifier 1 of the MFK
                PVKA_SLOT_NO,                   //{2} Cryptogram of left VISA Key Pair encrypted under modifier 4 of the MFK
                PVKB_SLOT_NO,                   //{3} Cryptogram of right VISA Key Pair encrypted under modifier 4 of the MFK
                BY_pvki,                        //{4} PIN Verification Key Index (PVKI)
                bzCardNumber,                   //{5} Partial PAN, 11 rightmost digits excluding check digit 
                AK_additionalPinBlock,          //{6} Additional PIN block data
                CM_pinLength);                  //{7} Pin Length

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
                    case "AL":
                        objResult.PinBlock = data;
                        break;
                    case "BX":
                        objResult.PVV = data;
                        break;
                }
            }

            if (functionID == "GPIN" && objResult.PinBlock.Length > 0 && objResult.PVV.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.PinBlock = string.Empty;
                objResult.PVV = string.Empty;
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

        public HSMResult GenerateCvcAndCvc2(string cardNumber, DateTime expiryDate, int serviceCode)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string avCardNumber = cardNumber;
            string faExpDate = expiryDate.ToString("yyMM");

            string commandParam = "[AOGCVC;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FT;]";
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

            if (functionID == "GCVC" && objResult.CVV.Length > 0 && objResult.CVV2.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.CVV = string.Empty;
                objResult.CVV2 = string.Empty;
            }


            return objResult;
        }

        public HSMResult GetClearPIN(string ansiKeyBlock, string cardNumber)
        {
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            string functionID = "";

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string commandParam = "[AOCPIN;AW{0};AL{1};AXBD{2};AK{3};]";
            string cmd = string.Format(commandParam,
                AW_pinBlockFormat,          //{0}
                ansiKeyBlock,               //{1}
                CONTIS_PEK_SLOT_NO,         //{2}
                AK_additionalPinBlock);     //{3}

            string response = executeExcrypt(cmd);


            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split(';');
            string bb = string.Empty;

            for (int i = 0; i < resultArray.Length - 1; i++)
            {
                string str = resultArray[i].Substring(0, 2);
                string data = resultArray[i].Substring(2);
                switch (str)
                {
                    case "AO": //Function ID
                        functionID = data;
                        break;
                    case "AF": //Clear PIN value
                        objResult.ClearPIN = data;
                        break;
                    case "BB": //Error indicator returned on bad PIN block S=PIN block format error (Sanity)
                        bb = data;
                        break;
                }
            }

            if (functionID == "CPIN" && objResult.ClearPIN.Length > 0)
                objResult.IsSuccess = true;
            else if (bb == "S")
            {
                objResult.ClearPIN = string.Empty;
                objResult.ErrorMessage = "PIN block format error (Sanity)";
            }

            return objResult;
        }

        public HSMResult GeneratePVVOffset(string cardNumber, string encPinBlock)
        {
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            string BY_pvki = Index.ToString();

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string bzCardNumber = cardNumber.Substring(4, 11);

            string commandParam = "[AOGOFF;BF{0};AL{1};AXBD{2};CABD{3};CBBD{4};BY{5};BZ{6};AK{7};]";
            string cmd = string.Format(commandParam,
                BF_pinGenerationMethod,         //{0}
                encPinBlock,                    //{1}
                BT_PEK_SLOT_NO,                 //{2}
                PVKA_SLOT_NO,                   //{3}
                PVKB_SLOT_NO,                   //{4}
                BY_pvki,                        //{5}
                bzCardNumber,                   //{6}
                AK_additionalPinBlock);         //{7}


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
                        if (data.ToUpper() == "W")
                        {
                            objResult.IsUnSafePin = true;
                            objResult.ErrorMessage = "Weak Pin";
                        }
                        else
                        {
                            objResult.ErrorMessage = "Sanity error";
                        }
                        break;
                    case "BX":
                        objResult.PVV = data;
                        break;
                }
            }


            if (functionID == "GOFF" && objResult.PVV.Length > 0)
                objResult.IsSuccess = true;
            else
                objResult.PVV = string.Empty;

            return objResult;
        }

        public HSMResult GenerateContisPVVOffset(string cardNumber, string encPinBlock)
        {
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            string BY_pvki = Index.ToString();

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string bzCardNumber = cardNumber.Substring(4, 11);

            string commandParam = "[AOGOFF;BF{0};AL{1};AXBD{2};CABD{3};CBBD{4};BY{5};BZ{6};AK{7};]";
            string cmd = string.Format(commandParam,
                BF_pinGenerationMethod,         //{0}
                encPinBlock,                    //{1}
                CONTIS_PEK_SLOT_NO,             //{2}
                PVKA_SLOT_NO,                   //{3}
                PVKB_SLOT_NO,                   //{4}
                BY_pvki,                        //{5}
                bzCardNumber,                   //{6}
                AK_additionalPinBlock);         //{7}


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
                        if (data.ToUpper() == "W")
                        {
                            objResult.IsUnSafePin = true;
                            objResult.ErrorMessage = "Weak Pin";
                        }
                        else
                        {
                            objResult.ErrorMessage = "Sanity error";
                        }
                        break;
                    case "BX":
                        objResult.PVV = data;
                        break;
                }
            }


            if (functionID == "GOFF" && objResult.PVV.Length > 0)
                objResult.IsSuccess = true;
            else
                objResult.PVV = string.Empty;

            return objResult;
        }

        //internal HSMResult EncryptData(string clearData)
        //{
        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    string functionID = "";

        //    string commandParam = "[AOECID;AI{0};AK{1};]";
        //    string cmd = string.Format(commandParam,
        //        AK_ZPK_CRYPTOGRAM,
        //        clearData
        //        );

        //    string response = executeExcrypt(cmd);

        //    response = response.Remove(0, 1);
        //    response = response.Remove(response.Length - 1, 1);
        //    string[] resultArray = response.split(';');

        //    for (int i = 0; i < resultArray.Length - 1; i++)
        //    {
        //        string str = resultArray[i].Substring(0, 2);
        //        string data = resultArray[i].Substring(2);

        //        switch (str)
        //        {
        //            case "AO":
        //                functionID = data;
        //                break;
        //            case "AK":
        //                objResult.EncryptedValue = data;
        //                break;
        //        }
        //    }

        //    if (functionID == "ECID" && objResult.EncryptedValue.Length > 0)
        //        objResult.IsSuccess = true;
        //    else
        //    {
        //        objResult.EncryptedValue = string.Empty;
        //        objResult.Request = cmd;
        //        objResult.Response = response;
        //    }

        //    return objResult;
        //}

        //internal HSMResult DecryptData(string encData)
        //{
        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    string functionID = "";
        //    string commandParam = "[AODCID;AI{0};AK{1};]";
        //    string cmd = string.Format(commandParam,
        //        AK_ZPK_CRYPTOGRAM,
        //        encData
        //        );

        //    string response = executeExcrypt(cmd);

        //    response = response.Remove(0, 1);
        //    response = response.Remove(response.Length - 1, 1);
        //    string[] resultArray = response.split(';');

        //    for (int i = 0; i < resultArray.Length - 1; i++)
        //    {
        //        string str = resultArray[i].Substring(0, 2);
        //        string data = resultArray[i].Substring(2);

        //        switch (str)
        //        {
        //            case "AO":
        //                functionID = data;
        //                break;
        //            case "AK":
        //                objResult.ClearValue = data;
        //                break;
        //        }
        //    }

        //    if (functionID == "DCID" && objResult.ClearValue.Length > 0)
        //        objResult.IsSuccess = true;
        //    else
        //    {
        //        objResult.EncryptedValue = string.Empty;
        //    }

        //    return objResult;
        //}

        public HSMResult GetEncryptedPinBlock(string cardNumber, string clearPin)
        {

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string avCardNumber = cardNumber.Substring(3, 12);

            string commandParam = "[AOEPIN;AXBD{0};AF{1};AV{2};]";
            string cmd = string.Format(commandParam,
                BT_PEK_SLOT_NO,                 //{0}PIN Encryption Key encrypted under modifier 1 of the MFK
                clearPin,                       //{1}Clear PIN to be encrypted
                avCardNumber);                  //{2}Rightmost 12 PAN digits minus the check digit

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
                    case "AL":
                        objResult.PinBlock = data;
                        break;
                }
            }

            if (functionID == "EPIN" && objResult.PinBlock.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.PinBlock = string.Empty;
                objResult.PVV = string.Empty;
            }


            return objResult;
        }

        public HSMResult GetContisEncryptedPinBlock(string cardNumber, string clearPin)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string avCardNumber = cardNumber.Substring(3, 12);

            string commandParam = "[AOEPIN;AXBD{0};AF{1};AV{2};]";
            string cmd = string.Format(commandParam,
                CONTIS_PEK_SLOT_NO,                 //{0}PIN Encryption Key encrypted under modifier 1 of the MFK
                clearPin,                       //{1}Clear PIN to be encrypted
                avCardNumber);                  //{2}Rightmost 12 PAN digits minus the check digit

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
                    case "AL":
                        objResult.PinBlock = data;
                        break;
                }
            }

            if (functionID == "EPIN" && objResult.PinBlock.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.PinBlock = string.Empty;
                objResult.PVV = string.Empty;
            }


            return objResult;
        }

        public HSMResult TranslatePinBlockForContisFromIWK(string cardNumber, string encData)
        {
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string commandParam = "[AOTPIN;AW{0};AXBD{1};BTBD{2};AL{3};AK{4};]";
            string cmd = string.Format(commandParam,
                AW_pinBlockFormat,      //{0}
                BT_PEK_SLOT_NO,     //{1}
                CONTIS_PEK_SLOT_NO,         //{2}
                encData,                //{3}
                AK_additionalPinBlock   //{4}
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
                    case "AL":
                        objResult.PinBlock = data;
                        break;
                    case "BB":
                        objResult.IsSuccess = true;
                        break;
                }
            }

            if (functionID == "TPIN" && objResult.PinBlock.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.EncryptedValue = string.Empty;
            }

            return objResult;
        }

        public HSMResult TranslatePinBlockForPersonalizer(string cardNumber, string encData)
        {
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string commandParam = "[AOTPIN;AW{0};AXBD{1};BTBD{2};AL{3};AK{4};]";
            string cmd = string.Format(commandParam,
                AW_pinBlockFormat,      //{0}
                CONTIS_PEK_SLOT_NO,     //{1}
                AK_ZPK_SLOT_NO,         //{2}
                encData,                //{3}
                AK_additionalPinBlock   //{4}
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
                    case "AL":
                        objResult.PinBlock = data;
                        break;
                    case "BB":
                        objResult.IsSuccess = true;
                        break;
                }
            }

            if (functionID == "TPIN" && objResult.PinBlock.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.EncryptedValue = string.Empty;
            }

            return objResult;
        }

        public HSMResult GenerateMAC(string key, string messageData)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string cmd = $"[AOGMAC;BN0;AR{key};BO{messageData};BF1;AD8;CZ1;]";

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
                    case "BP":
                        objResult.MAC = data;
                        break;
                    case "AE":
                        objResult.CheckDigit = data;
                        break;
                }
            }

            if ((functionID == "GMAC" || functionID == "IMAC") && objResult.MAC.Length > 0)
                objResult.IsSuccess = true;

            return objResult;
        }

        public HSMResult GetEMVM(string cardNumber, string sequenceNumber, string atc, string dataCommand, int macLength, EnumKeyDerivationMethod enumKeyDerivationMethod)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;
            int km = (int)enumKeyDerivationMethod; // Niken Shah Case 128255

            string functionID = "";
            string commandParam = "[AOEMVM;FS0;KM{0};KOBD{1};KS{2};BO{3};BJ{4};KQ{5};KR{6};]";
            string cmd = string.Format(commandParam,
                km.ToString(),          //{0} - Derivation method
                MDK_MAC_SLOT_NO,        //{1} - ICC Master MAC Key (encrypted under modifier 13 of the MFK)
                atc,                    //{2} - ATC
                dataCommand,            //{3} - Datablock
                macLength,              //{4} - Mac Value Length
                cardNumber,             //{5} - PAN
                sequenceNumber          //{6} - Pan Sequence Number
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
                    case "KV":
                        objResult.MAC = data;
                        break;
                }
            }

            if (functionID == "EMVM" && objResult.MAC.Length > 0)
                objResult.IsSuccess = true;

            return objResult;
        }

        public HSMResult GenerateEmvMac(string cardNumber, string sequenceNumber, string atc, string dataCommand, EnumDerivationType enumDerivationType)
        {
            string AK_additionalPinBlock = cardNumber.Substring(3, 12);
            string BY_pvki = cardNumber.Substring(cardNumber.Length - 1, 1);
            int dt = (int)EnumDerivationType.EMV_VISA;  //(int)enumDerivationType; // Niken Shah Case 128255

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string bzCardNumber = cardNumber.Substring(4, 11);
            string commandParam = "<352#{0}#T{1}#{2}#{3}#{4}#1##{5}#>";

            string cmd = string.Format(commandParam,
                dt.ToString(),      //{0} EMV Derivation Type (1=Visa, 0=MC/Euro)
                MDK_MAC_SLOT_NO,    //{1} MDKmac - cryptogram stored on 
                cardNumber,         //{2} PAN
                sequenceNumber,     //{3} Sequence Number
                atc,                //{4} ATC
                dataCommand         //{5} 
                );

            string response = executeExcrypt(cmd, ">");

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split('#');

            functionID = resultArray[0];
            int length = 0;

            int.TryParse(resultArray[1], out length);
            string data = resultArray[2];

            if (functionID == "452" && length > 0)
            {
                objResult.IsSuccess = true;
                string[] dataarr = data.Split(' ');
                if (dataarr.Length > 0)
                    objResult.MAC = dataarr[0] + dataarr[1];
                else
                    objResult.MAC = data;
            }
            else
                objResult.IsSuccess = false;

            return objResult;

        }

        public HSMResult GenerateEmvMacNew(string cardNumber, string sequenceNumber, string atc, string dataCommand, EnumDerivationType enumDerivationType)
        {

            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;
            int dt = (int)EnumDerivationType.EMV_VISA; ///(int)enumDerivationType; // Niken Shah Case 128255

            string functionID = "";
            string commandParam = "<352#{0}#T{1}#{2}#{3}#{4}#3##{5}#>";

            string cmd = string.Format(commandParam,
                dt.ToString(),      //{0} EMV Derivation Type (1=Visa, 0=MC/Euro)
                MDK_MAC_SLOT_NO,    //{1} MDKmac - cryptogram stored on 
                cardNumber,         //{2} PAN
                sequenceNumber,     //{3} Sequence Number
                atc,                //{4} ATC
                dataCommand         //{5} 
                );

            string response = executeExcrypt(cmd, ">");

            response = response.Remove(0, 1);
            response = response.Remove(response.Length - 1, 1);
            string[] resultArray = response.Split('#');

            functionID = resultArray[0];
            int length = 0;

            int.TryParse(resultArray[1], out length);
            string data = resultArray[2];

            if (functionID == "452" && length > 0)
            {
                objResult.IsSuccess = true;
                string[] dataarr = data.Split(' ');
                if (dataarr.Length > 0)
                    objResult.MAC = dataarr[0] + dataarr[1] + dataarr[2] + dataarr[3]; //Change by prashant to make 8 digit MAC i.e. 16 digit Hex
                else
                    objResult.MAC = data;
            }
            else
                objResult.IsSuccess = false;

            return objResult;

        }

        //internal HSMResult TranslatePinBlockUnderMDKEnc(string cardNumber, string encData)
        //{
        //    string AK_additionalPinBlock = cardNumber.Substring(3, 12);
        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    string functionID = "";
        //    string commandParam = "[AOTPIN;AW{0};AXBD{1};BT{2};AL{3};AK{4};]";
        //    string cmd = string.Format(commandParam,
        //        AW_pinBlockFormat,      //{0}
        //        BT_PEK_SLOT_NO,         //{1}
        //        MDK_ENC_CRYPTOGRAM,  //{2}
        //        encData,                //{3}
        //        AK_additionalPinBlock   //{4}
        //        );

        //    string response = executeExcrypt(cmd);

        //    response = response.Remove(0, 1);
        //    response = response.Remove(response.Length - 1, 1);
        //    string[] resultArray = response.split(';');

        //    for (int i = 0; i < resultArray.Length - 1; i++)
        //    {
        //        string str = resultArray[i].Substring(0, 2);
        //        string data = resultArray[i].Substring(2);

        //        switch (str)
        //        {
        //            case "AO":
        //                functionID = data;
        //                break;
        //            case "AL":
        //                objResult.PinBlock = data;
        //                break;
        //            case "BB":
        //                objResult.IsSuccess = true;
        //                break;
        //        }
        //    }

        //    if (functionID == "TPIN" && objResult.PinBlock.Length > 0)
        //        objResult.IsSuccess = true;
        //    else
        //    {
        //        objResult.EncryptedValue = string.Empty;
        //    }

        //    return objResult;
        //}

        //internal HSMResult GenerateEMVPin(string cardNumber,string currentPinblock, string newPinblock)
        //{
        //    string AK_additionalPinBlock = cardNumber.Substring(3, 12);


        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    string functionID = "";
        //    string bzCardNumber = cardNumber.Substring(4, 11);

        //    string commandParam = "[AOEMVP;FS{0};KM{1};AW{2};AL{3};FW{4};AXBD{5};FXBD{6};AV{7};]";
        //    string cmd = string.Format(commandParam,
        //        1,                          //{0} Perform Pin change with current pin
        //        DERIVATION_METHOD,          //{1} Derivation Method 0 = EMV 2000,1 = EMV CSK
        //        AW_pinBlockFormat,          //{2} Pinblock Format
        //        currentPinblock,            //{3} Current Pinblock (encrypted under the incoming PEK)
        //        newPinblock,                //{4} New pinblock (encrypted under the outgoing PEK)
        //        BT_PEK_SLOT_NO,             //{5} Incoming PIN Encryption Key (encrypted under modifier 1 of the MFK)
        //        BT_PEK_SLOT_NO,             //{6} Outgoing PIN Encryption Key (encrypted under modifier 1 of the MFK)
        //        cardNumber,                 //{7} Validation data for PIN Block, usually a Personal Account Number (PAN)

        //    string response = executeExcrypt(cmd);

        //    response = response.Remove(0, 1);
        //    response = response.Remove(response.Length - 1, 1);
        //    string[] resultArray = response.split(';');

        //    for (int i = 0; i < resultArray.Length - 1; i++)
        //    {
        //        string str = resultArray[i].Substring(0, 2);
        //        string data = resultArray[i].Substring(2);
        //        switch (str)
        //        {
        //            case "AO":
        //                functionID = data;
        //                break;
        //            case "AL":
        //                objResult.PinBlock = data;
        //                break;
        //            case "BX":
        //                objResult.PVV = data;
        //                break;
        //        }
        //    }

        //    if (functionID == "GPIN" && objResult.PinBlock.Length > 0 && objResult.PVV.Length > 0)
        //        objResult.IsSuccess = true;
        //    else
        //    {
        //        objResult.PinBlock = string.Empty;
        //        objResult.PVV = string.Empty;
        //    }


        //    return objResult;
        //}

        //internal HSMResult GetICCMasterKey(string cardNumber, string sequenceNo)
        //{
        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    string functionID = "";

        //    string commandParam = "[AOEMVG;APBD{0};AQ{1};AS0;KQ{2};KR{3};]";
        //    string cmd = string.Format(commandParam,
        //        AK_ZPK_SLOT_NO,         //{0}Key Encryption Key (encrypted under modifier 0 of the MFK)
        //        "8FC60F7830DC271ED493B47F0E0E4125",         //{1}Issuer Master Key (encrypted under modifier 9 of the MFK)
        //        cardNumber,             //{2}Primary Account Number
        //        sequenceNo);            //{3}PAN Sequence Number

        //    string response = executeExcrypt(cmd);

        //    response = response.Remove(0, 1);
        //    response = response.Remove(response.Length - 1, 1);
        //    string[] resultArray = response.split(';');

        //    for (int i = 0; i < resultArray.Length - 1; i++)
        //    {
        //        string str = resultArray[i].Substring(0, 2);
        //        string data = resultArray[i].Substring(2);
        //        switch (str)
        //        {
        //            case "AO":
        //                functionID = data;
        //                break;
        //            case "BG":
        //                objResult.ICCMasterKey = data;
        //                break;
        //            case "XB":
        //                objResult.KCV = data;
        //                break;
        //        }
        //    }

        //    if (functionID == "EMVG" && objResult.PinBlock.Length > 0)
        //        objResult.IsSuccess = true;
        //    else
        //    {
        //        objResult.ICCMasterKey = string.Empty;
        //        objResult.KCV = string.Empty;
        //    }

        //    return objResult;
        //}

        //internal HSMResult VerifyEMVPinChange(string cardNumber, string currentPinblock, string newPinBlock,string sequenceNo,string ATC,string paddedData)
        //{
        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;
        //    string avCardNumber = cardNumber.Substring(3, 12);
        //    string functionID = "";
        //    string CD = "00000000000000000000000000000000";
        //    string commandParam = "[AOEMVP;FS0;KM0;AW{0};AL{1};FW{2};AXBD{3};FXBD{4};AV{5};KNBD{6};KD{7};KQ{8};KR{9};KS{10};AK{11};CC1;CD{12};]";
        //    string cmd = string.Format(commandParam,
        //        AW_pinBlockFormat,      //{0} Pinblock Format
        //        currentPinblock,        //{1} Current Pinblock
        //        newPinBlock,            //{2} New pinblock
        //        BT_PEK_SLOT_NO,         //{3} Incoming Pin Encryption Key
        //        BT_PEK_SLOT_NO,         //{4} Outgoing Pin Encryption Key
        //        avCardNumber,           //{5} Validation data for PIN block (usually PAN)
        //        MDK_ENC_SLOT_NO,        //{6} ENC Issuer Master Key (encrypted under modifier 14 of the MFK)
        //        KMAC_CRYPTOGRAM,        //{7} MAC Issuer Master Key (encrypted under modifier 13 of the MFK)
        //        cardNumber,             //{8} Primary Account Number
        //        sequenceNo,             //{9} PAN Sequence Number
        //        ATC,                    //{10}Application Transaction Counter (Derivation Method 0) or Application Cryptogram (Derivation Method 1)
        //        paddedData,             //{11}ARQC / previous MAC + padded command message + TLV-encoding data + proprietary data (the encrypted PIN block is appended to this to form the script message that will have its MAC computed)
        //        CD                      //{12}Derivation IV (only for derivation method 0)
        //        );

        //    string response = executeExcrypt(cmd);

        //    response = response.Remove(0, 1);
        //    response = response.Remove(response.Length - 1, 1);
        //    string[] resultArray = response.split(';');


        //    for (int i = 0; i < resultArray.Length - 1; i++)
        //    {
        //        string str = resultArray[i].Substring(0, 2);
        //        string data = resultArray[i].Substring(2);
        //        switch (str)
        //        {
        //            case "AO":
        //                functionID = data;
        //                break;
        //            case "BB":
        //                if(data.ToUpper() == "Y")
        //                    objResult.IsSuccess = true;
        //                break;
        //            case "KV":
        //                objResult.MAC= data;
        //                break;
        //            case "KU":
        //                objResult.PinBlock = data;
        //                break;
        //        }
        //    }

        //    if (functionID == "EMVP" && objResult.IsSuccess && objResult.MAC.Length > 0 && objResult.PinBlock.Length > 0)
        //        objResult.IsSuccess = true;
        //    else
        //    {
        //        objResult.MAC = string.Empty;
        //        objResult.PinBlock = string.Empty;
        //    }

        //    return objResult;
        //}

        public HSMResult GenerateCAVV(string cardNumber, string merchantName, long tranSeqID)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;
            string merchantSH1Hash = GetSha1HashData(merchantName).Substring(0, 16);
            string functionID = "";
            string tranSeq = tranSeqID.ToString("X2").PadLeft(8, '0');

            string commandParam = "[AOCAAV;KC{0};KD{1};KE{2};KF{3};KG{4};KH{5};KI{6};KJBD{7};]";

            string cmd = string.Format(commandParam,
                cardNumber,             //{0} Card Number
                KD_controlByte,         //{1} AAV Control Byte
                merchantSH1Hash,        //{2} SHA-1 Hash of the Merchant name.
                KF_acsIdentifier,       //{3} Access Control Server Identifier.
                KG_authenticationMethod,//{4} Authentication Method 0 = No Cardholder Authentication,1 = Password,2 = Secret Key
                Index.ToString(),       //{5} Key Identifier.
                tranSeq,                //{6} Transaction Sequence Number.
                CAK_SLOT_NO             //{7} AAV Hash Key
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
                    case "KL":
                        objResult.CAVV = data;
                        break;
                }
            }

            if (functionID == "CAAV" && objResult.CAVV.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.CAVV = string.Empty;
            }

            return objResult;
        }

        public HSMResult GenerateCavvCVV(string cardNumber, string unPredictableNumber, string serviceCode)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string avCardNumber = cardNumber;

            string commandParam = "[AOGCVV;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FT;]";
            string cmd = string.Format(commandParam,
                cardNumber,     //{0} Card Number
                CAKA_SLOT_NO,   //{1} Card Authentication Key Part 1
                CAKB_SLOT_NO,   //{2} Card Authentication Key Part 2
                unPredictableNumber,            //{3} Un PredictableNumber
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

        public HSMResult GenerateCavcCVc(string cardNumber, string unPredictableNumber, string serviceCode)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string avCardNumber = cardNumber;

            string commandParam = "[AOGCVC;AV{0};CABD{1};CBBD{2};FA{3};FB{4};FT;]";
            string cmd = string.Format(commandParam,
                cardNumber,     //{0} Card Number
                CAKA_SLOT_NO,   //{1} Card Authentication Key Part 1
                CAKB_SLOT_NO,   //{2} Card Authentication Key Part 2
                unPredictableNumber,            //{3} Un PredictableNumber
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

            if (functionID == "GCVC" && objResult.CVV.Length > 0 && objResult.CVV2.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.CVV = string.Empty;
                objResult.CVV2 = string.Empty;
            }


            return objResult;
        }

        public HSMResult GenerateDynamicCvv(string cardNumber, int panSeqNum, int unPredNum, int atc, string trackData)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";
            string avCardNumber = cardNumber;
            string panSeqNumber = panSeqNum.ToString().PadLeft(2, '0');

            //string commandParam = "[AOGDCV;CABD{0};BZ{1};KR{2};FY{3};KS{4};KT{5};FS{6};BJ{7};CM{8};]";
            string commandParam = "[AOGDCV;CABD{0};BZ{1};KR{2};FY{3};KS{4};KT{5};CM3;]";
            string cmd = string.Format(commandParam,
                MDK_SLOT_NO,           //{0} Card Number
                cardNumber,             //{1} Card Verificaion Key Part 1
                panSeqNumber,           //{2} Pan Seq. Number
                unPredNum.ToString(),   //{3} Unpredictable number
                atc.ToString(),         //{4} ATC
                trackData               //{5} Track 2 data
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
                        objResult.DCVV = data;
                        break;
                    case "AE": //Check Digit of Derived CVC Key (default is 4 digit check digit)
                        //objResult.CVV2 = data;
                        break;
                    case "KX":
                        //Check Digit of Issuer Master CVC Key (default is 4 digit check digit) 
                        break;
                }
            }

            if (functionID == "GDCV" && objResult.CVV.Length > 0 && objResult.CVV2.Length > 0)
                objResult.IsSuccess = true;
            else
            {
                objResult.DCVV = string.Empty;

            }

            return objResult;
        }

        //public HSMResult GetAVV(string hexData) // Niken Shah Case 128255 MasterCard
        //{
        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;


        //    string commandParam = "[AOHMAC;RG3;ARBD{0};BO{1};BN0;]";
        //    string cmd = string.Format(commandParam,
        //            AAV_SLOT_NO,
        //            hexData    //{0}
        //            );          //{5}

        //    string response = executeExcrypt(cmd);

        //    response = response.Remove(0, 1);
        //    response = response.Remove(response.Length - 1, 1);
        //    string[] resultArray = response.Split(';');

        //    for (int i = 0; i < resultArray.Length - 1; i++)
        //    {
        //        string str = resultArray[i].Substring(0, 2);
        //        string data = resultArray[i].Substring(2);
        //        switch (str)
        //        {
        //            case "CF":

        //                if (data != string.Empty)
        //                {
        //                    objResult.AAV = data.Substring(0, 7);
        //                    objResult.IsSuccess = true;
        //                }
        //                break;
        //        }
        //    }

        //    return objResult;
        //}

        ///// <author>Rikunj Suthar</author>
        ///// <created>02-Dec-2020</created>
        ///// <summary>
        ///// Generate issuer working key.
        ///// </summary>
        ///// <param name="keyType"></param>
        ///// <returns></returns>
        //public HSMResult GenerateIssuerWorkingKey(EnumGenerateKeyType keyType)
        //{
        //    HSMResult objResult = new HSMResult();
        //    objResult.IsSuccess = false;

        //    string functionID = "";

        //    string cmd = $"[AOGWKS;AS1;APBD{FPS_ZMK_SLOT_NO};FS{(int)keyType};]";

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
        //                objResult.RandomKey = data;
        //                break;
        //            case "BH":
        //                objResult.EncryptedValue = data;
        //                break;
        //            case "AE":
        //                objResult.CheckDigit = data;
        //                break;
        //        }
        //    }

        //    if (functionID == "GWKS" && objResult.RandomKey.Length > 0)
        //        objResult.IsSuccess = true;
        //    else
        //        objResult.RandomKey = objResult.EncryptedValue = objResult.CheckDigit = string.Empty;

        //    return objResult;
        //}

        public HSMResult GenerateIssuerWorkingKeyForFPS(EnumGenerateKeyType keyType)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            string cmd = $"[AOGWKS;AS0;APBD{FPS_ZMK_SLOT_IN};FS{(int)keyType};]";

            string response = executeExcrypt(cmd);

            Console.WriteLine($"Req: {cmd}, Res: {response}");

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
                        objResult.RandomKey = data;
                        break;
                    case "BH":
                        objResult.EncryptedValue = data;
                        break;
                    case "AE":
                        objResult.CheckDigit = data;
                        break;
                }
            }

            if (functionID == "GWKS" && objResult.RandomKey.Length > 0)
                objResult.IsSuccess = true;
            else
                objResult.RandomKey = objResult.EncryptedValue = objResult.CheckDigit = string.Empty;

            return objResult;
        }

        /// <author>Rikunj Suthar/author>
        /// <created>31-Aug-2020</created>
        /// <summary>
        /// Get new working key.
        /// </summary>
        /// <param name="keyType"></param>
        /// <returns></returns>
        public HSMResult GenerateRandomKey(EnumGenerateKeyType keyType)
        {
            HSMResult objResult = new HSMResult();
            objResult.IsSuccess = false;

            string functionID = "";

            string cmd = $"[AORKEY;FS{(int)keyType};]";

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
                    case "AK":
                        objResult.RandomKey = data;
                        break;
                }
            }

            if (functionID == "RKEY" && objResult.RandomKey.Length > 0)
                objResult.IsSuccess = true;
            else
                objResult.RandomKey = string.Empty;


            return objResult;
        }
    }
}
