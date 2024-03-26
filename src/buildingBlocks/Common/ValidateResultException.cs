using CredECard.Common.BusinessService;
using System;
using System.ComponentModel.DataAnnotations;

namespace Common.CustomException
{
    [Serializable]
    public class ValidateResultException : ValidationException
    {
        public ValidateResultException(ValidateResult validateResult) : this(validateResult, 0)
        {
        }

        public ValidateResultException(EnumErrorConstants errorCode) : this(new ValidateResult(errorCode), 0)
        {
        }

        public ValidateResultException(EnumErrorConstants errorCode, int statusCode) : this(new ValidateResult(errorCode), statusCode)
        {
        }

        public ValidateResultException(EnumErrorConstants errorCode, EnumFieldErrorConstants fieldError) : this(new ValidateResult(errorCode, fieldError), 0)
        {
        }

        public ValidateResultException(EnumErrorConstants errorCode, EnumFieldErrorConstants fieldError, int statusCode) : this(new ValidateResult(errorCode, fieldError), statusCode)
        {
        }

        public ValidateResultException(ValidateResult validateResult, int statusCode)
        {
            this.Result = validateResult;
            if (statusCode > 0)
            {
                this.StatusCode = statusCode;
            }
            else
            {
                this.StatusCode = 400; //StatusCodes.Status400BadRequest;
            }
        }

        public int StatusCode
        {
            get;
            private set;
        }

        public ValidateResult Result
        {
            get;
            private set;
        }

        public override string Message
        {
            get
            {
                return this.Result.ToString();
            }
        }
    }
}