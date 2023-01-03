# LenovoBatteryManagementSvc

This is a Windows Service for Lenovo laptops battery management enhancement. 

## How it Works

This service will provide two main functionalities:

1. When you connect/disconnect your laptop from power supply it can automatically switch your energy profile to the one you select
1. You can set a different target energy level instead of the 60% Lenovo's hardcoded. 

## Installation

### From Binary file

You can install it from binary zip file [here](https://github.com/caiohamamura/LenovoBatteryManagementSvc/releases/download/v0.1/LenovoBatteryManagementSvc.zip). 

1. Download it [here](https://github.com/caiohamamura/LenovoBatteryManagementSvc/releases/download/v0.1/LenovoBatteryManagementSvc.zip)
1. Extract it anywhere (this should be the final "installation" path, you cannot remove it later)
1. Open `cmd` as administrator
1. Now install it as a service using `sc create "Lenovo Battery Management" binPath=<C:\Path\To\LenovoBatteryManagement.exe> start=auto`
1. Run the service with `net start "Lenovo Battery Management"`

### Compile from source

To compile from source you must install [.NET SDK](https://dotnet.microsoft.com/download). Then just run:

```
dotnet publish -c Release
```

Then just proceed as instructed before from step 3.

## Configuration

For configuring the service the appsettings.json has four relevant options:

1. TargetBatteryLevel (integer): the target battery level (default 70)
2. BatteryLevelThreshold (integer): the amount below which the default charging method will trigger again. For exemple, if you set TargetBatteryLevel to 80% and BatteryLevelThreshold to 5%, then whenether the battery level is below 75% charging will be enabled, when it reaches 80% charging mode will be set to Conservation mode (default 5).
3. ChargingMode: the charging mode to be set when battery drops below the set threshold (Normal|Rapid, default Normal).
4. PowerPlanPower: the power plan to be set when the laptop is working on power (EfficiencyMode | IntelligentCooling | ExtremePerformance, default IntelligentCooling). 
5. PowerPlanBattery: the power plan to be set when the laptop is working on battery (EfficiencyMode | IntelligentCooling | ExtremePerformance, default: EfficiencyMode).
6. CheckIntervalSeconds (integer): this service works by checking the battery level, you can set the amount of seconds to adjust the frequency of checking (default 60). 

## Known Issues

1. As this is a windows service and not an integrated hardware solution it won't work while Windows is asleep or computer is shutdown.
