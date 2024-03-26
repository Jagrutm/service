using AgencyService.Infrastructure.Persistence;
using BuildingBlocks.Tests.TestFixtures;

namespace AgencyService.Infrastructure.IntegrationTests.TestFixtures
{
    public class AgencyServiceInMemorySqliteDbTestFixture : InMemorySqliteEFDbContextTestFixture<AgencyDbContext>
    {
    }
}
