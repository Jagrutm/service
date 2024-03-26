using Amazon.DynamoDBv2.DataModel;

namespace BuildingBlocks.Aws.DynamoDb.Application.Contracts.Persistence
{
    public interface IDynamoDbRepository
    {
        Task<T> LoadAsync<T>(object hashKey, CancellationToken cancellationToken = default);

        Task<T> LoadAsync<T>(T keyObject, CancellationToken cancellationToken = default);

        Task SaveAsync<T>(T value, CancellationToken cancellationToken = default);

        Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default);

        Task DeleteAsync<T>(object hashKey, CancellationToken cancellationToken = default);

        BatchWrite<TEntity> CreateBatchWrite<TEntity>();
    }
}
