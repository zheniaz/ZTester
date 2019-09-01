using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTester.Services;
using System.Threading;
using Util;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace ZTester.Tests
{
    class SleepTest
    {
        private CMDService _cmdService = new CMDService();
        private FileService _fileService = new FileService();

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
            
            IntPtr ptr;
            bool logonUser = LogonUser("\\\\spsrv\\public /u:ntdev\\texas", System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName, "Tp3Rh4uwLF!#!yO", 9, 0, out ptr);

            Console.WriteLine();
            Console.WriteLine("logonUser: " + logonUser);
            Console.WriteLine();


            WindowsIdentity windowsIdentity = new WindowsIdentity(ptr);
            var impersonationContext = windowsIdentity.Impersonate();
            
            string PROCESSOR_ARCHITECTURE = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            string sourcePath = $@"\\spsrv\Public\Base-11B\RS3\Pwtest\{PROCESSOR_ARCHITECTURE}\";
            string targetPath = $"{systemDrive}\\TestBin\\";
            _fileService.CopyFiles(sourcePath, targetPath);
            Thread.Sleep(30000);

            _cmdService.RunCMDCommand("/sleep /c:4 /p:120 /d:150 /s:all", "pwrtest.exe", testBinPath);

            impersonationContext.Undo();
        }
    }
}
