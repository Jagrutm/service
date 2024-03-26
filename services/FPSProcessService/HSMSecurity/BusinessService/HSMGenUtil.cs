using System;
using System.Text;
using CredECard.BugReporting.BusinessService;
using CredECard.CardProduction.BusinessService;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;
using CredECard.HSMSecurity.BusinessService;
using CredEncryption.BusinessService;

namespace ContisGroup.CardUtil.BusinessService
{
    public class HSMGenUtil
    {
        ///// <summary>
        ///// Generate New Pin Block and new PVV offset for Card
        ///// </summary>
        ///// <param name="cardNumber">Card Number</param>
        ///// <param name="pinBlock">PinBlock</param>
        ///// <param name="pvv">PVV Offset</param>
        //public static HSMResult GetNewPinData(string cardNumber,short pvki)
        //{
        //    //string encPan = CardData.GetEncryptedPAN(cardNumber);

        //    HSMGeneration objHelper = new HSMGeneration(cardNumber,pvki);
        //    HSMResult result = objHelper.GeneratePin(cardNumber);

        //    return result;
        //}

        ///// <summary>
        ///// Generate New Pin Block and new PVV offset for Card
        ///// </summary>
        ///// <param name="cardNumber">Card Number</param>
        ///// <param name="pinBlock">PinBlock</param>
        ///// <param name="pvv">PVV Offset</param>
        //public static HSMResult GetPrevCardPinData(string currentCardNumber, short pvki, string prevCardNumber, string prevCardInfo)
        //{
        //    HSMResult result = new HSMResult();
        //    result.IsSuccess = false;

        //    //Get clear PIN of previous card
        //    string prevPIN = string.Empty;
        //    string pinBlock =string.Empty;

        //    HSMResult resultPIN = HSMGenUtil.GetPIN(prevCardNumber, prevCardInfo);

        //    if (resultPIN.IsSuccess)
        //    {
        //        prevPIN = resultPIN.ClearPIN;

        //        //Get PIN Offset for prev PIN

        //        HSMGeneration objHSMGen = new HSMGeneration(currentCardNumber, pvki, 1); // zoneKeyIndex = 1 as we will have only one Zone PIN key with production house
        //        HSMResult resultNewPINBlock = objHSMGen.GetContisEncryptedPinBlock(currentCardNumber, prevPIN);
        //        if (resultNewPINBlock.IsSuccess)
        //        {
        //            result.PinBlock = resultNewPINBlock.PinBlock;

        //            HSMResult resultNewPVV = objHSMGen.GenerateContisPVVOffset(currentCardNumber, result.PinBlock);

        //            if (resultNewPVV.IsSuccess)
        //            {
        //                result.PVV = resultNewPVV.PVV;
        //                result.IsSuccess = true;
        //            }
        //            else
        //                HSMGenUtil.RaiseBug("Unable to get PVV of previous card", resultPIN);
        //        }
        //        else
        //            HSMGenUtil.RaiseBug("Unable to get Pin Block for new card", resultPIN);
        //     }
        //     else
        //        HSMGenUtil.RaiseBug("Unable to get PIN of previous card", resultPIN);


        //    return result;
        //}

        ///// <summary>
        ///// Get Clear - Pin Reminder
        ///// </summary>
        ///// <param name="cardNumber"></param>
        ///// <returns></returns>
        //public static HSMResult GetPIN(string cardNumber)
        //{
        //    string clearPIN = string.Empty;
        //    string strCardInfo = CardData.GetCardInfo(cardNumber);
        //    HSMResult resultClearPin = new HSMResult();

        //    if (strCardInfo.Trim().Length > 0)
        //    {
        //        HSMGeneration objHelper = new HSMGeneration(cardNumber,1);
        //        resultClearPin = objHelper.GetClearPIN(strCardInfo, cardNumber);
        //    }

        //    return resultClearPin;
        //}

        //private static HSMResult GetPIN(string cardNumber,string cardInfo)
        //{
        //    string clearPIN = string.Empty;
        //    HSMResult resultClearPin = new HSMResult();

        //    if (cardInfo.Trim().Length > 0)
        //    {
        //        HSMGeneration objHelper = new HSMGeneration(cardNumber, 1);
        //        resultClearPin = objHelper.GetClearPIN(cardInfo, cardNumber);
        //    }

        //    return resultClearPin;
        //}

