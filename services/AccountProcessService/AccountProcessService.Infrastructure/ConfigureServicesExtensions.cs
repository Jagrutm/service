using AccountProcessService.Application.Contracts.Persistence;
using AccountProcessService.Infrastructure.Repositories;
using BuildingBlocks.Aws.DynamoDb.Infrastructure.Contexts;
using BuildingBlocks.Aws.DynamoDb.Infrastructure.Extensions;
using BuildingBlocks.Aws.Infrastructure.Contexts;
using BuildingBlocks.Core.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountProcessService.Infrastructure
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAwsServices(configuration);
            services.AddDynamoDbServices(configuration);
            services.AddTransient<IDynamoDbContext, DynamoDbContext>();

            services.AddMassTransitServices(configuration);

            services.AddTransient<IAccounProcessRepository, AccountProcessRepository>();

            return services;
        }
    }
}
