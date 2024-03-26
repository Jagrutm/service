using Amazon.DynamoDBv2.DataModel;
using BuildingBlocks.Aws.DynamoDb.Application.Contracts.Persistence;

namespace BuildingBlocks.Infrastructure.Repositories
{
    public abstract class DynamoDbRepositoryBase : IDynamoDbRepository
    {
        public DynamoDbRepositoryBase(IDynamoDBContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IDynamoDBContext Context { get; }

        public async virtual Task<T> LoadAsync<T>(object hashKey, CancellationToken cancellationToken = default)
        {
            return await Context.LoadAsync<T>(hashKey, cancellationToken);
        }

        public async virtual Task<T> LoadAsync<T>(T keyObject, CancellationToken cancellationToken = default)
        {
            return await Context.LoadAsync(keyObject, cancellationToken);
        }

        public async virtual Task SaveAsync<T>(T value, CancellationToken cancellationToken = default)
        {
            await Context.SaveAsync(value, cancellationToken);
        }

        public async virtual Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default)
        {
            await Context.DeleteAsync(value, cancellationToken);
        }

        public async virtual Task DeleteAsync<T>(object hashKey, CancellationToken cancellationToken = default)
        {
            await Context.DeleteAsync(hashKey, cancellationToken);
        }

        public virtual BatchWrite<TEntity> CreateBatchWrite<TEntity>()
        {
            return Context.CreateBatchWrite<TEntity>();
        }
    }
}
