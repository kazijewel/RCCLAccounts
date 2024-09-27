## Architectural View
When building a .NET Core web app using N-Tier or three-tier architecture, you can structure your project in a way that separates the different layers or tiers clearly. Here's an example of an ideal project structure for your Provident Fund web app:

### Presentation Layer:
RCCLAccounts.WebUi (ASP.NET Core Web Application)
 - Controllers (Handles HTTP requests and defines routes)
 - Views (Contains UI templates)
 - Models (Defines view models for data transfer)

### Business Layer:

RCCLAccounts.Core (Class Library)
 - Services (Contains business logic and services)
 - Models (Defines business models)

### Data Access Layer:
 - RCCLAccounts.Data (Class Library)
 - Repositories (Handles data access and persistence)
 - Entities (Defines entity models for database tables)

### Tests:

RCCLAccounts.Tests (Unit and Integration Tests)

### Tree View:
This project structure follows a modular and decoupled approach, allowing you to easily maintain and test each layer independently. It promotes separation of concerns and improves code reusability.
```javascript
├── .gitignore
├── RCCLAccounts.sln
├── RCCLAccounts.WebUi
│   ├── Controllers
│   ├── Views
│   └── ...
├── RCCLAccounts.Core
│   ├── Services
│   ├── Models
│   └── ...
├── RCCLAccounts.Data
│   ├── Repositories
│   ├── Entities
│   └── ...
├── RCCLAccounts.Tests
│   ├── Unit
│   ├── Integration
│   └── ...
└── README.md
```

## Prohject Reference Cycle
Based on the architecture and the separate DI configurations in each project, the project reference cycle would typically follow a hierarchical structure without circular dependencies. Here's an example of how the project references might look:

1. RCCLAccounts.Web (Presentation Layer):
 - References: RCCLAccounts.Core
2. RCCLAccounts.Core (Business Layer):
 - References: RCCLAccounts.Data
3. RCCLAccounts.Data (Data Access Layer):
 - References: None (Or references to ORM libraries, if applicable)

The references are set in such a way that each layer only depends on the layers beneath it, following a unidirectional flow. This helps maintain a clear separation of concerns and avoids circular dependencies.

To summarize, the project reference cycle in the given architecture is:

RCCLAccounts.Web (Presentation Layer) -> RCCLAccounts.Core (Business Layer) -> RCCLAccounts.Data (Data Access Layer)

This hierarchy allows for a modular and layered approach to application development, where each layer has its own responsibilities and can be developed and tested independently.

## Migrations
The migration commands may vary depending on the specific tools and frameworks you are using for database migrations. However, if you are using Entity Framework Core for your data access layer and migrations, here are some common migration commands:

Add Migration:

The add-migration command is used to generate a new migration file based on the changes detected in your data context and entity classes.
Example command:
```bash
dotnet ef migrations add <MigrationName> --project RCCLAccounts.Data --startup-project RCCLAccounts.WebUi  --context=AppDbContext
```
Apply Migrations:

The update-database command is used to apply any pending migrations to the database. It ensures that the database schema is in sync with the latest migration.
Example command: 
```bash
dotnet ef database update --project RCCLAccounts.Data --startup-project RCCLAccounts.WebUi --context=AppDbContext
```
Revert Migrations:

The remove-migration command is used to revert the most recent migration. It undoes the changes applied by the last migration file.
Example command:
```bash
dotnet ef migrations remove --project RCCLAccounts.Data --startup-project RCCLAccounts.WebUi --context=AppDbContext
```
These commands assume that you have the Entity Framework Core tools installed (dotnet ef) and that you have specified the appropriate project paths for your Data Access Layer (--project RCCLAccounts.Data) and your startup project (--startup-project RCCLAccounts.Web).

## Scaffold Db Context 
#### [NB: Please Copy-paste the generated models in RCCLAccounts.Data.Entities Folder and then make migration to add it to the context]
We can Scaffold the entire database tables with following Package Manager Console Command
```bash
Scaffold-DbContext "Server=localhost,1401;Database=RCCL-CPF;User Id=sa;Password=P@ssword123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir TempDirectory 'directory to save the scaffolded tables' 
```
To Scaffold one table
```bash
Scaffold-DbContext "Server=localhost,1401;Database=RCCL-CPF;User Id=sa;Password=P@ssword123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir TempDirectory -t table-name  
```
To Update the existing table
```bash
Scaffold-DbContext "Server=localhost,1401;Database=RCCL-CPF;User Id=sa;Password=P@ssword123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir TempDirectory -t table-name -force -verbose
```