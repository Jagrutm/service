using BuildingBlocks.Core.Domain.Entities;
using SqlKata;

namespace BuildingBlocks.Infrastructure.Repositories
{
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        string TableName { get; }

        Task CreateAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<IList<TEntity>> FindAsync();

        Task<TEntity> FindByIdAsync(TPrimaryKey id);

        Task<TEntity> FindSingleOrDefaultAsync(Query query, object param);
    }
}
