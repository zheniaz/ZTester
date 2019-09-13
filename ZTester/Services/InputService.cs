using System;
using System.Collections.Generic;
using System.Linq;

namespace ZTester.Services
{
    public class InputService
    {
        public int SelectNumberFromTheRange(int from, int to, string query = null)
        {
            if (query != null) Console.WriteLine(query);

            string inputString;
            int count = 0;

            do
            {
                inputString = Console.ReadLine();
                if (!Int32.TryParse(inputString, out count) || count < from || count > to)
                {
                    Console.WriteLine($"Enter digit from {from} to {to}:");
                }
            } while (count < from || count > to);

            return count;
        }

        public List<int> CreateTestList()
        {
            string str = Console.ReadLine();
            List<int> testList = null;

            do
            {
                str = Console.ReadLine();
                if (str.Contains(','))
                {
                    testList = SelectSetOfTests(str);
                }
                else
                {
                    testList = SelectOneTest(str);
                }

                if (testList == null)
                {
                    Console.WriteLine("You have entered incorrect value, try again (e.g \"1\" or \"1,2\"");
                }
            } while (testList != null);

            return testList;
        }

        public List<int> SelectOneTest(string str)
        {
            str = str.Trim();
            List<int> testList = new List<int>();
            int x;
            if (Int32.TryParse(str, out x))
            {
                if (x >= 0 && x <= 100)
                {
                    testList.Add(x);
                }
            }
            return testList.Count == 1 ? testList : null;
        }

        public List<int> SelectSetOfTests(string str)
        {
            str = str.Trim();
            str = str.Replace(" ", String.Empty);
            string[] testArr = str.Split(',').Where(s => s != "").ToArray();
            bool isValidated = false;
            List<int> testList = new List<int>();
            int converted;
            foreach (var item in testArr)
            {
                isValidated = Int32.TryParse(item, out converted);
                if (!isValidated)
                {
                    break;
                }
                if (converted > 100 || converted < 1)
                {
                    isValidated = false;
                }
                testList.Add(converted);
            }
            if (!isValidated)
            {

                return null;
            }
            return testList;
        }

        public bool CheckYesOrNo(string query)
        {
            Console.WriteLine(query);
            Console.WriteLine("Press Y/N");
            char c = Console.ReadKey().KeyChar;
            Console.WriteLine();

            while (c != 'y' && c != 'Y' && c != 'n' && c != 'N')
            {
                Console.WriteLine("Press Y/N");
                c = Console.ReadKey().KeyChar;
                Console.WriteLine();
            }
            return (c == 'y' || c == 'Y') ? true : false;
        }

        public string ReadValueFromConsole()
        {
            string str = Console.ReadLine();
            return str;
        }

        public string ReadIPAddress()
        {
            string str = Console.ReadLine();
            while (!ValidateIPv4(str))
            {
                Console.WriteLine("Wrong IP address, try again:");
                str = Console.ReadLine();
            }
            return str;
        }

        private bool ValidateIPv4(string ipString)
        {
            if (ipString == null)
            {
                return false;
            }
            else if (ipString == "")
            {
                return true;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

        public string ReadPortNumberBetween(string errorMessage, int min, int max)
        {
            string inputString = Console.ReadLine();
            if (inputString == "")
            {
                return "49152";
            }
            int count = 0;
            Int32.TryParse(inputString, out count);

            while (count < min || count > max)
            {
                Console.WriteLine(errorMessage);
                inputString = Console.ReadLine();
                Int32.TryParse(inputString, out count);
            }
            return inputString;
        }

        public string ReadRSVersion()
        {
            string str = Console.ReadLine();
            str = str.ToLower();
            while (!ValidateRS(str))
            {
                Console.WriteLine("Please select from RS1 to RS5:");
                str = Console.ReadLine();
            }
            return str;
        }

        private bool ValidateRS(string str)
        {
            if (str.Length != 3)
            {
                Console.WriteLine("You entered incorect value");
                return false;
            }

            int rs = 0;
            Int32.TryParse(str[2].ToString(), out rs);
            if (str.Contains("rs") && (rs > 0 && rs <= 5))
            {
                return true;
            }
            return false;
        }
    }
}
