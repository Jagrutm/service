namespace BuildingBlocks.Core.Domain.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

        int Version { get; set; }
    }
}
