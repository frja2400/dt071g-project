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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Kunde inte hämta kunder från databasen:");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            return customers;
        }








        //Skapar ett Customer-objekt från en rad i databasen som hämtas från metoderna.
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