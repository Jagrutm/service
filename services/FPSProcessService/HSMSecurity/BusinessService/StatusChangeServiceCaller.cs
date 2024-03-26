using System;
//using ContisGroup.CardUtil.CardStatusChangeService;
using CredECard.Common.BusinessService;
using CredECard.Common.Enums.Card;
using CredECard.Common.Enums.Transaction;
using System.Net;

namespace ContisGroup.CardUtil.BusinessService
{

    /// <author>Keyur Parekh</author>
    /// <created>19-Mar-2013</created>
    /// <summary>
    /// This class call the status change service of card issuer and change status
    /// </summary>
    public class StatusChangeServiceCaller
    {
        CardData objCardData = null;
        delegate CallResponse AsyncMethodCaller(CardData objCD, int cardStatusHistoryID, EnumCardStatus cardStatus);
        private string _statusChangeWSURL = string.Empty;

        public StatusChangeServiceCaller(string cardStatusChangeURL)
        {
            _statusChangeWSURL = cardStatusChangeURL;
        }

        /// <author>Keyur Parekh</author>
        /// <created>19-Mar-2013</created>
        /// <summary>Constructor
        /// </summary>
        /// <param name="authData">CardData object
        /// </param>
        public StatusChangeServiceCaller(CardData objCD) 
        {
            objCardData = objCD;
            _statusChangeWSURL = objCD.CardStatusChangeURL;
        }

        ///// <author>Keyur Parekh</author>
        ///// <created>19-Mar-2013</created>
        ///// <summary>This method call third party card status change service.
        ///// </summary>
        ///// <param name="isCardBlock">Bool</param>
        ///// <param name="cardStatusHistoryID">Int</param>
        //public void ChangeCardStatus(int cardStatusHistoryID, EnumCardStatus CardStatus)
        //{
        //    //Async call webservice
        //    AsyncMethodCaller caller = new AsyncMethodCaller(this.CallStatusChangeService);
        //    IAsyncResult result = caller.BeginInvoke(this.objCardData, cardStatusHistoryID, CardStatus, new AsyncCallback(StatusChangeCallbackOut), caller);
        //}


        //public void ChangeCardStatusSync(ContisCardStatusRqstInfo objRequestInfo, int cardStatusHistoryID)
        //{
        //    CardStatusClient service = new CardStatusClient();
        //    service.Endpoint.Address = new System.ServiceModel.EndpointAddress(_statusChangeWSURL);
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    ContisCardStatusRspsInfo objResponse = service.ContisCardStatusChange(objRequestInfo);

        //    if (objResponse != null && cardStatusHistoryID > 0)
        //    {
        //        EnumProcessStatuses processStatus = EnumProcessStatuses.Pending;

        //        if (objResponse.ActionCode == "000")
        //            processStatus = EnumProcessStatuses.Notified;

        //        CardData.UpdateCardStatusHistoryProcessStatus(cardStatusHistoryID, processStatus);
        //    }
        //}

        ///// <author>Keyur Parekh</author>
        ///// <created>19-Mar-2013</created>
        ///// <summary>this method make a call to authservice 
        ///// </summary>
        ///// <param name="objCardData">CardData
        ///// </param>
        ///// <param name="isCardBlock">bool
        ///// </param>
        ///// <param name="cardStatusHistoryID">Int</param>
        ///// <returns>it returns the CallResponse object.
        ///// </returns>
        //private CallResponse CallStatusChangeService(CardData objCardData, int cardStatusHistoryID, EnumCardStatus cardStatus)
        //{
        //    if (objCardData == null) return null;

        //    ContisCardStatusRqstInfo request = new ContisCardStatusRqstInfo();
        //    CallResponse webCallResponse = null;

        //    try
        //    {
        //        request.ProcessorCardID  = objCardData.CardID;
        //        request.StatusCode = EnumUtil.GetEnumDescription(cardStatus);

        //        CardStatusClient service = new CardStatusClient("BasicHttpBinding_ICardStatus");
        //        service.Endpoint.Address = new System.ServiceModel.EndpointAddress(_statusChangeWSURL);
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        ContisCardStatusRspsInfo objResponse = service.ContisCardStatusChange(request);

