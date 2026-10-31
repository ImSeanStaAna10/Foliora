---
title: "ADR-001: Supabase PostgreSQL"
status: accepted
decided: 2026-10-31
tags: [foliora, adr, database]
---

# ADR-001: Supabase PostgreSQL

## Status

Accepted platform direction. Runtime configuration, hosting tier, pooling mode, and operational limits remain to be selected.

## Context

Foliora needs a relational database for account associations, professional records, portfolio drafts, immutable snapshots, feedback, notifications, subscriptions/entitlements, and analytics aggregates. The updated product decision is Supabase PostgreSQL.

## Decision

- Use Supabase-hosted PostgreSQL as the database platform.
- Connect from the trusted ASP.NET Core backend using EF Core and the Npgsql provider.
- Use PostgreSQL-native `jsonb` for versioned block documents and published snapshot payloads where a flexible document is appropriate; keep ownership, identity relations, status, and query-critical fields relational.
- Keep provider-specific Supabase service calls behind interfaces.
- Keep database credentials server-side. The React client must not connect with privileged database credentials.

## Consequences

- EF Core migrations must be tested against PostgreSQL/Npgsql, not an in-memory substitute that differs in behavior.
- Use PostgreSQL data types, indexes, constraints, transactions and concurrency semantics deliberately.
- Deployment must use the appropriate Supabase connection/pooling endpoint for the runtime; verify prepared statement behavior and connection limits for the selected pooler.
- RLS is valuable defense in depth for direct client access. If only the backend connects using a privileged role, application authorization remains mandatory; do not assume RLS protects backend routes automatically.
- Choose a safe migration runner and backup/restore process before production.

## Alternatives considered

- Self-managed PostgreSQL: offers control but adds operations burden without a current requirement.
- Multiple databases: unnecessary for initial modular monolith.

## Review triggers

Revisit if provider limits, compliance needs, cost, region availability, or operational requirements cannot be met. Record any change in a new ADR.

See [[../03 Database/Database Architecture and ERD|database design]] and [[ADR-Supabase-Optional-Services|optional Supabase services]].
