---
title: Testing Strategy
status: proposed quality strategy
updated: 2026-10-31
tags: [foliora, testing, quality]
---

# Testing Strategy

Testing is a planned quality requirement. This note describes future verification; it does not imply tests have been run or that an implementation exists.

## Test layers

### Unit tests

- Domain invariants: slug normalization, publish state transitions, block validation, entitlement policy, feedback status.
- Application use cases with faked module/provider interfaces.
- AI output validation and safe URL/content policy.

### Module/architecture tests

- Prohibit module references to internal types of other modules.
- Detect circular dependencies and accidental controller-to-infrastructure coupling.
- Ensure external provider packages stay within adapters.

### Integration tests

- Run against PostgreSQL/Npgsql-compatible database; prefer a real ephemeral PostgreSQL instance for constraints, JSONB, transactions and migrations.
- Verify EF Core migration from clean database and upgrade from previous schema.
- Test ownership filters, concurrency, publish atomicity, idempotency and outbox retries.
- Test object storage/email/identity adapters with provider sandbox or contract fakes.
- Never treat EF Core InMemory as proof of PostgreSQL behavior.

### API and end-to-end tests

- Registration/login once auth ADR is accepted, recovery and account linking.
- Profile/draft edit, drag/drop, keyboard reorder, preview, publish, public read, unpublish.
- Confirm draft changes do not affect current snapshot.
- Confirm failed publish leaves last-good public version.
- Verify feedback rate limit/moderation, analytics privacy, notifications and AI entitlements.

### Security and accessibility

- Authorization matrix and IDOR regression tests for every resource family.
- Stored XSS/unsafe URL, upload, SSRF-prevention, abuse/rate-limit and secret leakage checks.
- Keyboard-only, screen-reader, contrast, zoom and responsive review.
- Dependency/container scanning and threat-model review.

### Performance and resilience

- Load test public page and draft save/publish paths at agreed traffic profile.
- Test database pool saturation, provider timeout, email retry, storage outage, cache invalidation lag, and AI budget exhaustion.
- Restore backup into isolated environment and verify data integrity before launch.

## Test data

Use synthetic cross-industry profiles and clearly fake identities. Do not copy real CVs, credentials, emails, or production data into test environments unless a reviewed, redacted fixture process explicitly allows it.

## CI quality gates (proposal)

Format/lint, compile, unit tests, module rules, migration validation, integration test suite, frontend typecheck and accessible component checks. Deployment promotion requires environment-specific approvals and smoke checks. Exact CI vendor is open.
