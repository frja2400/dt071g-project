using System;

namespace CustomerRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Skapar databas och tabell (om de inte finns).");

            var db = new CustomerDatabase();

            Console.WriteLine("Databasen är klar!.");
        }
    }
}