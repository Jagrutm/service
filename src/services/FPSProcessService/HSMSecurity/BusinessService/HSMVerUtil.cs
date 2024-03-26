using System;
using System.Text;
using ContisGroup.CardUtil.DataObjects;
using CredECard.BugReporting.BusinessService;
using CredECard.CardProduction.BusinessService;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Card;
using CredECard.HSMSecurity.BusinessService;
using CredEcard.CredEncryption.BusinessService;
using System.Security.Cryptography;
using CredECard.Common.Enums.Authorization;
using CredEncryption.BusinessService;

namespace ContisGroup.CardUtil.BusinessService
{
    public class HSMVerUtil
    {
        #region Public Methods
        /*
        /// <author>Keyur Parekh</author>
        /// <created>20-Apr-2012</created>
        /// <summary>
        /// Verify PIN
        /// </summary>
        /// <param name="objCardData"></param>
        /// <param name="pinblock"></param>
        /// <param name="zoneKeyIndex"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes VeryfyPIN(CardData objCardData, string pinblock, short zoneKeyIndex, string workingKey = null) // RS#128208 - workingKey new parameter
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.InvalidPIN;

            try
            {
                if (objCardData != null)
                {
                    HSMVerification objHelper = new HSMVerification(objCardData.PAN, objCardData.PVKI, zoneKeyIndex);
                    HSMResult objHSMResult = null;

                    if (!string.IsNullOrEmpty(workingKey)) // RS#128208
                        objHSMResult = objHelper.VerifyPinDynamicKey(objCardData.PAN, pinblock, objCardData.PVV, workingKey);
                    else
                        objHSMResult = objHelper.VerifyPin(objCardData.PAN, pinblock, objCardData.PVV);


                    if (objHSMResult.IsSuccess) response = EnumPinResponseCodes.Success;

                    //Update Failed Attempt if current card status is Normal
                    if (response == EnumPinResponseCodes.InvalidPIN)
                    {
                        try
                        {
                            if (objCardData.Status == EnumCardStatus.Normal)
                            {
                                objCardData.UpdateFailedAttempt(EnumCardStatus.PIN_Tries_Exceeded);
                            }
                            else if (objCardData.Status == EnumCardStatus.PIN_Tries_Exceeded)
                            {
                                objCardData.UpdatePinUnblockAttempt();
                            }
                        }
                        catch (Exception ex)
                        {
                            PostToBugscout.PostDataToBugScout(ex);
                        }
                    }

                    if (objHSMResult.HSMResultCode == EnumHSMResultCode.PINFormatError) // RS#128208
                    {
                        response = EnumPinResponseCodes.UnableToVerifyPIN;
                    }

                }
            }
            catch (Exception ex)
            {
                response = EnumPinResponseCodes.UnableToVerifyPIN;
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <author>Keyur Parekh</author>
        /// <created>20-Apr-2012</created>
        /// <summary>
        /// Verify CVV
        /// </summary>
        /// <param name="objCardData"></param>
        /// <param name="cvv"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes VeryfyCVV(CardData objCardData, string cvv)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                if (objCardData != null)
                {
                    HSMVerification objHelper = new HSMVerification(objCardData.PAN, 1);
                    HSMResult objHSMResult = objHelper.VerifyCVV(objCardData.PAN, objCardData.ExpiryDate, objCardData.ServiceCode, cvv);

                    if (objHSMResult.IsSuccess)
                        response = EnumPinResponseCodes.Success;
                    else
                        objCardData.UpdateFailedAttempt(EnumCardStatus.Fraudulent_use);
                }
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <author>Niken Shah</author>
        /// <created>05-Aug-2020</created>
        /// <summary>
        /// Verify CVC (MasterCard)
        /// </summary>
        /// <param name="objCardData"></param>
        /// <param name="cvv"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes VeryfyCVC(CardData objCardData, string cvv)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                if (objCardData != null)
                {
                    HSMVerification objHelper = new HSMVerification(objCardData.PAN, 1);
                    HSMResult objHSMResult = objHelper.VerifyCVC(objCardData.PAN, objCardData.ExpiryDate, objCardData.ServiceCode, cvv);

                    if (objHSMResult.IsSuccess)
                        response = EnumPinResponseCodes.Success;
                    else
                        objCardData.UpdateFailedAttempt(EnumCardStatus.Fraudulent_use);
                }
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>18-Mar-2015</created>
        /// <summary>Veryfies the CVV2.</summary>
        public static EnumPinResponseCodes VeryfyCVV2(int CardID, int ServiceCode, string EncryptedPAN, DateTime ExpiryDate, string cvv2)
        {
            CardData objCardData = new CardData();
            objCardData._cardID = CardID;
            objCardData._serviceCode = ServiceCode;
            objCardData._encPAN = EncryptedPAN;
            objCardData._expiryDate = ExpiryDate;

            return VeryfyCVV2(objCardData, cvv2);
        }

        /// <author>Niken Shah</author>
        /// <created>05-Aug-2020</created>
        /// <summary>Veryfies the CVC2.</summary>
        public static EnumPinResponseCodes VeryfyCVC2(int CardID, int ServiceCode, string EncryptedPAN, DateTime ExpiryDate, string cvv2)
        {
            CardData objCardData = new CardData();
            objCardData._cardID = CardID;
            objCardData._serviceCode = ServiceCode;
            objCardData._encPAN = EncryptedPAN;
            objCardData._expiryDate = ExpiryDate;

            return VeryfyCVC2(objCardData, cvv2);
        }

        /// <author>Keyur Parekh</author>
        /// <created>29-Jul-2013</created>
        /// <summary>
        /// Verify CVV2
        /// </summary>
        /// <param name="objCardData"></param>
        /// <param name="cvv2"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes VeryfyCVV2(CardData objCardData, string cvv2)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                HSMVerification objHelper = new HSMVerification(objCardData.PAN, 1);
                HSMResult objHSMResult = objHelper.VerifyCVV2(objCardData.PAN, objCardData.ExpiryDate, objCardData.ServiceCode, cvv2);

                if (objHSMResult.IsSuccess)
                    response = EnumPinResponseCodes.Success;
                else
                    objCardData.UpdateFailedAttempt(EnumCardStatus.CVV2_tries_exceeded);

            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <author>Niken Shah</author>
        /// <created>05-Aug-2020</created>
        /// <summary>
        /// Verify CVC2
        /// </summary>
        /// <param name="objCardData"></param>
        /// <param name="cvv2"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes VeryfyCVC2(CardData objCardData, string cvv2)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                HSMVerification objHelper = new HSMVerification(objCardData.PAN, 1);
                HSMResult objHSMResult = objHelper.VerifyCVC2(objCardData.PAN, objCardData.ExpiryDate, objCardData.ServiceCode, cvv2);

                if (objHSMResult.IsSuccess)
                    response = EnumPinResponseCodes.Success;
                else
                    objCardData.UpdateFailedAttempt(EnumCardStatus.CVV2_tries_exceeded);

            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <author>Keyur Parekh</author>
        /// <created>20-Apr-2012</created>
        /// <summary>
        /// Verify ICVV
        /// </summary>
        /// <param name="objCardData"></param>
        /// <param name="icvv"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes VeryfyICVV(CardData objCardData, string icvv)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                if (objCardData != null)
                {
                    HSMVerification objHelper = new HSMVerification();
                    HSMResult objHSMResult = new HSMResult();
                    if (objCardData.IsProblemCard == false)
                    {
                        objHelper = new HSMVerification(objCardData.PAN, 1);
                        objHSMResult = objHelper.VerifyICVV(objCardData.PAN, objCardData.ExpiryDate, objCardData.ICvvCode, icvv);
                    }
                    else
                    {
                        objHelper = new HSMVerification(objCardData.PAN, 1);
                        objHSMResult = objHelper.GenerateCvvAndCvv2(objCardData.PAN, objCardData.ExpiryDate, objCardData.ICvvCode);
                        if (objHSMResult.CVV == icvv)
                        {
                            objHSMResult.IsSuccess = true;
                        }
                        else
                        {
                            objHSMResult.IsSuccess = false;
                        }
                    }

                    if (objHSMResult.IsSuccess)
                        response = EnumPinResponseCodes.Success;
                    else
                        objCardData.UpdateFailedAttempt(EnumCardStatus.Fraudulent_use);
                }
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <author>Niken Shah</author>
        /// <created>05-Aug-2020</created>
        /// <summary>
        /// Verify ICVC
        /// </summary>
        /// <param name="objCardData"></param>
        /// <param name="icvv"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes VeryfyICVC(CardData objCardData, string icvv)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                if (objCardData != null)
                {
                    HSMVerification objHelper = new HSMVerification(objCardData.PAN, 1);
                    HSMResult objHSMResult = objHelper.VerifyICVC(objCardData.PAN, objCardData.ExpiryDate, objCardData.ICvvCode, icvv);

                    if (objHSMResult.IsSuccess)
                        response = EnumPinResponseCodes.Success;
                    else
                        objCardData.UpdateFailedAttempt(EnumCardStatus.Fraudulent_use);
                }
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <author>Keyur Parekh</author>
        /// <created>20-Apr-2012</created>
        /// <summary>
        /// Verify ARQC and return ARPC if success
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="sequenceNumber"></param>
        /// <param name="atc"></param>
        /// <param name="data"></param>
        /// <param name="ARQC"></param>
        /// <param name="responseCode"></param>
        /// <returns></returns>
        public static string VerifyARQC(string cardNumber, string sequenceNumber, string atc, string data, string ARQC, string responseCode)
        {
            HSMResult objHSMResult = null;
            string arpc = string.Empty;
            try
            {
                HSMVerification objHelper = new HSMVerification(cardNumber, 1);
                objHSMResult = objHelper.VerifyARQC(cardNumber, sequenceNumber, atc, data, ARQC, responseCode);

                if (objHSMResult.IsSuccess) arpc = objHSMResult.ARPC;
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return arpc;
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
        public static string VerifyEMVAARQC(string cardNumber, string sequenceNumber, string atc, string data, string ARQC, string responseCode, EnumKeyDerivationMethod keyDerivationMethod)  //Prashant Soni:7-Apr-20 :  change from bool to string
        {
            HSMResult objHSMResult = null;
            string arpc = string.Empty;
            try
            {
                HSMVerification objHelper = new HSMVerification(cardNumber, 1);
                objHSMResult = objHelper.VerifyEMVAARQC(cardNumber, sequenceNumber, atc, data, ARQC, responseCode, keyDerivationMethod);
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            if (objHSMResult.IsSuccess) arpc = objHSMResult.ARPC;

            return arpc;
            //if (objHSMResult != null)
            //    return objHSMResult.IsSuccess;
            //else
            //    return false;
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
        public static string VerifyEMVAARQC_MC(string cardNumber, string sequenceNumber, string atc, string data, string ARQC, string responseCode, EnumKeyDerivationMethod keyDerivationMethod, string UniqueNumber)  //Prashant Soni:7-Apr-20 :  change from bool to string
        {
            HSMResult objHSMResult = null;
            string arpc = string.Empty;
            try
            {
                HSMVerification objHelper = new HSMVerification(cardNumber, 1);
                objHSMResult = objHelper.VerifyEMVAARQC_MC(cardNumber, sequenceNumber, atc, data, ARQC, responseCode, keyDerivationMethod, UniqueNumber);
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            if (objHSMResult.IsSuccess) arpc = objHSMResult.ARPC;

            return arpc;
            //if (objHSMResult != null)
            //    return objHSMResult.IsSuccess;
            //else
            //    return false;
        }
        public static EnumPinResponseCodes VerifyCAVV(string cardNumber, string merchantName, string xid, string cavv)
        {
            HSMResult objHSMResult = null;
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                long id = 0;
                long.TryParse(xid, out id);

                HSMVerification objHelper = new HSMVerification(cardNumber, 1);
                objHSMResult = objHelper.VerifyCAVV(cardNumber, merchantName, id, cavv);

                if (objHSMResult.IsSuccess) response = EnumPinResponseCodes.Success;

            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
                response = EnumPinResponseCodes.Failed;
            }
            return response;
        }

        /// <author>Keyur Parekh</author>
        /// <created>20-Apr-2012</created>
        /// <summary>
        /// Change EMV PIN & return pinblock and mac
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="sequenceNo"></param>
        /// <param name="newEncPinBlock_IWK"></param>
        /// <param name="atc"></param>
        /// <param name="dataBlock"></param>
        /// <param name="zoneKeyIndex"></param>
        /// <returns>HSMREsult</returns>
        public static HSMResult ChangeEMVPin(string pan, string sequenceNo, string newEncPinBlock_IWK, string atc, string dataBlock, short zoneKeyIndex, EnumDerivationType enumDerivationType)
        {
            HSMVerification objHelper = new HSMVerification(pan, 1, zoneKeyIndex);
            HSMResult objHSMResult = objHelper.ChangeEMVPin(pan, sequenceNo, newEncPinBlock_IWK, atc, dataBlock, enumDerivationType);

            return objHSMResult;
        }

        /// <summary>
        /// This pin verification method will used only for Online pin verfication, because thid method uses
        /// </summary>
        /// <param name="cardNumber">PAN</param>
        /// <param name="clearPIN">Pin</param>
        /// <returns></returns>
        public static EnumPinResponseCodes OnlinePinVerification(string cardNumber, string clearPIN, string cardStatusChangeURL = "", int cardID = 0)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                string cardClearPIN = string.Empty;

                HSMResult objResult = HSMGenUtil.GetPIN(cardNumber);

                if (objResult.IsSuccess)
                {
                    cardClearPIN = objResult.ClearPIN;

                    if (cardClearPIN.ToLower() != clearPIN.ToLower())
                    {
                        response = EnumPinResponseCodes.InvalidPIN;
                        try
                        {
                            CardData objCD = new CardData();
                            objCD._encPAN = new MasterCardEncrypt().EncryptPANUsingKey(cardNumber); // Case 26734 : Madhuri Solanki
                            objCD.CardStatusChangeURL = cardStatusChangeURL;//C#153060 Vishal S
                            objCD._cardID = cardID;//C#153060 Vishal S
                            objCD.UpdateFailedAttempt(EnumCardStatus.PIN_Tries_Exceeded);
                        }
                        catch (Exception ex)
                        {
                            PostToBugscout.PostDataToBugScout(ex);
                        }

                    }
                    else
                        response = EnumPinResponseCodes.Success;
                }
            }
            catch (Exception ex)
            {
                response = EnumPinResponseCodes.Failed;
                PostToBugscout.PostDataToBugScout(ex);
            }


            return response;
        }
        */
        #endregion

