namespace ContisGroup.CardUtil.DataService
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using CredECard.Common.BusinessService;
    using CredECard.Common.Enums.Card;

    internal class WriteCardData : StandardDataService
    {

        internal WriteCardData(DataController conn)
            : base(conn)
        {
        }

        /// <author>Keyur Parekh</author>
        /// <created>18-Aug-2010</created>
        /// <summary>
        /// Write data to DB
        /// </summary>
        /// <param name="data">Card</param>
        internal void UpdateCardData(string cardNumber, string pvv)
        {
            SqlCommand myCommand = new SqlCommand("spUpdateCardPVV");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardNumber = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 150);
            paramCardNumber.Value = cardNumber;
            myCommand.Parameters.Add(paramCardNumber);

            SqlParameter paramPVV = new SqlParameter("@strPVV", SqlDbType.NVarChar, 4);
            paramPVV.Value = pvv;
            myCommand.Parameters.Add(paramPVV);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>18-Aug-2010</created>
        /// <summary>
        /// Update Failed Pin Attempt
        /// </summary>
        /// <param name="data">Card</param>
        internal void UpdateFailedAttempt(string cardNumber, EnumCardStatus cardStatus, out int cardStatusHistoryID)
        {
            cardStatusHistoryID = 0;

            SqlCommand myCommand = new SqlCommand("spUpdateFailedAttempt");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardNumber = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 150);
            paramCardNumber.Value = cardNumber;
            myCommand.Parameters.Add(paramCardNumber);

            SqlParameter paramCardStatus = new SqlParameter("@intCardStatusID", SqlDbType.SmallInt);
            paramCardStatus.Value = (Int16)cardStatus;
            myCommand.Parameters.Add(paramCardStatus);

            SqlParameter paramCardHistoryID = new SqlParameter("@intCardStatusHistoryID", SqlDbType.Int, 4);
            paramCardHistoryID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(paramCardHistoryID);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();

                cardStatusHistoryID = (int)paramCardHistoryID.Value;
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Vipul Patel</author>
        /// <created>20-Dec-2017</created>
        /// <summary>
        /// Update Pinunblock Attempt
        /// </summary>
        /// <param name="cardID">int</param>
        internal void UpdatePinUnblockAttempt(int cardID)
        {

            SqlCommand myCommand = new SqlCommand("spUpdatePinUnblockdAttempt");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramcardID = new SqlParameter("@intCardID", SqlDbType.Int);
            paramcardID.Value = cardID;
            myCommand.Parameters.Add(paramcardID);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Vipul Patel</author>
        /// <created>20-Feb-2020</created>
        /// <summary>
        /// Block/unblock Token OTP 
        /// </summary>
        /// <param name="cardID">int</param>
        internal void BlockUnblockGetCVMOTP(int cardID, bool blockUnblock)
        {

            SqlCommand myCommand = new SqlCommand("spBlockUnblockbGetCVMOTP");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramcardID = new SqlParameter("@intCardID", SqlDbType.Int);
            paramcardID.Value = cardID;
            myCommand.Parameters.Add(paramcardID);

            SqlParameter paramblockUnblock = new SqlParameter("@bBlockUnblock", SqlDbType.Bit);
            paramblockUnblock.Value = blockUnblock;
            myCommand.Parameters.Add(paramblockUnblock);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }
        /// <author>Vipul Patel</author>
        /// <created>11-Jan-2019</created>
        /// <summary>
        /// Update all token to suspend
        /// </summary>
        /// <param name="cardDetailID">int</param>
        internal void SuspendAllTokensOfCard(int cardDetailID, int statusChangeReasonCodeID)
        {

            SqlCommand myCommand = new SqlCommand("spSuspendAllTokensOfCard");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardDetailID = new SqlParameter("@intCardDetailID", SqlDbType.Int);
            paramCardDetailID.Value = cardDetailID;
            myCommand.Parameters.Add(paramCardDetailID);

            SqlParameter paramStatusChangeReasonCodeID = new SqlParameter("@intStatusChangeReasonCodeID", SqlDbType.Int);
            paramStatusChangeReasonCodeID.Value = statusChangeReasonCodeID;
            myCommand.Parameters.Add(paramStatusChangeReasonCodeID);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Vipul Patel</author>
        /// <created>14-Apr-2020</created>
        /// <summary>
        /// Update all token to suspend
        /// </summary>
        /// <param name="cardDetailID">int</param>
        internal void TokenVerificationFailAction(int cardDetailID, string activationVerificationResult, Int64 tokenRequestorID, string encToken)
        {

            SqlCommand myCommand = new SqlCommand("spTokenVerificationFailAction");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardDetailID = new SqlParameter("@intCardDetailID", SqlDbType.Int);
            paramCardDetailID.Value = cardDetailID;
            myCommand.Parameters.Add(paramCardDetailID);

            SqlParameter paramTokenRequestorID = new SqlParameter("@intTokenRequestorID", SqlDbType.BigInt);
            paramTokenRequestorID.Value = tokenRequestorID;
            myCommand.Parameters.Add(paramTokenRequestorID);

            SqlParameter paramresult = new SqlParameter("@strActivationVerificationResult", SqlDbType.NVarChar, 3);
            paramresult.Value = activationVerificationResult;
            myCommand.Parameters.Add(paramresult);

            SqlParameter paramencToken = new SqlParameter("@strToken", SqlDbType.NVarChar, 150);
            paramencToken.Value = encToken;
            myCommand.Parameters.Add(paramencToken);


            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Vipul Patel</author>
        /// <created>03-Mar-2020</created>
        /// <summary>
        /// Update all token to suspend
        /// </summary>
        /// <param name="cardDetailID">int</param>
        internal void DeleteAllTokensOfCard(int cardDetailID)
        {

            SqlCommand myCommand = new SqlCommand("spDeleteAllTokensOfCard");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardDetailID = new SqlParameter("@intCardDetailID", SqlDbType.Int);
            paramCardDetailID.Value = cardDetailID;
            myCommand.Parameters.Add(paramCardDetailID);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }
        /// <author>Vipul Patel</author>
        /// <created>11-Jan-2019</created>
        /// <summary>
        /// Update all token to Resume
        /// </summary>
        /// <param name="cardDetailID">int</param>
        internal void ResumeAllTokensOfCardSuspendedBySystem(int cardDetailID)
        {

            SqlCommand myCommand = new SqlCommand("spResumeAllTokensOfCard");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardDetailID = new SqlParameter("@intCardDetailID", SqlDbType.Int);
            paramCardDetailID.Value = cardDetailID;
            myCommand.Parameters.Add(paramCardDetailID);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }
        /// <author>Keyur Parekh</author>
        /// <created>18-Aug-2010</created>
        /// <summary>
        /// Write data to DB
        /// </summary>
        ///<param name="cardNumber">Card Number</param>
        internal void UnblockCard(string cardNumber, out int cardStatusHistoryID)
        {
            cardStatusHistoryID = 0;

            SqlCommand myCommand = new SqlCommand("spResetBlockedPIN");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardNumber = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 150);
            paramCardNumber.Value = cardNumber;
            myCommand.Parameters.Add(paramCardNumber);

            SqlParameter paramCardHistoryID = new SqlParameter("@intCardStatusHistoryID", SqlDbType.Int, 4);
            paramCardHistoryID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(paramCardHistoryID);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();

                cardStatusHistoryID = (int)paramCardHistoryID.Value;
            }
            finally
            {
                myCommand.Dispose();
            }
        }


        /// <author>Keyur Parekh</author>
        /// <created>09-Nov-2011</created>
        /// <summary>
        /// Write Card Info
        /// </summary>
        ///<param name="cardID">Card Number</param>
        ///<param name="cardInfo">Card Info</param>
        internal void WriteCardInfo(string cardNumber, string cardInfo)
        {
            SqlCommand myCommand = new SqlCommand("spSaveCardInfo");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardNumber = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 150);
            paramCardNumber.Value = cardNumber;
            myCommand.Parameters.Add(paramCardNumber);

            SqlParameter paramCardInfo = new SqlParameter("@strInfo", SqlDbType.NVarChar, 200);
            paramCardInfo.Value = cardInfo;
            myCommand.Parameters.Add(paramCardInfo);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>24-Nov-2010</created>
        /// <summary>
        /// Block Card
        /// </summary>
        ///<param name="cardNumber">Card Number</param>
        internal void BlockCard(string cardNumber, out int cardStatusHistoryID)
        {
            cardStatusHistoryID = 0;

            SqlCommand myCommand = new SqlCommand("spBlockCard");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardNumber = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 150);
            paramCardNumber.Value = cardNumber;
            myCommand.Parameters.Add(paramCardNumber);

            SqlParameter paramCardHistoryID = new SqlParameter("@intCardStatusHistoryID", SqlDbType.Int, 4);
            paramCardHistoryID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(paramCardHistoryID);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();

                cardStatusHistoryID = (int)paramCardHistoryID.Value;
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>24-Nov-2010</created>
        /// <summary>
        /// Reverse PVV of Card
        /// </summary>
        ///<param name="cardNumber">Card Number</param>
        internal void ReverseCardPVV(string cardNumber)
        {
            SqlCommand myCommand = new SqlCommand("spReverseCardPVV");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardNumber = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 150);
            paramCardNumber.Value = cardNumber;
            myCommand.Parameters.Add(paramCardNumber);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>24-Nov-2010</created>
        /// <summary>
        /// Reverse Card Info
        /// </summary>
        ///<param name="cardNumber">Card Number</param>
        internal void ReverseCardInfo(string cardNumber)
        {
            SqlCommand myCommand = new SqlCommand("spReverseCardInfo");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardNumber = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 150);
            paramCardNumber.Value = cardNumber;
            myCommand.Parameters.Add(paramCardNumber);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        ///// <author>Keyur Parekh</author>
        ///// <created>20-Jan-2012</created>
        ///// <summary>
        ///// Reset Failed Attempt to max of Card
        ///// </summary>
        /////<param name="cardNumber">Card Number</param>
        //internal void ResetFailedPinAttemptOfCard(string cardNumber)
        //{
        //    SqlCommand myCommand = new SqlCommand("spResetFailedPINAttemptOfCard");
        //    myCommand.CommandType = CommandType.StoredProcedure;

        //    SqlParameter paramCardNumber = new SqlParameter("@strEncPAN", SqlDbType.NVarChar, 150);
        //    paramCardNumber.Value = cardNumber;
        //    myCommand.Parameters.Add(paramCardNumber);

        //    try
        //    {
        //        this.Controller.AddCommand(myCommand);
        //        myCommand.ExecuteNonQuery();
        //    }
        //    finally
        //    {
        //        myCommand.Dispose();
        //    }
        //}

        /// <author>Keyur Parekh</author>
        /// <created>19-Mar-2013</created>
        /// <summary>
        /// Update process Status of Card History
        /// </summary>
        /// <param name="cardStatusHistoryID"></param>
        /// <param name="processStatusID"></param>
        internal void UpdateCardStatusHistory(int cardStatusHistoryID, int processStatusID)
        {
            SqlCommand myCommand = new SqlCommand("spUpdateCardStausHistory");
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter paramCardStatusHistoryID = new SqlParameter("@intCardStatusHistoryID ", SqlDbType.Int, 4);
            paramCardStatusHistoryID.Value = cardStatusHistoryID;
            myCommand.Parameters.Add(paramCardStatusHistoryID);

            SqlParameter paramProcessStatusID = new SqlParameter("@intProcessStatusID", SqlDbType.Int, 4);
            paramProcessStatusID.Value = processStatusID;
            myCommand.Parameters.Add(paramProcessStatusID);



            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }


        /// <author>Mantan Bhatti</author>
        /// <created>04-Mar-2020</created>
        /// <summary>
        /// Update Pinunblock Attempt
        /// </summary>
        /// <param name="cardID">int</param>
        /// <param name="FraudulentReasonID">int</param>
        internal void UpdateInValidExpiryDateCount(int cardID, Int16 currentCardStatus)
        {

            SqlCommand myCommand = new SqlCommand("spUpdateInValidExpiryDateCount");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramcardID = new SqlParameter("@intCardID", SqlDbType.Int);
            paramcardID.Value = cardID;
            myCommand.Parameters.Add(paramcardID);

            SqlParameter paramCurrentCardStatus = new SqlParameter("@intCurrentCardStatusID", SqlDbType.SmallInt);
            paramCurrentCardStatus.Value = (Int16)currentCardStatus;
            myCommand.Parameters.Add(paramCurrentCardStatus);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Vipul Patel</author>
        /// <created>04-Mar-2020</created>
        /// <summary>
        /// Notify to card holder
        /// </summary>
        /// <param name="cardID">int</param>
        /// <param name="tokenRequestorID">int</param>
        internal void NotifyTocardHolder(int cardID, int notificationEventID, string remarks)
        {
            SqlCommand myCommand = new SqlCommand("spNotifyToCardHolder");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardID = new SqlParameter("@intCardID", SqlDbType.Int);
            paramCardID.Value = cardID;
            myCommand.Parameters.Add(paramCardID);

            SqlParameter paramEventID = new SqlParameter("@intNotificationEventID", SqlDbType.Int);
            paramEventID.Value = notificationEventID;
            myCommand.Parameters.Add(paramEventID);

            SqlParameter paramRemarks = new SqlParameter("@strRemarks", SqlDbType.NVarChar, 100);
            paramRemarks.Value = remarks;
            myCommand.Parameters.Add(paramRemarks);

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }

        /// <author>Manthan Bhatti</author>
        /// <created>27-Apr-2020</created>
        /// <summary>
        /// Update Card Latest ATC
        /// </summary>
        /// <param name="data">Card</param>
        internal void UpdateCardATC(int cardID, int ATC, int PrevATMPOSTransCurrencyCode = 0)
        {

            SqlCommand myCommand = new SqlCommand("spUpdateCardATC");
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter paramCardID = new SqlParameter("@intCardID", SqlDbType.Int);
            paramCardID.Value = cardID;
            myCommand.Parameters.Add(paramCardID);

            if (ATC > 0)
            {
                SqlParameter paramATC = new SqlParameter("@intATC", SqlDbType.Int);
                paramATC.Value = ATC;
                myCommand.Parameters.Add(paramATC);
            }

            if (PrevATMPOSTransCurrencyCode > 0)
            {
                SqlParameter paramPrevATMPOSTransCurrencyCode = new SqlParameter("@intPrevATMPOSTransCurrencyCode", SqlDbType.Int);
                paramPrevATMPOSTransCurrencyCode.Value = PrevATMPOSTransCurrencyCode;
                myCommand.Parameters.Add(paramPrevATMPOSTransCurrencyCode);
            }

            try
            {
                this.Controller.AddCommand(myCommand);
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                myCommand.Dispose();
            }
        }
    }
}