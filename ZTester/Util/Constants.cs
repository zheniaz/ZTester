using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class Constants
    {
        public const string AppName = "ZTester.exe";
        public const string LogFileName = "rebootLog.txt";
        public const string RebootLoopShortcutName = "ZTester - Shortcut";

    }

    public enum TestType { None, RebootSystemTest, SleepTest, WSHTest };
}
