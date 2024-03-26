using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Models.Maintenances;
using AgencyService.BatchJobs.Clients;
using AgencyService.Domain.Entities;
using AgencyService.Domain.Enums;

namespace AgencyService.BatchJobs.Services
{
    public class MaintenanceServiceBatchRequestExecutor
    {
        private readonly IAgencyRepository _agencyRepository;
        private readonly MaintenanceClient _maintenanceClient;

        public MaintenanceServiceBatchRequestExecutor(
            IAgencyRepository agencyRepository,
            MaintenanceClient maintenanceClient)
        {
            _agencyRepository = agencyRepository;
            _maintenanceClient = maintenanceClient;
        }

        public async Task GenerateMaintenanceDetailsForAgency(Guid agencyId, string qualifiedAcceptanceCode)
        {
            var maintenanceDto = new CreateMaintenanceDto
            {
                FromDate = DateTime.Now.AddDays(2),
                ToDate = DateTime.Now.AddDays(3),
                ResponseCode = qualifiedAcceptanceCode
            };

            await _maintenanceClient.GenerateMaintenanceDetailsAsync(agencyId, maintenanceDto);
        }
    }
}
