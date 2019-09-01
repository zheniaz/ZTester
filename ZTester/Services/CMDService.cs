using System;
using System.Threading;
using Util;

namespace ZTester.Services
{
    class CMDService
    {
        public void RunCMDCommand(string arguments)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = arguments;
                startInfo.UseShellExecute = true;
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"###################### {arguments} ######################");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public void RunCMDCommand(string arguments, string fileName = "cmd.exe", string workDirectory = "", string verb = "", bool waitForExit = false)
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
            if (verb != "")
            {
                startInfo.Verb = verb;
            }


            process.StartInfo = startInfo;
            process.Start();
            if ( waitForExit)
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
