using System;
using Util;

namespace ZTester.Services
{
    class CMDService
    {
        public void RunCMDCommand(string arguments)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = arguments;
            process.StartInfo = startInfo;
            process.Start();
        }

        public void RunCMDCommand(TestType testType, string arguments, string fileName = "cmd.exe", string workDirectory = "")
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = true,
                RedirectStandardInput = false
            };

            if (workDirectory != "")
            {
                startInfo.WorkingDirectory = workDirectory;
            }
            
            process.StartInfo = startInfo;
            process.Start();
            if (testType == TestType.WSHTest)
            {
                process.WaitForExit();
            }
        }

        public void RunCMDCommands(string[] cmdCommandArray)
        {
            throw new NotImplementedException();
        }
    }
}
