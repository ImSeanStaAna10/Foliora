---
title: Backend Conventions
status: proposed coding guidance
updated: 2026-10-31
tags: [foliora, backend, csharp]
---

# Backend Conventions

## ASP.NET Core and C#

- Organize by business module/use case; controllers or minimal API endpoints are transport adapters.
- Use request/response DTOs; never serialize EF Core entities directly.
- Keep application logic in explicit handlers/services with clear inputs and outputs. Do not introduce a mediator package by default.
- Register dependencies at the composition root; avoid service locator patterns.
- Use typed options for configuration and fail startup on missing required settings.
- Propagate `CancellationToken` through HTTP, database and provider calls.
- Use `async` for I/O; do not wrap synchronous EF calls in `Task.Run`.
- Centralize exception mapping and structured logs; do not leak stack traces or provider messages to clients.

## EF Core and Npgsql

- Configure provider-specific mappings intentionally; verify generated SQL and migrations against PostgreSQL.
- Use migrations as reviewed source artifacts; serialize production migration execution.
- Avoid N+1 queries; project to DTOs and paginate collections.
- Use `AsNoTracking` for read-only queries when appropriate.
- Do not expose `IQueryable` across module/API boundaries.
- Use explicit concurrency tokens/version fields for draft updates.
- Avoid generic repository wrappers that duplicate EF Core without adding a useful boundary.
- Keep module query/write ownership clear; an application service, not a controller, coordinates transactions.

## Validation and authorization

- Validate format/length at request edges; validate business invariants in domain/application logic.
- Authorize resource ownership server-side on every request.
- Normalize and validate slugs, URLs, and asset IDs consistently.
- Treat all portfolio JSON, feedback text, and uploaded documents as untrusted input.

## Errors and observability

- Use stable machine-readable error codes and Problem Details response shape.
- Add correlation/trace IDs and meaningful domain event names.
- Never log passwords, access/refresh tokens, OIDC codes, full uploaded CV content, or unnecessary message text.
- Measure publish failures, editor save conflicts, queue age, email delivery, AI cost/latency, and database pool pressure.

## Tests and migrations

- Unit-test business invariants without external infrastructure.
- Integration-test PostgreSQL-specific behavior against PostgreSQL, not EF InMemory.
- Migration scripts are reviewed, backward-compatible where feasible, and tested with realistic data volume.
- See [[../09 Quality/Testing Strategy|testing]] and [[../09 Quality/Deployment and CI-CD|release controls]].
