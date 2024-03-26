using AgencyService.Domain.Entities;
using BuildingBlocks.Application.Contracts.Persistence;

namespace AgencyService.Application.Contracts.Persistence
{
    public interface IMaintenanceRepository : IEFRepository<Maintenance, int>
    {
    }
}
