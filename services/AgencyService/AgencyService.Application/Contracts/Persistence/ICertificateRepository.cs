using AgencyService.Domain.Entities;
using BuildingBlocks.Application.Contracts.Persistence;

namespace AgencyService.Application.Contracts.Persistence
{
    public interface ICertificateRepository : IDapperRepository<Certificate, int>
    {
        Task<List<Certificate>> GetCertificatesForAgency(int agencyId);
    }
}