        #region Private Methods

        /// <author>Keyur Parekh</author>
        /// <created>20-Apr-2012</created>
        /// <summary>
        /// Raise bug to fog bug
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="result"></param>
        private static void raiseBug(string functionName, HSMResult result)
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

            PostToBugscout.PostDataToBugScout(new PersistException(sb.ToString()));
        }

        #endregion


        /// <author>Keyur Parekh</author>
        /// <created>20-Apr-2012</created>
        /// <summary>
        /// Verify ICVV
        /// </summary>
        /// <param name="objCardData"></param>
        /// <param name="icvv"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes VeryfyCavvCvv2(string pan, short index, string unPredictableNumber, string servicecode, string cavv_cvv2)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {

                HSMVerification objHelper = new HSMVerification(pan, index);
                HSMResult objHSMResult = objHelper.VerifyCavvCvv2(pan, unPredictableNumber, servicecode, cavv_cvv2);

                if (objHSMResult.IsSuccess) response = EnumPinResponseCodes.Success;

            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <author>Niken Shah</author>
        /// <created>05-Aug-2020</created>
        /// <summary>
        /// Verify AAV
        /// </summary>       
        /// <returns></returns>
        public static EnumPinResponseCodes VerifyAAV(string pan, short index, string unPredictableNumber, string servicecode, string cavv_cvv2)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {

                HSMVerification objHelper = new HSMVerification(pan, index);
                HSMResult objHSMResult = objHelper.VerifyAAV(pan, unPredictableNumber, servicecode, cavv_cvv2);

                if (objHSMResult.IsSuccess) response = EnumPinResponseCodes.Success;

            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <author>Aarti Meswania</author>
        /// <created>03-May-2019</created>
        /// <summary>Verify CAVV according to version 7</summary>
        public static EnumPinResponseCodes VerifyCavvCvv2(string pan, short index, string unPredictableNoOrSeedValue, string servicecode, string cavv_cvv2, string supplementaryData)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                HSMVerification objHelper = new HSMVerification(pan, index);

                supplementaryData = generateCAVVVersion7(pan, supplementaryData);

                HSMResult objHSMResult = objHelper.VerifyCavvCvv2(supplementaryData, unPredictableNoOrSeedValue, servicecode, cavv_cvv2);
                if (objHSMResult.IsSuccess) response = EnumPinResponseCodes.Success;
            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }
            return response;
        }

        /// <author>Aarti Meswania</author>
        /// <created>06-May-2019</created>
        /// <summary>generate Pan+supplementary data input according to version 7</summary>
        private static string generateCAVVVersion7(string cardNumber, string supplementaryData)
        {
            //step 1: Extract the pan
            //step 2: Extract the Purchase Amount, Purchase Currency Code and Purchase date from the authentication request and Concatenate (=> here it is available in supplementaryData)
            //step 3: Concate pan and step2 value
            //step 4: Compute the cryptographic hash for the result obtained from step 3 using SHA-256.
            string step4 = getSHA256(cardNumber + supplementaryData); //Where supplementaryData holds Purchase Amount, Purchase Currency Code and Purchase date 
            //step 5: Extract the first 16 numerical digits (digits 0 through 9) from the result of step 4. If 16 numerical digits are not found, subtract 10 from all character digits (digits A through F) and use the first result(s) for as many digits missing of the 16-digit value.
            string step5 = getFirst16Digits(step4);
            return step5;
        }

        /// <author>Aarti Meswania</author>
        /// <created>06-May-2019</created>
        /// <summary>get SHA 256</summary>
        private static string getSHA256(string text)
        {

            using (SHA256Managed sha2 = new SHA256Managed())
            {
                var hash = sha2.ComputeHash(Encoding.UTF8.GetBytes(text));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
            /*
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);
            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
            */
        }

        /// <author>Aarti Meswania</author>
        /// <created>06-May-2019</created>
        /// <summary>get First 16 Digits</summary>
        private static string getFirst16Digits(string text)
        {
            string first16Digits = string.Empty;
            string first16CharacterEquivalentInt = string.Empty;
            for (int i = 0; i <= text.Length - 1; i++)
            {
                if (char.IsDigit(text[i]))
                    first16Digits += text[i];
                else
                    first16CharacterEquivalentInt += (((int)text[i]) - 10).ToString();
                if (first16Digits.Length == 16)
                    break;
            }

            if (first16Digits.Length < 16)
                first16Digits = (first16Digits + first16CharacterEquivalentInt).Substring(0, 16);
            return first16Digits;
        }

        /// <author>Keyur Parekh</author>
        /// <created>02-Apr-2014</created>
        /// <summary>
        /// Verify Dynamic CVV
        /// </summary>
        /// <param name="cardNumber">String Pan</param>
        /// <param name="panSeqNum">Int Pan sequence number</param>
        /// <param name="unPredNum">Int un predictable number</param>
        /// <param name="atc">int atc</param>
        /// <param name="trackData">string track data</param>
        /// <param name="dcvv">string dcvv</param>
        /// <returns>EnumPinResponseCodes</returns>
        public static EnumPinResponseCodes VeryfyDCVV(string cardNumber, string panSeqNum, string unPredNum, string atc, string trackData, string dcvv)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {

                HSMVerification objHelper = new HSMVerification(cardNumber, 1);
                HSMResult objHSMResult = objHelper.VerifyDCVV(cardNumber, panSeqNum, unPredNum, atc, trackData, dcvv);

                if (objHSMResult.IsSuccess) response = EnumPinResponseCodes.Success;

            }
            catch (Exception ex)
            {
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }
    }
}
