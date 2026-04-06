# Local Development & Debugging

This document is the source of truth for local development and debugging of Nimbo WMS. It lists the stable, project-specific commands and configuration used today.

## 1. .NET Aspire 9.0 AppHost Entry Point

- **Primary entry point:** `Nimbo.Wms.AppHost` is the orchestration container for local development.
- **Orchestrated services:** The AppHost automatically provisions and manages:
  - PostgreSQL database
  - Apache Kafka broker
  - Kafka UI (for topic and message inspection)
  - Nimbo WMS API (`Nimbo.Wms`)
  - Outbox background processor (`Nimbo.Wms.OutboxProcessor`)
- **Starting the local environment:** Run the AppHost project:

```bash
dotnet run --project Nimbo.Wms.AppHost
```

- **Aspire Dashboard:** Open the Aspire Dashboard (typically `https://localhost:17360`) to access:
  - Real-time service logs and output
  - Environment variables and configuration
  - Distributed traces and telemetry

The Dashboard is the primary tool for debugging, observability, and monitoring during local development.

## 2. Env vars and Appsettings

- Development appsettings: `Nimbo.Wms/appsettings.Development.json` contains a default connection string key `ConnectionStrings:NimboWmsDb`.
- To override connection string via environment variable use the ASP.NET Core configuration naming: `ConnectionStrings__NimboWmsDb`.
- Set environment for local run:

```bash
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__NimboWmsDb='Host=localhost;Port=5432;Database=nimbo_wms;Username=nimbo_admin;Password=nimbo_password'
```

On Windows PowerShell use `setx` or `$env:` for the session.

## 3. How to apply migrations (script run)

- Recommended (project-provided scripts): use the repository scripts when available: `ef_pg_init.sh` and `ef_add_migration_and_update.sh` (Linux/macOS). These wrap common `dotnet ef` operations and project parameters.
- Direct `dotnet ef` usage (explicit):

```bash
dotnet ef migrations add <Name> --project Nimbo.Wms.Infrastructure --startup-project Nimbo.Wms
dotnet ef database update --project Nimbo.Wms.Infrastructure --startup-project Nimbo.Wms
```

- Integration tests and local verification apply migrations via `Database.Migrate()` (the application code or tests may call this). Do not use `EnsureCreated()` for migration validation.

## 4. Standard `dotnet` commands used in the project

- Build the solution:

```bash
dotnet build NimboWMS.sln
```

- Run the AppHost orchestrator (recommended; replaces direct `dotnet run` for the API):

```bash
dotnet run --project Nimbo.Wms.AppHost
```

- Run the API directly (bypasses AppHost; for debugging specific API issues only):

```bash
dotnet run --project Nimbo.Wms
```

- Run tests (unit & integration; integration requires Docker available):

```bash
dotnet test
```

- Create migrations and update database (see section 3):

```bash
dotnet ef migrations add <Name> --project Nimbo.Wms.Infrastructure --startup-project Nimbo.Wms
dotnet ef database update --project Nimbo.Wms.Infrastructure --startup-project Nimbo.Wms
```

## Notes and constraints

- Docker is required locally to run integration tests that validate persistence and to run the AppHost. CI runs integration tests in Docker as well.
- The repository includes `appsettings.Development.json` with a default connection string for local development; the AppHost manages service provisioning and connection strings automatically.
- `EnsureCreated()` is not used for validating migrations or production-like schemas.
- The AppHost orchestrator is the recommended entry point; direct `dotnet run` on individual projects is for development debugging only.

---

This file intentionally contains only the stable, project-specific local setup steps. For onboarding or extended environment setups, add a separate `docs/onboarding.md` rather than expanding this file.
