# Topology Module — Architectural Decision Record

**Status:** Accepted

**Scope:** Topology bounded context (Warehouse, Zones, Locations)

## Module Overview

- Purpose: Manage physical layout of warehouses: creation, structure (zones, locations), and structural updates required to place inventory later.
- Why before Inventory: Inventory requires stable, authoritative physical locations; topology defines those locations and their identity first.
- Responsibility: CRUD for warehouses, zones, and locations; enforce structural invariants; provide read models for UI and downstream services.

## Aggregate Structure

**Aggregate root:** Warehouse

- Zones and Locations are stored as collections inside Warehouse;
- Location references its parent Zone via ZoneId rather than nested collections.

Decisions:
- Warehouse is the only aggregate root to preserve transactional integrity across its zones and locations within a single write operation.
- Zones and Locations are modeled as sub-entities (not standalone aggregates) because:
  - Their lifecycles are scoped to a Warehouse.
  - Invariants span parent and child (e.g., zone deletion depends on contained locations).
  - Most operations require transactional consistency across the aggregate.
- All mutations to Zones or Locations must be executed via Warehouse methods (e.g., `warehouse.AddZone(...)`, `warehouse.AddLocation(...)`).

Trade-offs:
- Pro: Strong consistency for structural changes and simpler invariants enforcement.
- Con: Larger aggregate size for heavy-write scenarios; may require refactoring to separate aggregates if scale demands.

## API Lifecycle

Routing is flat for usability; aggregate boundaries are enforced in handlers and domain logic, not by URL nesting.

Expected high-level flow for consumer interactions:

1. Create Warehouse (POST `/api/topology/warehouses`)
2. Add Zones (POST `/api/topology/warehouses/{warehouseId}/zones`)
3. Add Locations (POST `/api/topology/warehouses/{warehouseId}/locations`)
4. Patch Zone/Location (PATCH `/api/topology/zones/{zoneId}` or `/api/topology/locations/{locationId}`)
5. Patch Warehouse (PATCH `/api/topology/warehouses/{warehouseId}`)
6. Delete operations:
   - DELETE `/api/topology/warehouses/{warehouseId}`
   - DELETE `/api/topology/zones/{zoneId}`
   - DELETE `/api/topology/locations/{locationId}`

Sub-resource routing decisions:
- Routes reflect the aggregate hierarchy to make intent explicit and to emphasize that mutations are scoped to the Warehouse aggregate.
- Examples:
  - `/api/topology/warehouses/{wId}/zones` — create zone for a specific warehouse.
  - `/api/topology/warehouses/{wId}/locations` — create location within a specific warehouse.
- Controllers are thin: they translate HTTP => Command/Query messages and forward to handlers.

## Update Strategy

Decisions:
- Use `PATCH` for partial updates to Warehouse, Zone, and Location resources.
- Use typed patch DTOs (feature-specific records) rather than JSON Patch.
- Represent partial updates with nullable properties on patch DTOs.
- Implement updates as Commands handled in the Application layer; commands call domain methods which enforce invariants.
- Domain entities have no public setters; state changes occur only through explicit domain methods.

Rationale and trade-offs:
- Typed patch DTOs:
  - Pro: Strong typing, compile-time validation, explicit accepted fields, easier tests.
  - Con: More DTO types and mapping code compared to a generic JSON Patch approach.
- Nullable properties as partials:
  - Pro: Clear intent—unset / null = do not change; explicit values replace existing state.
  - Con: Requires explicit handling of null vs value in handlers.
- Commands for updates:
  - Pro: Keeps write paths explicit, audit-friendly, and testable; centralizes authorization/validation in handlers.
  - Con: Slightly more boilerplate than direct service methods.
- No public setters:
  - Pro: Invariants are enforced only via domain methods.
  - Con: Mapping from DTOs requires translators that call domain methods rather than assign properties directly.

## Domain Invariants (enforced inside `Warehouse`)

- Zone cannot be deleted if it contains any Locations.
- Warehouse cannot be deleted if it contains any Zones or Locations (i.e., not empty).
- Location must always belong to a Zone; creation of a Location requires a parent Zone reference.
- Location and Zone identifiers are strongly typed and unique within their containing Warehouse.
- All mutations that affect structure or referential integrity must be handled by Warehouse methods.

Enforcement strategy:
- Command handlers call domain methods on the aggregate and persist the aggregate via repositories and Unit of Work.
- Handlers validate command-level invariants before invoking domain methods when appropriate (e.g., existence checks).

## Architectural Boundaries

- Domain (`Nimbo.Wms.Domain`)
  - Contains aggregates, sub-entities, value objects, and domain logic.
  - No dependency on EF Core, Application, or Infrastructure.

- Application (`Nimbo.Wms.Application`)
  - Defines Commands, Queries, and handler abstractions for Topology use cases.
  - Command handlers orchestrate domain method calls and use repository interfaces and Unit of Work.

- Infrastructure (`Nimbo.Wms.Infrastructure`)
  - Contains `NimboWmsDbContext`, EF Core configurations, repository implementations, and query handlers.
  - Implements read-side handlers projecting to DTOs in `Nimbo.Wms.Contracts`.

- API (`Nimbo.Wms` project)
  - Thin controllers map HTTP requests to Commands/Queries and return DTOs.

Notes:
- No EF Core types or `IQueryable` are exposed outside Infrastructure.
- DTOs used in API responses live in `Nimbo.Wms.Contracts` and are stable read models separate from domain entities.

## Explicit Non-Goals

These are intentional decisions for the current implementation:

- No soft delete: deletes remove identity from active dataset immediately.
- No cascade delete: deletes are guarded by domain invariants; removals are explicit operations.
- No concurrency tokens or optimistic concurrency control are implemented.
- No authorization logic in the domain or handlers (handled externally or added later as a cross-cutting concern).
- No domain event publishing: state changes are applied synchronously and not emitted as domain events.

Trade-offs of non-goals:
- Simplicity and predictable behavior vs weaker resilience to concurrent edits and harder recovery from accidental deletes.
- Avoids complexity now; these capabilities can be introduced later if the operational model requires them.

## Maintenance Notes

- If write scale requires it, consider splitting Zones or Locations into separate aggregates and introducing eventual consistency for read models.
- If concurrent edits become frequent, add concurrency tokens and handle `DbUpdateConcurrencyException` in command handlers.
- If soft-delete or event publishing is required later, model and surface them explicitly in the domain and update Contracts accordingly.

---

Document author: Architecture team
Date: 2026-02-16
