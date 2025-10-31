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

        private void ShowAllCustomers()
        {
            Console.Clear();
            //Hämtar kunder från databasen.
            var customers = _repository.GetAllCustomers();

            //Kontrollerar att det finns registrerade kunder.
            if (customers.Count == 0)
            {
                Console.WriteLine("Inga kunder är registrerade. Tryck på valfri tangent för att återgå till meny.");
                Console.ReadKey();
                return;
            }

            foreach (var c in customers)
            {
                Console.WriteLine($"{c.Id}. {c.Name} - {c.Email} - {c.City} - {c.CreatedAt:yyyy-MM-dd}");
            }

            Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn.");
            Console.ReadKey();
        }

        private void AddCustomer()
        {
            Console.Clear();
            Console.WriteLine("LÄGG TILL NY KUND\n");

            //Läs in namn och kontrollera att det inte är tomt.
            Console.Write("Namn: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Namn får inte vara tomt. Tryck på valfri tangent för att återgå.");
                Console.ReadKey();
                return;
            }

            //Läs in email och kontrollera att det inte är tomt och är giltigt.
            Console.Write("Email: ");
            string? email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                Console.WriteLine("Ogiltig e-postadress. Tryck på valfri tangent för att återgå.");
                Console.ReadKey();
                return;
            }

            Console.Write("Stad (valfritt): ");
            string? city = Console.ReadLine();

            //newCustomer kan vara null eller ett nytt customer-objekt.
            //Kalla metoden AddCustomer från CustomerRepository. Om city är null används tom sträng.
            Customer? newCustomer = _repository.AddCustomer(name, email, city ?? "");

            //Kontrollera om kund är tillagd.
            if (newCustomer != null)
            {
                Console.WriteLine($"\nKunden '{newCustomer.Name}' lades till med ID {newCustomer.Id}.");
            }
            else
            {
                Console.WriteLine("\nKunde inte lägga till kunden. Tryck på valfri tangent för att återgå.");
            }

            Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn.");
            Console.ReadKey();
        }
    }
}