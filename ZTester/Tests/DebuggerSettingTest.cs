using System;
using System.Threading;
using Util;
using ZTester.Interfaces;
using ZTester.models;
using ZTester.Services;

namespace ZTester.Tests
{
    class DebuggerSettingTest : IZTester
    {
        private CMDService _cmdService = new CMDService();
        private InputService _inputService = new InputService();
        private XMLService _xmlService = new XMLService();

        public void SetTheEnvironment()
        {
            ZTestSettingModel testSetting = new ZTestSettingModel()
            {
                TestName = TestType.DebuggerSetting.ToString(),
                NeedToRunTimes = 1,
                IsSettedEnvironment = true
            };

            Console.WriteLine("Please open new CMD window as Administrator and run following commands to set the Environment and after that press any key");
            Console.WriteLine("  bcdedit /set {bootmgr} testsigning on");
            Console.WriteLine("  bcdedit /set {bootmgr} bootdebug on");
            Console.WriteLine("  bcdedit /set testsigning on");
            Console.WriteLine("  bcdedit /bootdebug on");
            Console.WriteLine("  bcdedit /debug on");
            Console.WriteLine("press any key:");
            Console.ReadKey();

            _xmlService.EditZTestSetting(testSetting);
            StartTest();
        }

        public void StartTest()
        {
            Console.WriteLine("DebuggerSetting running...");
            Thread.Sleep(1500);
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


            //List<string> arguments = new List<string>()
            //{
            //    "bcdedit /set {bootmgr} testsigning on",
            //    "bcdedit /set {bootmgr} bootdebug on",
            //    "bcdedit /set testsigning on",
            //    "bcdedit /bootdebug on",
            //    "bcdedit /debug on",
            //};
            //List<string> arguments = new List<string>()
            //{
            //    "/set {bootmgr} testsigning on",
            //    "/set {bootmgr} bootdebug on",
            //    "/set testsigning on",
            //    "/bootdebug on",
            //    "/debug on",
            //};

            //foreach (var item in arguments)
            //{
            //    Process.Start("bcdedit.exe", item);
            //}

            string hostIPDerault = "10.216.79.136";

            Console.WriteLine("Enter hostIP (eg: 10.216.79.136) ");
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

            Console.WriteLine("Please open new CMD window as Administrator and run following commands to set dbgsettings");
            Console.WriteLine($@"    bcdedit /dbgsettings net hostip:{hostIP} port:{portNumber} key:{key}");

            _cmdService.RunCMDCommand($@"/dbgsettings net hostip:{hostIP} port:{portNumber} key:{key}", fileName: Environment.SystemDirectory + "\\bcdedit.exe"); //            bcdedit /dbgsettings

            FinishTest();
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
