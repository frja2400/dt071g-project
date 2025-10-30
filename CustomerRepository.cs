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
                Console.ResetColor();
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
                Console.ResetColor();

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
                Console.ResetColor();

                return null;
            }
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