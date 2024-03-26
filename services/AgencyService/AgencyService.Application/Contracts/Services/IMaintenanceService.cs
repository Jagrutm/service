using AgencyService.Application.Models.Maintenances;

namespace AgencyService.Application.Contracts.Services
{
    public interface IMaintenanceService
    {
        Task<List<MaintenanceDto>> GetMaintenancesAsync(Guid agencyId);

        Task<MaintenanceDto> GetMaintenanceDetailsAsync(Guid agencyId, int maintenanceId);

        Task CreateMaintenanceForAgencyAsync(Guid agencyId, CreateMaintenanceDto maintenanceDto);

        Task UpdateMaintenanceForAgencyAsync(
            Guid agencyId, 
            int maintenanceId, 
            UpdateMaintenanceDto maintenanceDto);

        Task DeleteMaintenanceForAgencyAsync(Guid agencyId, int maintenanceId);
    }
}
