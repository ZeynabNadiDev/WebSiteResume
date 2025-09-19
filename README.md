# Resume & Reservation Platform

An ASP.NET Core 9 web application following **Clean Architecture**, designed both as an online personal **resume/CV showcase** and a **reservation booking system** (e.g., for meetings, consultations, or events).

The project is built with modern backend practices, advanced libraries, and horizontal scalability in mind.

---

## ✨ Features

- **Resume Management** – Display personal info, skills, work history, and portfolio.
- **Reservation System** – Users can view and book available time slots.
- **Clean Architecture** – Clear separation of concerns across layers.
- **CQRS with MediatR** – Command & Query segregation for maintainability.
- **FluentValidation** – Centralized and clean request validation.
- **AutoMapper** – Seamless mapping between domain models and DTOs.
- **Redis Cache** – High-performance caching for frequently accessed data.
- **Serilog** – Structured logging for monitoring and debugging.

---

## 🏗 Project Structure
```
WebsiteResume/
 ├── Resume.Application     # Application logic, CQRS handlers, validation
 ├── Resume.Domain          # Core domain entities & contracts
 ├── Resume.Infra.Data      # Database access via EF Core
 ├── Resume.Infra.IOC       # Dependency injection & service registration
 ├── Resume.Web             # Web API layer & controllers
```

---

## 🛠 Tech Stack

| Area            | Technology |
|-----------------|------------|
| Architecture    | Clean Architecture, CQRS, MediatR |
| Mapping         | AutoMapper |
| Validation      | FluentValidation |
| Caching         | Redis |
| Logging         | Serilog (Console, File, Seq) |
| Database        | EF Core |
| Backend Runtime | ASP.NET Core 9 |

---

## 📚 CQRS & Pipeline Behaviors

This project uses **MediatR pipeline behaviors** for:
- **Validation**: All commands and queries pass through FluentValidation before execution.
- **Logging**: Serilog logs request execution details, correlation IDs, and performance timings.

---

## 🧩 Redis Caching

- Frequently called queries such as `GetResumeDetailsQuery` and `GetAvailableSlotsQuery` are cached.
- **Cache Aside Pattern** is used with TTL-based expiration.
- Cache is updated or invalidated on data changes.

---

## 📝 Developer Notes

- To run locally:
  ```bash
  cd Resume.Web
  dotnet run
  ```
- Environment-specific configs (e.g., DB connection, Redis settings) are in:
  - `appsettings.json`
  - `appsettings.Development.json`
- Adjust the runtime environment using:
  ```bash
  set ASPNETCORE_ENVIRONMENT=Development
  ```

---

## 📄 License
Distributed under the MIT License.
