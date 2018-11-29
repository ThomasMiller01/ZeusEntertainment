using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtherTools
{
    class Program
    {
        static void Main(string[] args)
        {
            control();
        }

        static void control()
        {
            ConsoleKeyInfo key;
            while (1 != 0)
            {
                BruteForceAttack bfa = new BruteForceAttack();
                PasswordGenerator pwg = new PasswordGenerator();
                Console.WriteLine("Press a key to continue ...");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[1] Brute-Force-Attack");
                Console.WriteLine("[2] Password-Generator");
                Console.WriteLine("[Esc] Close");
                Console.ForegroundColor = ConsoleColor.Gray;
                key = Console.ReadKey();
                Console.Clear();
                if (key.Key == ConsoleKey.D1)
                {
                    bfa.controlStart();
                    Console.WriteLine();
                }
                else if (key.Key == ConsoleKey.D2)
                {
                    pwg.controlStart();
                    Console.WriteLine();
                }
                else if(key.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
