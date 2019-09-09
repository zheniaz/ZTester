using System;
using System.Linq;

namespace ZTester.Services
{
    class InputService
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
            if(str.Length != 3)
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
