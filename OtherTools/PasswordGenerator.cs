using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherTools
{
    class PasswordGenerator
    {
        ConsoleKeyInfo key;
        char[] Match1 = {'a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                    'q','r','s','t','u','v','w','x','y','z'};
        char[] Match2 = {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
                    'Q','R','S','T','U','V','W','X','Y','Z'};
        char[] Match3 = {'0','1','2','3','4','5','6','7','8','9'};
        char[] Match4 = {'a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                    'q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
                    'Q','R','S','T','U','V','W','X','Y','Z'};
        char[] Match5 = {'0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                    'q','r','s','t','u','v','w','x','y','z'};
        char[] Match6 = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
                    'Q','R','S','T','U','V','W','X','Y','Z'};
        char[] Match7 = {'0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                    'q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
                    'Q','R','S','T','U','V','W','X','Y','Z'};
        char[] Match8 = {'0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                    'q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
                    'Q','R','S','T','U','V','W','X','Y','Z','@','!','$','%','&','?','*','#',};

        public void controlStart()
        {
            char[] currentMatch = {'a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                    'q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
                    'Q','R','S','T','U','V','W','X','Y','Z'};
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Press a key to continue ...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[1] Only lowercase");
            Console.WriteLine("[2] Only uppercase");
            Console.WriteLine("[3] Only Numbers");
            Console.WriteLine("[4] lowercase & uppercase");
            Console.WriteLine("[5] lowercase & numbers");
            Console.WriteLine("[6] uppercase & numbers");
            Console.WriteLine("[7] lowercase & uppercase & numbers");
            Console.WriteLine("[8] lowercase & uppercase & numbers & special characters");
            Console.WriteLine("[Esc] Close");
            Console.ForegroundColor = ConsoleColor.Gray;
            key = Console.ReadKey();
            if (key.Key == ConsoleKey.D1)
            {
                currentMatch = Match1;
            }
            else if (key.Key == ConsoleKey.D2)
            {
                currentMatch = Match2;
            }
            else if (key.Key == ConsoleKey.D3)
            {
                currentMatch = Match3;
            }
            else if (key.Key == ConsoleKey.D4)
            {
                currentMatch = Match4;
            }
            else if (key.Key == ConsoleKey.D5)
            {
                currentMatch = Match5;
            }
            else if (key.Key == ConsoleKey.D6)
            {
                currentMatch = Match6;
            }
            else if (key.Key == ConsoleKey.D7)
            {
                currentMatch = Match7;
            }
            else if (key.Key == ConsoleKey.D8)
            {
                currentMatch = Match8;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
            Console.Clear();
            Console.WriteLine("Select your password length ...");
            Console.WriteLine("[1] Length of 5");
            Console.WriteLine("[2] Length of 10");
            Console.WriteLine("[3] Length of 15");
            Console.WriteLine("[4] Length of 20");
            key = Console.ReadKey();
            int length = 5;
            if (key.Key == ConsoleKey.D1)
            {
                length = 5;
            }
            else if (key.Key == ConsoleKey.D2)
            {
                length = 10;
            }
            else if (key.Key == ConsoleKey.D3)
            {
                length = 15;
            }
            else if (key.Key == ConsoleKey.D4)
            {
                length = 20;
            }
            Console.Clear();
            Console.WriteLine("Your new generated Password is:");
            startPasswordGenerator(currentMatch, length);
        }

        void startPasswordGenerator(char[] currentMatch, int length)
        {
            string valid = "";
            for (int i=0;i<currentMatch.Length;i++)
            {
                valid = valid + currentMatch[i];
            }
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while(0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            string password = res.ToString();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(password);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
