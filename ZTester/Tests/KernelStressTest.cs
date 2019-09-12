using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using ZTester.TestEnvironmentSpaceName;
using ZTester.models;
using ZTester.Services;
using ZTester.Interfaces;
using System.Threading;

namespace ZTester.Tests
{
    public class KernelStressTest : IZTester
    {
        private InputService _inputService = new InputService();
        private XMLService _xmlService = new XMLService();
        private FileService _fileService = new FileService();
        private RemoteConnectionService _remoteConnectionService = new RemoteConnectionService();
        private CMDService _runCMDCommand = new CMDService();
        private RebootSystemTest _rebootSystem = new RebootSystemTest();
        private TestEnvironment _testEnvironment = new TestEnvironment();
        private LogInService _logInService = new LogInService();
        private CMDService _cmdService = new CMDService();


        public void StartTest()
        {
            _fileService.RemoveShortcutFromStartup();

            ZTestSettingModel testSettingModel = new ZTestSettingModel();
            testSettingModel = _xmlService.GetZTestSetting(TestType.KernelStressTest);
            string KSKfilePath = testSettingModel.TestFileFullPath;

            NetworkSettingsModel networkSetting = _xmlService.GetNetworkSettings(TestType.KernelStressTest);

            bool result = _remoteConnectionService.MapDrive("M", KSKfilePath, networkSetting.Domain + @"\" + networkSetting.UserName, networkSetting.Password);

            int testSettingsCount = _xmlService.GetZTestSettingCount();
            if (testSettingsCount == 1)
            {
                _fileService.RemoveFile(_fileService.AppPath, _fileService.ZTestSettingXMLfullPath, _fileService.IsZTestSettingXMLExists);
            }
            else
            {
                _xmlService.RemoveZtestSetting(TestType.KernelStressTest);
            }

            if (result)
            {
                Console.WriteLine("cmd.exe", @"/c M:\ksk.cmd");
                Process.Start("cmd.exe", @"/c M:\ksk.cmd");
            }


            // Implement this for other tests and remove

            //bool isTestBinExists = _fileService.CheckIfDirectoryExists(Constants.TestBinPath);
            //bool isKernelStressFileExists = false;
            //if (!isTestBinExists)
            //{
            //    _fileService.CreateDirectory(Constants.TestBinPath);
            //    Process.Start("cmd.exe", @"/c xcopy M:\pwrtest.exe c:\TestBin\* /s /y");
            //}
            //else
            //{
            //    isKernelStressFileExists = _fileService.CheckIfFileExists(Constants.TestBinPath);
            //    if (!isKernelStressFileExists)
            //    {
            //        Process.Start("cmd.exe", @"/c xcopy M:\pwrtest.exe c:\TestBin\* /s /y");
            //    }
            //}


        }

        public void SetTheEnvironment(/*int priority = 1*/)
        {
            //1. Enable complete memory dump
            _testEnvironment.EnableCompleteMemoryDump();

            //2. Create XML file 
            ZTestSettingModel testSettingModel = new ZTestSettingModel()
            {
                TestName = TestType.KernelStressTest.ToString(),
                NeedToRunTimes = 1,
                //Priority = priority,
                IsSettedEnvironment = true
            };
            string targetXMLPath = _fileService.AppPath + "\\" + Constants.ZTestSettingConfigName;

            Console.WriteLine("Please select from RS1 to RS5:");
            string windowsBuild = _inputService.ReadRSVersion().ToUpper();
            string operatingSystem = Environment.Is64BitOperatingSystem ? SystemType.AMD64.ToString().ToLower() : SystemType.x86.ToString();
            string KSKfilePath = $@"\\winbuilds\release\Milestone\{windowsBuild}\RTM\{operatingSystem}fre\bin\kernel\kstress";
            testSettingModel.TestFileFullPath = KSKfilePath;

            List<ZTestSettingModel> ztestSettingLins = new List<ZTestSettingModel>() { testSettingModel };
            if (_fileService.CheckIfFileExists(targetXMLPath))
            {
                _xmlService.AddToExistingTestSettingConfigFile(ztestSettingLins);
            }
            else
            {
                _xmlService.SerializeToXML(ztestSettingLins);
            }

            //3. reboot the system
            _logInService.EnableAutoLogIn();
            _fileService.CreateShortcutInStartupFolder();
            _rebootSystem.ShutDownTheSystem();
        }
    }
}
