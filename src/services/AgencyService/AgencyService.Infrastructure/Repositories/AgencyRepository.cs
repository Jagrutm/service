using AgencyService.Domain.Entities;
using AgencyService.Application.Contracts.Persistence;
using AgencyService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BuildingBlocks.Infrastructure.Repositories;

namespace AgencyService.Infrastructure.Repositories;

public class AgencyRepository : EFRepositoryBase<Agency, int>, IAgencyRepository
{
    public AgencyRepository(AgencyDbContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<IEnumerable<Agency>> GetAgencyByCode(string agencyCode)
    {
        return await Query()
            .Where(_ => _.Code == agencyCode)
            .ToListAsync();
    }

    public async Task<Agency> GetAgencyByUIdAsync(Guid uId)
    {
        return await Query()
           .Where(_ => _.UId == uId)
           .FirstOrDefaultAsync();
    }

    protected override IQueryable<Agency> IncludeChildEntitiesIn(IQueryable<Agency> queryable)
    {
        return queryable;
    }
}
