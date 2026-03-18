# Persistence Rules

This document is the source of truth describing how persistence is handled in this codebase. It is concise, prescriptive, and implementation-oriented — not a tutorial.

## General Principles

- **EF Core role:** EF Core is a persistence mechanism only; it must not drive domain model design.
- **Domain independence:** Domain model assemblies must not reference EF Core types or packages.
- **No lazy loading:** Lazy loading is forbidden; all navigation loading must be explicit to avoid hidden side effects.
- **Explicit configuration:** All mapping and behavior must be configured explicitly using `IEntityTypeConfiguration<T>` implementations.

## Entity Construction Rules

- **EF constructor:** Every entity must declare a private parameterless constructor for EF Core materialization.
- **Public constructors:** Any public constructors must enforce domain invariants and invariants must be validated at construction time.
- **No constructor-binding for domain params:** EF Core must never rely on public constructors that accept domain parameters as its primary construction mechanism. EF should materialize using the private parameterless constructor and property/field mapping only.

## Typed Identifiers

- **Strongly typed IDs:** All aggregate roots and entities use strongly typed identifier classes (for example `InventoryCountId`, `LocationId`).
- **Mapping typed IDs:** Typed identifier types are mapped to primitive storage (UUID) using `ValueConverter` implementations in the EF mapping layer.
- **FKs use typed IDs:** Foreign key properties and navigation keys use the same typed identifier types (not raw `Guid`/`string`).

## Value Objects

- **Immutability:** Value objects must be immutable. Their state is set at creation and cannot be changed after construction.
- **Implementation:** All value objects must be implemented as `readonly record struct` to leverage copy-by-value semantics. This is essential when the same value object type is used in multiple fields of a single entity (e.g., `Quantity` and `ExpectedQuantity` on the same line item). Copy-by-value prevents EF Core identity tracking collisions and ensures proper change detection without false "dirty" states.
- **Mapping Standard:** Value objects are mapped exclusively with `.ComplexProperty()` configuration in EF Core (available in EF Core 10+). This replaces the legacy `OwnsOne` / owned entity pattern and must be used for all value object persistence.
- **ComplexProperty benefits:** `.ComplexProperty()` removes the need for `WithOwner()` boilerplate and simplifies configuration when value objects appear in multiple fields. It provides clean, idiomatic EF Core 10+ semantics without owned entity overhead.
- **Optional complex values:** Optional value objects are modeled as nullable complex properties (the property can be null).
- **Scalar nullability:** Inner scalar properties of value objects must not be nullable unless the domain explicitly requires null as a valid value.

## Data Mapping & Projections (Mapperly)

