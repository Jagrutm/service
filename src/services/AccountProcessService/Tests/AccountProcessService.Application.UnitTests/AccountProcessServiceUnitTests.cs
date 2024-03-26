using AccountProcessService.Application.Contracts.Persistence;
using AccountProcessService.Application.Models;
using BuildingBlocks.Tests.Mocks;
using BuildingBlocks.Tests.TestFixtures;
using BuildingBlocks.Tests.Extensions;
using FizzWare.NBuilder;
using Moq;

namespace AccountProcessService.Application.Test
{
    [UsesVerify]
    public class AccountProcessServiceUnitTests : IClassFixture<ServiceTestFixture>
    {
        private readonly MapperMock _mapperMock;
        private readonly Mock<IAccounProcessRepository> _accountProcessRepository;
        private readonly Services.AccountProcessService _accountProcessService;

        public AccountProcessServiceUnitTests()
        {
            _accountProcessRepository = new Mock<IAccounProcessRepository>();
            _mapperMock = new MapperMock();

            _accountProcessService = new Services.AccountProcessService(
                _accountProcessRepository.Object,
                _mapperMock.Object);
        }

        [Fact]
        public void ValidateAccountCreateObject_ShouldValidateObject_WhenRequestISValid()
        {
            var agencyId = Guid.NewGuid();
            var createAccountDtos = Builder<CreateAccountDto>.CreateListOfSize(2).Build().ToList();

            var result = _accountProcessService.ValidateAccountCreateObject(agencyId, createAccountDtos);

            Assert.True(result);
        }

        [Fact]
        public async Task ValidateAccountCreateObject_ShouldThrowException_WhenAgencyIdISEmpty()
        {
            var agencyId = Guid.Empty;
            var createAccountDtos = Builder<CreateAccountDto>.CreateListOfSize(2).Build().ToList();
            var errorMessage = "Invalid AgencyId";

            Func<Task> result =
                async () =>
                {
                    await Task.CompletedTask;
                    _accountProcessService.ValidateAccountCreateObject(agencyId, createAccountDtos);
                };

            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task ValidateAccountCreateObject_ShouldThrowException_WhenCreateAccountDtoIsNull()
        {
            var agencyId = Guid.NewGuid();
            var errorMessage = "No accounts received";

            Func<Task> result =
                async () =>
                {
                    await Task.CompletedTask;
                    _accountProcessService.ValidateAccountCreateObject(agencyId, null);
                };

            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task ValidateAccountCreateObject_ShouldThrowException_WhenCreateAccountDtoCountIsZero()
        {
            var agencyId = Guid.NewGuid();
            var errorMessage = "No accounts received";

            Func<Task> result =
                async () =>
                {
                    await Task.CompletedTask;
                    _accountProcessService.ValidateAccountCreateObject(agencyId, new List<CreateAccountDto>());
                };

            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task ValidateAccountCreateObject_ShouldThrowException_WhenCreateAccountDtoCountIsMoreThanTen()
        {
            var agencyId = Guid.NewGuid();
            var errorMessage = "Limited number of records are allowed";
            var createAccountDtos = Builder<CreateAccountDto>.CreateListOfSize(15).Build().ToList();

            Func<Task> result =
                async () =>
                {
                    await Task.CompletedTask;
                    _accountProcessService.ValidateAccountCreateObject(agencyId, createAccountDtos);
                };

            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }
    }
}