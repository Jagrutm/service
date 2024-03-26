using BuildingBlocks.Core.Domain.EventObservers;
using EventBus.Messages.Contracts;
using EventBus.Messages.EventObservers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.Domain.Extensions
{
    /*
     * For more configuration settings please refer the below links:
     * https://masstransit-project.com/usage/configuration.html#configuration-2 
     * https://masstransit-project.com/usage/transports/rabbitmq.html#broker-topology           
     */
    public static class ConfigureMassTransitExtension
    {
        public static IServiceCollection AddMassTransitServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            bool IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;

            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(IsRunningInContainer ? configuration["EventBussSetting:HostAddressContainer"] : configuration["EventBussSetting:HostAddress"]),
                        configure: h =>
                        {
                            h.Username(configuration["EventBussSetting:UserName"]);
                            h.Password(configuration["EventBussSetting:Password"]);
                            h.ConfigureBatchPublish(x =>
                            {
                                x.Enabled = true;
                            });
                        });

                    cfg.ConfigureEndpoints(ctx);

                    cfg.ConnectBusObserver(new BusObserver());
                    cfg.ConnectReceiveObserver(new ReceiveObserver());
                    cfg.ConnectConsumeObserver(new ConsumeObserver());
                    cfg.ConnectSendObserver(new SendObserver());
                    cfg.ConnectPublishObserver(new PublishObserver());
                });
            });

            services.AddTransient<IPublisherMq, MassTransitRabbitMq>();

            return services;
        }
    }
}
