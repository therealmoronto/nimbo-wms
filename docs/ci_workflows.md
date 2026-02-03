# CI Workflows

This document is the source of truth for CI workflows used in this repository. It lists stable, project-specific jobs, their intent, and which jobs run on pull requests vs the `main` branch.

## 1. Build workflow

- Purpose: restore dependencies and compile the entire solution to fail fast on compilation or SDK issues.
- Steps performed in CI:
  - `dotnet restore` for `NimboWMS.sln`.
  - `dotnet build --no-restore` for the solution.
  - (Main branch) publish artifacts as a build output if a release pipeline depends on them.
- Required runner capability: .NET SDK matching repository SDK and ability to run `dotnet` commands.

## 2. Test workflow

- Purpose: validate correctness of domain/application logic and persistence mappings.
- Test job composition:
  - Unit tests: run with `dotnet test` against test projects; these do not require Docker.
  - Integration tests: run with `dotnet test` against integration projects and require Docker (Testcontainers). Integration tests:
    - Start ephemeral PostgreSQL via Testcontainers or CI Docker service.
    - Apply migrations via `Database.Migrate()` or `dotnet ef database update` as part of test setup.
    - Run repository/mapping/migration smoke tests to validate schema and converters.
  - Migration smoke test: explicitly run `Database.Migrate()` (or the migration update command) to ensure generated migrations apply cleanly against PostgreSQL.
- Order and orchestration: unit tests run first; integration tests run after successful unit tests. Both must pass for the job to succeed.
- Required runner capability: Docker (daemon) available for integration tests.

## 3. Required checks and branch rules

- Pull requests (targeting any long-lived branch including `main`):
  - CI build job (restore + build) must pass.
  - Unit tests must pass.
  - Integration tests must run and pass (CI must provide Docker). Integration tests include migration smoke tests.
  - If integration tests are marked slow, the PR pipeline still runs them; flaky or slow tests must be fixed or the test marked/removed.
- `main` branch (merge / push to `main`):
  - All PR checks must be green before merge (build, unit, integration, migration checks).
  - After merge, a main-branch pipeline runs: full build, integration test-suite, and optional artifact publish or release job.
- Failure policy:
  - Any failing required check blocks merging to `main`.
  - Flaky tests are not permitted; failing flaky tests must be fixed and cannot be used to bypass CI requirements.
- Runner and environment constraints:
  - CI runners executing integration tests must have Docker available and sufficient resources for PostgreSQL containers.
  - Migrations must be validated against PostgreSQL in CI; using alternate DB engines in CI is forbidden for persistence validation.

---

This file documents the current, enforced CI contract. If CI runner capabilities change (for example Docker availability), update this file and the CI configuration together.
