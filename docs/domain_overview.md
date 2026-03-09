# Domain Overview

This document is the source of truth for core domain concepts and models used in Nimbo WMS. It lists stable, project-specific decisions and constraints.

## 1. Scope of the domain

- Covers warehouse operational concerns: inbound receipts, putaway, picking, shipments, relocations, cycle counts, adjustments, and stock reconciliation.
- Includes master data required to operate the warehouse: items, locations, customers, and organizational identifiers.
- Excludes external transport execution and ERP master-data systems; these are integrated via adapters in Infrastructure.

## 2. Types of domain structures used

- Aggregates / Aggregate Roots: transactional consistency boundaries (e.g., `CycleCountDocument`, `ShipmentDocument`, `ReceivingDocument`).
- Entities: identity-bearing objects within aggregates (e.g., line items, inventory items).
- Value Objects: immutable descriptors and measurements (dimensions, quantities, addresses).
- Domain Services: stateless domain logic that does not belong to a single aggregate.
- State Machines: where workflows require explicit state transitions (e.g., inbound/receiving state machine implementations).

## 3. Domain modeling approach

- Typed IDs: every aggregate/entity uses a strongly typed identifier (e.g., `CycleCountDocumentId`, `LocationId`) wrapping `Guid`.
- Immutability & construction: value objects are immutable; entities expose behavior methods. Public constructors enforce invariants. Private parameterless constructors exist solely for ORM materialization.
- Persistence ignorance: domain code contains no EF Core types, attributes, or mapping concerns.
- Encapsulation: collections are mutated via explicit aggregate methods; aggregates expose read-only collections (`IReadOnlyCollection<T>`).

## 4. Bounded contexts

- Inventory/Stock: inventory items, quantities, stock movements, reservations.
- Documents (operational): `ReceivingDocument`, `ShipmentDocument`, `CycleCountDocument`, `RelocationDocument`, `AdjustmentDocument` — they encapsulate transactional workflows.
- Master Data: `Item`, `Location`, `Customer` — reference data used by documents and inventory.
- Integration/Adapters: external system adapters and anti-corruption layers live in Infrastructure and map to domain models.

## 5. Documents vs Master Data vs Stock

- Documents: transactional records that represent work or processes (e.g., shipments, counts). Documents are aggregates, own their lines, and manage lifecycle/state.
- Master Data: stable reference entities (items, locations). They are modeled as aggregates when they carry business logic; otherwise read-only reference data in application flows.
- Stock: representations of physical quantity and location (`InventoryItem`, aggregated stock views). Stock models capture available/allocated quantities and are authoritative for local warehouse state.

## 6. Core domain entities / aggregates and responsibilities

- `CycleCountDocument` (aggregate root): represents a counted inventory event. Responsible for lines, reconciliation results, and count status transitions.
- `ShipmentDocument` (aggregate root): represents outbound work. Responsible for allocation, pick lists, and shipment status.
- `ReceivingDocument` (aggregate root): represents inbound receipt work. Responsible for receiving lines, putaway decisions, and receipt status.
- `RelocationDocument` (aggregate root): models inter-location transfer orders and their state transitions.
- `AdjustmentDocument` (aggregate root): models inventory adjustments and discrepancies with reason tracking.
- `Item` (master aggregate): product definition, unit-of-measure, identification rules, and any item-level constraints.
- `Location` (master aggregate): storage location definition, type, capacity constraints, and permitted item attributes.
- `InventoryItem` / Stock entity: physical stock per item+location, quantity bookkeeping, and reservation/commit operations.

## 7. Core invariants

- `CycleCountDocument`:
  - Each count line references the parent `CycleCountDocumentId`, an item, and a location.
  - All lines must have actual quantity recorded before completion or posting.
  - Reconciliation must not produce negative stock; adjustments are validated before commit.

- `ShipmentDocument`:
  - Request lines define planned quantities; pick lines track actual picks from locations.
  - Total picked quantity per item cannot exceed requested quantity.
  - Status transitions follow the defined state machine (e.g., Draft → InProgress → Posted).

- `ReceivingDocument`:
  - Received quantity per line must be positive; expected quantity (if provided) must be non-negative.
  - Receiving completes only when all lines are valid (document-level validations pass).

- `RelocationDocument`:
  - Source and destination locations must be different.
  - Duplicate lines (same item and same from/to pair) are not allowed.
  - Lines must have at least one relocation; document cannot be empty when posted.

- `AdjustmentDocument`:
  - Each line adjusts inventory by a non-zero delta (positive or negative) at a specific location.
  - No duplicate adjustments for the same item and location in draft state.
  - Reason code is mandatory; reason text is optional.

- `Item` (master):
  - Item must have a valid identification (SKU/ID) and a defined base unit of measure.
  - If dimensions or special handling flags exist, they must be set at creation and validated.

- `Location` (master):
  - Location type and capacity constraints must be defined; moves/reservations must respect capacity rules.
  - Locations may be enabled/disabled; disabled locations cannot receive new allocations without explicit override.

## 8. Physical Facts & The Stock Ledger

**Operational Intent vs. Physical Reality:**
- Documents represent *operational intent*: what the business plans to do (receive stock, ship orders, relocate items, count inventory, adjust discrepancies).
- The Stock Ledger represents *physical reality*: the immutable historical record of what actually happened to inventory.

**Stock Ledger (Immutable Fact Aggregate):**
The `StockLedgerEntry` aggregate records every physical movement or adjustment:
- Each entry is immutable once posted; entries are never updated or deleted.
- Entries carry audit metadata: document ID, timestamp, warehouse, operator.
- `BalanceAfter` field records the running balance at the moment the entry was posted.
- Entries are the source of truth for historical stock positions and account reconciliation.

**Double Entry Rule (Internal Movements):**
- Every internal relocation (`RelocationDocument` posting) generates two ledger entries:
  - A `TransferOut` entry (negative delta) at the source location.
  - A `TransferIn` entry (positive delta) at the destination location.
- This double-entry principle ensures that total quantity is conserved and audit trails are complete.

**Lifecycle Contrast:**
- Documents transition: Draft → InProgress → Completed → Posted.
- Ledger entries are created only when the document reaches "Posted" state.
- Once posted, a document is sealed; subsequent ledger entries cannot be reversed (though a new corrective document may be created).

---

If a contributor needs a concrete modeling example for a specific aggregate (mapping, DTO boundary, or sample invariants), request one and a focused minimal example will be provided separately.
