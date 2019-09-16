using System.Collections.Generic;
using System;
using ZTester.Services;
using System.Threading;
using ZTester.models;
using System.Diagnostics;
using Util;
using ZTester.Interfaces;

namespace ZTester.Tests
{
    class SleepTest : IZTester
    {
        private CMDService _cmdService = new CMDService();
        private FileService _fileService = new FileService();
        private XMLService _xmlService = new XMLService();
        private RemoteConnectionService _remoteConnectionService = new RemoteConnectionService();

        public void StartTest()
        {
            if (_fileService.IsShortcutOfRebootLoopExists)
            {
                _fileService.RemoveShortcutFromStartup();
            }

            _cmdService.RunCMDCommand("https://www.youtube.com/watch?v=_U24PBWOpjM", fileName: "iexplore.exe", waitForExit: false);
            Thread.Sleep(30000);

            _cmdService.RunCMDCommand("https://www.youtube.com/watch?v=jj_Mei27E7Q", fileName: "iexplore.exe", waitForExit: false);
            Thread.Sleep(30000);

            _cmdService.RunCMDCommand("https://www.youtube.com/watch?v=5TcOvHigjYE", fileName: "iexplore.exe", waitForExit: false);
            Thread.Sleep(30000);
            

            ZTestSettingModel testSettingModel = new ZTestSettingModel();
            testSettingModel = _xmlService.GetZTestSetting(TestType.SleepTest);
            string TestfilePath = testSettingModel.TestFileFullPath;

            NetworkSettingsModel networkSetting = _xmlService.GetNetworkSettings(TestType.KernelStressTest);

            if (TestfilePath == null || TestfilePath == "")
            {
                Console.WriteLine("The TestFileFullPath property does not setted, please open ZTester.config file and set TestFileLocation for SleepTest section");
                return;
            }

            bool result = _remoteConnectionService.MapDrive("M", networkSetting.TestFileLocation, networkSetting.Domain + @"\" + networkSetting.UserName, networkSetting.Password);




            string systemDrive = _fileService.GetPathRoot(Environment.SystemDirectory);
            string testBinPath = systemDrive + "TestBin";
            bool isTestBinExist = _fileService.CheckIfDirectoryExists(testBinPath);
            bool isSleepTestFileExist = false;
            if (!isTestBinExist)
            {
                _fileService.CreateDirectory(Constants.TestBinPath);
                Process.Start("cmd.exe", @"/c xcopy M:\pwrtest.exe c:\TestBin\* /s /y");
            }
            else
            {
                isSleepTestFileExist = _fileService.CheckIfFileExists(testBinPath);
                if (!isSleepTestFileExist)
                {
                    Process.Start("cmd.exe", @"/c xcopy M:\pwrtest.exe c:\TestBin\* /s /y");
                }
            }



            Thread.Sleep(30000);

            _cmdService.RunCMDCommand("/sleep /c:4 /p:120 /d:150 /s:all", "pwrtest.exe", testBinPath);

            FinishTest();
        }

        public void SetTheEnvironment()
        {
            ZTestSettingModel testSetting = new ZTestSettingModel()
            {
                TestName = TestType.SleepTest.ToString(),
                NeedToRunTimes = 1,
                IsSettedEnvironment = true
            };
            _xmlService.EditZTestSetting(testSetting);
            StartTest();
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
