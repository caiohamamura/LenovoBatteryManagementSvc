using System.Management;

namespace LenovoBatteryManager.Services;
public class CIM_Battery
{
    public ushort Availability { get; private set; }
    public ushort BatteryStatus { get; private set; }
    public ulong DesignVoltage { get; private set; }
    public ushort EstimatedChargeRemaining { get; private set; }
    public uint EstimatedRunTime { get; private set; }
    public string? Name { get; private set; }
    public string? Status { get; private set; }
    public string? SystemName { get; private set; }

    public CIM_Battery()
    {
        FetchData();
    }

    public void FetchData()
    {
        ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_Battery");
        foreach (var obj in mos.Get())
        {
            this.Availability = (ushort)obj.GetPropertyValue("Availability");
            this.BatteryStatus = (ushort)obj.GetPropertyValue("BatteryStatus");
            this.DesignVoltage = (ulong)obj.GetPropertyValue("DesignVoltage");
            this.EstimatedChargeRemaining = (ushort)obj.GetPropertyValue("EstimatedChargeRemaining");
            this.EstimatedRunTime = (uint)obj.GetPropertyValue("EstimatedRunTime");
            this.Name = (string)obj.GetPropertyValue("Name");
            this.Status = (string)obj.GetPropertyValue("Status");
            this.SystemName = (string)obj.GetPropertyValue("SystemName");
        }        
    }
}
