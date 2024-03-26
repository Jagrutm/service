using System;
using System.Security.Cryptography;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Text;

namespace ContisGroup.MessageParser.ISO8586Parser
{
	/// <author>Prashant Soni</author>
	/// <created>17-Feb-2006</created>
	/// <summary>Error message constants..
	/// </summary>
	public enum EnumErrorNumber
	{
		SUCCESS = 0,
        ACCOUNT_NOT_FOUND=1,
        TRANSACTION_NOT_ALLOWED =2,
        INVALID_MESSAGE_FORMAT=3,
        FIELD_NOT_EXIST=4,
        CURRENCY_NOT_SUPPORTED =5,
        UNABLE_TO_PROCESS = 6,
        SYSTEM_ERROR = 7,
        PROVIDER_INSUFFICIENT_ACCOUNT_BALANCE = 9,
        BARCODE_TOPUP_NOT_ALLOWED = 12,
        CARD_TOPUP_NOT_ALLOWED = 13,
        ONLINE_TOPUP_NOT_ALLOWED = 14,
        NO_TRANSACTION_DONE = 15,
        SETTLEMENT_PROCESSED = 17,
        CARD_NOT_ACTIVE = 18

	}

    /// <author>Prashant Soni</author>
    /// <created>17-Feb-2006</created>
    /// <summary>This class return common error messages based on parameter passed
    /// </summary>
    /// 
    [Serializable()]
    public class MessageResource
    {
        public static string GetMessage(int errorNumber, params string[] args)
        {
            ResourceManager rm = new ResourceManager("MastercardParser.ErrorMessages", Assembly.GetExecutingAssembly());
            string message = rm.GetString(errorNumber.ToString()); ;
            string formatedString = String.Format(message, args);
            return formatedString;
        }

        public static string GetMessage(EnumErrorNumber errorEnum, params string[] args)
        {
            return GetMessage((int)errorEnum, args);
        }
    }
}
