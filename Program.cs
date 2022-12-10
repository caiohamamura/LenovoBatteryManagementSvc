using IdeapadToolkit.Services;
using LenovoBatteryManager.Services;
using LenovoBatteryManager;

new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<LenovoPowerSettingsService>();
        services.AddSingleton<CIM_Battery>();
        services.AddHostedService<WindowsBackgroundService>();
    })
    .ConfigureLogging((context, logging) =>
        logging.AddConfiguration(context.Configuration.GetSection("Logging"))
    )
    .UseWindowsService(options => options.ServiceName = "Lenovo Battery Management")
    .Build()
    .Run();