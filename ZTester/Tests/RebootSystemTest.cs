using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Util;
using ZTester.Interfaces;
using ZTester.Services;

namespace ZTester
{
    class RebootSystemTest : IZTester
    {
        private CMDService _cmdService = new CMDService();
        private InputService _inputService = new InputService();
        private FileService _fileService = new FileService();
        private LogInService _logInService = new LogInService();

        public string LogFileFullPath { get { return IsLogFileExists ? Path.GetFullPath($"{_fileService.AppPath}\\{Constants.LogFileName}") : ""; } }
        public string StartupDirectoryFullPath { get { return _fileService.GetStartupFolderPath(); } set { } }
        public string ShortcutRebootLoopFullPath { get { return $"{StartupDirectoryFullPath}\\{Constants.RebootLoopShortcutName}"; } }

        public int NeedToReboot { get { return GetCountNeedToReboot(); } set { } }
        public int RebootCount = 0;

        public bool IsLogFileExists { get { return _fileService.CheckIfFileExists($"{_fileService.AppPath}\\{Constants.LogFileName}"); } set { } }
        public bool IsShortcutOfRebootLoopExists { get { return _fileService.CheckIfFileExists($"{StartupDirectoryFullPath}\\{Constants.RebootLoopShortcutName}.lnk"); } }

        public RebootSystemTest()
        {
            this.RebootCount = GetRebootCountFromLogFile();
        }

        public void StartTest()
        {
            int rebootCount = CheckHowManyTimesReboot();
            if (rebootCount > 0 && rebootCount <= 10)
            {
                CreateLogFile(rebootCount);
                if (!IsShortcutOfRebootLoopExists)
                {
                    _fileService.CreateShortcut(_fileService.AppPath, Constants.RebootLoopShortcutName, IsShortcutOfRebootLoopExists);
                    _fileService.MoveFileToFolder(_fileService.AppPath + "\\" + Constants.RebootLoopShortcutName + ".lnk", StartupDirectoryFullPath + "\\" + Constants.RebootLoopShortcutName + ".lnk");
                }
                Reboot();
            }
        }

        public int CheckHowManyTimesReboot()
        {
            return _inputService.SelectNumberFromTheRange(1, 10, "How many times do you want to reboot your system?");
        }

        public void Reboot()
        {
            if (RebootCount > 0 && RebootCount <= NeedToReboot)
            {
                ReadAndRewriteRebootCountInLogFile();
                AddEntryToLogFile();
            }
            if (NeedToReboot == GetRebootsCountFromLogFile())
            {
                FinishRebootProcess();
                return;
            }
            string time = (NeedToReboot - RebootCount == 1) ? "time" : "times";
            Console.WriteLine($"{NeedToReboot - RebootCount} {time} left to reboot the system");
            Thread.Sleep(1500);
            Console.WriteLine("Rebooting System...");
            _logInService.EnableAutoLogIn();
            Thread.Sleep(2500);
            ShutDownTheSystem();
        }

        public void ShutDownTheSystem()
        {
            System.Diagnostics.Process.Start("ShutDown", "-r -t 0");
        }

        private void FinishRebootProcess()
        {
            RenameLogFile();
            Console.WriteLine("Removing ShortCut from Sturtup folder");
            Thread.Sleep(3000);
            _fileService.RemoveFile(StartupDirectoryFullPath, Constants.RebootLoopShortcutName + ".lnk", IsShortcutOfRebootLoopExists);
            _logInService.DisableAutoLogIn();
        }

        //#region Input Hadler Region

        //public void SelectTestPoint()
        //{
        //    Console.WriteLine(@"Please select a test:
        //            1.  Reboot System Test
        //            2.  Sleep Test
        //            3.  Reboot System & Sleep Test
        //            4.  To quit");
        //    int selectedTest = _inputService.SelectNumberFromTheRange(1, 4);

        //    switch (selectedTest)
        //    {
        //        case 1:
        //            {
        //                StartRebootLoop();
        //                break;
        //            }

        //        case 2:
        //            {
        //                Console.WriteLine("This feature is under development.");
        //                Thread.Sleep(3500);
        //                break;
        //            }

        //        case 3:
        //            {
        //                Console.WriteLine("This feature is under development.");
        //                Thread.Sleep(3500);
        //                break;
        //            }

        //        case 4:
        //            {
        //                return;
        //            }

        //        default:
        //            {
        //                break;
        //            }
        //    }
        //}

        //#endregion

        #region Region For Working  With Files

        public void CreateLogFile(int rebootCount)
        {
            string path = $"{_fileService.AppPath}\\{Constants.LogFileName}";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine($"Created: {DateTime.Now}");
                    sw.WriteLine($"Need to reboot #{rebootCount}# times");
                    sw.WriteLine("rebooted: |0| times");
                }
            }
        }

        public void ReadAndRewriteRebootCountInLogFile()
        {
            if (IsLogFileExists == false)
            {
                throw new Exception("File rebootLog.txt does not exist.");
            }

            try
            {
                string text = File.ReadAllText($"{ Constants.LogFileName}"); //"rebooted: |0| times";
                string rebootCountString = Regex.Match(text, @"(?<=\|)(.*?)(?=\|)").ToString();
                int rebootCount = 0;
                if (!Int32.TryParse(rebootCountString, out rebootCount))
                {
                    throw new Exception("Cannot convert from string to int.");
                }
                text = text.Replace($@"|{rebootCount}|", $@"|{++rebootCount}|");

                File.WriteAllText(LogFileFullPath, text);
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong!");
            }
        }

        public void AddEntryToLogFile()
        {
            string path = $"{_fileService.AppPath}\\{Constants.LogFileName}";
            if (File.Exists(path))
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("");
                    sw.WriteLine($"reboot #{RebootCount}");
                    sw.WriteLine($"rebooted: {DateTime.Now}");
                }
            }
        }

        public int GetRebootsCountFromLogFile()
        {
            string text = File.ReadAllText($"{Constants.LogFileName}");
            string rebootCountString = Regex.Match(text, @"(?<=\|)(.*?)(?=\|)").ToString();
            int rebooted = 0;
            Int32.TryParse(rebootCountString, out rebooted);
            return rebooted;
        }

        private int GetRebootCountFromLogFile()
        {
            if (!IsLogFileExists) return 0;

            string text = File.ReadAllText($"{Constants.LogFileName}");
            string rebootCountString = Regex.Match(text, @"(?<=\|)(.*?)(?=\|)").ToString();
            int rebooted = 0;
            if (!Int32.TryParse(rebootCountString, out rebooted))
            {
                throw new Exception("Cannot convert from string to int.");
            }
            return ++rebooted;
        }

        private int GetCountNeedToReboot()
        {
            string text = File.ReadAllText($"{Constants.LogFileName}");
            string rebootCountString = Regex.Match(text, @"(?<=\#)(.*?)(?=\#)").ToString();
            int needToReboot = 0;
            if (!Int32.TryParse(rebootCountString, out needToReboot))
            {
                throw new Exception("Cannot convert from string to int.");
            }
            return needToReboot;
        }

        private void RenameLogFile()
        {
            string oldfileName = $"{_fileService.AppPath}\\{Constants.LogFileName}";
            string newFileNameTitle = $"{_fileService.AppPath}\\rebootLog rebooted-{RebootCount}-times {DateTime.Now:MM-dd-yyyy_hh-mm-ss}.txt";
            _fileService.RenameFile(oldfileName, newFileNameTitle);
        }

        #endregion
    }
}
