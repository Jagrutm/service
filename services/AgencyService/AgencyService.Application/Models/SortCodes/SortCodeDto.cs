using AgencyService.Domain.Enums;

namespace AgencyService.Application.Models.SortCodes
{
    public class SortCodeDto 
    {

        public string Value { get; set; }

        public int AccountNumberSize { get; set; }

        public CheckSumLogicType ChecksumLogic { get; set; }

        public string Weightage { get; set; } = "1";

        public bool IsReachable { get; set; } = false;

        public string? BIC { get; set; }

        public bool IsActive { get; set; } = false;
    }
}
