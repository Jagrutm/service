using AgencyService.Application.Contracts.Persistence;
using AgencyService.Domain.Entities;
using BuildingBlocks.Dapper.Contexts;
using BuildingBlocks.Infrastructure.Repositories;
using SqlKata;

namespace AgencyService.Infrastructure.Repositories
{
    public class CertificateRepository : DapperRepositoryBase<Certificate, int>, ICertificateRepository
    {

        public CertificateRepository(IDapperContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Certificate>> GetCertificatesForAgency(int agencyId)
        {
            var query = new Query(TableName)
                .Where(nameof(Certificate.AgencyId), agencyId);

            return await base.GetByQueryAsync(query);
        }
    }
}
