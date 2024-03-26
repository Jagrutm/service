using AgencyService.Domain.Enums;
using BuildingBlocks.Core.Domain.Entities;

namespace AgencyService.Domain.Entities
{
    public class Certificate : BaseAuditEntity<int>
    {
        public int AgencyId { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsRevoked { get; set; } = false;

        public string? RevokeReason { get; set; }

        public CertificateType Type { get; set; }
    }
}
