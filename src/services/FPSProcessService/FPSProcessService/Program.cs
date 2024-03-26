using FPSProcessService;


var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "FPS Parser service.";
    })
    .ConfigureServices(services =>
    {
        var appSetting = config.GetSection("appSettings");

        if(appSetting["IsServer"] == "0")
        {
            services.AddHostedService<ClientWorker>();
        }
        else
        {
            services.AddHostedService<ListnerWorker>();
        }

    })
    .Build();

await host.RunAsync();
