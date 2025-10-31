using System;
using Microsoft.Data.Sqlite;


namespace CustomerRegister
{
    //Klass som hanterar kopplingen till SQLite-databasen.
    public class CustomerDatabase
    {
        //Konstant som innehåller sökväg till databasen.
        private const string ConnectionString = "Data Source=./customers.db";

        //Konstruktorn
        public CustomerDatabase()
        {

            try
            {

                //Skapar ett nytt SQLite-anslutningsobjekt och öppnar anslutningen.
                using var connection = new SqliteConnection(ConnectionString);
                connection.Open();

                //Skapar ett SQL-kommandoobjekt som skapar tabellen om den inte redan finns.
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Customers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    City TEXT,
                    CreatedAt TEXT NOT NULL
                    );";
                //Kör kommandot på databasen
                tableCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Visa tydligt felmeddelande
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Kunde inte skapa databasen eller tabellen:");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }
}