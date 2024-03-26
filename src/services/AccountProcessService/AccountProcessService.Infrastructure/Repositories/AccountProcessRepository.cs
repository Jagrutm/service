using AccountProcessService.Application.Contracts.Persistence;
using AccountProcessService.Domain.Entities;
using BuildingBlocks.Aws.DynamoDb.Infrastructure.Contexts;
using BuildingBlocks.Infrastructure.Repositories;

namespace AccountProcessService.Infrastructure.Repositories
{
    public class AccountProcessRepository : DynamoDbRepositoryBase, IAccounProcessRepository
    {
        public AccountProcessRepository(IDynamoDbContext context) : base(context)
        {

        }

        public async override Task<TEntity> LoadAsync<TEntity>(object hashKey, CancellationToken cancellationToken = default)
        {
            return await base.LoadAsync<TEntity>(hashKey, cancellationToken);
        }

        public async Task CreateAsync(Guid agencyId, List<Account> accounts, CancellationToken cancellationToken = default)
        {
            var bookBatch = base.CreateBatchWrite<Account>();
            bookBatch.AddPutItems(accounts);
            await bookBatch.ExecuteAsync(cancellationToken);
        }

        public async override Task SaveAsync<TEntity>(TEntity value, CancellationToken cancellationToken = default)
        {
            await base.SaveAsync(value, cancellationToken);
        }

        public async override Task DeleteAsync<TPrimaryKey>(TPrimaryKey value, CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(value, cancellationToken);
        }
    }
}
