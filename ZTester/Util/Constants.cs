using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class Constants
    {
        public const string AppName = "ZTester.exe";
        public const string LogFileName = "rebootLog.txt";
        public const string RebootLoopShortcutName = "ZTester - Shortcut";
        public const string ZTesterConfigName = "ZTester.config";
        public const string ZTestSettingConfigName = "ZTestSetting.config";
        public const string KernelStressName = "ksk.cmd";
        public const string TestBinPath = @"C:\TestBin";

        #region PowerShell scripts

        public const string checkIsHVCIrunning = "# begin\r\nfunction CheckDGRunning($_val)\r\n{\r\n    $DGObj = Get-CimInstance -classname Win32_DeviceGuard -namespace root\\Microsoft\\Windows\\DeviceGuard\r\n    for($i=0; $i -lt $DGObj.SecurityServicesRunning.length; $i++)\r\n    {\r\n        if($DGObj.SecurityServicesRunning[$i] -eq $_val)\r\n        {\r\n            return 1\r\n        }\r\n\r\n    }\r\n    return 0\r\n}\r\n\r\nif (CheckDGRunning(2) -eq 1)\r\n{\r\n    Write-Output \"HVCI is running\"\r\n}\r\nelse\r\n{\r\n    Write-Output \"HVCI is NOT running\" \r\n}\r\n# end";

        #endregion
    }

    public enum TestType { None, RebootSystemTest, SleepTest, DebuggerSetting, WSHTest, KernelStressTest };
    public enum SystemType { AMD64, x86 };
}
