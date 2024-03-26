using AgencyService.Domain.Entities;

namespace AgencyService.Application.Contracts.Validations
{
    public interface IMaintenanceValidator
    {
        Task ValidateMaintenanceWithIdExists(int maintenanceId);

        void ValidateMaintenanceIsNotNull(Maintenance maintenance);
    }
}