- **Mandatory tooling:** [Riok.Mapperly](https://mapperly.riok.app/) is the mandatory source generator for all transformations between Domain Entities and Contracts (DTOs). All cross-layer DTO mapping must be implemented via Mapperly mappers, not manual mapping code or LINQ projections.
- **Projection pattern:** For database-driven queries, always utilize `ProjectToDto(IQueryable<T>)` mapper methods. EF Core translates these mapping expressions directly into `SELECT` statement clauses, avoiding in-memory materialization and ensuring query efficiency at the database layer.
- **Value Object flattening:** Mappers must provide static mapping methods to flatten `readonly record struct` Value Objects into DTO primitives. For example, a domain `Quantity` structs (with `Value: decimal` and `Uom: string`) must be mapped to DTO properties such as `QuantityValue: decimal` and `QuantityUom: string`. This bridging logic belongs in the mapper, not scattered across domain or application code.
- **Explicit property mapping:** Use the `[MapProperty]` attribute to resolve naming mismatches between Domain Entities and Contracts. For example, `[MapProperty(nameof(BaseUomCode), nameof(BaseUom))]` explicitly maps `BaseUomCode` on the entity to `BaseUom` on the DTO, making the intent clear and maintainable.
- **Compile-time safety:** Mapperly provides compile-time validation of all mapping rules. Any unmapped required properties are reported as compilation errors and must be resolved via explicit configuration (e.g., `[MapProperty]`, mapper methods, or constructor injection). Do not ignore or suppress these warnings; all required properties must be explicitly addressed to ensure data integrity.
- **No manual mapping overhead:** Mappers are source-generated at compile time, eliminating the need for manual mapping boilerplate and reducing the risk of mapping bugs introduced by human error.

## Collections and Backing Fields

- **Read-only exposure:** Collections on aggregates are exposed to consumers as `IReadOnlyCollection<T>` (or another read-only interface).
- **Private backing fields:** EF Core maps collection navigations via private backing fields. Domain code must mutate the backing field through explicit methods on the aggregate (e.g., `AddLine(...)`, `RemoveLine(...)`).
- **Explicit field config:** Backing fields must be explicitly configured in the entity mapping (`HasField`, `UsePropertyAccessMode`) to avoid accidental reliance on conventions.
- **Public collection properties:** Public mutable collection properties should be ignored by EF (or only treated as non-persisted helpers). The persisted collection must be backed by the private field and mapped as the navigation.

## Lists of Identifiers (uuid[] storage)

- **Value-list semantics:** Lists of typed identifiers (for example `List<LocationId>`) represent value data, not relations, and are stored as `uuid[]` columns in PostgreSQL.
- **Mapping approach:** These lists are mapped using a custom `ValueConverter` (to/from `Guid[]`) together with a `ValueComparer` to ensure EF can detect changes.
- **No FK constraints:** Lists stored as `uuid[]` must not be modeled as foreign-key relationships; they are denormalized value data.

## Documents and Lines (Aggregate Structure)

- **Aggregate boundaries:** Documents (e.g., `InventoryCount`, `ShipmentOrder`) are aggregate roots and maintain their own transactional consistency boundaries.
- **Lines belong to aggregate:** Line items are entities within the document aggregate. They have identity but are not aggregate roots and must not be loaded or manipulated independently of their parent aggregate.
- **Parent reference:** Lines reference their parent document using the parent's typed identifier (not a raw primitive).

## Migrations

- **Apply migrations:** Database schema updates must be applied through `Database.Migrate()` in application startup or deployment migration jobs.
- **Avoid EnsureCreated():** `EnsureCreated()` is forbidden outside of throwaway/local experiments because it bypasses migrations and can cause drift; do not use it in integration, CI, or production code paths.
- **PostgreSQL compatibility:** All migrations and SQL generated by the project must be compatible with PostgreSQL (the targeted production DB).

## Integration Testing Constraints

- **Use PostgreSQL for tests:** Integration tests that validate persistence must run against PostgreSQL (for example via Docker/Testcontainers). Testcontainers or an equivalent is the preferred approach.
- **SQLite is not sufficient:** SQLite must not be used for persistence validation tests because its SQL dialect and behavior differ from PostgreSQL.
- **Testable rules:** Persistence rules and mappings must be verifiable by automated integration tests (schema, mapping, conversions, owned types, collection behavior, and value comparers).

## Mapping and Reliability Notes

- **Converters & comparers:** Any non-primitive mapping (typed IDs, lists, enums stored as text/integers) must provide both a `ValueConverter` and a `ValueComparer` when necessary so EF change tracking works reliably.
- **readonly record struct behavior:** Value objects implemented as `readonly record struct` are copied by value, not by reference. EF Core tracks changes by comparing the entire struct value; this prevents false "dirty" states when two fields hold identical value objects and one is modified.
- **No shadow FKs for domain logic:** Avoid relying on shadow foreign keys for domain logic; foreign key values used by the domain should be explicit, typed properties.
- **Explicit indexes & constraints:** Indexes, uniqueness constraints, and FK constraints required by the domain must be declared in migrations and mapping code — do not rely solely on ad-hoc SQL.

## Enforcement and Review

- **Code review checks:** Follow these rules in all pull requests that introduce or modify persistence mappings. Mapping changes must include tests demonstrating correctness (materialization, save/load, change detection).

---

If anything here is unclear or needs a concrete mapping example for a specific domain type, request a minimal focused example and it will be added as an appendix rather than general guidance.
