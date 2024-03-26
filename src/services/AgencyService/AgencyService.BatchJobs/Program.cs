using AgencyService.Application.Contracts.Persistence;
using AgencyService.BatchJobs.Clients;
using AgencyService.BatchJobs.Commands;
using AgencyService.BatchJobs.Services;
using BuildingBlocks.Core.RestClient;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace AgencyService.BatchJobs
{
    public static class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfiguration _configuration;

        public static async Task Main(string[] args)
        {
            ConfigureService();

            var parsedResult = Parser.Default.ParseArguments<GenerateMaintenanceCommand, EchoCommand>(args);

            if (parsedResult.Tag == ParserResultType.Parsed)
            {
                await parsedResult.WithParsedAsync((command) => ProcessCommand(command as ICommand));
            }
            else
            {
                //Log error or something or throw exception here.
                throw new Exception();
            }
        }

        private static void ConfigureService()
        {
            var builder = new ConfigurationBuilder();
            //builder.AddJsonFile("./appsettings.json").AddEnvironmentVariables();
            _configuration = builder.Build();
            RegisterDependencies();
        }

        private static void RegisterDependencies()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(_configuration);

            services.AddTransient<IRestClient>(_ => NewRestClient());
            services.AddSingleton<MaintenanceClient>();

            services.AddSingleton(serviceProvider =>
            {
                return new MaintenanceServiceBatchRequestExecutor(
                    serviceProvider.GetRequiredService<IAgencyRepository>(),
                    serviceProvider.GetRequiredService<MaintenanceClient>());
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        private static RestSharp.RestClient NewRestClient()
        {
            var restClient = new RestSharp.RestClient();
            restClient.UseNewtonsoftJson();
            return restClient;
        }

        private static Task ProcessCommand(ICommand command)
        {
            try
            {
                return command.ExecuteAsync(_serviceProvider);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
