using AgencyService.BatchJobs.Services;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlocks.Core.RestClient;
using CommandLine;


namespace AgencyService.BatchJobs.Commands
{
    [Verb("generate-maintenance", HelpText = "Generate maintenance details for agencies")]
    public class GenerateMaintenanceCommand : ICommand
    {
        public string Name => nameof(GenerateMaintenanceCommand);

        [Option('a', "agencyId", Required = true, HelpText = "agency Id for which maintenance to be created")]
        public Guid AgencyId { get; set; }

        [Option('c', "qualifiedAcceptanceCode", Required = true, HelpText = "qualified acceptance code for which maintenace to be created")]
        public string QualifiedAcceptanceCode { get; set; }

        public async Task ExecuteAsync(IServiceProvider serviceProvider)
        {
           var maintenanceServiceBatchRequestExecutor = serviceProvider.GetRequiredService<MaintenanceServiceBatchRequestExecutor>();
            await maintenanceServiceBatchRequestExecutor.GenerateMaintenanceDetailsForAgency(AgencyId, QualifiedAcceptanceCode);
            await Task.CompletedTask;
        }
    }
}
