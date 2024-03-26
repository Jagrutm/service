using BuildingBlocks.Core.Domain.Entities;

namespace BuildingBlocks.Dapper.DataModels
{
    public abstract class DataModelBase : DataModelBase<long>, IEntity<long>
    {
    }

    public abstract class DataModelBase<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }

        public int Version { get; set; }
    }
}
