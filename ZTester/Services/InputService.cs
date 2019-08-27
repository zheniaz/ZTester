using System;

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
    }
}
