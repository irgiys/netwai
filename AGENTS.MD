## Tech Stack
- .NET 10, ASP.NET Core Minimal APIs
- Entity Framework Core 10 with PostgreSQL
- Mediator for CQRS pattern (source-generated)
- FluentValidation for request validation
- Scalar for API documentation (OpenAPI)

## Project Structure
- `src/Api/` - Entry point, endpoints, middleware
- `src/Application/` - Commands, queries, handlers, DTOs
- `src/Domain/` - Entities, value objects, domain events
- `src/Infrastructure/` - EF Core, external integrations
- `tests/` - Unit and integration tests


## Architecture Philosophy
We follow Clean Architecture with CQRS separation:
- **Domain** has zero dependencies - pure business logic
- **Application** orchestrates use cases via Mediator handlers
- **Infrastructure** implements interfaces defined in Application
- **Api** is thin - just endpoint definitions and DI wiring

Why CQRS? We need different read/write models for performance.
Why Mediator? Decouples handlers from HTTP layer, enables pipeline behaviors, source-generated for better performance.

## Commands
- Build: `dotnet build`
- Test: `dotnet test --no-build`
- Run: `dotnet run --project src/Api`
- Migrations: `dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/Api`

## Workflow Rules
- Run `dotnet test` after every implementation
- Never modify files in `src/Domain/` without explicit approval
- Keep commits atomic - one logical change per commit

