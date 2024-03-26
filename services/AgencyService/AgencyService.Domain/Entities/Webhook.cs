using BuildingBlocks.Core.Domain.Entities;
namespace AgencyService.Domain.Entities
{
    public class Webhook : BaseAuditEntity<int>
    {
        public int AgencyId { get; set; }

        public int Type { get; set; }

        public string Url { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
