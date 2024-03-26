using CredECard.Common.BusinessService;

namespace BaseAPIController.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string FieldName { get; set; }

        public ErrorResponse(FieldError fieldError)
        {
            this.ErrorCode = fieldError.ErrorNumber;
            this.ErrorMessage = fieldError.ErrorDescription;
            this.FieldName = fieldError.FieldName;
        }

        public ErrorResponse(int errorNumber, string fieldName, string errorDescription)
        {
            this.ErrorCode = errorNumber;
            FieldName = fieldName;
            this.ErrorMessage = errorDescription;
        }
    }
}
