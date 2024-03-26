using AccountService.Application.Features.Consumers.CreateAccount;
using AccountService.Application.Features.Consumers.DeleteAccount;
using AccountService.Application.Features.Consumers.UpdateAccount;
using BuildingBlocks.Core.Domain.Extensions;
using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountService.Infrastructure
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            #region MassTransit
            services.AddMassTransitServices(configuration);

            //register consumers and their definition after initilizing MassTransit
            services.RegisterConsumer<CreateAccountConsumer, CreateAccountConsumerDefinition>();
            services.RegisterConsumer<UpdateAccountConsumer, UpdateAccountConsumerDefinition>();
            services.RegisterConsumer<DeleteAccountConsumer, DeleteAccountConsumerDefinition>();

            //add consumers as scoped services for Masstransit to access them
            services.AddScoped<CreateAccountConsumer>();
            services.AddScoped<UpdateAccountConsumer>();
            services.AddScoped<DeleteAccountConsumer>();
            #endregion

            //services.AddTransient<IAccounRepository, AccountRepository>();

            return services;
        }
    }
}
