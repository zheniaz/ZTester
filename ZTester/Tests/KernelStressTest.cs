using System;
using System.Diagnostics;
using Util;
using ZTester.TestEnvironmentSpaceName;
using ZTester.models;
using ZTester.Services;
using ZTester.Interfaces;

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

            if (result)
            {
                Console.WriteLine("cmd.exe", @"/c M:\ksk.cmd");
                Process.Start("cmd.exe", @"/c M:\ksk.cmd");
            }
        }

        public void SetTheEnvironment()
        {
            //1. Enable complete memory dump
            _testEnvironment.EnableCompleteMemoryDump();

            //2. Create XML file 
            ZTestSettingModel testSettingModel = new ZTestSettingModel()
            {
                TestName = TestType.KernelStressTest.ToString(),
                NeedToRunTimes = 1,
                IsSettedEnvironment = true
            };
            string targetXMLPath = _fileService.AppPath + "\\" + Constants.ZTestSettingConfigName;

            Console.WriteLine("Please select from RS1 to RS5:");
            string windowsBuild = _inputService.ReadRSVersion().ToUpper();
            string operatingSystem = Environment.Is64BitOperatingSystem ? SystemType.AMD64.ToString().ToLower() : SystemType.x86.ToString();
            string KSKfilePath = $@"\\winbuilds\release\Milestone\{windowsBuild}\RTM\{operatingSystem}fre\bin\kernel\kstress";
            testSettingModel.TestFileFullPath = KSKfilePath;

            _xmlService.EditZTestSetting(testSettingModel);

            //3. reboot the system
            _logInService.EnableAutoLogIn();
            _fileService.CreateShortcutInStartupFolder();
            _rebootSystem.ShutDownTheSystem();
        }

        public void FinishTest()
        {
            _xmlService.RemoveZtestSetting(TestType.RebootSystemTest);

            if (_xmlService.GetZTestSettingCount() > 0)
            {
                Program.SetConfiguration();
            }
        }
    }
}
