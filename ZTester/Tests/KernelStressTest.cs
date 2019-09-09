using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using ZTester.models;
using ZTester.Services;

namespace ZTester.Tests
{
    public class KernelStressTest
    {
        private InputService _inputService = new InputService();
        private XMLService _xmlService = new XMLService();
        private FileService _fileService = new FileService();
        private RemoteConnectionService _remoteConnectionService = new RemoteConnectionService();

        public void StartTest()
        {
            // implement remote connection

            Console.WriteLine("Please select from RS1 to RS5:");
            string windowsBuild = _inputService.ReadRSVersion().ToUpper();
            string operatingSystem = Environment.Is64BitOperatingSystem ? SystemType.AMD64.ToString() : SystemType.x86.ToString();
            string source = source = $@"\\spsrv\Public\Base-11B\{windowsBuild}\Pwtest\{operatingSystem}";
            
            NetworkSettingsModel networkSetting = _xmlService.GetNetworkSettings(TestType.KernelStressTest);

            bool result = _remoteConnectionService.MapDrive("M", source, networkSetting.Domain + @"\" + networkSetting.UserName, networkSetting.Password);
            

            // Implement reusable functionality for next steps:
            bool isTestBinExists = _fileService.CheckIfDirectoryExists(Constants.TestBinPath);
            bool isKernelStressFileExists = false;
            if (!isTestBinExists)
            {
                _fileService.CreateDirectory(Constants.TestBinPath);
                Process.Start("cmd.exe", @"/c xcopy M:\pwrtest.exe c:\TestBin\* /s /y");
            }
            else
            {
                isKernelStressFileExists = _fileService.CheckIfFileExists(Constants.TestBinPath);
                if (!isKernelStressFileExists)
                {
                    Process.Start("cmd.exe", @"/c xcopy M:\pwrtest.exe c:\TestBin\* /s /y");
                }
            }

                
        }
    }
}