        ///// <summary>
        ///// Get CVV1 and CVV2
        ///// </summary>
        ///// <param name="cardNumber"></param>
        ///// <param name="expDate"></param>
        ///// <param name="serviceCode"></param>
        ///// <returns></returns>
        //public static HSMResult GetCvv1Cvv2(string cardNumber, DateTime expDate, int serviceCode)
        //{
        //    HSMGeneration objHSM = new HSMGeneration(cardNumber,1);
        //    HSMResult objHSMResult = objHSM.GenerateCvvAndCvv2(cardNumber, expDate, serviceCode);

        //    return objHSMResult;
        //}

        ///// <summary>
        ///// Get CVC1 and CVC2
        ///// </summary>
        ///// <param name="cardNumber"></param>
        ///// <param name="expDate"></param>
        ///// <param name="serviceCode"></param>
        ///// <returns></returns>
        //public static HSMResult GetCvc1Cvc2(string cardNumber, DateTime expDate, int serviceCode)
        //{
        //    HSMGeneration objHSM = new HSMGeneration(cardNumber, 1);
        //    HSMResult objHSMResult = objHSM.GenerateCvcAndCvc2(cardNumber, expDate, serviceCode);

        //    return objHSMResult;
        //}

        ///// <summary>
        ///// Get ICVV
        ///// </summary>
        ///// <param name="cardNumber"></param>
        ///// <param name="expDate"></param>
        ///// <param name="serviceCode"></param>
        ///// <returns></returns>
        //public static HSMResult GetICvv(string cardNumber, DateTime expDate, int serviceCode)
        //{
        //    HSMGeneration objHSM = new HSMGeneration(cardNumber,1);
        //    HSMResult objHSMResult = objHSM.GenerateCvvAndCvv2(cardNumber, expDate, serviceCode);

        //    if(objHSMResult.IsSuccess)objHSMResult.ICVV = objHSMResult.CVV;

        //    return objHSMResult;
        //}

        ///// <summary>
        ///// Get ICVC 
        ///// </summary>
        ///// <param name="cardNumber"></param>
        ///// <param name="expDate"></param>
        ///// <param name="serviceCode"></param>
        ///// <returns></returns>
        //public static HSMResult GetICvc(string cardNumber, DateTime expDate, int serviceCode)
        //{
        //    HSMGeneration objHSM = new HSMGeneration(cardNumber, 1);
        //    HSMResult objHSMResult = objHSM.GenerateCvcAndCvc2(cardNumber, expDate, serviceCode);

        //    if (objHSMResult.IsSuccess) objHSMResult.ICVV = objHSMResult.CVV;

        //    return objHSMResult;
        //}

        public static string GenerateEmvMac(string cardNumber, string sequenceNumber, string atc, string dataCommand, EnumDerivationType enumDerivationType)
        {
            HSMGeneration objHSM = new HSMGeneration(cardNumber, 1);
            HSMResult objResult = objHSM.GenerateEmvMac(cardNumber, sequenceNumber, atc, dataCommand, enumDerivationType);

            if (objResult.IsSuccess)
                return objResult.MAC;
            else
            {
                return string.Empty;
            }
        }

        public static string GenerateEmvMacNew(string cardNumber, string sequenceNumber, string atc, string dataCommand, EnumDerivationType enumDerivationType)
        {
            HSMGeneration objHSM = new HSMGeneration(cardNumber, 1);
            HSMResult objResult = objHSM.GenerateEmvMacNew(cardNumber, sequenceNumber, atc, dataCommand, enumDerivationType);

            if (objResult.IsSuccess)
                return objResult.MAC;
            else
            {
                return string.Empty;
            }
        }

        public static string GetEMVM(string cardNumber, string sequenceNumber, string atc, string dataCommand, int macLength, EnumKeyDerivationMethod enumKeyDerivationMethod)
        {
            HSMGeneration objHSM = new HSMGeneration(cardNumber, 1);
            HSMResult objResult = objHSM.GetEMVM(cardNumber, sequenceNumber, atc, dataCommand, macLength, enumKeyDerivationMethod);

            if (objResult.IsSuccess)
                return objResult.MAC;
            else
            {
                return string.Empty;
            }
        }

        //public static HSMResult DecryptData(string cardNumber, string encData)
        //{
        //    HSMGeneration objHSM = new HSMGeneration(cardNumber, 1);
        //    HSMResult objResult = objHSM.DecryptData(encData);

        //    return objResult;
        //}

        public static void RaiseBug(string functionName, HSMResult result)
        {
            StringBuilder sb = new StringBuilder(functionName);

            sb.Append("Date Time");
            sb.Append(DateTime.Now.ToString("dd-MMM-yy HH:mm"));
            sb.Append("Request : \t");
            sb.Append(result.Request);
            sb.Append("\n\r");
            sb.Append("Response : \t");
            sb.Append(result.Response);
            sb.Append("\n\r");

            PersistException pEx = new PersistException(sb.ToString());
            PostToBugscout.PostDataToBugScout(pEx);

            throw pEx;
        }

