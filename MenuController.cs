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
                UIHelper.PrintHeader("Kundregister");
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
                        UIHelper.PrintError("Ogiltigt val.");
                        UIHelper.WaitForKey();
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
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
                UIHelper.PrintError("Inga kunder är registrerade.");
                UIHelper.WaitForKey();
                return;
            }

            //Skriver ut tabell med design enligt metod i UIHelper.
            UIHelper.PrintCustomerTable(customers);
            UIHelper.WaitForKey();
        }

        private void AddCustomer()
        {
            Console.Clear();
            UIHelper.PrintSubHeader("Lägg till ny kund");

            string name;
            while (true)
            {
                Console.Write("Namn: ");
                //Vid null så sätts name till tom sträng istället så att name alltid är en giltig sträng.
                name = Console.ReadLine() ?? "";

                //Om namn inte är tomt - bryt loopen.
                if (!string.IsNullOrWhiteSpace(name)) break;

                //Annars meddela att det inte får vara tomt.
                UIHelper.PrintError("Namn får inte vara tomt.");
                UIHelper.TryAgain();
            }

            string email;
            while (true)
            {
                Console.Write("Epost: ");
                email = Console.ReadLine() ?? "";

                //Om email inte är tomt och innehåller @ - bryt loopen.
                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@")) break;

                UIHelper.PrintError("Ogiltig e-postadress.");
                UIHelper.TryAgain();
            }

            Console.Write("Stad (valfritt): ");
            string? city = Console.ReadLine();

            //newCustomer kan vara null eller ett nytt customer-objekt.
            //Kalla metoden AddCustomer från CustomerRepository. Om city är null används tom sträng.
            Customer? newCustomer = _repository.AddCustomer(name, email, city ?? "");

            //Kontrollera om kund är tillagd.
            if (newCustomer != null)
            {
                UIHelper.PrintSuccess($"Kunden '{newCustomer.Name}' lades till med ID {newCustomer.Id}.");
            }
            else
            {
                UIHelper.PrintError("Kunde inte lägga till kunden.");
            }
            UIHelper.WaitForKey();
        }

        private void DeleteCustomer()
        {
            Console.Clear();
            UIHelper.PrintSubHeader("Radera kund");
            Console.Write("Skriv in ID på kunden du vill radera: ");
            string? input = Console.ReadLine();

            //Konverterar inmatning till heltal men om det inte lyckas så skriv ut nedan:
            if (!int.TryParse(input, out int id))
            {
                UIHelper.PrintError("Ogiltigt ID.");
                UIHelper.WaitForKey();
                return;
            }

            //Hämta kund med det ID från databas och kontrollera att den finns.
            var customer = _repository.GetCustomerById(id);
            if (customer == null)
            {
                UIHelper.PrintError("Ingen kund med det ID:t hittades.");
                UIHelper.WaitForKey();
                return;
            }

            Console.Write($"Är du säker på att du vill radera kunden '{customer.Name}'? (J/N): ");
            string? confirm = Console.ReadLine();
            //Om användaren inte svarar J så avbryts raderingen.
            if (confirm?.ToUpper() != "J")
            {
                UIHelper.PrintError("Radering avbruten.");
                UIHelper.WaitForKey();
                return;
            }

            //Annars genomförs raderingen:
            bool success = _repository.DeleteCustomer(id);
            if (success)
            {
                UIHelper.PrintSuccess("Kunden raderades.");
            }
            else
            {
                UIHelper.PrintError("Kunde inte radera kunden.");
            }
            UIHelper.WaitForKey();
        }

        private void EditCustomer()
        {
            Console.Clear();
            UIHelper.PrintSubHeader("Redigera kund");
            Console.Write("Skriv in ID på kunden du vill redigera: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int id))
            {
                UIHelper.PrintError("Ogiltigt ID.");
                UIHelper.WaitForKey();
                return;
            }

            var customer = _repository.GetCustomerById(id);

            if (customer == null)
            {
                UIHelper.PrintError("Ingen kund med det ID:t hittades.");
                UIHelper.WaitForKey();
                return;
            }

            //Visa nuvarande information om kunden.
            UIHelper.PrintCustomerTable([customer]);
            Console.WriteLine("FYLL I NY INFORMATION\n");

            string name;
            while (true)
            {
                Console.Write("Namn: ");
                name = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(name)) break;

                UIHelper.PrintError("Namn får inte vara tomt.");
                UIHelper.TryAgain();
            }

            string email;
            while (true)
            {
                Console.Write("Epost: ");
                email = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@")) break;

                UIHelper.PrintError("Ogiltig e-postadress.");
                UIHelper.TryAgain();
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
                UIHelper.PrintSuccess("Kunden uppdaterades.");
            }
            else
            {
                UIHelper.PrintError("Kunde inte uppdatera kunden.");
            }
            UIHelper.WaitForKey();
        }

        public void SearchCustomer()
        {
            Console.Clear();
            UIHelper.PrintSubHeader("Sök kund");

            //Loopar tills användaren anger ett giltigt sökord
            string term;
            while (true)
            {
                Console.Write("Sök på namn, epost eller stad: ");
                term = Console.ReadLine() ?? "";

                //Om fältet inte är tomt - bryt loopen
                if (!string.IsNullOrWhiteSpace(term))
                    break;

                //Annars meddela att det inte får vara tomt
                UIHelper.PrintError("Sökfält får ej vara tomt.");
                UIHelper.TryAgain();
            }

            var results = _repository.SearchCustomers(term);

            if (results.Count == 0)
            {
                UIHelper.PrintError("Ingen kund hittades.");
                UIHelper.WaitForKey();
                return;
            }

            Console.Clear();
            UIHelper.PrintSubHeader("Sökresultat");
            UIHelper.PrintCustomerTable(results);
            UIHelper.WaitForKey();
        }

        public void SortCustomer()
        {
            Console.Clear();
            UIHelper.PrintSubHeader("Sortera kunder");
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
                    UIHelper.PrintError("Ogiltigt val.");
                    UIHelper.WaitForKey();
                    return;
            }

            //Hämtar sorterade kunder från databasen via repositoryns metod SortCustomers().
            var sorted = _repository.SortCustomers(sortBy);

            if (sorted.Count == 0)
            {
                UIHelper.PrintError("Inga kunder i databasen.");
                UIHelper.WaitForKey();
                return;
            }

            Console.Clear();
            UIHelper.PrintSubHeader("Sorterad kundlista");
            UIHelper.PrintCustomerTable(sorted);
            UIHelper.WaitForKey();
        }
    }
}