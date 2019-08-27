using System;
using System.Threading;
using ZTester.Services;
using ZTester.Tests;

namespace ZTester
{
    class Program
    {

        static InputService _inputService = new InputService();
        static RebootSystem _rebootSystem = new RebootSystem();
        static WSHTester _wshTester = new WSHTester();
        static void Main(string[] args)
        {
            Console.WriteLine("ZTester, (c) 2019 ZHENIA Zgurovets Inc.");

            if (_rebootSystem.RebootCount > 10)
            {
                return;
            }

            if (_rebootSystem.IsLogFileExists)
            {
                _rebootSystem.Reboot();
            }
            else
            {
                SelectTestPoint();
            }
        }

        #region Input Hadler Region

        public static void SelectTestPoint()
        {
            Console.WriteLine(@"Please select a test:
    1.  Reboot System Test
    2.  Sleep Test
    3.  Reboot System & Sleep Test
    4.  WSHTester 
    5.  To quit");
            int selectedTest = _inputService.SelectNumberFromTheRange(1, 5);

            switch (selectedTest)
            {
                case 1:
                    {
                        _rebootSystem.StartTest();
                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("This feature is under development.");
                        Thread.Sleep(3500);
                        break;
                    }

                case 3:
                    {
                        Console.WriteLine("This feature is under development.");
                        Thread.Sleep(3500);
                        break;
                    }

                case 4:
                    {
                        _wshTester.StartTest();
                        break;
                    }

                case 5:
                    {
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        #endregion
    }
}
