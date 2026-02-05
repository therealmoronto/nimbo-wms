# Architecture

This document is the architectural contract for contributors. It lists stable, project-specific decisions and constraints for Nimbo WMS backend.

## 1. Overview

- What it is: a backend service implementing Warehouse Management System (WMS) domain logic. Modular and extendable by feature and bounded context.
- Architectural goals: correctness, explicit domain modeling, maintainability, and testability. Prioritize explicitness over framework magic.

## 2. Architectural Style

- DDD-first: a rich domain model with aggregates that enforce invariants and encapsulate state and behavior.
- Clean Architecture boundaries: Domain → Application → Infrastructure → API. Each layer has distinct responsibilities and artifacts.
- Dependency rule: outer layers may depend on inner layers only. Domain has no dependencies on Application, Infrastructure, or API.

## 3. Solution Structure and Responsibilities

- Domain (Nimbo.Wms.Domain): entities, value objects, typed IDs, domain rules, state machines. No external infra types allowed.
- Application (Nimbo.Wms.Application): use cases (commands/queries), orchestration, transaction boundaries, interfaces/ports (e.g., repository interfaces). No EF Core types here.
- Infrastructure (Nimbo.Wms.Infrastructure): EF Core implementations, repository wiring, persistence mappings, external service clients, and any adapter code that implements Application ports.
- API (Nimbo.Wms/Controllers): HTTP controllers, DTOs/contracts, composition root (Program.cs / startup), request/response mapping, authentication/authorization composition.

## 4. Domain Modeling Conventions

- Aggregates: model transactional boundaries explicitly. Documents (InventoryCount, ShipmentOrder, etc.) are aggregates; lines belong to their parent aggregate.
- Invariants: enforce in constructors and domain methods; state is valid after construction and after each operation.
- No EF attributes or references in domain code. Domain is persistence-ignorant.
- Mutability: avoid public setters. Use methods to change state. Provide private parameterless constructors for EF materialization only.

## 5. Identifiers and Type Safety

- Strongly typed IDs: domain identifiers wrap `Guid` in dedicated types (e.g., `InventoryCountId`, `LocationId`) to prevent accidental mixing.
- Raw `Guid` usage: permitted only at external boundaries (DTOs, external APIs) or in rare temporary internal cases documented in code. Prefer typed IDs everywhere else.
- Conversions: conversions between typed IDs and primitives are implemented in the infrastructure mapping layer (ValueConverters). Do not expose primitives in domain APIs.

## 6. Persistence Approach

- EF Core: used strictly as a persistence mechanism. Domain does not depend on EF Core.
- Explicit mapping: all mappings live in `IEntityTypeConfiguration<T>` implementations in Infrastructure. Do not rely on implicit conventions for critical mappings.
- Value objects: mapped via `OwnsOne` / owned types.
- Collections: mapped via private backing fields and exposed as `IReadOnlyCollection<T>` in domain models. Configure backing fields explicitly (`HasField`, `UsePropertyAccessMode`).
- Postgres specifics: production target is PostgreSQL — use `uuid` for identifiers, `uuid[]` for lists-of-ids where appropriate, `jsonb` for denormalized structured values only when justified, and explicit numeric precision for decimals.

## 7. Use Cases and Boundaries

- Application layer implements use cases (commands/queries) and coordinates transactions. Application orchestrates domain operations via domain models and repositories.
- Domain has no knowledge of controllers, DTOs, or EF; it exposes behavior and invariants only.
- Validation split: domain invariants are domain responsibilities; input validation (format, required request fields) belongs to API/Application.

## 8. Testing Strategy (high-level)

- Unit tests: test domain and application logic in memory without DB dependencies.
- Integration tests: validate persistence and mappings against PostgreSQL (Docker/Testcontainers). Do not rely on SQLite for persistence validation.
- Migrations: test migrations by applying `Database.Migrate()` in integration tests (smoke tests) to ensure schema compatibility.

## 9. Cross-cutting Concerns (current + future)

- Logging & observability: centralized via Infrastructure adapters; controllers and application code emit structured events.
- Health checks: lightweight health endpoints in API layer; infrastructure readiness checks for DB and external services.
- Transactions & concurrency: optimistic concurrency is the planned approach; domain models may include versioning when required.
- Background processing/events: background workers and eventual consistency are planned; consider an outbox pattern when integrating with external systems.

## 10. Decision Log (non-negotiable)

- DDD rich domain with typed IDs is mandatory.
- EF Core used only as persistence; mappings are explicit via `IEntityTypeConfiguration<T>`.
- PostgreSQL is the production database and the target for integration tests.
- Lazy loading is forbidden; loading is always explicit.

---

## Glossary

- Aggregate: a consistency boundary that groups related entities and value objects; has a single aggregate root.
- Aggregate root: the entity through which the aggregate is accessed and mutated.
- Value Object: immutable type that represents a value (no identity) and is compared by value.
- Typed ID: a domain-specific identifier type that wraps a `Guid` to provide type safety.
- OwnsOne: EF Core owned-entity mapping used to persist value objects.
- Backing field: a private field used to store collection state; EF maps against that field rather than public mutable properties.
- Outbox: a pattern for reliably publishing integration events as part of the same transactional boundary as database changes.
- ValueConverter / ValueComparer: EF Core primitives used in Infrastructure to map typed IDs and value-lists to DB primitives and to enable correct change detection.
