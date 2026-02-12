# CQRS Use Case Architecture

## Overview

Nimbo WMS implements a CQRS (Command Query Responsibility Segregation) pattern through explicit handler interfaces and message objects. Commands modify state; Queries retrieve state. This separation is enforced through the type system and project structure, not by a service bus or message queue.

---

## Core Design: Explicit Command and Query Handlers

### Why Explicit Handlers

Each use case maps to:
- **Command**: A record implementing `ICommand` or `ICommand<TResult>` (defined in Application layer)
- **CommandHandler**: A class implementing `ICommandHandler<TCommand>` or `ICommandHandler<TCommand, TResult>`
- **Query**: A record implementing `IQuery<TResult>` (defined in Application layer)
- **QueryHandler**: A class implementing `IQueryHandler<TQuery, TResult>`

**Decision: Explicit handlers over implicit service methods**

- Every use case is discoverable by searching for `ICommandHandler<>` or `IQueryHandler<>` implementations
- Command and Query signatures are self-documenting—parameters are visible in the type signature
- No hidden dependencies buried in service method parameters
- Eliminates ambiguous overload resolution or implicit parameter injection patterns
- Future static analysis tools can validate that all queries return DTOs and all commands use repositories

### Why Not Service Classes

We do not provide generic service methods like:
```csharp
// ❌ Not used in Nimbo WMS
public class WarehouseService
{
    public async Task CreateWarehouse(string code, string name) { ... }
    public async Task<List<WarehouseDto>> GetWarehouses() { ... }
}
```

**Reasons:**
- Service classes obscure the actual contract of an operation
- No type safety between caller and handler
- Difficult to track what data flows in and out
- Adds a naming indirection layer (is it `CreateWarehouse` or `AddWarehouse`?)
- Controller logic becomes tempted to add business logic around service calls

---

## Separation of Read and Write

### Commands (Write): Application Layer

Commands use **repositories and the Unit of Work pattern**.

**Location:** `Nimbo.Wms.Application.Abstractions.UseCases.{Feature}.Commands`

**Structure:**
```
CreateWarehouse : ICommand<WarehouseId>
CreateWarehouseHandler : ICommandHandler<CreateWarehouseCommand, WarehouseId>
  ├─ IWarehouseRepository (from Application abstractions)
  ├─ IUnitOfWork (from Application abstractions)
  └─ Domain entity creation and persistence
```

**Decision: Commands live in Application**

- Keeps domain business logic close to the command implementation
- Repositories provide a domain-centric view of data (entities, not rows)
- Unit of Work ensures transactional consistency across multiple aggregates
- No coupling of Application to Infrastructure DbContext

### Queries (Read): Infrastructure Layer

Queries use **DbContext directly** and return **DTOs**.

**Location:** `Nimbo.Wms.Infrastructure.UseCases.{Feature}.Queries`

**Structure:**
```
GetWarehousesQuery : IQuery<IReadOnlyList<WarehouseListItemDto>>
GetWarehousesHandler : IQueryHandler<GetWarehousesQuery, IReadOnlyList<WarehouseListItemDto>>
  ├─ NimboWmsDbContext (from Infrastructure)
  ├─ .AsNoTracking() (no entity tracking needed)
  └─ .Select(x => new WarehouseListItemDto(...))
```

**Decision: Queries in Infrastructure with DbContext**

- Queries don't modify state, so Entity Framework change tracking is unnecessary overhead
- `.AsNoTracking()` queries are fast and avoid change tracker memory pressure
- DTOs are read models—they are the exact shape needed for API responses
- No need for repository indirection; SQL projection directly maps to DTO shape
- Single responsibility: Infrastructure handles data access for reads

---

## Read-Side Architecture: DTOs and Contracts

### Why DTOs Live in Contracts

DTOs are defined in `Nimbo.Wms.Contracts` and imported by:
- Queries in Infrastructure (to project into DTOs)
- API Controllers (to return DTOs to HTTP clients)
- Test factories (to verify response shapes)

**Decision: DTOs in Contracts, not in Application**

- **Contracts represent the external boundary** of the system (what the API exposes)
- DTOs are part of the API contract, not internal application concerns
- Domain entities must never appear in API responses—DTOs provide translation
- Contracts are stable; once published, backwards compatibility matters
- Domain entities can evolve independently without affecting API clients

