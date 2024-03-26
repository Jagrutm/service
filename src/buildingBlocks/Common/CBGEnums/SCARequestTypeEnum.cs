using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Common.CBGEnums
{
    public enum EnumSCARequestType : short
    {
        None = 0,

        [Description("Beneficiary Status Change"), XmlEnum("1")]
        Edit_Beneficiary_Status = 1,

        [Description("Add Standing Order"), XmlEnum("2")]
        Add_Standing_Order = 2,

        [Description("Send Money"), XmlEnum("3")]
        Send_Money = 3,

        [Description("Account to Bank Transfer"), XmlEnum("4")]
        Bank_Transfer = 4,

        [Description("Account to Account Transfer"), XmlEnum("5")]
        Internal_Or_Third_Party_Transfer = 5,

        [Description("Login"), XmlEnum("6")]
        Login_Attempt = 6,

        [Description("Pay Request Money"), XmlEnum("7")]
        Pay_Request_Money = 7,

        [Description("Save"), XmlEnum("8")]
        Save = 8,

        [Description("Pay your fees"), XmlEnum("9")]
        Pay_Your_Fees = 9,

        [Description("Change Address"), XmlEnum("10")]
        Change_Address = 10,

        [Description("Update Third Party Account Details"), XmlEnum("11")]
        Update_Third_Party = 11,

        [Description("Change Mobile Number"), XmlEnum("12")]
        Change_Mobile_Number = 12,

        [Description("Historic Transactions"), XmlEnum("13")]
        Historical_Transactions = 13,

        [Description("Edit Standing Order"), XmlEnum("14")]
        Edit_Standing_Order = 14,

        [Description("Setup New Bank"), XmlEnum("15")]
        Setup_Bank = 15,

        [Description("Withdraw Money"), XmlEnum("16")]
        Withdraw_Money = 16,

        [Description("Update Contact Details - Mobile number and Address"), XmlEnum("17")]
        Update_Contact_Details_Mobile_And_Address = 17,

        [Description("3DS"), XmlEnum("18")]
        ThreeDS = 18,

        [Description("Token Provisioning"), XmlEnum("19")]
        Token_Provisioning = 19,

        [Description("CH Portal Search Transactions"), XmlEnum("20")]
        CH_Portal_Search_Transaction = 20,

        [Description("Authorisation"), XmlEnum("21")]
        Authorisation = 21,

        [Description("RegisterDevice"), XmlEnum("22")]
        RegisterDevice = 22,

        [Description("RegisterAccount"), XmlEnum("24")]
        RegisterAccount = 24
    }

    public enum EnumSCAOptionType : byte
    {
        None = 0,

        [Description("Device Binding (Possession)"), XmlEnum("1")]  // Pulin Case 117150
        ApproveRejectButton = 1,

        [Description("Face (Inherence)"), XmlEnum("2")]         // Pulin Case 117150
        Face = 2,

        [Description("Finger (Inherence)"), XmlEnum("3")]       // Pulin Case 117150
        Finger = 3,

        [Description("OTP (Possession)"), XmlEnum("4")]         // Pulin Case 117150
        OTP = 4,

        [Description("mPIN (Knowledge)"), XmlEnum("5")]         // Pulin Case 117150
        mPIN = 5,

        [Description("Password (Knowledge)"), XmlEnum("6")]     // Pulin Case 117150
        Password = 6,

        [Description("Other Knowledge Item"), XmlEnum("7")]     // Pulin Case 117150
        OtherKnowledgeItem = 7,

        [Description("Other Possession Item"), XmlEnum("8")]    // Pulin Case 117150
        OtherPossessionItem = 8,

        [Description("Other Inherence Item"), XmlEnum("9")]     // Pulin Case 117150
        OtherInherenceItem = 9,

    }
}