        //        webCallResponse = new CallResponse();
        //        webCallResponse.ResponseCode = objResponse.ActionCode;
        //        webCallResponse.CardStatusHistoryID = cardStatusHistoryID;

        //    }
        //    catch (Exception ex)
        //    {
        //        if (cardStatusHistoryID > 0)
        //        {
        //            try
        //            {
        //                CardData.UpdateCardStatusHistoryProcessStatus(cardStatusHistoryID, EnumProcessStatuses.Pending);
        //            }
        //            catch(Exception pEx)
        //            {
        //                CredECard.BugReporting.BusinessService.PostToBugscout.PostDataToBugScout(pEx);
        //            }
        //        }

        //        LogError(ex);
        //    }

        //    return webCallResponse;
        //}

        private void LogError(Exception ex)
        {
            //if (ConfigurationManager.AppSettings["logdata"].ToString() == "1")
            //{
            //    SimpleLogWriter write = new SimpleLogWriter(ConfigurationManager.AppSettings["logpath"].ToString());
            //    write.LogEntry(new TCPLogEntry(schemeCardStatusChangeURL, 0, ex.ToString()));
            //}
        }

       
        /// <author>Keyur Parekh</author>
        /// <created>19-Mar-2013</created>
        /// <summary>This method call after async operation of webservice call ended.
        /// </summary>
        /// <param name="result">
        /// </param>
        private void StatusChangeCallbackOut(IAsyncResult result)
        {
            
            AsyncMethodCaller dlgt = (AsyncMethodCaller)result.AsyncState;
            CallResponse response = dlgt.EndInvoke(result);
           
            if (response != null && response.CardStatusHistoryID > 0)
            {
                EnumProcessStatuses processStatus = EnumProcessStatuses.Pending;

                if( response.ResponseCode == "000") 
                    processStatus = EnumProcessStatuses.Notified;
                
                CardData.UpdateCardStatusHistoryProcessStatus(response.CardStatusHistoryID, processStatus);
            }
        }

        /// <author>Keyur Parekh</author>
        /// <created>19-Mar-2013</created>
        /// <summary>
        /// This class manages the response returned from statuschange service call.
        /// </summary>
        class CallResponse
        {
            public string ResponseCode = string.Empty;
            public int CardStatusHistoryID = 0;
        }

        ///// <author>Nidhi Thakrar</author>
        ///// <created>12-Apr-17</created>
        ///// <summary>Checks the eligibility.</summary>
        ///// <param name="objRequestInfo">The object request information.</param>
        //public CheckEligibilityResponseInfo CheckEligibility(CheckEligibilityRequestInfo objRequestInfo)
        //{
        //    CardStatusClient service = new CardStatusClient();
        //    service.Endpoint.Address = new System.ServiceModel.EndpointAddress(_statusChangeWSURL);
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    return service.Card_CheckEligibility(objRequestInfo);
        //}

        ///// <author>Vipul Patel</author>
        ///// <created>18-Mar-19</created>
        ///// <summary>VBV OTP</summary>
        ///// <param name="objRequestInfo">The object request information.</param>
        //public VBVOTPResponseInfo VBV_SendOTP(VBVOTPRequestInfo objRequestInfo)
        //{
        //    CardStatusClient service = new CardStatusClient();
        //    service.Endpoint.Address = new System.ServiceModel.EndpointAddress(_statusChangeWSURL);
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    return service.VBV_SendOTP(objRequestInfo);
        //}

        /*
        /// <author>Manthan Bhatti</author>
        /// <created>25-Nov-2019</created>
        /// <summary>Get OTP Option</summary>
        /// <param name="objRequestInfo">The object request information.</param>
        public VBVOTPResponseInfo VBV_GetOTPOption(VBVOTPRequestInfo objRequestInfo)
        {
            CardStatusClient service = new CardStatusClient();
            service.Endpoint.Address = new System.ServiceModel.EndpointAddress(_statusChangeWSURL);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return service.VBV_GetOTPOption(objRequestInfo);
        }
        */
    }
}
