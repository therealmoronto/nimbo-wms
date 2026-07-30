# Master Data Module — Architectural Decision Record

**Status:** Accepted

**Scope:** Master Data bounded context (Item, Location, Customer)

## Module Overview

- Purpose: Define and manage stable reference entities required to operate the warehouse.
- Why Master Data: Operational documents and inventory physically reside at or reference these core entities; they must exist prior to transactional operations.
- Responsibility: Lifecycle management of products, storage locations, and organizational identifiers.

## Aggregate Structure

**Aggregate roots:** `Item`, `Location`, `Customer`

- Each master data entity is a distinct aggregate root.
- They do not own deeply nested collections of sub-entities for transactional purposes; they are mostly standalone.

Decisions:
- Each type is a separate aggregate to allow independent lifecycle management and simple CRUD.
- Master data entities carry business logic and invariants when they model constraints (e.g., location capacity, item dimensions).

Trade-offs:
- Pro: Simple to retrieve, reference, and cache.
- Con: Cross-aggregate consistency (e.g., an Item referencing a specific restricted Location type) must be enforced in the Application layer or domain services.

## API Lifecycle

Routing is flat for usability; aggregate boundaries are enforced in handlers and domain logic, not by URL nesting.

Expected high-level flow for consumer interactions:

1. Create Master Data (POST `/api/masterdata/items`, `/api/masterdata/locations`)
2. Patch Master Data (PATCH `/api/masterdata/items/{itemId}`, `/api/masterdata/locations/{locationId}`)
3. Delete operations:
   - DELETE `/api/masterdata/items/{itemId}`
   - DELETE `/api/masterdata/locations/{locationId}`

Sub-resource routing decisions:
- Master data generally lacks sub-resources.
- Controllers are thin: they translate HTTP => Command/Query messages and forward to handlers.

## Update Strategy

Decisions:
- Use `PATCH` for partial updates to master data entities.
- Use typed patch DTOs rather than JSON Patch.
- Represent partial updates with nullable properties on patch DTOs.
- Implement updates as Commands handled in the Application layer; commands call domain methods which enforce invariants.
- Domain entities have no public setters; state changes occur only through explicit domain methods.

Rationale and trade-offs:
- Typed patch DTOs ensure strong typing and explicit accepted fields.
- Commands keep write paths explicit, audit-friendly, and testable.
- No public setters ensure invariants are strictly enforced via domain methods.

## Domain Invariants (enforced inside Aggregates)

- `Item` must have a valid identification (SKU) and a defined base unit of measure.
- `Location` type and capacity constraints must be defined.
- Identifiers are strongly typed and unique.
- Master data cannot be deleted if referenced by active documents or inventory (enforced by Application or DB constraints).

Enforcement strategy:
- Command handlers call domain methods on the aggregate and persist the aggregate via repositories and Unit of Work.
- Handlers validate command-level invariants before invoking domain methods.

## Architectural Boundaries

- Domain (`Nimbo.Wms.Domain`)
  - Contains aggregates, value objects, and domain logic.
  - No dependency on EF Core, Application, or Infrastructure.

- Application (`Nimbo.Wms.Application`)
  - Defines Commands, Queries, and handler abstractions for Master Data use cases.

- Infrastructure (`Nimbo.Wms.Infrastructure`)
  - Contains EF Core configurations, repository implementations, and query handlers.
  - Implements read-side handlers projecting to DTOs in `Nimbo.Wms.Contracts`.

- API (`Nimbo.Wms` project)
  - Thin controllers map HTTP requests to Commands/Queries and return DTOs.

## Explicit Non-Goals

These are intentional decisions for the current implementation:

- No soft delete: deletes remove identity from the active dataset immediately.
- No concurrency tokens or optimistic concurrency control are implemented.
- No domain event publishing for master data changes.

Trade-offs of non-goals:
- Simplicity vs weaker resilience to concurrent edits and harder recovery from accidental deletes.

## Maintenance Notes

- If soft-delete is required later (to preserve history for old documents), model and surface it explicitly in the domain.
- Consider aggressive caching in the read-side for Master Data if query volume increases.

---

Document author: Architecture team
Date: 2026-05-25
