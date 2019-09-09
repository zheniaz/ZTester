using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTester.Services;

namespace ZTester.CMDCommands
{
    class TestEnvironment
    {
        CMDService _cmdService = new CMDService();
        RegistryKeyService _registryKeyService = new RegistryKeyService();

        public void TurnOnHVCI()
        {
            _cmdService.RunCMDCommand($@"add HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard /v EnableVirtualizationBasedSecurity /t REG_DWORD /d 1 /f", fileName: "reg");
            _cmdService.RunCMDCommand($@"add HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard /v HyperVVirtualizationBasedSecurityOptout /t REG_DWORD /d 0 /f", fileName: "reg");
            _cmdService.RunCMDCommand($@"add HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard /v RequirePlatformSecurityFeatures /t REG_DWORD /d 0 /f", fileName: "reg");
            _cmdService.RunCMDCommand($@"add HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard /v HypervisorEnforcedCodeIntegrity /t REG_DWORD /d 1 /f", fileName: "reg");
        }

        public void TurnOffHVCI()
        {
            string keyPath = @"SYSTEM\CurrentControlSet\Control\DeviceGuard";
            List<string> keys = new List<string>()
            {
                "EnableVirtualizationBasedSecurity",
                "HyperVVirtualizationBasedSecurityOptout",
                "RequirePlatformSecurityFeatures",
                "HypervisorEnforcedCodeIntegrity"
            };

            foreach (var key in keys)
            {
                _registryKeyService.DeleteRegistryKey(keyPath, key);
            }
        }

        public void TurnOFFHyperThreading()
        {
            _cmdService.RunCMDCommand($"add \"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management\" /v FeatureSettingsOverride /t REG_DWORD /d 8192 /f", fileName: "reg");
            _cmdService.RunCMDCommand($"add \"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management\" /v FeatureSettingsOverrideMask /t REG_DWORD /d 3 /f", fileName: "reg");
        }

        public void TurnOnHyperThreading()
        {
            string keyPath = "SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management";
            List<string> keys = new List<string>()
            {
                "FeatureSettingsOverride",
                "FeatureSettingsOverrideMask",
            };

            foreach (var key in keys)
            {
                _registryKeyService.DeleteRegistryKey(keyPath, key);
            }
        }

        public void EnableWindowsUpdate()
        {
            _cmdService.RunCMDCommand("config wuauserv start= demand", fileName: "sc");
            _cmdService.RunCMDCommand("start wuauserv", fileName: "sc");
        }

        public void DisableWindowsUpdate()
        {
            _cmdService.RunCMDCommand("config wuauserv start= disabled", fileName: "sc");
            _cmdService.RunCMDCommand("stop wuauserv", fileName: "sc");
        }

        public void TurnOFFM5M11()
        {
            _cmdService.RunCMDCommand($"add \"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management\" /v FeatureSettingsOverride /t REG_DWORD /d 3 /f", fileName: "reg");
            _cmdService.RunCMDCommand($"add \"HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management\" /v FeatureSettingsOverrideMask /t REG_DWORD /d 3 /f", fileName: "reg");
        }

        public void TurnOnM5M11()
        {
            string keyPath = "SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management";
            List<string> keys = new List<string>()
            {
                "FeatureSettingsOverride",
                "FeatureSettingsOverrideMask",
            };

            foreach (var key in keys)
            {
                _registryKeyService.DeleteRegistryKey(keyPath, key);
            }
        }
    }
}
