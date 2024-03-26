using BuildingBlocks.Core.Domain.Entities;

namespace BuildingBlocks.Dapper.Extensions.Mappers
{
    public class ModelMapper<TEntity, TPrimaryKey> : PluralizedClassMapper<TEntity>
        where TEntity : EntityBase<TPrimaryKey>
    {
        public ModelMapper()
        {
            Type type = typeof(TEntity);
            Table(type.Name);
        }
    }
}
