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
                startInfo.FileName = "cmd";
                startInfo.Arguments = arguments;
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

        public void RunREG(string arguments)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "reg";
                startInfo.Arguments = arguments;
                process.StartInfo = startInfo;
                process.Start();
                string ErrorMessage = process.StandardError.ReadToEnd();
                if (ErrorMessage.Length > 0)
                {
                    throw new Exception("Error:" + ErrorMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"###################### {arguments} ######################");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }



        public void RunCMDCommand(string arguments, string fileName = "cmd", string workDirectory = "", string verb = "", bool waitForExit = false, bool redirectStandardInput = false)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = true,
                RedirectStandardInput = redirectStandardInput,
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
            
            if ( waitForExit)
            {
                process.WaitForExit();
            }
            process.Start();
            Thread.Sleep(500);
        }

        public void RunCMDCommands(string[] cmdCommandArray)
        {
            throw new NotImplementedException();
        }
    }
}
