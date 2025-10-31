using System;

namespace CustomerRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            //Skapa databas och tabell om den inte redan finns.
            var db = new CustomerDatabase();

            //Skapa instans av MenuController.
            var menu = new MenuController();

            //Starta menyn genom att köra min metod Run på den nya instansen.
            menu.Run();
        }
    }
}