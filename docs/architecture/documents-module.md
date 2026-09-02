# Documents Module — Architectural Decision Record

**Status:** Accepted

**Scope:** Documents bounded context (Receiving, Shipment, Relocation, CycleCount, Adjustment)

## Module Overview

- Purpose: Manage operational workflows and document states representing warehouse activities.
- Why Documents: They encapsulate transactional workflows, ensuring that physical movements of stock are driven by validated, authorized intent.
- Responsibility: Lifecycle management, validation of document lines, and enforcement of operational invariants prior to physical stock movement.

## Aggregate Structure

**Aggregate roots:** `ReceivingDocument`, `ShipmentDocument`, `CycleCountDocument`, `RelocationDocument`, `AdjustmentDocument`

- Document lines (e.g., `ReceivingDocumentLine`, `ShipmentDocumentLine`) are stored as collections inside their parent document.
- Lines reference items and locations but do not exist outside the document aggregate.

Decisions:
- Each document type is a separate aggregate root to preserve transactional integrity for its specific operational workflow.
- Document lines are modeled as sub-entities because:
  - Their lifecycles are scoped strictly to the parent document.
  - Invariants span parent and child (e.g., document cannot be posted if lines are invalid).
- All mutations to lines must be executed via document methods.

Trade-offs:
- Pro: Strong consistency for operational workflows and simpler invariants enforcement.
- Con: Large documents (many lines) may result in large aggregates, potentially impacting performance during concurrent updates.

## Stock Lot References on Document Lines

`ReceivingDocumentLine` carries scalar `BatchNumber`/`ExpiryDate` (entered by the receiving clerk) — these are
*input* to lot resolution at posting time, not a reference to an existing lot. `ReceivingDocumentPostingService`
resolves them into a `VendorLot` (find-or-create, only when `Item.IsBatchManaged`) and always mints a fresh
`StockLot` per line (see `docs/architecture/stock-module.md`, "Lot Genealogy"). No other document type creates
lots — they only reference `StockLotId`s that already exist.

The other four document types carry a `StockLotId` reference with an intentional nullability split:
- **Nullable** on `RelocationDocumentLine`, `CycleCountDocumentLine`, `AdjustmentDocumentLine`, and the "requested"
  `ShipmentDocumentLine` — a caller may omit it when only one stock lot exists at the relevant item/location;
  `IInventoryItemRepository.GetByCriteriaAsync` resolves that single-lot case unambiguously and throws a
  `DomainException` if more than one lot exists and none was specified. CycleCount/Adjustment posting additionally
  requires a `StockLotId` to materialize brand-new surplus stock (no prior `InventoryItem` row) — a `StockLot` can
  only originate from a `ReceivingDocument`, so posting has no lot to attribute otherwise.
- **Mandatory** on `ShipmentPickLine` — once more than one lot can coexist at an item/location (the entire point of
  FIFO/FEFO), an unspecified pick lot would be ambiguous, so the type system forces the caller to have already
  picked one (typically via `GetAvailableStockLotsQuery`, see `docs/architecture/stock-module.md`).

## API Lifecycle

Routing is flat for usability; aggregate boundaries are enforced in handlers and domain logic, not by URL nesting.

Expected high-level flow for consumer interactions:

1. Create Document (POST `/api/documents/{type}`)
2. Add Lines (POST `/api/documents/{type}/{documentId}/lines`)
3. Patch Line (PATCH `/api/documents/{type}/{documentId}/lines/{lineId}`)
4. Patch Document (PATCH `/api/documents/{type}/{documentId}`)
5. Transition State (POST `/api/documents/{type}/{documentId}/complete`, POST `/api/documents/{type}/{documentId}/post`)
6. Delete operations:
   - DELETE `/api/documents/{type}/{documentId}` (only in Draft state)
   - DELETE `/api/documents/{type}/{documentId}/lines/{lineId}`

Sub-resource routing decisions:
- Routes reflect the aggregate hierarchy to make intent explicit and emphasize that mutations are scoped to the Document aggregate.
- Controllers translate HTTP => Command/Query messages and forward to handlers.

## Update Strategy

Decisions:
- Use `PATCH` for partial updates to Documents and Lines.
- Use typed patch DTOs rather than JSON Patch.
- Represent partial updates with nullable properties on patch DTOs.
- Implement updates as Commands handled in the Application layer; commands call domain methods which enforce invariants.
- Domain entities have no public setters.

Rationale and trade-offs:
- Typed patch DTOs ensure strong typing and explicit accepted fields.
- Nullable properties clearly distinguish unset properties from those intended to be changed.
- Commands centralize authorization and keep write paths explicit.
- No public setters ensure invariants are strictly enforced via domain methods.

## Domain Invariants (enforced inside Documents)

- Document lines must reference a valid Item and/or Location depending on the document type.
- Received quantity per line must be positive; adjustments must be non-zero.
- Source and destination locations for relocations must be different.
- Duplicate lines are prevented based on document-specific rules.
- Documents transition through states (e.g., Draft → InProgress → Completed → Posted).
- A document cannot be modified once it is Posted.

Enforcement strategy:
- Command handlers call domain methods on the aggregate and persist the aggregate via repositories and Unit of Work.
- Handlers validate command-level invariants before invoking domain methods.

## Architectural Boundaries

- Domain (`Nimbo.Wms.Domain`)
  - Contains document aggregates, sub-entities, and state transitions.
  - No dependency on EF Core, Application, or Infrastructure.

- Application (`Nimbo.Wms.Application`)
  - Defines Commands, Queries, and handler abstractions for Document use cases.
  - Manages the Unit of Work for transactional posting workflows.

- Infrastructure (`Nimbo.Wms.Infrastructure`)
  - Contains EF Core configurations and query handlers projecting to DTOs in `Nimbo.Wms.Contracts`.
  - Implements posting services that translate posted documents into ledger entries.

- API (`Nimbo.Wms` project)
  - Thin controllers map HTTP requests to Commands/Queries.

## Explicit Non-Goals

These are intentional decisions for the current implementation:

- No soft delete for documents: deletes remove the draft document immediately.
- No concurrent edits resolution: optimistic concurrency control is not implemented.
- No direct event publishing from within domain entities.

Trade-offs of non-goals:
- Simplicity at the cost of weaker resilience to concurrent edits.

## Maintenance Notes

- If concurrent edits become frequent, add concurrency tokens and handle `DbUpdateConcurrencyException`.
- Ensure posting workflows are always verified via automated posting smoke tests.

---

Document author: Architecture team
Date: 2026-05-25
