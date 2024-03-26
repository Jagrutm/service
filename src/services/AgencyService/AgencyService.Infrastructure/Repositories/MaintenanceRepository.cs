using AgencyService.Application.Contracts.Persistence;
using AgencyService.Domain.Entities;
using AgencyService.Infrastructure.Persistence;
using BuildingBlocks.Infrastructure.Repositories;

namespace AgencyService.Infrastructure.Repositories
{
    public class MaintenanceRepository : EFRepositoryBase<Maintenance, int>, IMaintenanceRepository
    {

        public MaintenanceRepository(AgencyDbContext dbContext) 
            : base(dbContext)
        {

        }

        protected override IQueryable<Maintenance> IncludeChildEntitiesIn(IQueryable<Maintenance> queryable)
        {
            throw new NotImplementedException();
        }
    }
}
