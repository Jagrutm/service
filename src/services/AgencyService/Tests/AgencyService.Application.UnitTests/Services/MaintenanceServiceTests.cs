using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Models.Maintenances;
using AgencyService.Application.Services;
using AgencyService.Domain.Entities;
using AgencyService.Domain.Enums;
using BuildingBlocks.Tests.Extensions;
using BuildingBlocks.Tests.Mocks;
using BuildingBlocks.Tests.TestFixtures;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Moq;
using static BuildingBlocks.Tests.Extensions.ServiceMockExtensions;

namespace AgencyService.Application.UnitTests.Services
{
    [UsesVerify]
    public class MaintenanceServiceTests : IClassFixture<ServiceTestFixture>
    {
        private readonly MapperMock _mapperMock;
        private readonly Mock<IMaintenanceRepository> _maintenanceRepository;
        private readonly Mock<IMaintenanceValidator> _maintenanceValidator;
        private readonly Mock<IAgencyValidator> _agencyValidator;
        private readonly MaintenanceService _maintenanceService;

        public MaintenanceServiceTests()
        {
            _mapperMock = new MapperMock();
            _maintenanceRepository = new Mock<IMaintenanceRepository>();
            _maintenanceValidator = new Mock<IMaintenanceValidator>();
            _agencyValidator = new Mock<IAgencyValidator>();

            _maintenanceService = new MaintenanceService(
                _mapperMock.Object,
                _maintenanceRepository.Object,
                _maintenanceValidator.Object,
                _agencyValidator.Object);
        }

        [Fact]
        public async Task CreateMaintenanceForAgencyAsync_ShouldCreateMaintenaceDetails_WhenRequestIsValid()
        {
            //Arrange
            var agenyId = Guid.NewGuid();
            var agency = Builder<Agency>.CreateNew()
                .With(_=>_.Id = 1)
                .With(_=>_.UId = agenyId)
                .Build();
            var createMaintenanceDto = Builder<CreateMaintenanceDto>.CreateNew().Build();
            var maintenanceToCreate = Builder<Maintenance>.CreateNew()
                .With(_=>_.AgencyId = agency.Id)
                .Build();

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agenyId))
                .ReturnsAsync(agency);
            _mapperMock.MockMap(createMaintenanceDto, maintenanceToCreate);
            _maintenanceRepository.Setup(_=>_.CreateAsync(maintenanceToCreate))
                .ReturnsAsync(maintenanceToCreate);

            //Act
            await _maintenanceService.CreateMaintenanceForAgencyAsync(agenyId, createMaintenanceDto);

            //Assert
            await Verify(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task CreateMaintenanceForAgencyAsync_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agenyId = Guid.NewGuid();
            var createMaintenanceDto = Builder<CreateMaintenanceDto>.CreateNew().Build();
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agenyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                async() =>
                await _maintenanceService.CreateMaintenanceForAgencyAsync(agenyId, createMaintenanceDto);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task GetMaintenancesAsync_ShouldGetMaintenances_WhenRequestIsValid()
        { 
            //Arrange
            var agencyId = Guid.NewGuid();
            var agency = Builder<Agency>.CreateNew()
                .With(_ => _.Id = 1)
                .With(_ => _.UId = agencyId)
                .Build();
            var existingMaintenances = new List<Maintenance>
            {
                Builder<Maintenance>.CreateNew()
                .With(_=>_.AgencyId = agency.Id)
                .With(_=>_.ResponseCode = QualifiedAcceptanceCode.NextCalendarDay)
                .Build()
            };

            var maintenanceDtos = new List<MaintenanceDto>
            {
                Builder<MaintenanceDto>.CreateNew()
                .With(_=>_.ResponseCode = QualifiedAcceptanceCode.NextCalendarDay.ToString())
                .Build()
            };

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .ReturnsAsync(agency);
            _maintenanceRepository.Setup(_ => _.GetAllAsync())
                .ReturnsAsync(existingMaintenances);
            _mapperMock.MockMap(existingMaintenances, maintenanceDtos);

            //Act
            var actualMaintenanceDtos = await _maintenanceService.GetMaintenancesAsync(agencyId);

            //Assert
            await Verify(actualMaintenanceDtos);
        }

        [Fact]
        public async Task GetMaintenancesAsync_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                async() =>
                await _maintenanceService.GetMaintenancesAsync(agencyId);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task GetMaintenanceDetailsAsync_ShouldGetMaintenanceDetails_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;
            var maintenanceDto = Builder<MaintenanceDto>.CreateNew()
                .With(_=>_.ResponseCode = QualifiedAcceptanceCode.NextCalendarDay.ToString())
                .Build();
            var maintenance = Builder<Maintenance>.CreateNew()
                .With(_=>_.ResponseCode = QualifiedAcceptanceCode.NextCalendarDay)
                .Build();

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _maintenanceRepository.Setup(_ => _.GetByIdAsync(maintenanceId))
                .ReturnsAsync(maintenance);
            _maintenanceValidator.Setup(_ => _.ValidateMaintenanceIsNotNull(maintenance));
            _mapperMock.MockMap(maintenance, maintenanceDto);

            //Act
            var actualMaintenance = await _maintenanceService.GetMaintenanceDetailsAsync(agencyId, maintenanceId);

            //Assert
            await Verify(actualMaintenance);
        }

