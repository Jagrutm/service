using AgencyService.Application.Contracts.Services;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Models.Maintenances;
using AgencyService.Controllers;
using BuildingBlocks.Tests.Extensions;
using BuildingBlocks.Tests.TestFixtures;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AgencyService.Api.UnitTests.Controllers
{
    public class MaintenanceControllerTests : IClassFixture<ServiceTestFixture>
    {
        private readonly Mock<IMaintenanceService> _maintenanceService;
        private readonly Mock<IMaintenanceValidator> _maintenanceValidator;
        private readonly MaintenanceController _maintenanceController;

        public MaintenanceControllerTests()
        {
            _maintenanceService = new Mock<IMaintenanceService>();
            _maintenanceValidator = new Mock<IMaintenanceValidator>();

            _maintenanceController = new MaintenanceController(
                _maintenanceService.Object,
                _maintenanceValidator.Object);
        }

        [Fact]
        public async Task CreateMaintenance_ShouldCreateMaintenance_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var createMaintenanceDto = Builder<CreateMaintenanceDto>.CreateNew().Build();

            _maintenanceService.Setup(_ => _.CreateMaintenanceForAgencyAsync(agencyId, createMaintenanceDto));

            //Act
            var createdResult = await _maintenanceController.CreateMaintenance(agencyId, createMaintenanceDto);

            //Assert
            Assert.NotNull(createdResult);
            Assert.IsType<CreatedResult>(createdResult);
            _maintenanceService.Verify(_ => _.CreateMaintenanceForAgencyAsync(agencyId, createMaintenanceDto), Times.Once);
        }

        [Fact]
        public async Task GetMaintenances_ShouldGetAllMaintenance_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceDtos = Builder<MaintenanceDto>.CreateListOfSize(3).Build().ToList();

            _maintenanceService.Setup(_ => _.GetMaintenancesAsync(agencyId))
                .ReturnsAsync(maintenanceDtos);

            //Act
            var okResult = await _maintenanceController.GetMaintenances(agencyId);

            //Assert
            Assert.NotNull(okResult);
            var objectResult = Assert.IsType<OkObjectResult>(okResult);
            objectResult.Value.ShouldBeEquivalent(maintenanceDtos);
            _maintenanceService.Verify(_ => _.GetMaintenancesAsync(agencyId), Times.Once);

        }

        [Fact]
        public async Task GetMaintenanceDetailById_ShouldGetMaintenance_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;
            var maintenanceDto = Builder<MaintenanceDto>.CreateNew().Build();

            _maintenanceService.Setup(_ => _.GetMaintenanceDetailsAsync(agencyId, maintenanceId))
               .ReturnsAsync(maintenanceDto);

            //Act
            var okResult = await _maintenanceController.GetMaintenanceDetailById(agencyId, maintenanceId);

            //Assert
            Assert.NotNull(okResult);
            var objectResult = Assert.IsType<OkObjectResult>(okResult);
            objectResult.Value.ShouldBeEquivalent(maintenanceDto);
            _maintenanceService.Verify(_ => _.GetMaintenanceDetailsAsync(agencyId, maintenanceId), Times.Once);
        }

        [Fact]
        public async Task UpdateMaintenanceDetails_ShouldUpdateMaintenance_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;
            var updateMaintenanceDto = Builder<UpdateMaintenanceDto>.CreateNew().Build();

            _maintenanceService.Setup(_ => _.UpdateMaintenanceForAgencyAsync(agencyId, maintenanceId, updateMaintenanceDto));

            //Act
            var noContentReqult = await _maintenanceController.UpdateMaintenanceDetails(agencyId, maintenanceId, updateMaintenanceDto);

            //Assert
            Assert.NotNull(noContentReqult);
            Assert.IsType<NoContentResult>(noContentReqult);
            _maintenanceService.Verify(_ => _.UpdateMaintenanceForAgencyAsync(agencyId, maintenanceId, updateMaintenanceDto), Times.Once);
        }

        [Fact]
        public async Task DeleteMaintenanceDetails_ShouldDeleteMaintenance_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var maintenanceId = 1;

            _maintenanceService.Setup(_ => _.DeleteMaintenanceForAgencyAsync(agencyId, maintenanceId));

            //Act
            var noContentReqult = await _maintenanceController.DeleteMaintenanceDetails(agencyId, maintenanceId);

            //Assert
            Assert.NotNull(noContentReqult);
            Assert.IsType<NoContentResult>(noContentReqult);
            _maintenanceService.Verify(_ => _.DeleteMaintenanceForAgencyAsync(agencyId, maintenanceId), Times.Once);
        }
    }
}
