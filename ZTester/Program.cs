using System;
using System.Security.Principal;
using System.Threading;
using ZTester.CMDCommands;
using ZTester.Services;
using ZTester.Tests;
using System.Collections.ObjectModel;
using Util;

namespace ZTester
{
    class Program
    {

        static InputService _inputService = new InputService();
        static RebootSystem _rebootSystem = new RebootSystem();

        static WSHTester _wshTester = new WSHTester();
        static SleepTest _sleepTest = new SleepTest();
        static DebuggerSettingTest _debuggerSetting = new DebuggerSettingTest();
        static KernelStressTest _kernelStressTest = new KernelStressTest();

        static CMDService _cmdService = new CMDService();
        static LogInService _logInService = new LogInService();
        static TestEnvironment _testEnvironment = new TestEnvironment();
        static PowerShellService powerShellService = new PowerShellService();
        static PowerShellService _powershellService = new PowerShellService();



        static void Main(string[] args)
        {
            #region testing region, remove after all
            //_kernelStressTest.StartTest();

            //_logInService.IsUserAdministrator();
            //_cmdService.RunCMDCommand($@"ADD HKLM\SYSTEM\CurrentControlSet\Control\CrashControl /v CrashDumpEnabled /t REG_DWORD /d 1 /f", fileName: "reg");
            //_cmdService.RunCMDCommand($@"ADD HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon /v AutoAdminLogon /t REG_SZ /d 1 /f", fileName: "reg");
            ////_hyperThreading.TurnOFFHyperThreading();

            //System.Diagnostics.ProcessStartInfo processprop = new System.Diagnostics.ProcessStartInfo();
            //processprop.FileName = "cmd";
            //processprop.Arguments = "REG ADD HKLM\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon /f /v AutoAdminLogon /t REG_SZ /d 1";
            //processprop.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;  
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            //process = System.Diagnostics.Process.Start(processprop);
            //return;

            #endregion


            _logInService.IsUserAdministrator();
            //Console.WriteLine("ZTester, (c) 2019 ZHENIA Zgurovets Inc.");

            if (_rebootSystem.RebootCount > 10)
            {
                return;
            }

            if (_rebootSystem.IsLogFileExists)
            {
                _rebootSystem.Reboot();
            }
            else
            {
                SelectTestPoint();
            }
        }

        #region Input Hadler Region

        public static void SelectTestPoint()
        {
            Console.WriteLine(@"Please select a test:
    1.  Reboot System Test
    2.  Sleep Test
    3.  Set Debugger
    4.  WSHTester 
    5.  KernelStress Test

Setting The Environment:
    11. Turn ON HVCI
    12. Turn OFF HVCI 
    21. Turn ON HyperThreading 
    22. Turn OFF HyperThreading 
    31. Enable Windows Update
    32. Disable Windows Update 
    41. Turn ON M5M11 
    42. Turn OFF M5M11

Checking The Environment:
    100. Check is HVCI running

    0.  To quit");
            int selectedTest = _inputService.SelectNumberFromTheRange(0, 100);

            switch (selectedTest)
            {
                case 1:
                    {
                        _rebootSystem.StartTest();
                        break;
                    }

                case 2:
                    {
                        _sleepTest.StartTest();
                        break;
                    }

                case 3:
                    {
                        _debuggerSetting.StartTest();
                        break;
                    }

                case 4:
                    {
                        _wshTester.StartTest();
                        break;
                    }

                case 5:
                    {
                        _kernelStressTest.StartTest();
                        break;
                    }

                case 11:
                    {
                        _testEnvironment.TurnOnHVCI();
                        break;
                    }

                case 12:
                    {
                        _testEnvironment.TurnOffHVCI();
                        break;
                    }

                case 21:
                    {
                        _testEnvironment.TurnOnHyperThreading();
                        break;
                    }

                case 22:
                    {
                        _testEnvironment.TurnOFFHyperThreading();
                        break;
                    }

                case 31:
                    {
                        _testEnvironment.EnableWindowsUpdate();
                        break;
                    }

                case 32:
                    {
                        _testEnvironment.DisableWindowsUpdate();
                        break;
                    }

                case 41:
                    {
                        _testEnvironment.TurnOnM5M11();
                        break;
                    }

                case 42:
                    {
                        _testEnvironment.TurnOFFM5M11();
                        break;
                    }

                case 100:
                    {
                        _powershellService.ExecutePowerShellScript(Constants.checkIsHVCIrunning);
                        break;
                    }

                case 0:
                    {
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        #endregion
    }
}
