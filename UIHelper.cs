using System;
using System.Collections.Generic;


namespace CustomerRegister
{

    //Klass som designar och formatterar applikationen.
    public static class UIHelper
    {
        //Metod som designar och skriver ut huvudrubrik
        public static void PrintHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=======================================");
            Console.WriteLine($"        {title.ToUpper()}");
            Console.WriteLine("=======================================\n");
            Console.ResetColor();
        }

        //Metod som designar och skriver ut sekundärrubrik
        public static void PrintSubHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{title.ToUpper()}");
            Console.WriteLine("--------------------------\n");
            Console.ResetColor();
        }

        //Design för felmeddelanden i konsollen
        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        //Design för bekräftelsemeddelanden i konsollen
        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        //Design på tabell. IEnumerable är en parameter som kräver att vi skickar en lite eller samling av någonting.
        public static void PrintCustomerTable(IEnumerable<Customer> customers)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ID   Namn                 Email                    Stad           Skapad");
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.ResetColor();

            foreach (var c in customers)
            {
                Console.WriteLine($"{c.Id,-4} {c.Name,-20} {c.Email,-24} {c.City,-13} {c.CreatedAt:yyyy-MM-dd}");
            }
            Console.WriteLine();
        }

        //Meddelande om hur användare återgår till menyn.
        public static void WaitForKey()
        {
            Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn.");
            Console.ReadKey();
        }

        //Meddelande om att användaren behöver prova igen
        public static void TryAgain()
        {
            Console.WriteLine("\nTryck på valfri tangent för att prova igen.\n");
            Console.ReadKey();
        }
    }
}