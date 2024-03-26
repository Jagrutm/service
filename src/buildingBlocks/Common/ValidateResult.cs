using CredECard.Common.BusinessService;
using CredECard.Common.Enums.EnumErrorDisplay;
using System;
using System.Collections.Generic;
using System.Text;

namespace CredECard.Common.BusinessService
{
    [Serializable]
    public class ValidateResult
    {
        private FieldErrorList _errorList = null;
        private bool _showErrorMessages = false;
        /// <author>Rikunj Suthar</author>
        /// <created>03-May-2021</created>
        /// <summary>
        /// To return empty validate result insted of null initialisation.
        /// </summary>
        public static ValidateResult Empty
        {
            get
            {
                return new ValidateResult(new FieldErrorList());
            }
        }

        /// <summary>
        /// Constructor initialises new instance with List or erros
        /// </summary>
        /// <param name="errorList">FieldErrorList</param>
        public ValidateResult(FieldErrorList errorList)
        {
            _errorList = errorList;
            //_showErrorMessages = true; // 23-Nov-2015 : Bhavik/JB - 24678
        }

        public ValidateResult(EnumErrorConstants errorCode) : this(errorCode, null)
        {
        }
        public ValidateResult(EnumErrorConstants errorCode, EnumFieldErrorConstants? fieldErrorConstant)
        {
            if (_errorList == null)
                _errorList = new FieldErrorList();

            int errorNumber = (int)errorCode;
            string fieldName = string.Empty;
            string errorDescription = CommonMessage.GetMessage(errorCode);
            if (fieldErrorConstant.HasValue)
                fieldName = FieldErrorMessage.GetFieldErrorMessage(fieldErrorConstant.Value);

            var fieldError = new FieldError(errorNumber, fieldName, errorDescription);

            _errorList.Add(fieldError);
        }

        /// <summary>
        /// Gets the list of fielderrors
        /// </summary>
        /// <value>FieldErrorList</value>
        public FieldErrorList ErrorList
        {
            get
            {
                if (_errorList == null) _errorList = new FieldErrorList();
                return _errorList;
            }
        }

        /// <summary>
        /// Gets whether any errors were found
        /// </summary>
        /// <value>bool</value>
        public bool DataOk
        {
            get { return (_errorList == null || _errorList.Count == 0); }
        }

        /// <summary>
        /// Method gets string representation for all the errors in the object
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (_errorList == null || _errorList.Count == 0) return string.Empty;
            foreach (FieldError singleError in _errorList)
            {
                builder.AppendFormat("{0}\r\n", singleError.ToString());
            }
            return builder.ToString();
        }


        /// <author>Prashant Soni</author>
        /// <created>10-Dec-2007</created>
        /// <summary>Get or Set.Assign value only if error message not wants to display while DataOK is false.
        /// </summary>
        /// <value>Boolean
        /// </value>
        public bool IsShowErrorMessage
        {
            get
            {
                return _showErrorMessages;
            }
            set
            {
                _showErrorMessages = value;
            }
        }

        public string ToString(bool displayWithErrorNo)
        {
            StringBuilder builder = new StringBuilder();
            if (_errorList == null || _errorList.Count == 0) return string.Empty;
            if (displayWithErrorNo)
            {
                foreach (FieldError singleError in _errorList)
                {
                    builder.AppendFormat("{0}\r\n", singleError.ToString(true));
                }
            }
            else
            {
                foreach (FieldError singleError in _errorList)
                {
                    builder.AppendFormat("{0}\r\n", singleError.ToString());
                }
            }
            return builder.ToString();
        }

        /// <author>Keyur Parekh</author>
        /// <created>10-Feb-2009</created>
        /// <summary>
        /// Display error result as string
        /// </summary>
        /// <param name="errorDisplay">EnumErrorDisplay</param>
        /// <returns>string</returns>
        public string ToString(EnumErrorDisplay errorDisplay)
        {
            string message = string.Empty;
            switch (errorDisplay)
            {
                case EnumErrorDisplay.None:
                    message = this.ToString();
                    break;
                case EnumErrorDisplay.DisplayWithErrorNo:
                    message = this.ToString(true);
                    break;
                case EnumErrorDisplay.DisplayWithFieldName:
                    message = displayWithFieldName();
                    break;
                case EnumErrorDisplay.DisplayFieldNameOnly:
                    message = displayFieldNameOnly();
                    break;
            }

            return message;
        }

        /// <author>Keyur Parekh</author>
        /// <created>10-Feb-2009</created>
        /// <summary>
        /// Display Discription Only
        /// </summary>
        /// <returns></returns>
        private string displayWithFieldName()
        {
            StringBuilder builder = new StringBuilder();

            if (_errorList == null || _errorList.Count == 0) return string.Empty;

            foreach (FieldError singleError in _errorList)
            {
                string msg = singleError.FieldName + (singleError.FieldName == string.Empty ? "" : " - " + singleError.ErrorDescription);
                builder.AppendFormat("{0}", msg + "\r\n");
            }

            return builder.ToString();
        }

        /// <author>Keyur Parekh</author>
        /// <created>10-Feb-2009</created>
        /// <summary>
        /// Display Fields name only
        /// </summary>
        /// <returns></returns>
        private string displayFieldNameOnly()
        {
            StringBuilder builder = new StringBuilder();

            if (_errorList == null || _errorList.Count == 0) return string.Empty;
            bool flag = false;
            foreach (FieldError singleError in _errorList)
            {
                if (flag) builder.Append(",");
                builder.Append(singleError.FieldName);
                flag = true;
            }

            return builder.ToString();
        }


    }
}
