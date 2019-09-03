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
using System.Diagnostics;
using System.Management;

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
            _fileService.CreateDirectory(testBinPath);

            string desktopPath = $@"C:\Users\{Environment.UserName}\Desktop";
            string ZTesterConfigFilePath = $@"{desktopPath}\ZTester.config";

            List<NetworkSettings> networkSettingsList = new List<NetworkSettings>();
            _xmlService.GetZTesterConfigData(ref networkSettingsList, ZTesterConfigFilePath);
            NetworkSettings networkSettings = networkSettingsList.Find(s => s.SettingName == "SleepTest");

            string PROCESSOR_ARCHITECTURE = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            string sourcePath = $@"{networkSettings.SleepTestFileLocation}\{PROCESSOR_ARCHITECTURE}\";
            string targetPath = $@"{systemDrive}\TestBin\";

            string arguments = $"/generic:{networkSettings.URL} /user:{networkSettings.UserName} /pass:{networkSettings.Password}";
            //string arguments = $"net use * {netSettings.URL} {netSettings.UserName} \"{netSettings.Password}\"";
            _cmdService.RunCMDCommand(arguments, fileName: @"C:\system32\mstsc.exe");
            _fileService.CopyFiles(sourcePath, targetPath);

            #region Network Region

            //IntPtr ptr;
            //bool logonUser = LogonUser("\\\\spsrv\\public /u:ntdev\\texas", "redmond.corp.microsoft.com", "Tp3Rh4uwLF!#!yO", 9, 0, out ptr);

            //Console.WriteLine("System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName: " + System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName);
            //Console.WriteLine("logonUser: " + logonUser);
            //Console.WriteLine();

            //WindowsIdentity windowsIdentity = new WindowsIdentity(ptr);
            //var impersonationContext = windowsIdentity.Impersonate







            //List<NetworkSettings> netSettingsList = _xmlService.GetZTesterConfigData(ZTesterConfigFilePath);
            //NetworkSettings netSettings = netSettingsList.Find(s => s.SettingName == "SleepTest");
            //var networks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);
            //bool isHasMicrosoftNetwork = false;



            //Console.WriteLine("Available networks:");
            //foreach (var item in networks)
            //{
            //    if (item.Name == netSettings.NetworkName)
            //    {
            //        isHasMicrosoftNetwork = true;
            //    }
            //}

            //if (isHasMicrosoftNetwork == false)
            //{
            //    throw new Exception("You do not have the corp.microsoft.com network. Connect And try again.");
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();

            //Process rdcProcess = new Process();

            //string executable = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\mstsc.exe");
            //if (executable != null)
            //{
            //    rdcProcess.StartInfo.FileName = executable;
            //    rdcProcess.StartInfo.Arguments = "/v " + netSettings.UserName;  // ip or name of computer to connect
            //    rdcProcess.Start();
            //}

            //ConnectionOptions connOptions = new ConnectionOptions()
            //{
            //    Username = netSettings.UserName,
            //    Password = netSettings.Password,
            //    EnablePrivileges = true,
            //    Impersonation = ImpersonationLevel.Impersonate
            //};



            //ManagementScope scope = new ManagementScope($@"\\spsrv\Public\Base-11B\RS3\Pwtest", connOptions);
            //scope.Connect();

            //NetworkCredential credential = new NetworkCredential(netSettings.UserName, netSettings.Password, netSettings.Domain);

            //using (new Services.NetworkConnection(netSettings.NetworkName, credential))
            //{

            //}

            #endregion



            Thread.Sleep(30000);

            //_cmdService.RunCMDCommand("/sleep /c:4 /p:120 /d:150 /s:all", "pwrtest.exe", testBinPath);

            #region test region

            var Testnetworks = NetworkListManager.GetNetworks(NetworkConnectivityLevels.Connected);

            #endregion
        }

        public void SetConnection()
        {

        }

        public void SetConnectio2()
        {

        }

        public void SetConnection3()
        {

        }

        public void SetConnection4()
        {

        }
    }
}
