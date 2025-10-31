using System;


namespace CustomerRegister
{
    //Klass som hanterar logiken bakom användarinteraktioner. 
    public class MenuController
    {
        //Skapar en ny instans av CustomerRepository. 
        private readonly CustomerRepository _repository = new();

        //Metod som kör hela menyn i en loop.
        public void Run()
        {
            while (true)
            {
                Console.Clear();
                ShowMenu();
                Console.Write("\nVälj ett alternativ: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllCustomers(); break;
                    case "2": AddCustomer(); break;
                    case "3": EditCustomer(); break;
                    case "4": DeleteCustomer(); break;
                    case "5": SearchCustomer(); break;
                    case "6": SortCustomer(); break;
                    case "7": return;
                    default:
                        Console.WriteLine("Ogiltigt val, tryck på valfri tangent för att återgå till menyn.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("KUNDREGISTER");
            Console.WriteLine("1. Visa alla kunder");
            Console.WriteLine("2. Lägg till ny kund");
            Console.WriteLine("3. Redigera kund");
            Console.WriteLine("4. Radera kund");
            Console.WriteLine("5. Sök kund");
            Console.WriteLine("6. Sortera kunder");
            Console.WriteLine("7. Avsluta programmet");
        }
    }
}