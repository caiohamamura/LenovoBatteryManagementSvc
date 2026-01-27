using IdeapadToolkit.Models;
using IdeapadToolkit.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using LenovoBatteryManager.Services;

namespace LenovoBatteryManager;



public class WindowsBackgroundService : IHostedService
{
    private readonly LenovoPowerSettingsService _service;
    private readonly ILogger<WindowsBackgroundService> _logger;
    private readonly CIM_Battery _battery;

    public WindowsBackgroundService(
        LenovoPowerSettingsService service,
        ILogger<WindowsBackgroundService> logger,
        CIM_Battery battery) =>
        (_service, _logger, _battery) = (service, logger, battery);

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        try
        {
            var confBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Dictionary<string, PowerPlan> dictPower = new () {
                {"None", PowerPlan.None},
                {"IntelligentCooling", PowerPlan.IntelligentCooling},
                {"EfficiencyMode", PowerPlan.EfficiencyMode},
                {"ExtremePerformance", PowerPlan.ExtremePerformance},
            };
            while (!stoppingToken.IsCancellationRequested)
            {
                var conf = confBuilder.Build();
                int targetLevel = conf.GetValue<int>("TargetBatteryLevel", 70);
                int threshold = conf.GetValue<int>("BatteryLevelThreshold", 5);
                string chargingModeString = conf.GetValue<string>("ChargingMode", "Normal") ?? "Normal";
                int checkInterval = conf.GetValue<int>("CheckIntervalSeconds", 60);
                string powerPlanBatteryString = conf.GetValue<string>("PowerPlanBattery") ?? "EfficiencyMode";
                string powerPlanPowerString = conf.GetValue<string>("PowerPlanPower") ?? "IntelligentCooling";
                _battery.FetchData();

                ChargingMode chargingMode = ChargingMode.Normal;
                if (chargingModeString.ToLower() == "rapid")
                    chargingMode = ChargingMode.Rapid;

                PowerPlan powerPlanBattery = PowerPlan.EfficiencyMode;
                PowerPlan powerPlanPower = PowerPlan.IntelligentCooling;

                if (dictPower.ContainsKey(powerPlanBatteryString))
                    powerPlanBattery = dictPower[powerPlanBatteryString];
                
                if (dictPower.ContainsKey(powerPlanPowerString))
                    powerPlanPower = dictPower[powerPlanPowerString];
                

                var loginfo = (ChargingMode cm) => {
                    _logger.LogInformation($@"Battery level is {_battery.EstimatedChargeRemaining}
Changing charging mode to {cm}
Target battery level: {targetLevel}
Target threshold: {threshold}
Charging mode preferred: {chargingModeString}");
                };

                var logInfoPp = (PowerPlan pp) => {
                    _logger.LogInformation($@"Changing power plan to {pp}
PowerPlanBattery: {powerPlanBattery}
PowerPlanPower: {powerPlanPower}");
                };

                if (
                    _service.GetChargingMode() != chargingMode &&
                    _battery.EstimatedChargeRemaining < (targetLevel - threshold)
                    )
                {
                    _service.SetChargingMode(chargingMode);
                    loginfo(chargingMode);
                } else if (
                    _service.GetChargingMode() != ChargingMode.Conservation &&
                    _battery.EstimatedChargeRemaining >= targetLevel
                )
                {
                    _service.SetChargingMode(ChargingMode.Conservation);
                    loginfo(ChargingMode.Conservation);
                }

                if (_battery.BatteryStatus == 1 && _service.GetPowerPlan() != powerPlanBattery)
                {
                    _service.SetPowerPlan(powerPlanBattery);
                    logInfoPp(powerPlanBattery);
                } else if (_battery.BatteryStatus == 2 && _service.GetPowerPlan() != powerPlanPower)
                {
                    _service.SetPowerPlan(powerPlanPower);
                    logInfoPp(powerPlanPower);
                }
                

                await Task.Delay(TimeSpan.FromSeconds(checkInterval), stoppingToken);
            }
        }
        catch (TaskCanceledException) { }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
         
            Environment.Exit(1);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopped.");
        return Task.CompletedTask;
    }
}

