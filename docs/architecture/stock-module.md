# Stock Module — Architectural Decision Record

**Status:** Accepted

**Scope:** Stock / Inventory bounded context (InventoryItem, VendorLot, StockLot)

## Module Overview

- Purpose: Track physical quantity and location of inventory items, and the lot/lineage that lets stock rotate FIFO/FEFO.
- Why Stock: Stock is the core operational reality of the warehouse, distinct from the operational intent captured by documents.
- Responsibility: Maintain running quantities of items at specific locations, and manage allocations and reservations.

## Aggregate Structure

**Aggregate root:** `InventoryItem`

- Represents a quantum of stock for a specific Item, StockLot, and Location.

Decisions:
- The combination of Item, StockLot, and Location acts as the natural key for an `InventoryItem`. Every receipt
  mints its own `StockLot` (see "Lot Genealogy" below), so two receipts of the same item at the same location are
  two distinct `InventoryItem` rows, not one aggregated balance — this is what makes per-receipt FIFO/FEFO tracking
  possible.
- Mutations to inventory are strictly controlled and typically driven by posting workflows from operational documents.

Trade-offs:
- Pro: Simple and direct tracking of quantities per location.
- Con: Concurrent operations hitting the same Item+StockLot+Location require robust concurrency control to prevent over-allocation or negative stock.

## Lot Genealogy: VendorLot vs StockLot

Two separate concepts replace the earlier single "Batch" model, because a supplier's declared batch identity and
the system's own receiving lineage answer different questions:

- **`VendorLot`** — the supplier's declared batch identity (`ItemId` + `BatchNumber` + `SupplierId` + `ExpiryDate`).
  Optional: only created when `Item.IsBatchManaged == true`. Resolved via find-or-create on the natural key
  (`IVendorLotRepository.FindOrCreateAsync`) during Receiving posting, so repeat receipts of the same physical
  batch share one `VendorLot`.
- **`StockLot`** — the system's own receiving lineage (`ItemId`, `ReceivingDocumentId`, `ReceivedAt`, optional
  `VendorLotId`). Always created, once per receiving line, regardless of `IsBatchManaged` — this is what lets
  *every* item participate in FIFO rotation, not just batch-managed ones. `InventoryItem.StockLotId` is mandatory.

**Ordering rule (FEFO with FIFO fallback):** when picking or listing available stock for an item, order by
`VendorLot.ExpiryDate` ascending when the linked `VendorLot` has one (FEFO — first-expiring, first-out); otherwise
fall back to `StockLot.ReceivedAt` ascending (FIFO). See `GetAvailableStockLotsQueryHandler`. There is no
auto-allocation engine — this ordering only informs a read-side query; callers (e.g. `ShipmentDocument.AddPickLine`)
still pass an explicit `StockLotId`.

## API Lifecycle

Routing is flat for usability; aggregate boundaries are enforced in handlers and domain logic, not by URL nesting.

Expected high-level flow for consumer interactions:

1. Query Stock (GET `/api/stock/inventory`)
2. Stock is typically mutated indirectly via Document posting (e.g., POST `/api/documents/shipment/{id}/post`).
3. Direct mutations to stock (outside of Document workflows) are discouraged but may be supported for low-level corrections if explicitly required by business rules.

Sub-resource routing decisions:
- The read-side for stock is the primary API surface for this context.
- Controllers translate HTTP => Query messages and forward to handlers.

## Update Strategy

Decisions:
- Updates to `InventoryItem` are performed by Application-level posting services, driven by documents (Adjustment, Relocation, Receiving, Shipment).
- Use Commands handled in the Application layer; commands call domain methods which enforce invariants.
- Domain entities have no public setters; state changes occur only through explicit domain methods (e.g., `AddQuantity`, `RemoveQuantity`).

Rationale and trade-offs:
- Tying inventory updates to document posting ensures an immutable audit trail (`StockLedgerEntry`).
- No public setters ensure invariants are strictly enforced via domain methods.

## Domain Invariants (enforced inside InventoryItem)

- Inventory quantity cannot fall below zero (unless specific configurations explicitly allow negative stock, which is an anti-pattern in WMS).
- Reservations cannot exceed available physical quantity.
- Inventory must reference a valid Item and Location.

Enforcement strategy:
- Command handlers (Posting Services) call domain methods on the aggregate and persist the aggregate via repositories and Unit of Work.
- Handlers validate command-level invariants before invoking domain methods.

## Architectural Boundaries

- Domain (`Nimbo.Wms.Domain`)
  - Contains the `InventoryItem` aggregate and its rules.
  - No dependency on EF Core, Application, or Infrastructure.

- Application (`Nimbo.Wms.Application`)
  - Defines the Document Posting Services that coordinate changes to `InventoryItem`.

- Infrastructure (`Nimbo.Wms.Infrastructure`)
  - Contains EF Core configurations, repository implementations, and query handlers projecting to DTOs.

- API (`Nimbo.Wms` project)
  - Thin controllers map HTTP requests to Queries and return DTOs.

## Explicit Non-Goals

These are intentional decisions for the current implementation:

- No direct CRUD API for inventory mutations: all mutations must go through Documents.
- No optimistic concurrency control is implemented yet (though it may be required under high contention).
- No domain event publishing for every stock increment/decrement.

Trade-offs of non-goals:
- Simplicity vs weaker resilience to concurrent updates on hot-spot locations.

## Maintenance Notes

- If concurrent edits become frequent, add concurrency tokens and handle `DbUpdateConcurrencyException` in posting handlers.
- Ensure the read-models for stock are performant, potentially utilizing materialized views if querying becomes slow.

---

Document author: Architecture team
Date: 2026-05-25
