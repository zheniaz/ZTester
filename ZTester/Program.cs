using System;
using System.Security.Principal;
using System.Threading;
using ZTester.TestEnvironmentSpaceName;
using ZTester.Services;
using ZTester.Tests;
using System.Collections.ObjectModel;
using Util;
using ZTester.Interfaces;
using System.Linq;
using ZTester.models;
using System.Collections.Generic;

namespace ZTester
{
    class Program
    {
        static XMLService _xmlService = new XMLService();
        static RebootSystemTest _rebootSystem = new RebootSystemTest();

        static WSHTester _wshTester = new WSHTester();
        static SleepTest _sleepTest = new SleepTest();
        static DebuggerSettingTest _debuggerSetting = new DebuggerSettingTest();
        static KernelStressTest _kernelStressTest = new KernelStressTest();

        static InputService _inputService = new InputService();
        static CMDService _cmdService = new CMDService();
        static LogInService _logInService = new LogInService();
        static TestEnvironment _testEnvironment = new TestEnvironment();
        static PowerShellService powerShellService = new PowerShellService();
        static PowerShellService _powershellService = new PowerShellService();
        static FileService _fileService = new FileService();

        static IZTester zTester = null;

        static 

        Program()
        {
            
        }

        static void Main(string[] args)
        {
            #region Prod Region

            _logInService.IsUserAdministrator();
            if (_fileService.CheckIfFileExists(_fileService.AppPath + "\\" + Constants.ZTestSettingConfigName))
            {
                SetConfiguration();
            }
            else
            {
                SelectTestPoint();
            }

            #endregion

            #region PreProd Region

            #endregion

            #region testing region, remove after all

            #endregion


            
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
    51. Enable AutoAdminLogon
    52. Disable AutoAdminLogon

Checking The Environment:
    100. Check is HVCI running

    0.  To quit");
            int selectedTest = -1;  // _inputService.SelectNumberFromTheRange(0, 100);

            while (selectedTest != 0)
            {
                selectedTest = _inputService.SelectNumberFromTheRange(0, 100);
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
                            _kernelStressTest.SetTheEnvironment();
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

                    case 51:
                        {
                            _logInService.EnableAutoLogIn();
                            break;
                        }

                    case 52:
                        {
                            _logInService.DisableAutoLogIn();
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
        }

        #endregion

        #region Seting ZTestSetting.Configuration

        static void SetConfiguration()
        {
            List<ZTestSettingModel> testConfigurationList = _xmlService.GetZTestSettingsList();

            TestType testType = (TestType)Enum.Parse(typeof(TestType), testConfigurationList.FirstOrDefault().TestName);

            switch (testType)
            {
                case TestType.None:
                    break;
                case TestType.RebootSystemTest:
                    zTester = new RebootSystemTest();
                    break;
                case TestType.SleepTest:
                    zTester = new SleepTest();
                    break;
                case TestType.WSHTest:
                    zTester = new WSHTester();
                    break;
                case TestType.KernelStressTest:
                    zTester = new KernelStressTest();
                    break;
                default:
                    break;
            }

            zTester.StartTest();
        }

        #endregion
    }
}