**Example: Warehouse DTOs**
```csharp
// Contracts/Topology/Dtos/WarehouseListItemDto.cs
public record WarehouseListItemDto(WarehouseId Id, string Code, string Name);

// Contracts/Topology/Dtos/WarehouseTopologyDto.cs
public record WarehouseTopologyDto(
    WarehouseId Id,
    string Code,
    string Name,
    IReadOnlyList<ZoneDto> Zones,
    IReadOnlyList<LocationDto> Locations
);
```

### Why Queries Return DTOs, Not Entities

**Decision: Queries project directly to DTOs**

- Entities are domain objects with business logic and invariants—they shouldn't exist outside their aggregate
- DTOs are data transfer objects—exactly the shape needed for external consumption
- EF Core `Select()` projections are efficient; no need to hydrate full entity graphs then map them
- Change tracking is not needed for read-only data
- API responses don't need entity methods, validation, or state machines—only data

### Why Read-Side Doesn't Use Repositories

Repositories are an abstraction over write operations. Read-side queries don't need them:

- Repositories ensure aggregate consistency (Unit of Work)
- Queries don't modify state, so no consistency guarantees needed
- Repositories use `DbSet<T>.Where()` which returns entities, not DTOs
- Direct DbContext queries with `.Select()` are simpler and more efficient
- Read-side is allowed to query across aggregates (queries don't have strict aggregate boundaries like commands do)

**Example: Repository (write) vs. DbContext Query (read)**
```csharp
// ✅ Write: Repository maintains consistency
var warehouse = await _repository.GetByIdAsync(id);
warehouse.AddZone(zone);
await _unitOfWork.SaveChangesAsync();

// ✅ Read: Direct query projection to DTO
var warehouse = await _db.Set<Warehouse>()
    .AsNoTracking()
    .Select(x => new WarehouseTopologyDto(...))
    .FirstOrDefaultAsync();
```

---

## Infrastructure Isolation: No EF Core in Application or Domain

### Constraint: DbContext is Infrastructure-Only

- `NimboWmsDbContext` exists only in `Nimbo.Wms.Infrastructure`
- Application layer has no reference to `Microsoft.EntityFrameworkCore`
- Domain layer has no reference to any persistence framework
- DTOs and query contracts live in `Nimbo.Wms.Contracts` or as local records in Infrastructure handlers

**Why this matters:**

- Persistence is a detail—the Application layer is persistence-agnostic
- If we switch from EF Core to Dapper, only Infrastructure changes
- Entity Framework imports in Application lead to repository anti-patterns (exposing `IQueryable<T>`)
- Clean separation ensures domain logic is testable without a database

### Example: IQueryable Leakage Prevention

**❌ What we don't do:**
```csharp
// Never expose DbContext queries to Application
public interface IWarehouseRepository
{
    IQueryable<Warehouse> Query(); // ❌ EF Core leaks into Application
}
```

**✅ What we do:**
```csharp
// Specific query handlers in Infrastructure
public class GetWarehousesHandler : IQueryHandler<GetWarehousesQuery, IReadOnlyList<WarehouseListItemDto>>
{
    public async Task<IReadOnlyList<WarehouseListItemDto>> HandleAsync(...)
    {
        return await _db.Set<Warehouse>()
            .AsNoTracking()
            .Select(x => new WarehouseListItemDto(...))
            .ToListAsync();
    }
}
```

---

## Explicitly Rejected Alternatives

### 1. Repository-Only Querying

**Rejected: Exposing repositories for all read operations**

- Repositories return entities, not DTOs—extra mapping required
- Entity Framework change tracking wastes memory on read-only data
- No advantage over direct DbContext queries for reads
- Repositories blur the write/read boundary

### 2. Specification Pattern

**Rejected: Specification<T> objects for complex queries**

- Adds abstraction layer over queries
- Queries in Infrastructure are already concrete and testable
- Specifications tend to hide complex filtering logic
- DbContext queries are explicit about their filtering

### 3. Exposing IQueryable Outside Infrastructure

**Rejected: Passing IQueryable<T> to Application layer**

- EF Core dependency leaks into Application
- Deferred execution surprises (queries execute in unexpected places)
- Makes it impossible to refactor persistence layer
- Violates Dependency Inversion Principle

### 4. Returning Domain Entities from API

**Rejected: Returning `Warehouse` entity directly from controllers**

- Domain entities have private setters and business logic methods
- API responses expose all internal entity state unnecessarily
- DTOs provide a stable, versioned contract
- Changes to domain logic would force API version bumps

---

## Architectural Boundaries

```
┌──────────────────────────────────────────────────────────────┐
│ Nimbo.Wms (API)                                              │
│ ├─ Controllers: Inject query/command handlers                │
│ └─ Call: await handler.HandleAsync(query/command)           │
└──────────────────────────────────────────────────────────────┘
                           ↓
┌──────────────────────────────────────────────────────────────┐
│ Nimbo.Wms.Contracts (External Data Shapes)                   │
│ ├─ DTOs: WarehouseListItemDto, WarehouseTopologyDto         │
│ └─ Used by: API responses, query handlers, test factories   │
└──────────────────────────────────────────────────────────────┘
                           ↓
┌──────────────────────────────────────────────────────────────┐
│ Nimbo.Wms.Application (Business Logic & Abstractions)        │
│ ├─ Commands: CreateWarehouse, AddZone, ...                  │
│ ├─ Command Handlers: Inject repositories & UnitOfWork       │
│ ├─ Queries: GetWarehouses, GetWarehouseTopology, ...        │
│ └─ Query Abstractions: Defined here, NOT implemented here   │
└──────────────────────────────────────────────────────────────┘
             ↙─────────────────────────────────────↘
            ↓                                        ↓
  ┌──────────────────────────┐      ┌──────────────────────────┐
  │ Nimbo.Wms.Domain         │      │ Nimbo.Wms.Infrastructure  │
  │ ├─ Entities & Aggregates │      │ ├─ DbContext             │
  │ ├─ Value Objects         │      │ ├─ Query Handlers        │
  │ ├─ Domain Logic          │      │ ├─ Repositories          │
  │ └─ Invariants            │      │ ├─ UnitOfWork            │
  │                          │      │ └─ Migrations            │
  │ (Persistence-agnostic)   │      │ (EF Core implementation) │
  └──────────────────────────┘      └──────────────────────────┘
```

### Layer Responsibilities

**API (Nimbo.Wms)**
- Route HTTP requests to handlers
- Serialize/deserialize HTTP bodies
- Return DTOs as JSON responses

**Contracts (Nimbo.Wms.Contracts)**
- Define external data shapes
- Backward-compatible DTO versioning
- Shared by API, queries, and tests

**Application (Nimbo.Wms.Application)**
- Define command and query messages
- Implement command business logic (handlers in Domain/Application)
- Declare query abstractions (implemented in Infrastructure)
- Define repository interfaces for commands

**Domain (Nimbo.Wms.Domain)**
- Entities, aggregates, value objects
- Business logic and invariants
- Zero dependencies on Infrastructure or Application

**Infrastructure (Nimbo.Wms.Infrastructure)**
- Query handler implementations
- Repository implementations
- DbContext and EF Core configuration
- Unit of Work implementation

---

## Command Handler Pattern

Commands always follow this structure:

```csharp
public sealed record CreateWarehouseCommand(string Code, string Name) : ICommand<WarehouseId>;

public sealed class CreateWarehouseHandler : ICommandHandler<CreateWarehouseCommand, WarehouseId>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWarehouseHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WarehouseId> HandleAsync(CreateWarehouseCommand command, CancellationToken ct = default)
    {
        // 1. Create domain entity
        var warehouse = new Warehouse(WarehouseId.New(), command.Code, command.Name);

        // 2. Persist via repository
        await _repository.AddAsync(warehouse, ct);

        // 3. Commit transaction
        await _unitOfWork.SaveChangesAsync(ct);

        return warehouse.Id;
    }
}
```

**Constraints:**
- Every command has exactly one handler
- Handlers are in Application or Infrastructure (not in Services)
- Commands are immutable records
- Commands return domain identities, not DTOs

---

## Query Handler Pattern

Queries always follow this structure:

```csharp
public sealed record GetWarehousesQuery : IQuery<IReadOnlyList<WarehouseListItemDto>>;

internal sealed class GetWarehousesHandler : IQueryHandler<GetWarehousesQuery, IReadOnlyList<WarehouseListItemDto>>
{
    private readonly NimboWmsDbContext _db;

    public GetWarehousesHandler(NimboWmsDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<WarehouseListItemDto>> HandleAsync(
        GetWarehousesQuery query,
        CancellationToken ct = default)
    {
        return await _db.Set<Warehouse>()
            .AsNoTracking()
            .Select(x => new WarehouseListItemDto(x.Id, x.Code, x.Name))
            .ToListAsync(ct);
    }
}
```

**Constraints:**
- Every query has exactly one handler
- Handlers are always in Infrastructure (access to DbContext)
- Query handlers are `internal`—called via injection, never directly
- All queries return DTOs, never domain entities
- `.AsNoTracking()` for all DbContext queries
- Queries project directly to DTOs in `.Select()`

---

## Why MediatR Is Not Used (Yet)

MediatR is a common CQRS library that provides a service bus for dispatching commands and queries. We do not use it because:

**Explicit handlers are sufficient:**
- Dependency injection resolves handlers directly
- No magic dispatch behavior to debug
- Compiler errors if handler is missing
- Simple and clear control flow

**MediatR adds overhead for our use cases:**
- Reflection-based handler discovery adds latency
- Pipelines/behaviors add middleware complexity we don't need yet
- Service bus semantics suggest async messaging (we're in-process)
- Adds a NuGet dependency for a thin wrapper over DI

**When MediatR may be introduced:**
- If we need cross-cutting concerns (logging, validation, authorization pipelines)
- If we adopt async event sourcing and event publishing
- If we move to microservices and true async messaging
- If we need to standardize command/query dispatch across multiple services

**Until then:** Direct dependency injection is clearer and requires no additional library.

---

## Evolution Path

### Phase 1: Current State (Explicit Handlers)

- Commands and queries with explicit handlers
- Commands use repositories and Unit of Work
- Queries project directly to DTOs
- No service bus or pipeline middleware

### Phase 2: Cross-Cutting Concerns (If Needed)

When we need to standardize validation, logging, or authorization:

**Option A: Decorator pattern**
```csharp
public sealed class ValidatingCommandHandler<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _inner;
    private readonly IValidator<TCommand> _validator;

    public async Task HandleAsync(TCommand command, CancellationToken ct)
    {
        await _validator.ValidateAsync(command);
        await _inner.HandleAsync(command, ct);
    }
}
```

**Option B: MediatR with pipelines**
- Add MediatR for pipeline behaviors
- Keep handler structure identical
- Enables validation, logging, and authorization middleware

### Phase 3: Read Model Optimization (If Needed)

As query complexity grows:

- **Denormalized read tables**: Separate "query database" schema optimized for specific queries
- **Event sourcing**: Rebuild read models from domain events
- **CQRS-specific query projections**: Real-time or eventual consistency models
- **Materialized views**: Database views for complex reporting

This remains in Infrastructure—Application layer never changes.

### Phase 4: Event Publishing (If Needed)

When domain events need external propagation:

- Domain events raised during command execution
- Query handlers subscribe to events for read model updates
- Or: Events published to event bus (Kafka, RabbitMQ, etc.)
- Or: Polling for changes if async event sourcing is adopted

---

## Rules for Contributors

1. **Every use case is a command or query**
   - Avoid service methods that mix read and write
   - Avoid implicit parameters—make them explicit in the message

2. **Commands live in Application, use repositories**
   - Commands always modify state
   - Use UnitOfWork for transactions
   - Return domain identities, not DTOs

3. **Queries live in Infrastructure, return DTOs**
   - Queries always return `IReadOnlyList<T>` or single DTOs
   - Always use `.AsNoTracking()`
   - Never pass Domain entities up the stack

4. **DTOs live in Contracts**
   - Contracts are the API boundary
   - DTOs are stable—treat as versioned
   - Never reuse domain entities as DTOs

5. **No persistence framework code outside Infrastructure**
   - No `DbContext`, `IQueryable`, or EF Core in Application or Domain
   - No `Repository<T>` base classes with generic query methods
   - DbContext is a deployment detail

6. **No service classes**
   - Use handlers instead
   - Inject handlers into controllers
   - One handler per use case

---

## References

- [Domain-Driven Design](../domain_overview.md)
- [Persistence Rules](../persistence_rules.md)
- [Testing Strategy](../testing_strategy.md)
