using AccountProcessService.Application.Models;
using AccountProcessService.Domain.Entities;

namespace AccountProcessService.Application.Contracts.Services
{
    public interface IAccountProcessService
    {
        Task<TEntity> LoadAsync<TEntity>(object hashKey, CancellationToken cancellationToken = default);

        Task CreateAsync(Guid agencyId, List<Account> accounts, CancellationToken cancellationToken = default);

        Task SaveAsync<TEntity>(TEntity value, CancellationToken cancellationToken = default);

        Task DeleteAsync<TEntity>(TEntity value, CancellationToken cancellationToken = default);

        bool ValidateAccountCreateObject(Guid agencyId, List<CreateAccountDto> accountForCreationDto);

        List<Account> GetAccountCreateMappingObject(Guid agencyId, List<CreateAccountDto> accountForCreationDto);

        bool ValidateAccountUpdatePayload(Guid accountId, UpdateAccountDto accountUpdateDto);

        bool ValidateAccountDeletePayload(Guid accountId);

        void UpdateAccountDetails(UpdateAccountDto accountUpdateDto, ref Account account);

        void CloseAccount(ref Account account);
    }
}
