using IdeapadToolkit.Models;
using System;
using System.Runtime.InteropServices;

namespace IdeapadToolkit.Services
{
    public class LenovoPowerSettingsService : ILenovoPowerSettingsService
    {
        [DllImport("PowerBattery.dll", EntryPoint = "?SetITSMode@CIntelligentCooling@PowerBattery@@QEAAHAEAW4ITSMode@12@@Z", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int SetITSMode(ref CIntelligentCooling var1, ref PowerPlan var2);


        [DllImport("PowerBattery.dll", EntryPoint = "??0CIntelligentCooling@PowerBattery@@QEAA@XZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern CIntelligentCooling CIntelligentCooling(ref CIntelligentCooling var1);


        [DllImport("PowerBattery.dll", EntryPoint = "?GetITSMode@CIntelligentCooling@PowerBattery@@QEAAHAEAHAEAW4ITSMode@12@@Z", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int GetITSMode(ref CIntelligentCooling var1, ref int var2, ref PowerPlan var3);


        [DllImport("PowerBattery.dll", EntryPoint = "??0CChargingMode@PowerBattery@@QEAA@XZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern CChargingMode CChargingMode(ref CChargingMode var1);


        [DllImport("PowerBattery.dll", EntryPoint = "?GetChargingMode@CChargingMode@PowerBattery@@QEBAHXZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int GetChargingMode(ref CChargingMode var1);

        [DllImport("PowerBattery.dll", EntryPoint = "?SetChargingMode@CChargingMode@PowerBattery@@QEBAHH@Z", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int SetChargingMode(ref CChargingMode var1, int var2);

        [DllImport("PowerBattery.dll", EntryPoint = "?SetChargingMode@CChargingMode@PowerBattery@@QEBAHH_N@Z", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        // I don't know what the newly added boolean does, but it doesn't seem to matter
        internal static extern int SetChargingModeFallBack(ref CChargingMode var1, int var2, bool var3);

        [DllImport("PowerBattery.dll", EntryPoint = "??0CUSBCharger@PowerBattery@@QEAA@XZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern CUSBCharger CUSBCharger(ref CUSBCharger var1);

        [DllImport("PowerBattery.dll", EntryPoint = "??0CUSBBatteryCharger@PowerBattery@@QEAA@XZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern CUSBBatteryCharger CUSBBatteryCharger(ref CUSBBatteryCharger var1);

        [DllImport("PowerBattery.dll", EntryPoint = "?OpenOrClose@CUSBBatteryCharger@PowerBattery@@UEAAHXZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int OpenOrClose(ref CUSBBatteryCharger var1);

        [DllImport("PowerBattery.dll", EntryPoint = "?OpenOrClose@CUSBCharger@PowerBattery@@UEAAHXZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int OpenOrClose(ref CUSBCharger var1);

        [DllImport("PowerBattery.dll", EntryPoint = "?OpenFeature@CUSBCharger@PowerBattery@@UEAAHXZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int OpenFeature(ref CUSBCharger var1);

        [DllImport("PowerBattery.dll", EntryPoint = "?OpenFeature@CUSBBatteryCharger@PowerBattery@@UEAAHXZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int OpenFeature(ref CUSBBatteryCharger var1);

        [DllImport("PowerBattery.dll", EntryPoint = "?CloseFeature@CUSBCharger@PowerBattery@@UEAAHXZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int CloseFeature(ref CUSBCharger var1);

        [DllImport("PowerBattery.dll", EntryPoint = "?CloseFeature@CUSBBatteryCharger@PowerBattery@@UEAAHXZ", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern int CloseFeature(ref CUSBBatteryCharger var1);


        public PowerPlan GetPowerPlan()
        {
            var instance = new CIntelligentCooling();
            instance = CIntelligentCooling(ref instance);
            var newmode = new PowerPlan();
            int trash = 0;
            _ = GetITSMode(ref instance, ref trash, ref newmode);
            return newmode;
        }

        public void SetPowerPlan(PowerPlan plan)
        {
            var instance = new CIntelligentCooling();
            instance = CIntelligentCooling(ref instance);
            _ = SetITSMode(ref instance, ref plan);
        }

        public ChargingMode GetChargingMode()
        {
            var instance = new CChargingMode();
            instance = CChargingMode(ref instance);
            ChargingMode mode = (ChargingMode)(GetChargingMode(ref instance));
            return mode;
        }

        public void SetChargingMode(ChargingMode chargingMode)
        {
            var instance = new CChargingMode();
            instance = CChargingMode(ref instance);
            try
            {
                _ = SetChargingMode(ref instance, (int)chargingMode);
            }
            catch (SystemException)
            {
                _ = SetChargingModeFallBack(ref instance, (int)chargingMode, false);
            }
        }

        public bool IsAlwaysOnUsbEnabled()
        {
            var instance = new CUSBCharger();
            instance = CUSBCharger(ref instance);
            var res = (OpenOrClose(ref instance));
            return res == 1;
        }

        public void SetAlwaysOnUsb(bool alwaysOnUsbEnabled)
        {
            var instance = new CUSBCharger();
            instance = CUSBCharger(ref instance);
            switch (alwaysOnUsbEnabled)
            {
                case true:
                    _ = OpenFeature(ref instance);
                    break;
                case false:
                    _ = CloseFeature(ref instance);
                    break;
            }
        }

        public bool IsAlwaysOnUsbBatteryEnabled()
        {
            var instance = new CUSBBatteryCharger();
            instance = CUSBBatteryCharger(ref instance);
            var res = (OpenOrClose(ref instance));
            return res == 1;
        }

        public void SetAlwaysOnUsbBattery(bool alwaysOnUsbBattryEnabled)
        {
            var instance = new CUSBBatteryCharger();
            instance = CUSBBatteryCharger(ref instance);
            switch (alwaysOnUsbBattryEnabled)
            {
                case true:
                    _ = OpenFeature(ref instance);
                    break;
                case false:
                    _ = CloseFeature(ref instance);
                    break;
            }
        }
    }
}
