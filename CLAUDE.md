# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Nimbo WMS: a .NET 10 Warehouse Management System backend built with DDD and Clean Architecture. Core concept is a
double-entry **Stock Ledger** — every inventory change is an immutable, auditable ledger entry, not a mutable balance
update.

The `docs/` directory is the authoritative, human-maintained architectural contract — read it before making
non-trivial changes, and treat conflicts between this file and `docs/` in favor of `docs/`:

- `docs/architecture.md` — layering, dependency rules, decision log.
- `docs/domain_overview.md` — bounded contexts, aggregates, invariants.
- `docs/persistence_rules.md` — EF Core mapping rules (typed IDs, value objects, collections, migrations).
- `docs/testing_strategy.md` — test types, Testcontainers approach, posting smoke test contract.
- `docs/ci_workflows.md` — CI job composition and required checks.
- `docs/development.md` — local dev setup, Aspire, migrations.
- `docs/architecture/*.md` — one ADR per bounded context (documents, ledger, master-data, stock, topology) plus
  `use-cases.md`, the full CQRS command/query handler pattern with rationale.

Each key change that alters the project's core structure, dependencies, or architecture should be documented in the corresponding project description file in `docs/`. If no suitable file is found, create a new file in the appropriate directory and briefly describe the concept used in these changes, as is done in other `docs/` files. 

## Commands

```bash
# Run everything via Aspire (Postgres + Kafka + API + Outbox processor) — preferred for local dev
dotnet run --project Nimbo.Wms.AppHost

# Build
dotnet build NimboWMS.sln

# Run the API directly (bypasses AppHost, for targeted debugging only)
dotnet run --project Nimbo.Wms

# All tests
dotnet test

# Unit tests only (no Docker needed) — this is what CI runs as the "unit" job
dotnet test NimboWMS.sln --filter "Category!=Integration"

# Integration tests only (requires Docker; Testcontainers spins up ephemeral Postgres)
dotnet test NimboWMS.sln --filter "Category=Integration"

# Single test
dotnet test --filter "FullyQualifiedName~ReceivingDocumentPostingSmokeTests"

# EF Core migrations (Infrastructure holds DbContext/migrations, Nimbo.Wms is the startup project)
dotnet ef migrations add <Name> --project Nimbo.Wms.Infrastructure --startup-project Nimbo.Wms
dotnet ef database update --project Nimbo.Wms.Infrastructure --startup-project Nimbo.Wms
# or the wrapper scripts:
./ef_pg_init.sh
./ef_add_migration_and_update.sh
```

## Architecture

Strict Clean Architecture with a one-way dependency rule: `Domain ← Application ← Infrastructure ← API`. Never add a
reference in the reverse direction, and never let EF Core / `DbContext` / `IQueryable` leak out of Infrastructure.

- **Nimbo.Wms.Domain** — aggregates, entities, value objects, typed IDs, state machines. Zero external
  dependencies, zero EF Core. Private parameterless constructors exist only for ORM materialization; public
  constructors and methods enforce invariants.
- **Nimbo.Wms.Application** — commands + command handlers (write side). Handlers depend on repository interfaces
  and `IUnitOfWork` (defined in `Application/Abstractions`), never on `DbContext`. Also holds
  `IDocumentPostingService<T>` implementations (`Services/Documents/*PostingService.cs`) that translate a posted
  document into `InventoryItem` updates and `StockLedgerEntry` records.
- **Nimbo.Wms.Infrastructure** — EF Core: `DbContext`, `IEntityTypeConfiguration<T>` mappings, repository
  implementations, migrations, and **queries** (read side — see CQRS split below). Organized under
  `UseCases/{Documents,MasterData,Stock,Topology}`.
- **Nimbo.Wms.Contracts** — DTOs, the external API boundary shape. Used by query handlers (to project into) and by
  controllers (to return). Domain entities must never appear in API responses.
- **Nimbo.Wms** — ASP.NET Core API: controllers, DI composition root (`Program.cs`), HTTP concerns only.
- **Nimbo.Wms.AppHost** — .NET Aspire orchestrator; the standard way to run the full stack locally (Postgres, Kafka,
  Kafka UI, API, Outbox processor).
- **Nimbo.Wms.OutboxProcessor** — background service polling the outbox table and publishing to Kafka.

### CQRS split (commands vs. queries land in different layers)

This is the one thing that looks backwards if you assume queries and commands live together — read
`docs/architecture/use-cases.md` for full rationale.

- **Commands** (writes) live in **Application**, under `Application/Abstractions/UseCases/{Feature}/Commands`, use
  repositories + `IUnitOfWork`, and return domain identities (not DTOs).