        public static string GenerateMAC(string key, string messageData)
        {
            HSMGeneration objHSM = new HSMGeneration();
            
            HSMResult objResult = objHSM.GenerateMAC(key, messageData);

            if (objResult.IsSuccess)
                return objResult.MAC;
            else
                return string.Empty;
        }


        public static string GetEncryptedPinBlock(string cardNumber, string clearPIN, short pvki)
        {
            HSMGeneration objHelper = new HSMGeneration(cardNumber,pvki);
            HSMResult result = objHelper.GetEncryptedPinBlock(cardNumber, clearPIN);

            string pinBlock = string.Empty;

            if (result.IsSuccess)
                pinBlock = result.PinBlock;

            return pinBlock;
        }

        public static HSMResult TranslatePinBlockForPersonalizer(string cardNumber, string encData)
        {
            HSMGeneration objHSM = new HSMGeneration(cardNumber, 1);
            HSMResult objResult = objHSM.TranslatePinBlockForPersonalizer(cardNumber, encData);

            return objResult;
        }

        public static HSMResult TranslatePinBlockForContisFromIWK(string cardNumber, string encData,short zoneKeyIndex)
        {
            HSMGeneration objHSM = new HSMGeneration(cardNumber, 1,zoneKeyIndex);
            HSMResult objResult = objHSM.TranslatePinBlockForContisFromIWK(cardNumber, encData);

            return objResult;
        }

        //public static string GetCAVV(string cardNumber, string merchantName, long tranSeq )
        //{
        //    string cavv = string.Empty;
        //    try
        //    {
        //        HSMGeneration objHSM = new HSMGeneration(cardNumber, 1);
        //        HSMResult objResult = objHSM.GenerateCAVV(cardNumber, merchantName, tranSeq);

        //        if (objResult.IsSuccess)
        //        {
        //            cavv = objResult.CAVV;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PostToBugscout.PostDataToBugScout(ex);
        //    }

        //    return cavv;
        //}

        ////public static HSMResult TranslatePinBlockUnderMDKEnc(string cardNumber, string encData, short zoneKeyIndex)
        ////{
        ////    HSMGeneration objHSM = new HSMGeneration(cardNumber, 1, zoneKeyIndex);
        ////    HSMResult objResult = objHSM.TranslatePinBlockUnderMDKEnc(cardNumber, encData);

        ////    return objResult;
        ////}


        //public static HSMResult GetCavvCvv2(string cardNumber, string unPredictableNumber, string serviceCode, short index)
        //{
        //    HSMGeneration objHSM = new HSMGeneration(cardNumber, index);
        //    HSMResult objHSMResult = objHSM.GenerateCavvCVV(cardNumber, unPredictableNumber, serviceCode);

        //    if (objHSMResult.IsSuccess) objHSMResult.CavvCVV2 = objHSMResult.CVV;

        //    return objHSMResult;
        //}

        //public static HSMResult GetCavcCvc2(string cardNumber, string unPredictableNumber, string serviceCode, short index)
        //{
        //    HSMGeneration objHSM = new HSMGeneration(cardNumber, index);
        //    HSMResult objHSMResult = objHSM.GenerateCavcCVc(cardNumber, unPredictableNumber, serviceCode);

        //    if (objHSMResult.IsSuccess) objHSMResult.CavvCVV2 = objHSMResult.CVV;

        //    return objHSMResult;
        //}

        ///// <author>Keyur Parekh</author>
        ///// <created>02-Apr-2014</created>
        ///// <summary>
        ///// Generate Dynamic CVV
        ///// </summary>
        ///// <param name="cardNumber">String Pan</param>
        ///// <param name="panSeqNum">Int Pan sequence number</param>
        ///// <param name="unPredNum">Int un predictable number</param>
        ///// <param name="atc">int atc</param>
        ///// <param name="trackData">string track data</param>
        ///// <param name="dcvv">string dcvv</param>
        ///// <returns>HSMResult</returns>
        //public static HSMResult GetDCVV(string cardNumber, int panSeqNum, int unPredNum,int atc , string trackData)
        //{
        //    HSMGeneration objHSM = new HSMGeneration(cardNumber, 1);
        //    HSMResult objHSMResult = objHSM.GenerateDynamicCvv(cardNumber,panSeqNum,unPredNum,atc,trackData);

        //    //if (objHSMResult.IsSuccess) objHSMResult.DCVV = objHSMResult.DCVV;

        //    return objHSMResult;
        //}

