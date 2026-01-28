# Task Tracker API (.NET 8)

A production-style **.NET 8 Minimal API** built on macOS, demonstrating clean architecture, async programming, EF Core with SQLite, background services, and proper error handling.

This project was intentionally built end-to-end to reflect **real-world backend engineering practices**, not demo-only code.

---

## 🚀 Features

- .NET 8 Minimal API
- Clean service layer with Dependency Injection
- Async/Await used correctly across the application
- EF Core 8 with SQLite (open-source database)
- Database migrations & schema versioning
- Unique constraints enforced at DB level
- Graceful handling of constraint violations (HTTP 409)
- Background hosted service with correct DI scoping
- Swagger/OpenAPI support

---

## 🧱 Architecture Overview



TaskTrackerApi
│
├── BackgroundServices
│ └── TaskMonitorService.cs
│
├── Data
│ └── AppDbContext.cs
│
├── Models
│ └── TaskItem.cs
│
├── Services
│ ├── ITaskService.cs
│ └── TaskService.cs
│
├── Program.cs
└── tasktracker.db


---

## 🛠️ Tech Stack

- **Framework:** .NET 8
- **ORM:** Entity Framework Core 8
- **Database:** SQLite
- **API Style:** Minimal APIs
- **Background Processing:** Hosted BackgroundService
- **Tooling:** dotnet CLI, EF Core migrations

---

## 🗄️ Database

- SQLite is used for simplicity and portability
- Database file: `tasktracker.db`
- Unique constraint enforced on `TaskItem.Title`
- Migrations managed via EF Core

### Apply migrations:
```bash
dotnet ef database update

▶️ Running the Application
dotnet run


Swagger UI will be available at:

http://localhost:5000/swagger

🔄 Background Service

A hosted background service runs alongside the API and periodically:

Creates a scoped service provider

Queries task state

Logs incomplete task counts

This follows the recommended DI lifetime pattern for background services.

🧠 Key Engineering Decisions

In-memory storage was replaced with EF Core + SQLite to ensure persistence

Async operations are used end-to-end (no fake async)

Database constraints act as the final authority

Application-level validation is combined with DB enforcement

DI lifetime issues are handled using IServiceScopeFactory

📌 Future Improvements

Unit tests

Authentication & authorization

Pagination & filtering

PostgreSQL integration

Docker support

👤 Author
Rakhi Ranglani

Built by Bijendra Gaur
Senior Backend / .NET Engineer
