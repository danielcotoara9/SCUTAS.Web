# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```bash
# Build the entire solution
dotnet build SCUTAS.slnx

# Run with .NET Aspire (recommended — provisions SQL Server + email containers)
dotnet run --project src/SCUTAS.AspireHost/SCUTAS.AspireHost.csproj

# Run the web project standalone (uses SQLite fallback)
dotnet run --project src/SCUTAS.Web/SCUTAS.Web.csproj
```

## Testing

```bash
# Run all tests
dotnet test SCUTAS.slnx

# Run a specific test project
dotnet test tests/SCUTAS.UnitTests/SCUTAS.UnitTests.csproj
dotnet test tests/SCUTAS.IntegrationTests/SCUTAS.IntegrationTests.csproj
dotnet test tests/SCUTAS.FunctionalTests/SCUTAS.FunctionalTests.csproj

# Run a single test by name filter
dotnet test --filter "FullyQualifiedName~ContributorConstructor"

# Run with parallel execution settings
dotnet test --settings .runsettings
```

Functional tests use **Testcontainers** (SQL Server 2022 Docker image). Docker Desktop must be running. If Docker is unavailable, tests fall back to SQLite automatically.

## EF Core Migrations

Run from `src/SCUTAS.Web/`:

```bash
dotnet ef migrations add MigrationName -c AppDbContext -p ../SCUTAS.Infrastructure/SCUTAS.Infrastructure.csproj -s SCUTAS.Web.csproj -o Data/Migrations
dotnet ef database update -c AppDbContext -p ../SCUTAS.Infrastructure/SCUTAS.Infrastructure.csproj -s SCUTAS.Web.csproj
```

## Architecture

This is a **Clean Architecture** solution with four layers plus an Aspire orchestration host.

```
SCUTAS.Core          — domain model (no dependencies)
SCUTAS.UseCases      — application logic (depends on Core)
SCUTAS.Infrastructure — EF Core, repos, email (depends on Core + UseCases)
SCUTAS.Web           — HTTP layer (depends on Infrastructure + UseCases)
SCUTAS.AspireHost    — orchestration only (not a library)
```

### Core layer (`src/SCUTAS.Core`)

- Entities derive from `EntityBase<TEntity, TId>` (Ardalis.SharedKernel). Aggregates also implement `IAggregateRoot`.
- **Strongly-typed IDs and value objects** use **Vogen** (`[ValueObject<T>]` attribute). Use `ContributorId.From(value)` and `ContributorName.From(value)` — they throw on invalid input (parse-don't-validate).
- Domain events are registered via `RegisterDomainEvent()` and dispatched by `EventDispatcherInterceptor` in EF Core's `SaveChanges`.
- Specifications (Ardalis.Specification) live next to the aggregate they query.

### UseCases layer (`src/SCUTAS.UseCases`)

- Uses **Mediator source generator** (not MediatR). Commands implement `ICommand<Result<T>>`, queries implement `IQuery<Result<T>>`. Handlers implement `ICommandHandler<,>` / `IQueryHandler<,>`.
- Read-only list queries delegate to an `IListContributorsQueryService` interface — the implementation lives in Infrastructure to avoid loading full aggregates.
- `PagedResult<T>` wraps paginated responses. Default page size: `Constants.DEFAULT_PAGE_SIZE = 10`, max: `Constants.MAX_PAGE_SIZE = 100`.

### Infrastructure layer (`src/SCUTAS.Infrastructure`)

- `InfrastructureServiceExtensions.AddInfrastructureServices()` auto-selects the database provider:
  1. `cleanarchitecture` connection string (injected by Aspire) → SQL Server
  2. `DefaultConnection` on Windows (or `USE_SQL_SERVER=true`) → SQL Server
  3. `SqliteConnection` → SQLite fallback
- `EfRepository<T>` implements both `IRepository<T>` and `IReadRepository<T>` via Ardalis.Specification.EntityFrameworkCore.
- Domain events are dispatched inside `EventDispatchInterceptor` (EF Core `SaveChangesInterceptor`), which calls `IDomainEventDispatcher` before saving.

### Web layer (`src/SCUTAS.Web`)

**Primary API style: FastEndpoints.** Endpoint classes live under feature folders (e.g., `Contributors/`), each in its own file. Each endpoint overrides `Configure()` (route, method, auth) and `ExecuteAsync()` or `HandleAsync()`.

**Secondary: traditional MVC controllers** in `Controllers/`. Because FastEndpoints registers `HttpGet`, `HttpPost`, `FromBody`, etc. as global usings, MVC controllers must alias the MVC attributes to avoid ambiguity:

```csharp
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
// etc.
```

**`ResultExtensions`** (`Extensions/ResultExtensions.cs`) maps `Ardalis.Result` status to typed HTTP results. Use `ToCreatedResult`, `ToGetByIdResult`, `ToUpdateResult`, `ToDeleteResult` in endpoints and controllers.

**Mediator** is registered as Scoped in `MediatorConfig.cs`. Assemblies scanned: Core, UseCases, Infrastructure, Web. Pipeline includes `LoggingBehavior<,>`.

**Startup** is split across `Configurations/`:
- `ServiceConfigs.cs` — infrastructure + mediator + email DI
- `MiddlewareConfig.cs` — middleware pipeline, DB migrations/seeding, `MapControllers()`
- `OptionConfigs.cs` — options binding
- `MediatorConfig.cs` — mediator assembly scanning

### Aspire host (`src/SCUTAS.AspireHost`)

Provisions a persistent SQL Server container (`cleanarchitecture` database) and a Papercut SMTP container. Injects connection strings and environment variables into the Web project automatically.

## Key Libraries

| Library | Purpose |
|---|---|
| FastEndpoints | Primary HTTP endpoint pattern |
| Mediator.SourceGenerator | Source-generated IMediator (not MediatR) |
| Vogen | Strongly-typed value objects / IDs |
| Ardalis.Result | Operation result type (`Result<T>`, `ResultStatus`) |
| Ardalis.Specification | Repository query specs |
| Ardalis.SharedKernel | `EntityBase`, `ValueObject`, `IAggregateRoot` |
| Shouldly | Test assertions |
| NSubstitute | Test mocking |
| Testcontainers.MsSql | SQL Server in Docker for functional tests |
