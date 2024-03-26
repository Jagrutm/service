using AgencyService.Application.Contracts.Persistence;
using AgencyService.Domain.Entities;
using AgencyService.Domain.Enums;
using AgencyService.Infrastructure.IntegrationTests.TestFixtures;
using AgencyService.Infrastructure.Persistence;
using AgencyService.Infrastructure.Repositories;
using BuildingBlocks.Tests.Extensions;
using FizzWare.NBuilder;
using Xunit;

namespace AgencyService.Infrastructure.IntegrationTests.Repositories.InMemory
{
    public class MaintenanceRepositoryTests : IClassFixture<AgencyServiceInMemorySqliteDbTestFixture>
    {
        private readonly AgencyServiceInMemorySqliteDbTestFixture _testFixture;

        public MaintenanceRepositoryTests(AgencyServiceInMemorySqliteDbTestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMaintenances()
        {
            await _testFixture.ExecuteBaseAsync(async dbContext =>
            {
                //Arrange
                var (maintenanceToGet, repository) = await CreateMaintenance(dbContext, 1);

                await dbContext.AddAsync(maintenanceToGet);
                await dbContext.SaveChangesAsync();

                //Act
                var createdMaintenances = await repository.GetAllAsync();

                //Assert
                Assert.NotNull(createdMaintenances);
                Assert.True(createdMaintenances.Any());
                createdMaintenances.First().ShouldBeEquivalent(maintenanceToGet);
            });
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMaintenance()
        {
            await _testFixture.ExecuteBaseAsync(async dbContext =>
            {
                //Arrange
                var (maintenanceToGet, repository) = await CreateMaintenance(dbContext, 2);
                await dbContext.AddAsync(maintenanceToGet);
                await dbContext.SaveChangesAsync();

                //Act
                var createdMaintenance = await repository.GetByIdAsync(maintenanceToGet.Id);

                //Assert
                Assert.NotNull(createdMaintenance);
                createdMaintenance.ShouldBeEquivalent(maintenanceToGet);
            });
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateMaintenance()
        {
            await _testFixture.ExecuteBaseAsync(async dbContext =>
            {
                //Arrange
                var (maintenanceToCreate, repository) = await CreateMaintenance(dbContext, 3);

                //Act
                var createdMaintenance = await repository.CreateAsync(maintenanceToCreate);

                //Assert
                Assert.NotNull(createdMaintenance);
                createdMaintenance.ShouldBeEquivalent(maintenanceToCreate);
            });
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateMaintenance()
        {
            await _testFixture.ExecuteBaseAsync(async dbContext =>
            {
                //Arrange
                var (maintenanceToCreate, repository) = await CreateMaintenance(dbContext, 4);
                await dbContext.AddAsync(maintenanceToCreate);
                await dbContext.SaveChangesAsync();

                var maintenanceToUpdate = await dbContext.FindAsync<Maintenance>(maintenanceToCreate.Id);
                maintenanceToUpdate.ResponseCode = QualifiedAcceptanceCode.WithinTwoHours;

                //Act
                await repository.UpdateAsync(maintenanceToUpdate);

                //Assert
                var updatedMaintenance = await repository.GetByIdAsync(maintenanceToUpdate.Id);
                updatedMaintenance.ResponseCode.ShouldBeEquivalent(maintenanceToUpdate.ResponseCode);
            });
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteMaintenance()
        {
            await _testFixture.ExecuteBaseAsync(async dbContext =>
            {
                //Arrange
                var (maintenanceToDelete, repository) = await CreateMaintenance(dbContext, 5);
                await dbContext.AddAsync(maintenanceToDelete);
                await dbContext.SaveChangesAsync();

                //Act
                await repository.DeleteAsync(maintenanceToDelete);

                //Assert
                var deletedMaintenance = await repository.GetByIdAsync(maintenanceToDelete.Id);
                Assert.Null(deletedMaintenance);
            });
        }

        private async Task<(Maintenance, IMaintenanceRepository)> CreateMaintenance(AgencyDbContext dbContext, int agencyId)
        {
            var repository = GetRepositoryInstance(dbContext);
            var agency = Builder<Agency>.CreateNew()
               .With(_ => _.Id = agencyId)
               .Build();
            await dbContext.AddAsync(agency);
            await dbContext.SaveChangesAsync();

            return (Builder<Maintenance>.CreateNew()
                .With(_ => _.Id = agencyId)
                .With(_ => _.AgencyId = agency.Id)
                .Build(), repository);
        }

        private static IMaintenanceRepository GetRepositoryInstance(
            AgencyDbContext dbContext)
        {
            return new MaintenanceRepository(dbContext);
        }
    }
}