        /// <author>Rikunj Suthar</author>
        /// <created>31-Aug-2020</created>
        /// <summary>
        /// Generate new working key based on key lenght parameter.
        /// </summary>
        /// <param name="keyType"></param>
        /// <returns>result of hsm</returns>
        public static string GetRandomKey(EnumGenerateKeyType keyType)
        {
            var hsmGen = new HSMGeneration(1);
            var objHSMResult = hsmGen.GenerateRandomKey(keyType);

            if (objHSMResult != null && objHSMResult.IsSuccess)
                return objHSMResult?.RandomKey;

            return string.Empty;
        }

        ///// <author>Rikunj Suthar</author>
        ///// <created>16-Oct-2020</created>
        ///// <summary>
        ///// Translate key
        ///// </summary>
        ///// <param name="kekSlotNo">kek slot number</param>
        ///// <param name="keyToTranslate">key to translate</param>
        ///// <returns>result of hsm</returns>
        //public static HSMResult TranslateKey(string keyToTranslate)
        //{
        //    if (string.IsNullOrWhiteSpace(keyToTranslate)) return new HSMResult() { IsSuccess = false };

        //    var hsmEncDec = new HSMEncDec();
        //    var objHSMResult = hsmEncDec.TranslateKey(keyToTranslate);
        //    return objHSMResult;
        //}

        public static HSMResult TranslateKeyWithoutModifier(string keyToTranslate, bool isOut = false)
        {
            if (string.IsNullOrWhiteSpace(keyToTranslate)) return new HSMResult() { IsSuccess = false };

            var hsmEncDec = new HSMEncDec();
            var objHSMResult = hsmEncDec.TranslateKeyWithoutModifier(keyToTranslate, isOut);
            return objHSMResult;
        }


        ///// <author>Rikunj Suthar</author>
        ///// <created>07-Dec-2020</created>
        ///// <summary>
        ///// Translate key for storage.
        ///// </summary>
        ///// <param name="keyToTranslate"></param>
        ///// <returns></returns>
        //public static HSMResult TranslateKeyForStorage(string keyToTranslate)
        //{
        //    if (string.IsNullOrWhiteSpace(keyToTranslate)) return new HSMResult() { IsSuccess = false };

        //    var hsmEncDec = new HSMEncDec();
        //    var objHSMResult = hsmEncDec.TranslateKeyForStorage(keyToTranslate);
        //    return objHSMResult;
        //}

        /// <author>Rikunj Suthar</author>
        /// <created>02-Nov-2020</created>
        /// <summary>
        /// Export key
        /// </summary>
        /// <param name="keyToExport">key to export</param>
        /// <returns>result of hsm</returns>
        public static HSMResult GetNextZMKForVerification()
        {
            var hsmEncDec = new HSMEncDec();
            var objHSMResult = hsmEncDec.GetNextZMK();
            if (objHSMResult == null && !objHSMResult.IsSuccess) return null;
            return objHSMResult;
        }

        ///// <author>Rikunj Suthar</author>
        ///// <created>02-Dec-2020</created>
        ///// <summary>
        ///// Generate new issuer working key.
        ///// </summary>
        ///// <param name="keyType">FS key type</param>
        ///// <returns></returns>
        //public static HSMResult GetNewIssuerWorkingKey(EnumGenerateKeyType keyType)
        //{
        //    var hsmGen = new HSMGeneration(1);
        //    var objHSMResult = hsmGen.GenerateIssuerWorkingKey(keyType);
        //    return objHSMResult;
        //}

        public static HSMResult GenerateIssuerWorkingKeyForFPS(EnumGenerateKeyType keyType)
        {
            var hsmGen = new HSMGeneration(1);
            var objHSMResult = hsmGen.GenerateIssuerWorkingKeyForFPS(keyType);
            return objHSMResult;
        }

        ///// <author>Vipul patel</author>
        ///// <created>16-Dec-2020</created>
        ///// <summary>
        ///// GetAAV
        ///// </summary>
        ///// <param name="PAN"></param>
        ///// <param name="index"></param>
        /////  <param name="hexData"></param>
        ///// <returns></returns>
        //public static string GetAAV(string pan, short index, string hexData)
        //{
        //    string aav = string.Empty;
        //    try
        //    {
        //        HSMGeneration objHelper = new HSMGeneration(pan, index);
        //        HSMResult objHSMResult = objHelper.GetAVV(hexData);

        //        if (objHSMResult.IsSuccess) aav = objHSMResult.AAV;

        //    }
        //    catch (Exception ex)
        //    {
        //        PostToBugscout.PostDataToBugScout(ex);
        //    }

        //    return aav;
        //}
    }
}