- **Queries** (reads) live in **Infrastructure**, under `Infrastructure/UseCases/{Feature}`, query `DbContext`
  directly with `.AsNoTracking()`, and project straight into Contracts DTOs via `.Select(...)`. Query handlers are
  `internal`. No repository indirection on the read side.
- Handlers are dispatched via MediatR (`ISender`/`IRequestHandler<TCommand,TResult>`), with `LoggingBehavior`,
  `ValidationBehavior`, and `TransactionBehavior` as pipeline behaviors (`Application/Common/Behaviors`). Controllers
  call `sender.Send(command, ct)` rather than resolving handlers directly.

### Document posting workflow

Documents (`ReceivingDocument`, `ShipmentDocument`, `RelocationDocument`, `CycleCountDocument`,
`AdjustmentDocument`) are aggregate roots with lifecycle `Draft → InProgress → Completed → Posted`. Posting a
document is the operation that turns operational intent into physical fact:

1. Command handler loads the aggregate via its repository and calls `document.Complete()` (domain enforces
   invariants).
2. Handler resolves the matching `IDocumentPostingService<T>` and calls `PostAsync` — this updates `InventoryItem`
   quantities and appends `StockLedgerEntry` records.
3. Handler calls `document.Post()` and commits via `IUnitOfWork` — **the same command handler**, not the posting
   service, owns the transaction, so Document + Inventory + Ledger commit atomically in one `SaveChanges()`.

`StockLedgerEntry` records are immutable once written (never updated/deleted) and carry a running `BalanceAfter`.
Internal relocations post two entries (`TransferOut` at source, `TransferIn` at destination) — this is the
"double-entry" principle the project is named for.

### Persistence conventions worth knowing before touching mappings

- Typed IDs (`WarehouseId`, `ItemId`, …) everywhere; converted to `uuid` only via `ValueConverter`s in Infrastructure.
- Value objects are `readonly record struct`, mapped with EF Core's `.ComplexProperty()` (not `OwnsOne` — this is an
  EF Core 10+ project and `.ComplexProperty()` is the mandated replacement).
- Collections are exposed as `IReadOnlyCollection<T>` backed by private fields; mutate only through aggregate
  methods (`document.AddLine(...)`), never by exposing a mutable collection.
- Lists of typed IDs (e.g. `List<LocationId>`) are value data stored as Postgres `uuid[]` via a custom
  `ValueConverter`/`ValueComparer` pair — not modeled as FK relationships.
- Cross-layer mapping (entity → DTO) is done exclusively with **Riok.Mapperly** source-generated mappers; use
  `ProjectToDto(IQueryable<T>)` so EF translates the projection into SQL. No manual mapping, no LINQ-in-handlers
  mapping code.
- Lazy loading is forbidden. `EnsureCreated()` is forbidden outside throwaway local experiments — schema always
  comes from migrations (`Database.Migrate()`).
- Domain events / integration events never publish directly to Kafka from UseCases, repositories, or
  `EfUnitOfWork`. They're written as `OutboxMessage` rows in the same `SaveChanges()` transaction as the domain
  change; `OutboxBackgroundService` (in `Nimbo.Wms.OutboxProcessor`) is the only thing that talks to the broker.

## Testing

- xUnit throughout. Tests are split `Category=Integration` vs. not — CI runs these as two separate jobs (unit tests
  first, then integration), see `docs/ci_workflows.md`.
- Integration tests use `WebApplicationFactory` + **Testcontainers** for Postgres — **not** `Aspire.Hosting.Testing`
  (deliberately avoided; it causes DI lifetime conflicts with Aspire's DbContext pooling). The test connection
  string is injected via `Environment.SetEnvironmentVariable("ConnectionStrings__nimboDb", ...)` in the
  `WebApplicationFactory` constructor rather than by touching the DI container.
- Postgres is the only DB engine used in tests — SQLite is explicitly forbidden for persistence validation because
  its behavior diverges from Postgres.
- Every document type must have a posting smoke test (see `Nimbo.Wms.Infrastructure.Tests/Smoke/`) that drives a
  document through its full lifecycle and asserts on three things: document status, `InventoryItem` quantities, and
  the resulting `StockLedgerEntry` rows.
- `Nimbo.Wms.Tests.Common` and `PostgresCollection`/`PostgresFixture` provide shared fixtures for spinning up an
  ephemeral database per test collection.
- `Nimbo.Wms.Api.Tests` exercises the full HTTP surface via `NimboWmsApiFactory`; `Nimbo.Wms.Infrastructure.Tests`
  covers repository/mapping/migration behavior directly against `DbContext`.
