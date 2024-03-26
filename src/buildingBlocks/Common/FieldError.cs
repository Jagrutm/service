using System;
using System.Collections.Generic;
using System.Text;

namespace CredECard.Common.BusinessService
{
    [Serializable]
    public class FieldError
    {
        public string FieldName { get; set; } = string.Empty;

        public string ErrorDescription { get; set; } = string.Empty;

        public int ErrorNumber { get; set; } = -1;

        public string[] ArgumentValues { get; set; } = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorNumber">int</param>
        /// <param name="fieldName">string</param>
        /// <param name="errorDescription">string</param>
        public FieldError(int errorNumber, string fieldName, string errorDescription)
        {
            ErrorNumber = errorNumber;
            FieldName = fieldName;
            ErrorDescription = errorDescription;
        }

        public FieldError( string fieldName, string errorDescription)
        {            
            FieldName = fieldName;
            ErrorDescription = errorDescription;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorNumber">int</param>
        /// <param name="fieldName">string</param>
        /// <param name="errorDescription">string</param>
        public FieldError(int errorNumber, string fieldName, string errorDescription, params string[] args)
        {
            ErrorNumber = errorNumber;
            FieldName = fieldName;
            ErrorDescription = errorDescription;
            ArgumentValues = args;
        }
       

        /// <summary>
        /// Gets the string description for the error
        /// </summary>
        /// <returns>string</returns>
		public override string ToString()
        {
            //string output = "\"" + FieldName + "\"" + " " + ErrorNumber;
            //return output;

            string output = string.Empty;

            int indexPipe = 0;  //Dharati Metra: Case 46457
            int.TryParse(ErrorDescription.IndexOf('|').ToString(), out indexPipe);

            if (!string.IsNullOrEmpty(FieldName))
                output = "\"" + FieldName + "\"" + " " + ErrorDescription.Substring(indexPipe + 1);
            else
                output = ErrorDescription.Substring(indexPipe + 1);
            return output;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>25-Sep-2018</created>
        /// <summary>To the simple string.</summary>
        /// <returns></returns>
        public string ToSimpleString()
        {
            string output = string.Empty;

            int indexPipe = 0;  //Dharati Metra: Case 46457
            int.TryParse(ErrorDescription.IndexOf('|').ToString(), out indexPipe);

            if (!string.IsNullOrEmpty(FieldName))
                output = FieldName + " " + ErrorDescription.Substring(indexPipe + 1);
            else
                output = ErrorDescription.Substring(indexPipe + 1);
            return output;
        }

        /// <summary>
        /// Gets the string description with or without error number
        /// </summary>
        /// <param name="displayWithErrorNo">bool</param>
        /// <returns>string</returns>
        public string ToString(bool displayWithErrorNo)
        {
            string output = string.Empty;
            string subErrorCode = string.Empty;

            int indexPipe = 0;
            int.TryParse(ErrorDescription.IndexOf('|').ToString(), out indexPipe);

            //Dharati Metra: Case 46457 - Show SubErrorCode in the message. 
            //Now, "displayWithErrorNo" parameter is used for SubErrorCode 
            if (displayWithErrorNo && indexPipe > 0)
                subErrorCode = ErrorDescription.Substring(0, indexPipe + 1);

            if (!string.IsNullOrEmpty(FieldName))
                output = subErrorCode + "\"" + FieldName + "\"" + " " + ErrorDescription.Substring(indexPipe + 1);
            else
                output = subErrorCode + ErrorDescription.Substring(indexPipe + 1);

            return output;
        }

    }
}
