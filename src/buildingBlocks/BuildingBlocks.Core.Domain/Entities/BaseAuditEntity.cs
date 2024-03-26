namespace BuildingBlocks.Core.Domain.Entities
{
    public abstract class BaseAuditEntity<TPrimaryKey> : EntityBase<TPrimaryKey>
    {
        public DateTime CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;

        public string? LastModifiedBy { get; set; }
    }
}
