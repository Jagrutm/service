using System;
using System.Data;
using System.Data.SqlClient;
using ContisGroup.CardUtil.BusinessService;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Authorization;

namespace ContisGroup.CardUtil.DataService
{
    internal class ReadCardData
    {

        /// <author>Keyur Parekh</author>
        /// <created>23-Aug-2011</created>
        /// <summary>
        /// Read Card Data of card
        /// </summary>
        /// <param name="data">Card</param>
        internal static CardData ReadCard(string encPAN)
        {
            string pvv = string.Empty;
            
            SqlConnection myConnection = new SqlConnection(CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetCardData", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramPAN = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 200);
            paramPAN.Value = encPAN;
            myCommand.Parameters.Add(paramPAN);

            myConnection.Open();

            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            CardData data = null;
            try
            {
                if (myReader.Read())
                {
                    data = new CardData();

                    if (!(myReader["strPVVOffset"] is DBNull)) data._pvv = (string)myReader["strPVVOffset"];
                    if (!(myReader["intCardStatusID"] is DBNull)) data._statusID = Convert.ToInt16(myReader["intCardStatusID"]);
                    long ticks = (long)myReader["intExpiryDate"];
                    data._expiryDate = new DateTime(ticks);
                    data._serviceCode = (int)myReader["intServiceCode"];
                    data._iCvvCode = (int)myReader["intICvvCode"];
                    if (!(myReader["intPVKI"] is DBNull)) data._pvki = (short)myReader["intPVKI"];
                    data._cardID = (int)myReader["intCardID"];
                    data._cardDetailID = (int)myReader["intCardDetailID"];
                    data._activationCode = (string)myReader["strActivationCode"];
                    data._cardVerificationMethodID = (short)myReader["intCardVerificationMethodID"];
                    data._schemeID = (int)myReader["intSchemeID"];
                    if (!(myReader["dteDOB"] is DBNull)) data._dob = (DateTime)(myReader["dteDOB"]);
                    if (!(myReader["strLastName"] is DBNull)) data._surname = (string)(myReader["strLastName"]);
                    if (!(myReader["strPostCode"] is DBNull)) data._postCode = (string)(myReader["strPostCode"]);
                    data._cardTypeID = (short)myReader["intCardTypeID"];
                    data._checkTVR = (bool)myReader["bCheckTVR"];
                    data._cardTypeID = Convert.ToInt32(myReader["intCardTypeID"]);
                    data._isAllowTokenization = (bool)myReader["bAllowTokenization"];
                    if (!(myReader["strPanReferenceID"] is DBNull))
                    data._panReferenceID = (string)myReader["strPanReferenceID"];                  
                    data._line1 = (string)myReader["strLine1"];
                    if (!(myReader["strLine2"] is DBNull)) data._line2 = (string)(myReader["strLine2"]);
                    if (!(myReader["strLine3"] is DBNull)) data._line3 = (string)(myReader["strLine3"]);
                    data._checkForDuplicateAuth = (bool)myReader["bCheckDuplicateAuth"];
                    data._cardProgramID = (Int16)myReader["intCardProgramID"];
                    data._isCountryLevelBlock = (bool)myReader["bIsCountryLevelBlock"];
                    data._authServiceAPIURL = (string)myReader["strAuthServiceAPI"];
                    data.CardStatusChangeURL = (string)myReader["strStatusChangeAPI"];
                    data._pinUnBlockAttempt = Convert.ToInt16(myReader["intPinUnBlockAttempt"]);
                    data._isSendDeclineAuth = (bool)myReader["bIsSendDeclineAuth"];
                    data._isAllowContactlessTokenATMWithdrawal = (bool)myReader["bAllowContactlessTokenATMWithdrawal"];
                    data._isAllowVISADirectPayment = (bool)myReader["bIsAllowVISADirectPayment"];
                    data._isUseEMVA = (bool)myReader["bUseEMVA"];
                    data._isUseEMVM = (bool)myReader["bUseEMVM"];
                    data._isUseEMVP = (bool)myReader["bUseEMVP"];
                    data._isAllowNonAuthenticatedECI = (bool)myReader["bIsAllowNonAuthenticatedECI"];
                    data._isSCARequire = (bool)myReader["bSCARequire"];
                   
                    if (!(myReader["intLatestATC"] is DBNull)) data._latestATC = (int)(myReader["intLatestATC"]);//Mantan Bhatti : Case : 1158210

                    if (!(myReader["intStatusChangeReasonCodeID"] is DBNull)) data._statusChangeReasonCodeID = (int)(myReader["intStatusChangeReasonCodeID"]);//Niken Shah Case 123359

                    if (!(myReader["bStepupRequiredForDeviceBinding"] is DBNull)) data._isStepupRequiredForDeviceBinding = (bool)(myReader["bStepupRequiredForDeviceBinding"]);//Niken Shah Case 128147

                    if (!(myReader["strAuthClearingServiceAPI"] is DBNull)) data._authClearingServiceApiUrl = (string)(myReader["strAuthClearingServiceAPI"]); // RS#128206

                    if (!(myReader["intPrevATMPOSTransCurrencyCode"] is DBNull)) data._intPrevATMPOSTransCurrencyCode = (int)(myReader["intPrevATMPOSTransCurrencyCode"]);//Niken Shah Case 139421

                    if (!(myReader["intCardHolderCurrencyCode"] is DBNull)) data._intCardHolderCurrencyCode = (int)(myReader["intCardHolderCurrencyCode"]);//Niken Shah Case 139421

                    if (!(myReader["bCBPR2Required"] is DBNull)) data._isCBPR2Required = (bool)(myReader["bCBPR2Required"]);//Niken Shah Case 139421
                    if (!(myReader["intSoftDeclineECI7HigherLimit"] is DBNull)) data._softDeclineECI7HigherLimit = (int)(myReader["intSoftDeclineECI7HigherLimit"]);//VP 140608
                    if (!(myReader["intSoftDeclineECI6HigherLimit"] is DBNull)) data._softDeclineECI6HigherLimit = (int)(myReader["intSoftDeclineECI6HigherLimit"]);//VP 140608
                    if (!(myReader["intLowvalueLimit"] is DBNull)) data._lowvalueLimit = (int)(myReader["intLowvalueLimit"]);//VP 140608
                    if (!(myReader["UseAquirerTRAFlag"] is DBNull)) data._useAquirerTRAFlag = (string)(myReader["UseAquirerTRAFlag"]);//VP 140608
                    if (!(myReader["bProblemCard"] is DBNull)) data._isProblemCard = (bool)(myReader["bProblemCard"]);
                }
            }
            finally 
            {
                myReader.Dispose();
            }

            return data;
        }

        /// <author>Keyur Parekh</author>
        /// <created>23-Aug-2011</created>
        /// <summary>
        /// Read Card Data of card
        /// </summary>
        /// <param name="data">Card</param>
        internal static CardData ReadCard(int cardID)
        {
            string pvv = string.Empty;

            SqlConnection myConnection = new SqlConnection(CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetCardDataByCardID", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramcardID = new SqlParameter("@intCardID", SqlDbType.Int, 4);
            paramcardID.Value = cardID;
            myCommand.Parameters.Add(paramcardID);

            myConnection.Open();

            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            CardData data = null;
            try
            {
                if (myReader.Read())
                {
                    data = new CardData();

                    if (!(myReader["strPVVOffset"] is DBNull)) data._pvv = (string)myReader["strPVVOffset"];
                    if (!(myReader["intCardStatusID"] is DBNull)) data._statusID = Convert.ToInt16(myReader["intCardStatusID"]);

                    long ticks = (long)myReader["intExpiryDate"];
                    data._expiryDate = new DateTime(ticks);

                    data._serviceCode = (int)myReader["intServiceCode"];
                    data._iCvvCode = (int)myReader["intICvvCode"];
                    if (!(myReader["intPVKI"] is DBNull)) data._pvki = (short)myReader["intPVKI"];

                    data._cardID = (int)myReader["intCardID"];
                    data._cardDetailID = (int)myReader["intCardDetailID"];
                    data._activationCode = (string)myReader["strActivationCode"];
                    data._cardVerificationMethodID = (short)myReader["intCardVerificationMethodID"];
                    data._schemeID = (int)myReader["intSchemeID"];

                    if (!(myReader["dteDOB"] is DBNull)) data._dob = (DateTime)(myReader["dteDOB"]);
                    if (!(myReader["strLastName"] is DBNull)) data._surname = (string)(myReader["strLastName"]);
                    if (!(myReader["strPostCode"] is DBNull)) data._postCode = (string)(myReader["strPostCode"]);

                    data._cardTypeID = (short)myReader["intCardTypeID"];

                    data._encPAN = (string)myReader["strPan"];

                    data._failedCVV2Attempt = (int)myReader["intCVV2FailedCount"];
                    data._isAllowTokenization = (bool)myReader["bAllowTokenization"];
                    data._cardDisplayName = (string)myReader["strCardDisplayName"];                    
                    data._cardProgramID = (Int16)myReader["intCardProgramID"]; //Manthan Bhatti : Case : 78165

                    // Niken Shah Case 119275 Start
                    if (!(myReader["strLine1"] is DBNull)) data._line1 = (string)myReader["strLine1"];
                    if (!(myReader["strLine2"] is DBNull)) data._line2 = (string)myReader["strLine2"];
                    if (!(myReader["strCity"] is DBNull)) data._city = (string)myReader["strCity"];
                    if (!(myReader["CountryA2Code"] is DBNull)) data._countryCode = (string)myReader["CountryA2Code"];
                    // Niken Shah Case 119275 End
                    data.CardStatusChangeURL = (string)myReader["strStatusChangeAPI"];//C#153060 Vishal S
                    if (!(myReader["bIsNameOnlyCard"] is DBNull)) data._isNameOnlyCard = (bool)myReader["bIsNameOnlyCard"]; // Tejas Choksi Case 153103

                    if (!(myReader["intProductTypeID"] is DBNull)) data._enumProductType = (EnumProductType)(int)myReader["intProductTypeID"]; // Niken Shah Case 128255
                }
            }
            finally
            {
                myReader.Dispose();
            }

            return data;
        }
        /// <author>Keyur Parekh</author>
        /// <created>09-Nov-2011</created>
        /// <summary>
        /// Read Card Info
        /// </summary>
        internal static string GetCardInfo(string encPAN)
        {
            string info = string.Empty;

            SqlConnection myConnection = new SqlConnection(CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetCardInfo", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramPAN = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 200);
            paramPAN.Value = encPAN;
            myCommand.Parameters.Add(paramPAN);

            myConnection.Open();
            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            try
            {
                if (myReader.Read())
                {
                    info = (string)myReader["strInfo"];
                }
            }
            finally
            {
                myReader.Dispose();
            }

            return info;
        }

        /// <author>Manthan Bhatti</author>
        /// <created>03-Mar-2020</created>
        /// <summary>
        /// Read Card Data of card
        /// </summary>
        /// <param name="data">Card</param>
        internal static CardData ReadCardFraudData(int cardID)
        {
            string pvv = string.Empty;

            SqlConnection myConnection = new SqlConnection(CredECardConfig.GetReadOnlyConnectionString());
            SqlCommand myCommand = new SqlCommand("spGetCardFraudDataByCardID", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramcardID = new SqlParameter("@intCardID", SqlDbType.Int, 4);
            paramcardID.Value = cardID;
            myCommand.Parameters.Add(paramcardID);

            myConnection.Open();

            SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            CardData data = null;
            try
            {
                if (myReader.Read())
                {
                    data = new CardData();

                    data._cardID = (int)myReader["intCardID"];
                    if (!(myReader["intCardFraudSetupId"] is DBNull)) data._cardFraudSetupID = Convert.ToInt64(myReader["intCardFraudSetupId"]);
                    if (!(myReader["intInvalidExpiryDateCount"] is DBNull)) data._invalidExpiryDateCount = (int)myReader["intInvalidExpiryDateCount"];
                }
            }
            finally
            {
                myReader.Dispose();
            }

            return data;
        }
    }
}


