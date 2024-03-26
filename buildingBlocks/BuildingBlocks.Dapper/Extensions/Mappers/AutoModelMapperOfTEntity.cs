using BuildingBlocks.Core.Domain.Entities;

namespace BuildingBlocks.Dapper.Extensions.Mappers
{
    public class AutoModelMapper<IEntity> : ModelMapper<IEntity, int> where IEntity : EntityBase<int>
    {
        public AutoModelMapper()
        {
            Map(_ => _.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }

    public class AutoModelMapperOfGuidPrimaryKey<IEntity> : ModelMapper<IEntity, Guid> where IEntity : EntityBase<Guid>
    {
        public AutoModelMapperOfGuidPrimaryKey()
        {
            Map(_ => _.Id).Key(KeyType.Guid);
            AutoMap();
        }
    }
}
