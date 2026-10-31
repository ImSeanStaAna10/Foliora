---
title: Module Boundaries
status: proposed logical boundaries
updated: 2026-10-31
tags: [foliora, architecture, modules]
---

# Modular Monolith: Module Boundaries

## Boundary definition

Deploy one application organized into cohesive business modules. Each module owns its use cases, validation, persistence mapping and public contracts. A modular monolith is not a folder collection: enforce allowed dependencies and prevent direct mutation of another module's state.

## Proposed modules

| Module | Owns | Exposes |
|---|---|---|
| Identity | Internal user mapping, account lifecycle, OIDC integration seam | Current actor and account management contracts |
| Profiles | Profile, skills, experience, education, credentials, projects | Owner-facing profile use cases and public projection input |
| Portfolios | Portfolio identity, slug, status, draft association | Create/list/owner portfolio contracts |
| Builder | Templates, typed blocks, schema versions, validation | Template catalog and configuration validation |
| Publishing | Snapshot creation, current publication, public read model | Publish/unpublish and public portfolio query |
| Analytics | Event ingestion, aggregation, retention | Owner-scoped summary queries |
| Feedback | Anonymous submission, owner inbox, moderation | Submission and owner-management use cases |
| Notifications | In-app items, preferences, dispatch state | Notification query and enqueue contracts |
| Entitlements | Plans, feature access, quota accounting | Server-side feature/usage decision contract |
| AI | Prompt/task orchestration, provider adapter, safety pipeline | Typed AI assistance use cases |

## Dependency rules

1. API endpoints are adapters. They depend on application contracts, never on another module's database tables.
2. A module exposes a narrow application contract or publishes an internal domain event. Consumers must not reference internal entities or `DbContext` types.
3. Shared kernel stays tiny: stable IDs, clock, result/error abstractions, and common primitives only when genuinely shared.
4. Domain/application code depends on abstractions; infrastructure implements persistence and provider interfaces (dependency inversion).
5. Avoid cycles. If A needs B and B needs A, move orchestration to an application boundary or redefine ownership.
6. Same-database transactions across modules require deliberate coordination. Prefer one owning module and outbox/event for cross-module side effects; do not create hidden distributed-transaction assumptions.
7. Schema ownership can begin as separate EF configurations/schema namespaces in one PostgreSQL database. Separate physical databases are not an initial goal.
8. External integrations are adapters behind interfaces; no provider SDK types leak into domain models or API contracts.

## Proposed solution layout

```text
src/
  Foliora.Api/             # HTTP composition and endpoints
  Foliora.Worker/          # Background job host
  Modules/
    Identity/
    Profiles/
    Portfolios/
    Builder/
    Publishing/
    Analytics/
    Feedback/
    Notifications/
    Entitlements/
    AI/
  SharedKernel/                # deliberately small
tests/
  Unit/ Integration/ Architecture/
```

Each module can contain `Domain`, `Application`, and `Infrastructure` namespaces/projects only if that improves build boundaries. Avoid creating dozens of projects or generic repositories by default. Start with a solution structure that compiles simply and add hard project boundaries as dependency enforcement becomes valuable.

## Cross-module examples

- Publishing asks Builder to validate a schema-versioned draft and Profiles to create a public projection; it stores the resulting immutable snapshot in Publishing-owned storage.
- Feedback commits a feedback record, then records notification intent through an in-process contract/outbox after commit.
- AI asks Entitlements for permission and usage capacity before invoking an `IAiProvider`; the provider never checks subscription tables itself.
- Analytics reads only portfolio public identifiers and aggregates; it does not join private profile fields.

## Extraction criteria

Consider splitting a module into a service only when there is a demonstrated independent scaling, release, fault-isolation, team ownership, or compliance need that outweighs network, data consistency, deployment, and observability costs. See [[System Context and Components|system shape]].
