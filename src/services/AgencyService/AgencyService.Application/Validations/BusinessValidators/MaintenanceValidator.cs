using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AgencyService.Application.Validations.BusinessValidators
{
    public class MaintenanceValidator : IMaintenanceValidator
    {

        private readonly ILogger<MaintenanceValidator> _logger;
        private readonly IMaintenanceRepository _maintenanceRepository;

        public MaintenanceValidator(
            ILogger<MaintenanceValidator> logger,
            IMaintenanceRepository maintenanceRepository)
        {
            _logger = logger;
            _maintenanceRepository = maintenanceRepository;
        }

        public void ValidateMaintenanceIsNotNull(Maintenance maintenance)
        {
            if (maintenance == null)
            {
                _logger.LogInformation("Maintenance details not found");
                throw new Exception("Maintenance details not found");
            }

            _logger.LogDebug("Validated maintenance with id {MaintenanceId} exists", maintenance?.Id);
        }

        public async Task ValidateMaintenanceWithIdExists(int maintenanceId)
        {
            var maintenance = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            ValidateMaintenanceIsNotNull(maintenance);
        }
    }
}
