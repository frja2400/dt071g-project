using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace CustomerRegister
{
    public class CustomerRepository
    {
        private const string ConnectionString = "Data Source=./customers.db";


        //Hämtar alla kunder från databasen och returnerar som en lista.
        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();

            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                //Skapar ett SQL-kommando att köra mot databasen.
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT * FROM Customers;";

                //Loopar igenom varje rad i resultatet och skickar raden till metoden ReadCustomer.
                using var reader = selectCmd.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(ReadCustomer(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kunde inte hämta kunder från databasen:");
                Console.WriteLine(ex.Message);
            }

            return customers;
        }

        //Hämtar kunder baserat på ID om den finns.
        public Customer? GetCustomerById(int id)
        {
            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT * FROM Customers WHERE Id = @id;";
                //Parametrisera för säkerhet.
                selectCmd.Parameters.AddWithValue("@id", id);

                using var reader = selectCmd.ExecuteReader();
                if (reader.Read())
                {
                    return ReadCustomer(reader);
                }
                else
                {
                    return null; //Ingen kund hittades
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kunde inte hämta kunden från databasen:");
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        //Adderar kund till databasen, returnerar customer-objekt om det lyckas, annars null.
        public Customer? AddCustomer(string name, string email, string city)
        {
            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"
                    INSERT INTO Customers (Name, Email, City, CreatedAt)
                    VALUES (@name, @email, @city, @createdAt);";

                //Skapa CreatedAt-värde för när kunden läggs till.
                var createdAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                insertCmd.Parameters.AddWithValue("@name", name);
                insertCmd.Parameters.AddWithValue("@email", email);
                //Om city är tomt sickas DBNull till databsen, annars skickas värdet.
                insertCmd.Parameters.AddWithValue("@city", string.IsNullOrEmpty(city) ? DBNull.Value : (object)city);
                insertCmd.Parameters.AddWithValue("@createdAt", createdAt);

                insertCmd.ExecuteNonQuery();

                //Hämta den rad som just lagts till
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT * FROM Customers WHERE Id = last_insert_rowid();";

                using var reader = selectCmd.ExecuteReader();
                if (reader.Read())
                {
                    //Använd ReadCustomer för att skapa Customer-objektet
                    return ReadCustomer(reader);
                }
                else
                {
                    return null; //Något gick fel
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kunde inte addera kunden till databasen:");
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        //Raderar kund från databasen.
        public bool DeleteCustomer(int id)
        {
            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var deleteCmd = connection.CreateCommand();
                deleteCmd.CommandText = "DELETE FROM Customers WHERE Id = @id;";
                deleteCmd.Parameters.AddWithValue("@id", id);

                //ExecuteNonQuery returnerar antalet rader som påverkades.
                int rowsAffected = deleteCmd.ExecuteNonQuery();

                //Returnerar true om minst en rad raderades.
                return rowsAffected > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Kunde inte radera kunden från databasen:");
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        //Söker i databasen efter kunder vars namn, e-post eller stad innehåller söktermen.
        public List<Customer> SearchCustomers(string term)
        {
            var customers = new List<Customer>();

            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var searchCmd = connection.CreateCommand();
                searchCmd.CommandText = @"SELECT * FROM Customers 
                    WHERE Name LIKE @term OR Email LIKE @term OR City LIKE @term;";
                //% tillåter träffar i mitten av orden.
                searchCmd.Parameters.AddWithValue("@term", "%" + term + "%");

                using var reader = searchCmd.ExecuteReader();
                while (reader.Read())
                {
                    //Lägg till varje hittad kund i listan.
                    customers.Add(ReadCustomer(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ett fel uppstod vid sökningen:");
                Console.WriteLine(ex.Message);
            }

            return customers;
        }

        //Sorterar kunder i databasen baserat på namn, stad eller datum.
        public List<Customer> SortCustomers(string sortBy)
        {
            //Lista som kommer innehålla alla kunder i vald ordning.
            var customers = new List<Customer>();

            //Tillåtna kolumner att sortera efter
            var validColumns = new List<string> { "Name", "City", "CreatedAt" };

            //Kontrollera att sortBy är giltig innan vi bygger SQL.
            if (!validColumns.Contains(sortBy))
            {
                Console.WriteLine("Ogiltigt sorteringsval, sorterar i standardordning (Id).");

                sortBy = "Id"; //Standardkolumn om valet är fel
            }

            try
            {
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                var sortCmd = connection.CreateCommand();
                //Använd redan validerad sortBy här
                sortCmd.CommandText = $"SELECT * FROM Customers ORDER BY {sortBy};";

                using var reader = sortCmd.ExecuteReader();
                while (reader.Read())
                {
                    //Lägg till varje kund i listan
                    customers.Add(ReadCustomer(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ett fel uppstod vid sorteringen:");
                Console.WriteLine(ex.Message);
            }

            return customers;
        }


        //Läser rader från databasen som hämtats av metoderna ovan och skapar ett customer-objekt.
        private Customer ReadCustomer(SqliteDataReader reader)
        {
            return new Customer
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                City = reader.IsDBNull(reader.GetOrdinal("City"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("City")),  //Kollar om den är null.
                CreatedAt = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedAt")))   //Konverteras från sträng till DateTime.
            };
        }
    }
}