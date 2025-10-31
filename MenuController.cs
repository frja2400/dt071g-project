using System;
using System.Data.Common;


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
                    case "3": DeleteCustomer(); break;
                    case "4": EditCustomer(); break;
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

        private void DeleteCustomer()
        {
            Console.Clear();
            Console.Write("Skriv in ID på kunden du vill radera: ");
            string? input = Console.ReadLine();

            //Konverterar inmatning till heltal men om det inte lyckas så skriv ut nedan:
            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Ogiltigt ID. Tryck på valfri tangent för att återgå.");
                Console.ReadKey();
                return;
            }

            //Hämta kund med det ID från databas och kontrollera att den finns.
            var customer = _repository.GetCustomerById(id);
            if (customer == null)
            {
                Console.WriteLine("Ingen kund med det ID:t hittades. Tryck på valfri tangent för att återgå.");
                Console.ReadKey();
                return;
            }

            Console.Write($"Är du säker på att du vill radera kunden '{customer.Name}'? (J/N): ");
            string? confirm = Console.ReadLine();
            //Om användaren inte svarar J så avbryts raderingen.
            if (confirm?.ToUpper() != "J")
            {
                Console.WriteLine("Radering avbruten.");
                Console.ReadKey();
                return;
            }

            //Annars genomförs raderingen:
            bool success = _repository.DeleteCustomer(id);
            if (success)
            {
                Console.WriteLine("Kunden raderades.");
            }
            else
            {
                Console.WriteLine("Kunde inte radera kunden. Tryck på valfri tangent för att återgå.");
            }
            Console.ReadKey();
        }

        private void EditCustomer()
        {
            Console.Clear();
            Console.Write("Skriv in ID på kunden du vill redigera: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Ogiltigt ID. Tryck på valfri tangent för att återgå.");
                Console.ReadKey();
                return;
            }

            var customer = _repository.GetCustomerById(id);
            if (customer == null)
            {
                Console.WriteLine("Ingen kund med det ID:t hittades. Tryck på valfri tangent för att återgå.");
                Console.ReadKey();
                return;
            }

            Console.Write("Namn: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Namn får inte vara tomt. Tryck på valfri tangent för att återgå.");
                Console.ReadKey();
                return;
            }

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

            //Uppdatera customer-objektet som vi hämtade med de nya värdena innan vi skickar det till UpdateCustomer.
            customer.Name = name;
            customer.Email = email;
            customer.City = city ?? "";

            //Skicka objektet till repository för uppdatering.
            bool success = _repository.UpdateCustomer(customer);

            if (success)
            {
                Console.WriteLine("Kunden uppdaterades.");
            }
            else
            {
                Console.WriteLine("Kunde inte uppdatera kunden. Tryck på valfri tangent för att återgå.");
            }
            Console.ReadKey();
        }
    }
}