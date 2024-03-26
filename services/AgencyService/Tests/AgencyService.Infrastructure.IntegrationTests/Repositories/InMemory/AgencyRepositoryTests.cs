using AgencyService.Application.Contracts.Persistence;
using AgencyService.Domain.Entities;
using AgencyService.Infrastructure.IntegrationTests.TestFixtures;
using AgencyService.Infrastructure.Persistence;
using AgencyService.Infrastructure.Repositories;
using BuildingBlocks.Infrastructure.Contexts;
using BuildingBlocks.Tests.Extensions;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace AgencyService.Infrastructure.IntegrationTests.Repositories.InMemory
{
    public class AgencyRepositoryTests 
        : IClassFixture<AgencyServiceInMemorySqliteDbTestFixture>
    {
        private readonly AgencyServiceInMemorySqliteDbTestFixture _testFixture;

        public AgencyRepositoryTests(AgencyServiceInMemorySqliteDbTestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public async Task GetAgencyByCode_ShouldReturnAgencies()
        {
            await _testFixture.ExecuteBaseAsync(async dbContext =>
            {
                //Arrange
                var repository = GetRepositoryInstance(dbContext);
                var agencyCode = "ABC123";
                var agency = Builder<Agency>.CreateNew()
                    .With(_ => _.Id = 1)
                    .With(_ => _.Code = agencyCode)
                    .Build();

                await dbContext.AddAsync(agency);
                await dbContext.SaveChangesAsync();

                //Act
                var createdAgencies = await repository.GetAgencyByCode(agencyCode);

                //Assert
                Assert.NotNull(createdAgencies);
                Assert.True(createdAgencies.Any());
                createdAgencies.First().ShouldBeEquivalent(agency);
            });
        }

        [Fact]
        public async Task GetAgencyByUIdAsync_ShouldReturnAgency()
        {
            await _testFixture.ExecuteBaseAsync(async dbContext => 
            {
                //Arrange
                var repository = GetRepositoryInstance(dbContext);
                var agencyId = Guid.NewGuid();
                var agency = Builder<Agency>.CreateNew()
                    .With(_ => _.Id = 2)
                    .With(_ => _.UId = agencyId)
                    .Build();

                await dbContext.AddAsync(agency);
                await dbContext.SaveChangesAsync();

                //Act
                var createdAgency = await repository.GetAgencyByUIdAsync(agencyId);

                //Assert
                //Assert.NotNull(createdAgency);
                //createdAgency.ShouldBeEquivalent(agency);
                
            });
        }

        private static IAgencyRepository GetRepositoryInstance(
            AgencyDbContext dbContext)
        {
            return new AgencyRepository(dbContext);
        }
    }
}
