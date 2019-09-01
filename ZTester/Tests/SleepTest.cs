using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using ZTester.Services;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using Microsoft.WindowsAPICodePack.Net;
using ZTester.models;

namespace ZTester.Tests
{
    class SleepTest
    {
        private CMDService _cmdService = new CMDService();
        private FileService _fileService = new FileService();
        private XMLService _xmlService = new XMLService();

        [DllImport("advapi32.dll")]
        public static extern bool LogonUser(string name, string domain, string pass, int logType, int logpv, out IntPtr pht);

        public void StartTest()
        {
            //_cmdService.RunCMDCommand("https://www.youtube.com/watch?v=_U24PBWOpjM", fileName: "iexplore.exe", waitForExit: false);
            //Thread.Sleep(30000);

            //_cmdService.RunCMDCommand("https://www.youtube.com/watch?v=jj_Mei27E7Q", fileName: "iexplore.exe", waitForExit: false);
            //Thread.Sleep(30000);

            //_cmdService.RunCMDCommand("https://www.youtube.com/watch?v=5TcOvHigjYE", fileName: "iexplore.exe", waitForExit: false);
            //Thread.Sleep(30000);

            string systemDrive = _fileService.GetPathRoot(Environment.SystemDirectory);
            string testBinPath = systemDrive + "TestBin";
            bool isTestBinExist = _fileService.CheckIfFileExists(testBinPath);
            if (_fileService.CheckIfFileExists(testBinPath))
            {
                _fileService.RemoveFile(systemDrive, testBinPath, isTestBinExist);
            }
            else
            {
                _fileService.CreateDirectory(testBinPath);
            }
;
            //_cmdService.RunCMDCommand("net use * \\\\spsrv\\public /u:ntdev\\texas \"Tp3Rh4uwLF!#!yO\"");

            //IntPtr ptr;
            //bool logonUser = LogonUser("\\\\spsrv\\public /u:ntdev\\texas", "redmond.corp.microsoft.com", "Tp3Rh4uwLF!#!yO", 9, 0, out ptr);

            //Console.WriteLine("System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName: " + System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName);
            //Console.WriteLine("logonUser: " + logonUser);
            //Console.WriteLine();

            //WindowsIdentity windowsIdentity = new WindowsIdentity(ptr);
            //var impersonationContext = windowsIdentity.Impersonate

            string desktopPath = $"C:\\Users\\{Environment.UserName}\\Desktop";
            string ZTesterConfigFilePath = $"{desktopPath}\\ZTester.config";
            List<NetworkSettings> netSettingsList = _xmlService.GetZTesterConfigData(ZTesterConfigFilePath);
            NetworkSettings netSettings = netSettingsList.Find(s => s.SettingName == "SleepTest");
            var networks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);

            Console.WriteLine("Available networks:");
            foreach (var item in networks)
            {
                Console.WriteLine("Name: " + item.Description);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            NetworkCredential credential = new NetworkCredential(netSettings.UserName, netSettings.Password, netSettings.Domain);
            using (new Services.NetworkConnection(netSettings.NetworkName, credential))
            {
                string PROCESSOR_ARCHITECTURE = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
                string sourcePath = $@"\\spsrv\Public\Base-11B\RS3\Pwtest\{PROCESSOR_ARCHITECTURE}\";
                string targetPath = $"{systemDrive}\\TestBin\\";
                _fileService.CopyFiles(sourcePath, targetPath);
            }

            Thread.Sleep(30000);

            _cmdService.RunCMDCommand("/sleep /c:4 /p:120 /d:150 /s:all", "pwrtest.exe", testBinPath);

            #region test region

            var Testnetworks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);

            #endregion
        }
    }
}
