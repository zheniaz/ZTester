using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTester.Services;
using System.Threading;
using Util;

namespace ZTester.Tests
{
    class SleepTest
    {
        private CMDService _cmdService = new CMDService();
        private string[] cmdCommandsArray =
        {
            "\"start \"%systemdrive%\\Program Files\\Internet Explorer\\iexplore.exe\" https://www.youtube.com/watch?v=_U24PBWOpjM",
            "timeout /t 30",
            "\"start \"%systemdrive%\\Program Files\\Internet Explorer\\iexplore.exe\" https://www.youtube.com/watch?v=jj_Mei27E7Q",
            "timeout /t 30",
            "\"start \"%systemdrive%\\Program Files\\Internet Explorer\\iexplore.exe\" https://www.youtube.com/watch?v=5TcOvHigjYE",
            "timeout /t 30",
            "IF EXIST %Systemdrive%\\TestBin (rmdir /q /s %Systemdrive%\\TestBin) ELSE (md %Systemdrive%\\TestBin)",
            "net use * \\\\spsrv\\public /u:ntdev\texas \"Tp3Rh4uwLF!#!yO\"",
            "xcopy \\\\spsrv\\Public\\Base-11B\\RS3\\Pwtest\\%PROCESSOR_ARCHITECTURE%\\*.* %Systemdrive%\\TestBin\\*",
            "timeout /t 30",
            "%Systemdrive%\\TestBin\\pwrtest /sleep /c:4 /p:120 /d:150 /s:all"
        };
        
        public void StartTest()
        {
            foreach (var item in cmdCommandsArray)
            {
                Console.WriteLine(item);
                _cmdService.RunCMDCommand(item);
            }
        }


    }
}
