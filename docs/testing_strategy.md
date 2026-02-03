# Testing Strategy

This document is the source of truth for how tests are executed in this repository. It describes the testing scope, types, and constraints as implemented today.

## 1. Scope of testing

- Unit tests: verify domain and application logic in isolation from infrastructure.
- Integration tests: verify persistence mappings, migrations, and repository behavior against PostgreSQL.
- Smoke tests: minimal end-to-end checks that exercise the API surface and persistence wiring in CI.
- Tests do not validate third-party external services; those are mocked or stubbed in tests.

## 2. Types of tests used in the project

- xUnit: the test framework used across unit and integration tests.
- Unit tests: run in-process without Docker or a database dependency.
- Integration tests: run against PostgreSQL using Docker/Testcontainers; exercise EF Core mappings, converters, owned types, and change-tracking behavior.
- Migration checks: integration tests apply migrations via `Database.Migrate()` to validate schema evolution.

## 3. Integration testing approach

- Execution: integration tests create ephemeral PostgreSQL instances via Testcontainers (or equivalent) during the test run.
- Schema provisioning: tests run `Database.Migrate()` at container startup; migrations are the canonical schema source for tests.
- Seeding: tests seed only the minimal data required for the scenario; shared global seed data is avoided.
- Isolation: tests must not rely on shared, long-lived database instances; containers are disposed at test end.
- Determinism: tests avoid time-based or flakey assertions; retry logic is limited and explicitly documented per-test when necessary.

## 4. Database strategy for tests

- Engine: PostgreSQL is the only engine used for persistence validation in integration tests.
- Forbidden: SQLite is not used for validation because its behavior differs from PostgreSQL.
- State reset: each integration test run uses a fresh database state (container or clean database schema) to avoid cross-test interference.
- Schema management: migrations are applied with `Database.Migrate()`; `EnsureCreated()` is not used in integration tests.

## 5. Test execution in CI/CD

- CI environment: CI runs unit and integration tests as part of the pipeline. Docker must be available in CI runners for integration tests.
- Order: unit tests run first; integration tests and migration smoke tests run after unit tests and before packaging/deploy stages.
- Failure handling: integration test failures are treated as blocking; flaky tests must be fixed or disabled with a ticket linked in the PR.

## 6. Local development constraints

- Docker requirement: developers must have Docker available to run integration tests locally (Testcontainers need Docker daemon).
- Fast feedback: unit tests are the primary fast feedback loop; integration tests are heavier and may be run selectively during development.
- Skipping: integration tests can be skipped locally via test filters or categories, but CI must run the full integration suite on every merge/PR.

## 7. Non-goals and forbidden approaches

- Do not use SQLite for persistence validation or to assert EF Core/Postgres behavior.
- Do not use `EnsureCreated()` in tests that aim to validate migrations or production schema compatibility.
- Do not rely on a shared, persistent developer database for CI or integration tests.
- Do not make integration tests depend on external internet services or non-deterministic external APIs; such dependencies must be mocked.

---

If adjustments are required for CI runner capabilities (Docker availability, resource limits), document those as CI configuration changes; testing rules above remain the contract for tests.
