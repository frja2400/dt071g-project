using System;


namespace CustomerRegister
{
    //Definierar datamodellen och gör den åtkomlig för resten av applikationen.
    public class Customer
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public string? City { get; set; } //Kan vara null

        public DateTime CreatedAt { get; set; }
    }
}