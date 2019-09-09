using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZTester.Services;

namespace ZTester.Tests
{
    class DebuggerSettingTest
    {
        private CMDService _cmdService = new CMDService();
        private InputService _inputService = new InputService();

        public void StartTest()
        {
            _cmdService.RunCMDCommand($@"ADD HKLM\SYSTEM\CurrentControlSet\Control\CrashControl /v CrashDumpEnabled /t REG_DWORD /d 1 /f", fileName: "reg");
            _cmdService.RunCMDCommand("bcdedit /set {bootmgr} testsigning on");
            _cmdService.RunCMDCommand("bcdedit /set {bootmgr} bootdebug on");
            _cmdService.RunCMDCommand("bcdedit /set testsigning on");
            _cmdService.RunCMDCommand("bcdedit /bootdebug on");
            _cmdService.RunCMDCommand("bcdedit /debug on");


            //var cmdFullFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
            //                                   Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess
            //                                       ? @"Sysnative\cmd.exe"
            //                                       : @"System32\cmd.exe");
            var cmdFullFileName = Environment.SystemDirectory + @"\";


            List<string> arguments = new List<string>()
            {
                "bcdedit /set {bootmgr} testsigning on",
                "bcdedit /set {bootmgr} bootdebug on",
                "bcdedit /set testsigning on",
                "bcdedit /bootdebug on",
                "bcdedit /debug on",
            };
            //List<string> arguments = new List<string>()
            //{
            //    "/set {bootmgr} testsigning on",
            //    "/set {bootmgr} bootdebug on",
            //    "/set testsigning on",
            //    "/bootdebug on",
            //    "/debug on",
            //};

            foreach (var item in arguments)
            {
                _cmdService.RunCMDCommand(item);
            }

            string hostIPDerault = "10.216.79.136";

            Console.WriteLine("Enter hostIP (Example: 10.216.79.136) ");
            string hostIP = _inputService.ReadIPAddress();
            if (hostIP == "")
            {
                hostIP = hostIPDerault;
                Console.WriteLine(hostIPDerault);
            }
            
            Console.WriteLine("Enter port number (Example: 1) ");
            string portNumber = _inputService.ReadPortNumberBetween("The port must be greater than 49151 and less than 65536.", 49151, 65536);
            string key = $@"x.y.z.{portNumber}";
            Console.WriteLine("port: " + portNumber);
            Console.WriteLine("key:  " + key);

            _cmdService.RunCMDCommand($@"/dbgsettings net hostip:{hostIP} port:{portNumber} key:{key}", fileName: Environment.SystemDirectory + "\\bcdedit.exe"); //            bcdedit /dbgsettings
        }
    }
}
