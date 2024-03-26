using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace CredECard.Common.BusinessService
{
    [Serializable]
    public static class FieldErrorMessage
    {        
        ///<summary>GetFieldErrorMessage</summary>
        ///<param name="errorEnum"></param>
        ///<returns></returns>
        public static string GetFieldErrorMessage(EnumFieldErrorConstants errorEnum)
        {
            return GetFieldErrorMessage((int)errorEnum);
        }
                
        ///<summary>GetFieldErrorMessage</summary>
        ///<param name="errorNumber"></param>
        ///<returns></returns>
        public static string GetFieldErrorMessage(int errorNumber)
        {
            // Get a private key from resource file.
            ResourceManager rm = new ResourceManager("Common.FieldErrorMessage", Assembly.GetExecutingAssembly());

            string message = rm.GetString(errorNumber.ToString());
            return message;
        }
                
        ///<summary>GetFieldErrorMessage</summary>
        ///<param name="errorEnum"></param>
        ///<param name="language"></param>
        ///<param name="isAppendSubErrorCode"></param>
        ///<returns></returns>
        public static string GetFieldErrorMessage(EnumFieldErrorConstants errorEnum, string languageCode, bool isAppendSubErrorCode = false)
        {
            ResourceManager resx = new ResourceManager("Common.FieldErrorMessage", Assembly.GetExecutingAssembly());

            string message = resx.GetString(Convert.ToString((int)errorEnum), CultureInfo.CreateSpecificCulture(languageCode));
            return message;
        }
    }
}
