using AgencyService.Application.Models.SortCodes;

namespace AgencyService.Application.Contracts.Services
{
    public interface ISortCodeService
    {
        Task CreateSortCodeForAgencyAsync(Guid agencyId, CreateSortCodeDto sortCodeToCreate);

        Task<List<SortCodeDto>> GetSortcodesForAgencyAsync(Guid agencyId);

        Task<bool> VerifySortCodeForAgencyAsync(Guid agencyId, string sortCode);

        Task<List<AgencyIdSortCodeTupleDto>> GetSortCodeForAllAgenciesAsync();
    }
}
