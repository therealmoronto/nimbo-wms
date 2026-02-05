# Local Development & Debugging

This document is the source of truth for local development and debugging of Nimbo WMS. It lists the stable, project-specific commands and configuration used today.

## 1. Docker compose for PostgreSQL

- Recommended file: `docker-compose.postgres.yml` (place next to repository root). The project expects a local Postgres database named `nimbo_wms` with user `nimbo_admin` by default (see `appsettings.Development.json`).
- Minimal service (example):

```yaml
version: '3.8'
services:
  db:
    image: postgres:15
    environment:
      POSTGRES_DB: nimbo_wms
      POSTGRES_USER: nimbo_admin
      POSTGRES_PASSWORD: nimbo_password
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data
volumes:
  pgdata:
```

- Start/stop:

```bash
docker compose -f docker-compose.postgres.yml up -d
docker compose -f docker-compose.postgres.yml down -v
```

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

- Run the API locally (uses `appsettings.Development.json` when `ASPNETCORE_ENVIRONMENT=Development`):

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

- Docker is required locally to run integration tests that validate persistence. CI runs integration tests in Docker as well.
- The repository includes `appsettings.Development.json` with a default connection string for local development; overriding via environment variables is the supported approach for per-developer credentials.
- `EnsureCreated()` is not used for validating migrations or production-like schemas.

---

This file intentionally contains only the stable, project-specific local setup steps. For onboarding or extended environment setups, add a separate `docs/onboarding.md` rather than expanding this file.
