# Kundregister i C# (.NET & SQLite)

Detta projekt är en konsollapplikation skriven i **C#** med **.NET 9.0**.  
Applikationen låter användaren hantera ett kundregister där alla uppgifter lagras i en **SQLite-databas** med full **CRUD-funktionalitet** (Create, Read, Update, Delete).

### Funktioner

- Visa alla kunder.
- Lägga till ny kund.
- Redigera kunder.
- Radera kunder.
- Söka på kunder.
- Sortera kunder.

## Projektstruktur

```plaintext
DT071G-PROJECT/
│
├── Program.cs               # Startpunkt för applikationen
├── Customer.cs              # Datamodell för kunder
├── Database.cs              # Initierar och ansluter till databasen
├── CustomerRepository.cs    # CRUD-operationer mot databasen
├── MenuController.cs        # Hanterar meny och användarflöde
├── UIHelper.cs              # Skriver ut gränssnittet i konsollen
│
├── customers.db             # SQLite-databas (skapas automatiskt)
├── documentation/           # Diagram och mockups
│   ├── er-diagram.drawio
│   ├── uml-classdiagram.drawio
│   ├── flowchart.drawio
│   └── textmockup.drawio
│
└── README.md
```

## Installation och körning

- git clone https://github.com/frja2400/dt071g-project.git
- cd dt071g-project
- dotnet add package Microsoft.Data.Sqlite
- dotnet run
Databasen customers.db skapas automatiskt första gången programmet körs.