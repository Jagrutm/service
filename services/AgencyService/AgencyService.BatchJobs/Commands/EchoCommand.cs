using BuildingBlocks.Core.RestClient;
using CommandLine;

namespace AgencyService.BatchJobs.Commands
{
    public class EchoCommand : ICommand
    {
        public string Name => nameof(EchoCommand);

        [Option('m', "message", Required = false, HelpText = "Echo message")]
        public string Message { get; set; } = "Agency service batch jobs called";

        public Task ExecuteAsync(IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }
    }
}
