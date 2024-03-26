using AgencyService.Application.Models.Maintenances;
using RestSharp;

namespace AgencyService.BatchJobs.Clients
{
    public class MaintenanceClient
    {
        private readonly IRestClient _restClient;
        private const string GenerateMaintenanceDetailsApi = "agencies/{agencyId}/maintenance";

        public MaintenanceClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task GenerateMaintenanceDetailsAsync(
            Guid agencyId,
            CreateMaintenanceDto maintenanceToCreate)
        {
            var generateMaintenanceDetailsRequest = 
                new RestRequest(GenerateMaintenanceDetailsApi, Method.POST)
                .AddUrlSegment(nameof(agencyId), agencyId);

            generateMaintenanceDetailsRequest.AddJsonBody(maintenanceToCreate);
            await _restClient.ExecutePostAsync(generateMaintenanceDetailsRequest);
        }
    }
}
