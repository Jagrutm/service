using AccountProcessService.Application.Contracts.Persistence;
using AccountProcessService.Application.Contracts.Services;
using AccountProcessService.Application.Models;
using AccountProcessService.Domain.Entities;
using AutoMapper;

namespace AccountProcessService.Application.Services
{
    public class AccountProcessService : IAccountProcessService
    {
        private readonly IAccounProcessRepository _accountProcessRepository;
        private readonly IMapper _mapper;

        public AccountProcessService(IAccounProcessRepository accountProcessRepository, IMapper mapper)
        {
            _accountProcessRepository = accountProcessRepository;
            _mapper = mapper;
        }

        #region Create Account

        public bool ValidateAccountCreateObject(Guid agencyId, List<CreateAccountDto> accountForCreationDto)
        {
            if (agencyId == Guid.Empty)
            {
                throw new ArgumentException("Invalid AgencyId");
            }
            if (accountForCreationDto == null || accountForCreationDto.Count == 0)
            {
                throw new ArgumentException("No accounts received");
            }
            if (accountForCreationDto.Count > 10)
            {
                throw new ArgumentException("Limited number of records are allowed");
            }

            return true;
        }

        public List<Account> GetAccountCreateMappingObject(Guid agencyId, List<CreateAccountDto> accountForCreationDto)
        {
            var accounts = _mapper.Map<List<CreateAccountDto>, List<Account>>(accountForCreationDto);

            accounts.ForEach(account =>
            {
                account.DocumentId = Guid.NewGuid();
                account.Id = default;
                account.UId = Guid.NewGuid();
                account.AgencyId = agencyId;
                account.CreatedOn = DateTime.UtcNow;
                account.Version = 1;
            });

            return accounts;
        }

        public async Task CreateAsync(Guid agencyId, List<Account> accounts, CancellationToken cancellationToken = default)
        {
            await _accountProcessRepository.CreateAsync(agencyId, accounts, cancellationToken);
        }
        #endregion

        #region Update Account

        public bool ValidateAccountUpdatePayload(Guid accountId, UpdateAccountDto accountUpdateDto)
        {
            if (accountId == Guid.Empty || string.IsNullOrEmpty(accountUpdateDto.Name))
            {
                throw new ArgumentException("Invalid AccountId or Name");
            }

            return true;
        }

        public void UpdateAccountDetails(UpdateAccountDto accountUpdateDto, ref Account account)
        {
            account.Name = accountUpdateDto.Name;
            account.Version++;
        }
        #endregion

        #region Delete Account

        public bool ValidateAccountDeletePayload(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentException("Invalid AccountIdentifier");
            }

            return true;
        }

        public void CloseAccount(ref Account account)
        {
            account.IsClose = true;
            account.Version++;
        }

        public async Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default)
        {
            await _accountProcessRepository.DeleteAsync(value, cancellationToken);
        }
        #endregion

        #region Common

        public async Task<T> LoadAsync<T>(object hashKey, CancellationToken cancellationToken = default)
        {
            return await _accountProcessRepository.LoadAsync<T>(hashKey, cancellationToken);
        }

        public async Task SaveAsync<T>(T value, CancellationToken cancellationToken = default)
        {
            await _accountProcessRepository.SaveAsync(value, cancellationToken);
        }
        #endregion
    }
}
