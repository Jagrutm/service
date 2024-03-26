using AgencyService.Domain.Entities;
using BuildingBlocks.Application.Contracts.Persistence;

namespace AgencyService.Application.Contracts.Persistence
{
    public interface ISortCodeRepository : IDapperRepository<SortCode, int>
    {
        Task<List<SortCode>> GetSortcodesForAgency(Guid agencyId);

        Task<List<SortCode>> GetSortcodesForAgencyUsingSp(Guid agencyId);

        Task<bool> VerifySortCodeForAgency(Guid agencyId, string sortCode);

        Task<List<AgencyIdSortCodeTuple>> GetSortCodeForAllAgencies();
    }
}
