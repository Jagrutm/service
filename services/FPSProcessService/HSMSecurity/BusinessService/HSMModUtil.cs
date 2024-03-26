using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContisGroup.CardUtil.DataObjects;
using CredECard.Common.Enums.Card;
using CredECard.BugReporting.BusinessService;
using CredECard.HSMSecurity.BusinessService;
using CredECard.CardProduction.BusinessService;

namespace ContisGroup.CardUtil.BusinessService
{
    public class HSMModUtil
    {

        /// <summary>
        /// This pin change method will used only for Online pin, because thid method uses
        /// clear Pin value. We have restricted this method for online pin only because, if Card Verification Method is Offline_Online  then
        /// it may happen if we allow then online pin may different then chip pin.
        /// 
        /// This method will use Contis Key to generate pinblock and pvv offset
        /// </summary>
        /// <param name="objCD"></param>
        /// <param name="currentClearPIN"></param>
        /// <param name="newClearPin"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes OnlinePinChange(CardData objCD, string currentClearPIN, string newClearPin)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            if (objCD != null && objCD.CardVerificationMethod == CredECard.CardProduction.Enums.EnumCardVerificationMethods.OnlineOnly)
            {
                try
                {
                    string cardClearPIN = string.Empty;
                    short zoneKeyIndex = 1;

                    HSMResult objResult = HSMGenUtil.GetPIN(objCD.PAN);

                    if (objResult.IsSuccess)
                    {
                        cardClearPIN = objResult.ClearPIN;

                        if (cardClearPIN.ToLower() != currentClearPIN.ToLower())
                        {
                            response = EnumPinResponseCodes.InvalidPIN;
                            try
                            {
                                objCD.UpdateFailedAttempt(EnumCardStatus.PIN_Tries_Exceeded);
                            }
                            catch (Exception ex)
                            {
                                PostToBugscout.PostDataToBugScout(ex);
                            }

                        }
                        else //Verification Success
                        {
                            HSMGeneration objHSMGen = new HSMGeneration(objCD.PAN, objCD.PVKI, zoneKeyIndex);
                            HSMResult objNewPinResult = objHSMGen.GetContisEncryptedPinBlock(objCD.PAN, newClearPin);

                            if (objNewPinResult.IsSuccess)
                            {
                                HSMResult resultNewPVV = objHSMGen.GenerateContisPVVOffset(objCD.PAN, objNewPinResult.PinBlock);

                                if (resultNewPVV.IsSuccess)
                                {
                                    try
                                    {
                                        CardData.UpdatePVV(objCD._encPAN, resultNewPVV.PVV, objNewPinResult.PinBlock);
                                        response = EnumPinResponseCodes.Success;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                                else
                                {
                                    if (resultNewPVV.IsUnSafePin) response = EnumPinResponseCodes.UnsafePIN;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    response = EnumPinResponseCodes.Failed;
                    PostToBugscout.PostDataToBugScout(ex);
                }
            }

            return response;
        }

        /// <summary>
        /// Change PIN
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="currentPinblock"></param>
        /// <param name="zoneKeyIndex"></param>
        /// <param name="newPinBlock"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes ChangePIN(CardData objCD, string cardNumber, string currentPinblock, short zoneKeyIndex, string newPinBlock)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                if (objCD != null)
                {
                    try
                    {
                        response = HSMVerUtil.VeryfyPIN(objCD, currentPinblock, zoneKeyIndex);
                    }
                    catch (Exception ex)
                    {
                        response = EnumPinResponseCodes.UnableToVerifyPIN;
                        PostToBugscout.PostDataToBugScout(ex);
                    }

                    if (response == EnumPinResponseCodes.Success)
                    {
                        //Replace PIN
                        HSMGeneration objHelper = new HSMGeneration(cardNumber, objCD._pvki, zoneKeyIndex);
                        HSMResult resultNewPVV = objHelper.GeneratePVVOffset(cardNumber, newPinBlock);

                        if (resultNewPVV.IsSuccess)
                        {
                            //Translate PIN block to Contis Key for DB Storage
                            HSMResult resultNewPinblock_conkey = HSMGenUtil.TranslatePinBlockForContisFromIWK(cardNumber, newPinBlock, zoneKeyIndex);

                            if (resultNewPinblock_conkey.IsSuccess)
                            {
                                try
                                {
                                    CardData.UpdatePVV(CardData.GetEncryptedPAN(cardNumber), resultNewPVV.PVV, resultNewPinblock_conkey.PinBlock);
                                    response = EnumPinResponseCodes.Success;
                                }
                                catch
                                {
                                    response = EnumPinResponseCodes.Failed;
                                }

                            }
                            else
                            {
                                response = EnumPinResponseCodes.Failed;
                            }


                        }
                        else if (resultNewPVV.IsUnSafePin)
                        {
                            response = EnumPinResponseCodes.UnsafePIN;
                        }
                        else
                            response = EnumPinResponseCodes.Failed;

                    }
                }
            }
            catch (Exception ex)
            {
                response = EnumPinResponseCodes.Failed;
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <summary>
        /// Unblock PIN
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="currentPinblock"></param>
        /// <param name="zoneKeyIndex"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes UnblockPIN(CardData objCD, string cardNumber, string currentPinblock, short zoneKeyIndex)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                if (objCD != null)
                {

                    try
                    {
                        response = HSMVerUtil.VeryfyPIN(objCD, currentPinblock, zoneKeyIndex);
 
                        //No need to update failed aatempt in case of failed because pin is already blocked
                    }
                    catch (Exception ex)
                    {
                        response = EnumPinResponseCodes.UnableToVerifyPIN;
                        PostToBugscout.PostDataToBugScout(ex);
                    }

                    if (response == EnumPinResponseCodes.Success)
                    {
                        objCD.UnblockCard();
                    }

                }
            }
            catch (Exception ex)
            {
                response = EnumPinResponseCodes.Failed;
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;
        }

        /// <summary>
        /// Unblock Pin Reversal function block the card
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes UnblockPinReversal(string cardNumber, string CardStatusChangeURL, int CardID)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                CardData.BlockCard(cardNumber,CardStatusChangeURL,CardID);
                response = EnumPinResponseCodes.Success;
            }
            catch (Exception ex)
            {
                response = EnumPinResponseCodes.Failed;
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;

        }

        /// <summary>
        /// Change Pin reversal reset the old PIN value
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static EnumPinResponseCodes ChangePinReversal(string cardNumber)
        {
            EnumPinResponseCodes response = EnumPinResponseCodes.Failed;

            try
            {
                CardData.ReversePin(cardNumber);

                response = EnumPinResponseCodes.Success;
            }
            catch (Exception ex)
            {
                response = EnumPinResponseCodes.Failed;
                PostToBugscout.PostDataToBugScout(ex);
            }

            return response;

        }

        //public static HSMResult ChangeEMVPIN(string cardNumber, string currentPinblock, short zoneKeyIndex, string newPinBlock,string ATC,string paddedData,string seqNo)
        //{
        //    HSMGeneration objHelper = new HSMGeneration(cardNumber, 1, zoneKeyIndex);
        //    HSMResult result = objHelper.VerifyEMVPinChange(cardNumber, currentPinblock, newPinBlock, seqNo, ATC, paddedData);          

        //    return result;
        //}
    }
}

