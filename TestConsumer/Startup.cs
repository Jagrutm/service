using EventBus.Messages.Common;
using EventBusConsumer;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestConsumer
{
    public static class Startup
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration Configuration = builder.Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    Console.WriteLine("Startup..................................");

                    //MassTransit configuration is created hear. -----------------
                    services.AddMassTransit(config =>
                    {
                        config.SetKebabCaseEndpointNameFormatter();

                        //config.AddConsumer<EventOneConsumer>();

                        config.UsingRabbitMq((ctx, cfg) =>
                        {
                            cfg.Host(new Uri(Configuration["EventBussSetting:HostAddress"]),
                                configure: h =>
                                {
                                    h.Username(Configuration["EventBussSetting:UserName"]);
                                    h.Password(Configuration["EventBussSetting:Password"]);
                                });

                            cfg.ReceiveEndpoint(EventBusConstants.AccountCreateQueue, c =>
                            {
                                c.Consumer<EventOneConsumerQ2>(ctx);
                                // limit the number of messages consumed concurrently
                                // this applies to the consumer only, not the endpoint
                                c.ConcurrentMessageLimit = 8;
                            });

                            cfg.ReceiveEndpoint(EventBusConstants.AccountUpdateQueue, c =>
                            {
                                c.Consumer<EventOneConsumer>(ctx);
                                // limit the number of messages consumed concurrently
                                // this applies to the consumer only, not the endpoint
                                c.ConcurrentMessageLimit = 8;
                            });

                            //cfg.ConnectBusObserver(new BusObserver());
                            //cfg.ConnectReceiveObserver(new ReceiveObserver());
                            //cfg.ConnectConsumeObserver(new ConsumeObserver());
                        });
                    });

                    services.AddMassTransitHostedService();
                    services.AddScoped<EventOneConsumer>();
                    services.AddScoped<EventOneConsumerQ2>();
                    //-----------------------------------------------------------
                });
        }
    }
}
