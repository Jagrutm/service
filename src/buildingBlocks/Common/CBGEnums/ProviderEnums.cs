using System;
namespace CredECard.Common.Enums.Provider
{
    public enum EnumProviderModule : int
    {
        None = 0
        , BACS_Inbound_STD18_PAYMENTS = 1
        , BACS_Inbound_STD18_AUDDIS = 2
        , BACS_Outbound_Standard18_AUDDIS_Return = 3 //DDI rejection = AUDDIS bank return
        , BACS_Outbound_Standard18_ADDACS = 4 //(DDI amendment/cancellation)
        , BACS_Outbound_Standard18_AWACS = 5 //(Adive of wrong account credit)
        , BACS_Outbound_Standard18_ARUDD = 6 //(Automated return of unpaid dd payment)
        , BACS_Outbound_Standard18_ARUCS = 7 //(Automated return of unapplied direct credit)        
        , BACS_Inbound_STD18_ADDACS = 8
        , BACS_Inbound_STD18_FILES_RESPONSE = 9
        , BACS_Outbound_Standard18_Response = 10
        , FPS_Outbound_MIDEP_Files = 11   //FPS MIDEP file (MFT)
    }

    public enum EnumScheduleType
    {
        None = 0,
        Standing_Order = 1,
        Monthly_Account_Fee = 2
    }

    //public enum EnmWeekDay
    //{
    //    None = 0,
    //    Monday = 32,
    //    Tuesday = 64,
    //    Wednesday = 128,
    //    Thursday = 256,
    //    Friday = 512,
    //    Saturday = 1024,
    //    Sunday = 2048
    //}

    //[Flags()]
    //public enum enmScheduleFrequency
    //{
    //    Daily = 1,
    //    Weekly = 2,
    //    Monthly = 3
    //}

    public enum EnumFileTypes
    {
        None = 0,
        Requested = 1,
        Issued = 2,
        Exception = 3,
        PersonlizeRequest = 4,
        PersonlizeResponse = 5,
        ClearingExport = 6,
        Visa_Transaction = 7,
        Exchange_Rate = 8,
        Merchant_Reference_File = 9,
        Initial_Data_load_File = 10,
        Initial_Data_load_Response_File = 11,
        Card_Updater_Request_File = 12,
        Card_Updater_Response_File = 13,
        Stop_Advice_Request_File = 14,
        Stop_Advice_Response_File = 15,
        AuthExport = 16,
        ThreeDS_Transaction_File = 17, //86131 vipul
        Card_Updater_Report_Response_File = 18 //105293 : Manthan Bhatti
    }

    /// <author>Vipul patel</author>
    /// <created>20-Feb-2017</created>
    /// <summary>Record Type Enum
    /// </summary>
    public enum EnumRecordType
    {
        Header = 2,
        AuthAdv = 21,
        AuthRev = 22,
        FinAdv = 23,
        FinRev = 24,
        RepRes = 25,
        RepResRev = 26,
        Return = 27,
        Chargeback = 28, //Darshit Desai : 54349
        ChargeBackRev = 29, //Darshit Desai : 54349
        RFC = 30
    /*
        * NOTE: ROLLBACKED by Aarti Meswania 29-May-2019. Right now R2 is being treated as chargeback-reversal, 
        * this case can start treating them as pre-arbitrary. Until UK team get clarity on functional flow they are not willing to live this case.*/
    //Aarti Meswania: Case 87550 DAF V1337 - pre-arbitration - starts
    /* When merchant wins chargeback; and we had already refunded cardholder's money. ultimately either contis or contis-client will bare the loss. 
       If loss is huge then we(contis) raise court-case to avoid baring the loss (case filed either through VISA Justice-Panel or through third-party court) 
        //, PreArbitary = 31//When contis(UK-team) raise court-case then will manually report it to VISA using VROL(Visa's site). --> "R2" (Dispute status)
        //, PreArbitaryResponse = 33//for e.g. contis withdraws case or merchant accepts liablity case ends. If it is not the scenerio then court will consider case for hearings and will take decision. --> "L2" (Dispute status)
        //, PreArbitaryDecision = 32//Indicates decision of court-case. --> "R3" (Dispute status)
        //, PreArbitaryDecisionResponse = 34//for e.g. court-case decision have been rolled-back/considered-null&void. --> "L3" (Dispute status)
        //Aarti Meswania: Case 87550 DAF V1337 - pre-arbitration - overs
    */
    }

    /// <author>Niken Shah</author>
    /// <created>14-Sep-2020</created>
    /// <summary>File Type Field Enum
    /// </summary>
    public enum EnumFileTypeFieldValue
    {
        None = 0,
        CountryName = 1,
        CountryA2Code = 2
    }
}
