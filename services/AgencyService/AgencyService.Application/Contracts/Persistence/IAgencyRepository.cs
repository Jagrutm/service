using AgencyService.Domain.Entities;
using BuildingBlocks.Application.Contracts.Persistence;

namespace AgencyService.Application.Contracts.Persistence;

public interface IAgencyRepository : IEFRepository<Agency, int>
{
    Task<IEnumerable<Agency>> GetAgencyByCode(string agencyCode);

    Task<Agency> GetAgencyByUIdAsync(Guid uId);

}
