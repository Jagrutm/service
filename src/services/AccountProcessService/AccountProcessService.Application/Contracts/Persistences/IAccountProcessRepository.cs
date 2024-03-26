using AccountProcessService.Domain.Entities;
using BuildingBlocks.Aws.DynamoDb.Application.Contracts.Persistence;

namespace AccountProcessService.Application.Contracts.Persistence
{
    public interface IAccounProcessRepository : IDynamoDbRepository
    {
        Task CreateAsync(Guid agencyId, List<Account> accounts, CancellationToken cancellationToken = default);
    }
}
