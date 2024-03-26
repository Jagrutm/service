using AgencyService.Domain.Enums;
using BuildingBlocks.Core.Domain.Entities;

namespace AgencyService.Domain.Entities
{
    public class Maintenance : BaseAuditEntity<int>
    {
        public int? AgencyId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public QualifiedAcceptanceCode ResponseCode { get; set; }
    }
}
