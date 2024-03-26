using AgencyService.Domain.Enums;
using BuildingBlocks.Core.Domain.Entities;

namespace AgencyService.Domain.Entities
{
    public class SortCode : BaseAuditEntity<int>
    {
        public int AgencyId { get; set; }

        public string Value { get; set; }

        public string? DisplaySortCode { get; set; }

        public int AccountNumberSize { get; set; }

        public CheckSumLogicType ChecksumLogic { get; set; }

        public string Weightage { get; set; }

        public bool? IsReachable { get; set; }

        public string? BIC { get; set; }

        public string? BankId { get; set; }

        public bool IsActive { get; set; }
    }
}
