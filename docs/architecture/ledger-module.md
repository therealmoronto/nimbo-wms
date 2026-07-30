# Ledger Module — Architectural Decision Record

**Status:** Accepted

**Scope:** Ledger bounded context (StockLedgerEntry)

## Module Overview

- Purpose: Provide an immutable historical record of every physical movement or adjustment of inventory.
- Why Ledger: The Stock Ledger represents physical reality and is the source of truth for historical stock positions and account reconciliation.
- Responsibility: Capture operational events from documents as immutable facts with deltas and running balances.

## Aggregate Structure

**Aggregate root:** `StockLedgerEntry`

- Each entry is an immutable fact aggregate.
- Entries carry audit metadata: document ID, timestamp, warehouse, operator, and the delta applied.

Decisions:
- Ledger entries are standalone aggregates to ensure they are never modified or deleted once posted.
- They include a `BalanceAfter` field to record the running balance at the moment the entry was posted.
- Internal movements follow the Double Entry Rule: generating two entries (e.g., a `TransferOut` and a `TransferIn`).

Trade-offs:
- Pro: Complete, immutable audit trail for all stock movements.
- Con: High volume of ledger entries requires efficient indexing and potential archiving strategies over time.

## API Lifecycle

Routing is flat for usability; aggregate boundaries are enforced in handlers and domain logic, not by URL nesting.

Expected high-level flow for consumer interactions:

1. Query Ledger (GET `/api/ledger/entries`)
2. Ledger entries are created internally by Document Posting Services (e.g., when an Adjustment or Relocation is posted).
3. No external mutation APIs are provided.

Sub-resource routing decisions:
- The API is strictly read-only for external consumers.
- Controllers translate HTTP => Query messages and forward to handlers.

## Update Strategy

Decisions:
- Ledger entries are append-only.
- There are no `PATCH`, `PUT`, or `DELETE` operations for ledger entries.
- Created exclusively through internal Application-layer posting services during a document post transaction.

Rationale and trade-offs:
- Append-only design guarantees the integrity of the audit trail.
- Corrections must be made via new documents (e.g., a new AdjustmentDocument) rather than modifying past ledger entries.

## Domain Invariants (enforced inside StockLedgerEntry)

- Once created, a `StockLedgerEntry` is immutable.
- Must reference the source document, Item, and Location.
- Must accurately record the delta and the `BalanceAfter`.
- Double-entry movements (like relocations) must balance exactly across the warehouse.

Enforcement strategy:
- Posting Services construct the entries and persist them via repositories and Unit of Work alongside the document state change.
- Invariants are enforced during construction.

## Architectural Boundaries

- Domain (`Nimbo.Wms.Domain`)
  - Contains the `StockLedgerEntry` aggregate and construction rules.
  - No dependency on EF Core, Application, or Infrastructure.

- Application (`Nimbo.Wms.Application`)
  - Posting Services instantiate ledger entries and use repositories to append them.

- Infrastructure (`Nimbo.Wms.Infrastructure`)
  - Contains EF Core configurations and query handlers for historical read models.

- API (`Nimbo.Wms` project)
  - Thin controllers map HTTP requests to Queries and return DTOs.

## Explicit Non-Goals

These are intentional decisions for the current implementation:

- No external APIs to create, update, or delete ledger entries.
- No event sourcing based on the ledger (the ledger is an audit log, but not the primary event store for rebuilding aggregate state).

Trade-offs of non-goals:
- Simplicity and strict enforcement of the append-only rule.

## Maintenance Notes

- Due to the append-only nature, the `StockLedgerEntry` table will grow indefinitely. Implement partitioning or archiving strategies in Infrastructure if table size impacts performance.

---

Document author: Architecture team
Date: 2026-05-25
