# Domain Overview

This document is the source of truth for core domain concepts and models used in Nimbo WMS. It lists stable, project-specific decisions and constraints.

## 1. Scope of the domain

- Covers warehouse operational concerns: inbound receipts, putaway, picking, shipment orders, internal transfers, inventory counts, and stock reconciliation.
- Includes master data required to operate the warehouse: items, locations, customers, and organizational identifiers.
- Excludes external transport execution and ERP master-data systems; these are integrated via adapters in Infrastructure.

## 2. Types of domain structures used

- Aggregates / Aggregate Roots: transactional consistency boundaries (e.g., `InventoryCount`, `ShipmentOrder`, `InboundDelivery`).
- Entities: identity-bearing objects within aggregates (e.g., line items, inventory items).
- Value Objects: immutable descriptors and measurements (dimensions, quantities, addresses).
- Domain Services: stateless domain logic that does not belong to a single aggregate.
- State Machines: where workflows require explicit state transitions (e.g., inbound/receiving state machine implementations).

## 3. Domain modeling approach

- Typed IDs: every aggregate/entity uses a strongly typed identifier (e.g., `InventoryCountId`, `LocationId`) wrapping `Guid`.
- Immutability & construction: value objects are immutable; entities expose behavior methods. Public constructors enforce invariants. Private parameterless constructors exist solely for ORM materialization.
- Persistence ignorance: domain code contains no EF Core types, attributes, or mapping concerns.
- Encapsulation: collections are mutated via explicit aggregate methods; aggregates expose read-only collections (`IReadOnlyCollection<T>`).

## 4. Bounded contexts

- Inventory/Stock: inventory items, quantities, stock movements, reservations.
- Documents (operational): `InboundDelivery`, `ShipmentOrder`, `InventoryCount`, `InternalTransfer` — they encapsulate transactional workflows.
- Master Data: `Item`, `Location`, `Customer` — reference data used by documents and inventory.
- Integration/Adapters: external system adapters and anti-corruption layers live in Infrastructure and map to domain models.

## 5. Documents vs Master Data vs Stock

- Documents: transactional records that represent work or processes (e.g., shipments, counts). Documents are aggregates, own their lines, and manage lifecycle/state.
- Master Data: stable reference entities (items, locations). They are modeled as aggregates when they carry business logic; otherwise read-only reference data in application flows.
- Stock: representations of physical quantity and location (`InventoryItem`, aggregated stock views). Stock models capture available/allocated quantities and are authoritative for local warehouse state.

## 6. Core domain entities / aggregates and responsibilities

- `InventoryCount` (aggregate root): represents a counted inventory event. Responsible for lines, reconciliation results, and count status transitions.
- `ShipmentOrder` (aggregate root): represents outbound work. Responsible for allocation, pick lists, and shipment status.
- `InboundDelivery` (aggregate root): represents inbound receipt work. Responsible for receiving lines, putaway decisions, and receipt status.
- `InternalTransfer` (aggregate root): models inter-location transfer orders and their state transitions.
- `Item` (master aggregate): product definition, unit-of-measure, identification rules, and any item-level constraints.
- `Location` (master aggregate): storage location definition, type, capacity constraints, and permitted item attributes.
- `InventoryItem` / Stock entity: physical stock per item+location, quantity bookkeeping, and reservation/commit operations.

## 7. Core invariants

- `InventoryCount`:
  - Each count line references the parent `InventoryCountId` and an `InventoryItem` or `Location`-scoped identifier.
  - Reconciliation must not produce negative stock; adjustments are validated before commit.

- `ShipmentOrder`:
  - Allocation cannot exceed available (unallocated) stock for the same location/grouping rules.
  - Status transitions must follow the defined state machine (e.g., Created → Allocated → Picked → Shipped).

- `InboundDelivery`:
  - Received quantity per line must be non-negative and cannot exceed planned quantity unless an overreceive rule is explicitly applied.
  - Receiving completes only when mandatory checks (document-level validations) pass.

- `InternalTransfer`:
  - Source location must have sufficient available stock at transfer commit time.
  - Transfer lines are atomic within the aggregate; partial commits are not allowed without explicit domain action.

- `Item` (master):
  - Item must have a valid identification (SKU/ID) and a defined base unit of measure.
  - If dimensions or special handling flags exist, they must be set at creation and validated.

- `Location` (master):
  - Location type and capacity constraints must be defined; moves/reservations must respect capacity rules.
  - Locations may be enabled/disabled; disabled locations cannot receive new allocations without explicit override.

---

If a contributor needs a concrete modeling example for a specific aggregate (mapping, DTO boundary, or sample invariants), request one and a focused minimal example will be provided separately.
