using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherTools
{
    class BruteForceAttack
    {
        char[] Match1 = {'0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                    'q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
                    'Q','R','S','T','U','V','W','X','Y','Z'};
        char[] Match2 = {'0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                    'q','r','s','t','u','v','w','x','y','z'};
        char[] Match3 = {'0','1','2','3','4','5','6','7','8','9'};
        string FindPassword;
        int Combi;
        int Characters;
        DateTime start;
        ConsoleKeyInfo key;

        public void controlStart()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Press a key to continue ...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[1] Standard");
            Console.WriteLine("[2] Wifi");
            Console.WriteLine("[Esc] Close");
            Console.ForegroundColor = ConsoleColor.Gray;
            key = Console.ReadKey();
            if (key.Key == ConsoleKey.D1)
            {
                Console.Clear();
                startBruteForceStandard();
                Console.WriteLine();
            }
            else if (key.Key == ConsoleKey.D2)
            {
                Console.Clear();
                startBruteForceWifi();
                Console.WriteLine();
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
        }

        void startBruteForceStandard()
        {
            Console.Title = "Brute Force Attack";
            int Count;

            Console.WriteLine("Brute-Force-Attack-Standard started ...");

            FindPassword = "seba7";
            Characters = FindPassword.Length;
            start = DateTime.Now;



            for (Count = 0; Count <= 5; Count++)
            {
                Recurse(Count, 0, "", "standard");
            }
        }

        void startBruteForceWifi()
        {
            Console.Title = "Brute Force Attack";
            int Count;

            Console.WriteLine("Brute-Force-Attack-wifi started ...");

            FindPassword = "2936273647";
            Characters = FindPassword.Length;
            start = DateTime.Now;



            for (Count = 0; Count <= 10; Count++)
            {
                Recurse(Count, 0, "", "wifi");
            }
        }

        bool checkPass(string pass, string type)
        {
            if(type == "standard")
            {
                if(pass == FindPassword)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (type == "wifi")
            {
                if (pass == FindPassword)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        void Recurse(int Lenght, int Position, string BaseString, string type)
        {
            int Count = 0;
            char[] currentMatch;
            if (type == "standard")
            {
                currentMatch = Match2;
            }
            else if (type == "wifi")
            {
                currentMatch = Match3;
            }
            else
            {
                currentMatch = Match1;
            }

            for (Count = 0; Count < currentMatch.Length; Count++)
            {
                Combi++;
                //Console.WriteLine(BaseString + currentMatch[Count]);
                if (Position < Lenght - 1)
                {
                    Recurse(Lenght, Position + 1, BaseString + currentMatch[Count], type);
                }
                if (checkPass(BaseString + currentMatch[Count], type))
                {
                    Console.WriteLine("Brute-Force-Attack successful.");
                    Console.WriteLine();
                    Console.WriteLine("The password is: " + FindPassword);
                    Console.WriteLine();
                    DateTime end = DateTime.Now;
                    TimeSpan time = end - start;
                    time.ToString("hh\\:mm\\:ss");
                    Console.WriteLine("Used time:\t{0}\nCombinations tested:\t{1}", time, Combi);
                    return;
                }
            }
        }
    }
}