using System;
using CredECard.Common.BusinessService;
using ContisGroup.CardUtil.DataObjects;
using CredECard.Common.Enums.Card;
using CredECard.BugReporting.BusinessService;
using CredECard.HSMSecurity.BusinessService;
using System.Text;
using CredECard.CardProduction.BusinessService;

namespace ContisGroup.CardUtil.BusinessService
{
    public class HSMUtility
    {
        /// <summary>
        /// Reset PIN 
        /// </summary>
        /// <param name="cardNumber"></param>
        ///// <returns></returns>
        //public static void ResetPIN(string cardNumber, short pvki)
        //{
        //    string encPan = CardData.GetEncryptedPAN(cardNumber);
            
        //    HSMGeneration objHelper = new HSMGeneration(cardNumber, pvki);
        //    HSMResult result = objHelper.GeneratePin(cardNumber);

        //    if (result.IsSuccess)
        //    {
        //        try
        //        {
        //            //HSMResult objTran = HSMGenUtil.TranslatePinBlockForDBStorage(cardNumber, result.PinBlock);
        //            CardData.UpdatePVV(encPan, result.PVV, result.PinBlock);
        //        }
        //        catch (Exception ex)
        //        {
        //            PostToBugscout.PostDataToBugScout(ex);
        //        }
        //    }
        //}

        
        public static string ExcryptRequest(string request)
        {
            HSMCommon objHelper = new HSMCommon();
            return objHelper.ExcryptRequest(request);
        }


        public static string ExecuteCommand(string command)
        {
            HSMCommon objHelper = new HSMCommon();
            return objHelper.ExecuteCommand(command);
        }

        public static string ExecuteStdCommand(string command)
        {
            HSMCommon objHelper = new HSMCommon();
            return objHelper.ExecuteStdCommand(command);
        }
        
    }
}