        [Fact]
        public async Task GetMaintenanceDetailsAsync_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                async () =>
                await _maintenanceService.GetMaintenanceDetailsAsync(agencyId, maintenanceId);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
            //result.ThrowExceptionExactly<Exception>(errorMessage);
            //await Task.CompletedTask;
        }

        [Fact]
        public async Task GetMaintenanceDetailsAsync_ShouldThrowException_WhenMaintenanceNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;
            Maintenance maintenance = null;
            var errorMessage = "Maintenance details not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _maintenanceRepository.Setup(_ => _.GetByIdAsync(maintenanceId))
                .ReturnsAsync(maintenance);
            _maintenanceValidator.Setup(_ => _.ValidateMaintenanceIsNotNull(maintenance))
                .Throws<Exception>();


            //Act
            Func<Task> result =
                async () =>
                await _maintenanceService.GetMaintenanceDetailsAsync(agencyId, maintenanceId);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
            //result.ThrowExceptionExactly<Exception>(errorMessage);
            //await Task.CompletedTask;
        }

        [Fact]
        public async Task UpdateMaintenanceForAgencyAsync_ShouldUpdateMaintenanceDetails_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var agency = Builder<Agency>.CreateNew()
                .With(_ => _.Id = 1)
                .With(_ => _.UId = agencyId)
                .Build();
            var maintenanceId = 1;
            var maintenanceDto = Builder<UpdateMaintenanceDto>.CreateNew()
                .With(_=>_.ResponseCode = "SameDay")
                .Build();
            var maintenance = Builder<Maintenance>.CreateNew()
                .With(_=>_.Id = maintenanceId)
                .With(_=>_.AgencyId = agency.Id)
                .Build();

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .ReturnsAsync(agency);
            _maintenanceRepository.Setup(_ => _.GetByIdAsync(maintenanceId))
                .ReturnsAsync(maintenance);
            _maintenanceValidator.Setup(_ => _.ValidateMaintenanceIsNotNull(IsEquivalentTo(maintenance)));
            _mapperMock.MockMap(IsEquivalentTo(maintenanceDto), IsEquivalentTo(maintenance));
            _maintenanceRepository.Setup(_ => _.UpdateAsync(IsEquivalentTo(maintenance)));

            //Act
            await _maintenanceService.UpdateMaintenanceForAgencyAsync(agencyId, maintenanceId, maintenanceDto);

            //Assert
            await Verify(StatusCodes.Status204NoContent);
            //_agencyValidator.Verify(_ => _.ValidateAgencyWithIdExists(agencyId), Times.Once);
            //_maintenanceRepository.Verify(_ => _.GetByIdAsync(maintenanceId), Times.Once);
            //_maintenanceValidator.Verify(_ => _.ValidateMaintenanceIsNotNull(maintenance), Times.Once);
            //_mapperMock.VerifyMap<UpdateMaintenanceDto, Maintenance>(Times.Once());
            //_maintenanceRepository.Verify(_ => _.UpdateAsync(maintenance), Times.Once);
        }

        [Fact]
        public async Task UpdateMaintenanceForAgencyAsync_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;
            var maintenanceDto = Builder<UpdateMaintenanceDto>.CreateNew().Build();
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();
           
            //Act
            Func<Task> result =
                async () => 
                await _maintenanceService.UpdateMaintenanceForAgencyAsync(agencyId, maintenanceId, maintenanceDto);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task UpdateMaintenanceForAgencyAsync_ShouldThrowException_WhenMaintenanceNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;
            var maintenanceDto = Builder<UpdateMaintenanceDto>.CreateNew().Build();
            Maintenance maintenance = null;
            var errorMessage = "Maintenance details not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _maintenanceRepository.Setup(_ => _.GetByIdAsync(maintenanceId))
                .ReturnsAsync(maintenance);
            _maintenanceValidator.Setup(_ => _.ValidateMaintenanceIsNotNull(IsEquivalentTo(maintenance)))
                 .Throws<Exception>();

            //Act
            Func<Task> result =
               async () =>
               await _maintenanceService.UpdateMaintenanceForAgencyAsync(agencyId, maintenanceId, maintenanceDto);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task DeleteMaintenanceForAgencyAsync_ShouldDeleteMaintenanceDetails_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var agency = Builder<Agency>.CreateNew()
                .With(_ => _.Id = 1)
                .With(_ => _.UId = agencyId)
                .Build();
            var maintenanceId = 1;
            var maintenance = Builder<Maintenance>.CreateNew()
                .With(_ => _.Id = maintenanceId)
                .With(_ => _.AgencyId = agency.Id)
                .Build();

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .ReturnsAsync(agency);
            _maintenanceRepository.Setup(_ => _.GetByIdAsync(maintenanceId))
                .ReturnsAsync(maintenance);
            _maintenanceValidator.Setup(_ => _.ValidateMaintenanceIsNotNull(IsEquivalentTo(maintenance)));
            _maintenanceRepository.Setup(_ => _.DeleteAsync(IsEquivalentTo(maintenance)));


            //Act
            await _maintenanceService.DeleteMaintenanceForAgencyAsync(agencyId, maintenanceId);

            //Assert
            await Verify(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task DeleteMaintenanceForAgencyAsync_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                async () =>
                await _maintenanceService.DeleteMaintenanceForAgencyAsync(agencyId, maintenanceId);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task DeleteMaintenanceForAgencyAsync_ShouldThrowException_WhenMaintenanceNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;
            Maintenance maintenance = null;
            var errorMessage = "Maintenance details not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _maintenanceRepository.Setup(_ => _.GetByIdAsync(maintenanceId))
                .ReturnsAsync(maintenance);
            _maintenanceValidator.Setup(_ => _.ValidateMaintenanceIsNotNull(IsEquivalentTo(maintenance)))
                 .Throws<Exception>();

            //Act
            Func<Task> result =
               async () =>
               await _maintenanceService.DeleteMaintenanceForAgencyAsync(agencyId, maintenanceId);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }
    }
}
