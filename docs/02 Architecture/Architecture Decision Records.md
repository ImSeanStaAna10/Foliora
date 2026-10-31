---
title: Architecture Decision Records
aliases: [ADR Register]
status: decision index
updated: 2026-10-31
tags: [foliora, architecture, adr]
---

# Architecture Decision Records

## Status vocabulary

- **Accepted:** chosen direction; implementation may still be pending.
- **Proposed:** recommendation awaiting owner approval or validation.
- **Open:** alternatives remain undecided; do not implement a provider-specific path as final.
- **Superseded:** retained for history, replaced by a later decision.

## Accepted direction

| ADR | Decision | Status |
|---|---|---|
| [[ADR-Supabase-PostgreSQL|ADR-001]] | Supabase-hosted PostgreSQL is the database platform; backend access uses EF Core + Npgsql | Accepted |
| [[ADR-Modular-Monolith|ADR-002]] | Start with a modular monolith | Accepted |
| [[ADR-Portfolio-Snapshots|ADR-003]] | Keep draft and immutable published snapshots separate | Accepted |
| [[ADR-Block-Based-Builder|ADR-004]] | Build a full drag-and-drop editor for validated blocks | Accepted product direction |

## Open or proposed decisions

| ADR | Question | Status |
|---|---|---|
| [[ADR-Authentication-and-Google-OIDC|ADR-005]] | Which system owns auth: ASP.NET Core Identity or Supabase Auth? How is Google OIDC connected? | Open |
| [[ADR-Supabase-Optional-Services|ADR-006]] | Should Storage, Realtime, or direct Supabase client access be enabled? | Proposed optional; decide per need |
| [[ADR-Hosting-and-Operations|ADR-007]] | Hosting, migration execution, backups, regions and SLO targets | Open |
| [[../08 AI/Premium AI Architecture|AI provider and launch]] | Provider, processing terms, pricing, retention and release gate | Open |

## Historical note

An earlier draft's database choice has been superseded by accepted ADR-001: use Supabase PostgreSQL.

## Change process

For material changes, record context, decision drivers, alternatives, consequences, security/privacy effects, migration plan, and status. Update affected notes and mark replaced ADRs `superseded`; do not silently edit the history of an accepted decision.
