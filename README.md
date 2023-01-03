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

## Known Issues

1. As this is a windows service and not an integrated hardware solution it won't work while Windows is asleep or computer is shutdown.
