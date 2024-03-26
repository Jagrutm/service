using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace CredECard.Common.BusinessService
{
    [Serializable()]
    public static class CommonMessage
    {
        /// <summary>This method return error message based on error number passed
        /// </summary>
        /// <param name="errorEnum"> It is EnumerrorConstants
        /// </param>
        /// <returns>it returns string value.
        /// </returns>
        public static string GetMessage(EnumErrorConstants errorEnum)
        {
            return GetMessage((int)errorEnum);
        }
        /// <summary>
        /// Gets message string from the resource file based on the key provided
        /// </summary>
        /// <param name="keyName">string</param>
        /// <returns>string</returns>
        public static string GetMessage(string keyName)
        {

            string Language = "en-US";

            Language = CultureInfo.CurrentCulture.Name;

            //Get current culture
            CultureInfo cinfo = CultureInfo.CreateSpecificCulture(Language);
            switch (cinfo.Name.Trim())
            {
                case "en-GB"://UK english
                case "en-US"://US english
                    Thread.CurrentThread.CurrentCulture = cinfo;
                    break;
                default:
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Language);
                    break;
            }

            //ResourceManager resx = new ResourceManager("CredECard.Common" + "." + "Message" + "." + cinfo.Name.Trim(),
            ResourceManager resx = new ResourceManager("Common.CommonMessage" + "." + "Message" + "." + cinfo.Name.Trim(),
                (System.Reflection.Assembly)System.Reflection.Assembly.GetExecutingAssembly().GetSatelliteAssembly(cinfo));

            return resx.GetString(keyName);

        }
        public static string GetMessage(int errorNumber, params string[] args)
        {
            string message = GetMessage(errorNumber);
            string formatedString = String.Format(message, args);
            return formatedString;
        }

        public static string GetMessage(EnumErrorConstants errorEnum, params string[] args)
        {
            int errorNumber = (int)errorEnum;
            return GetMessage(errorNumber, args);
        }

        /// <summary> This method return error message based on error number passed
        /// <param name="errorNumber"> it is a error number.
        /// </param>
        /// <returns> it returns Error message. 
        /// </returns>
        public static string GetMessage(int errorNumber)
        {
            // Get a private key from resource file.
            ResourceManager rm = new ResourceManager("Common.CommonMessage", Assembly.GetExecutingAssembly());
            string message = string.Empty;

            message = rm.GetString(errorNumber.ToString());

            if (!string.IsNullOrEmpty(message))
            {
                int indexPipe = 0;
                int.TryParse(message.IndexOf('|').ToString(), out indexPipe);

                message = message.Substring(indexPipe + 1);
            }
            return message;
        }
    }

    
}
