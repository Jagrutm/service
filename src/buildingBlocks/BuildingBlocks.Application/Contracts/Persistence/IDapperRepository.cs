using BuildingBlocks.Core.Domain.Entities;
using SqlKata;

namespace BuildingBlocks.Application.Contracts.Persistence
{
    public interface IDapperRepository<TEntity, TPrimaryKey> 
        where TEntity : class, IEntity<TPrimaryKey>
    {
        string TableName { get; }

        Task CreateAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<List<TEntity>> GetAsync();

        Task<List<TEntity>> GetByQueryAsync(Query query);

        Task<TEntity> GetByIdAsync(TPrimaryKey id);

        Task<TEntity> GetSingleOrDefaultAsync(Query query, object param);
    }
}
