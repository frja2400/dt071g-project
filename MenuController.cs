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
            Console.WriteLine("3. Radera kund");
            Console.WriteLine("4. Redigera kund");
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
                Console.WriteLine("Inga kunder är registrerade. Tryck på valfri tangent för att återgå till menyn.");
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

            string name;
            while (true)
            {
                Console.Write("Namn: ");
                //Vid null så sätts name till tom sträng istället så att name alltid är en giltig sträng.
                name = Console.ReadLine() ?? "";

                //Om namn inte är tomt - bryt loopen.
                if (!string.IsNullOrWhiteSpace(name)) break;

                //Annars meddela att det inte får vara tomt.
                Console.WriteLine("Namn får inte vara tomt. Tryck på valfri tangent för att försöka igen.\n");
                Console.ReadKey();
            }

            string email;
            while (true)
            {
                Console.Write("Epost: ");
                email = Console.ReadLine() ?? "";

                //Om email inte är tomt och innehåller @ - bryt loopen.
                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@")) break;

                Console.WriteLine("Ogiltig e-postadress. Tryck på valfri tangent för att försöka igen.\n");
                Console.ReadKey();
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
                Console.WriteLine("\nKunde inte lägga till kunden. Tryck på valfri tangent för att återgå till menyn.");
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
                Console.WriteLine("Ogiltigt ID. Tryck på valfri tangent för att återgå till menyn.");
                Console.ReadKey();
                return;
            }

            //Hämta kund med det ID från databas och kontrollera att den finns.
            var customer = _repository.GetCustomerById(id);
            if (customer == null)
            {
                Console.WriteLine("Ingen kund med det ID:t hittades. Tryck på valfri tangent för att återgå till menyn.");
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
                Console.WriteLine("Kunde inte radera kunden. Tryck på valfri tangent för att återgå till menyn.");
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
                Console.WriteLine("Ogiltigt ID. Tryck på valfri tangent för att återgå till menyn.");
                Console.ReadKey();
                return;
            }

            var customer = _repository.GetCustomerById(id);

            //Visa nuvarande information om kunden.
            Console.WriteLine($"{customer.Id}. {customer.Name} - {customer.Email} - {customer.City} - {customer.CreatedAt:yyyy-MM-dd}");
            Console.WriteLine("\nFyll i ny information:\n");

            if (customer == null)
            {
                Console.WriteLine("Ingen kund med det ID:t hittades. Tryck på valfri tangent för att återgå till menyn.");
                Console.ReadKey();
                return;
            }

            string name;
            while (true)
            {
                Console.Write("Namn: ");
                name = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(name)) break;

                Console.WriteLine("Namn får inte vara tomt. Tryck på valfri tangent för att försöka igen.\n");
                Console.ReadKey();
            }

            string email;
            while (true)
            {
                Console.Write("Epost: ");
                email = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@")) break;

                Console.WriteLine("Ogiltig e-postadress. Tryck på valfri tangent för att försöka igen.\n");
                Console.ReadKey();
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
                Console.WriteLine("Kunden uppdaterades. Tryck på valfri tangent för att återgå till menyn.");
            }
            else
            {
                Console.WriteLine("Kunde inte uppdatera kunden. Tryck på valfri tangent för att återgå till menyn.");
            }
            Console.ReadKey();
        }

        public void SearchCustomer()
        {
            Console.Clear();
            Console.Write("Sök på namn, epost eller stad: ");
            string? term = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(term))
            {
                Console.WriteLine("Sökfält får ej vara tomt. Tryck på valfri tangent för att återgå till menyn.");
                Console.ReadKey();
                return;
            }

            var results = _repository.SearchCustomers(term);

            if (results.Count == 0)
            {
                Console.WriteLine("Ingen kund hittades. Tryck på valfri tangent för att återgå till menyn.");
                Console.ReadKey();
                return;
            }

            foreach (var c in results)
            {
                Console.WriteLine($"{c.Id}. {c.Name} - {c.Email} - {c.City} - {c.CreatedAt:yyyy-MM-dd}");
            }
            Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn.");
            Console.ReadKey();
        }

        public void SortCustomer()
        {
            Console.Clear();
            Console.WriteLine("SORTERA KUNDER");
            Console.WriteLine("1. Sortera efter namn");
            Console.WriteLine("2. Sortera efter stad");
            Console.WriteLine("3. Sortera efter datum (skapad)");

            Console.Write("\nVälj ett alternativ: ");
            string? choice = Console.ReadLine();

            //Variabel som lagrar fält som ska sorteras på.
            string sortBy = "";

            switch (choice)
            {
                case "1":
                    sortBy = "Name";
                    break;
                case "2":
                    sortBy = "City";
                    break;
                case "3":
                    sortBy = "CreatedAt";
                    break;
                default:
                    Console.WriteLine("Ogiltigt val. Tryck på valfri tangent för att återgå till menyn.");
                    Console.ReadKey();
                    return;
            }

            //Hämtar sorterade kunder från databasen via repositoryns metod SortCustomers().
            var sorted = _repository.SortCustomers(sortBy);

            if (sorted.Count == 0)
            {
                Console.WriteLine("Inga kunder i databasen. Tryck på valfri tangent för att återgå till menyn.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nSorterad kundlista:\n");
            foreach (var c in sorted)
            {
                Console.WriteLine($"{c.Id}. {c.Name} - {c.Email} - {c.City} - {c.CreatedAt:yyyy-MM-dd}");
            }

            Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn.");
            Console.ReadKey();
        }
    }
}