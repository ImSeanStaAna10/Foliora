---
title: "ADR-007: Hosting and Operations"
status: open
updated: 2026-10-31
tags: [foliora, adr, operations]
---

# ADR-007: Hosting and Operations

## Status

Open. Hosting provider, deployment platform, region, availability target, backup tier, pooling mode, and recovery objectives are not specified.

## Decision inputs required

- Expected launch traffic and peak public-page demand.
- Data residency and legal jurisdiction.
- Supabase region and supported application regions.
- Database connection pooling mode and application concurrency.
- Required uptime, RPO, RTO, and acceptable maintenance windows.
- CDN/cache design for public pages and unpublish invalidation.
- Secret management, TLS, network allowlists, observability and incident response.
- Migration runner that serializes production schema changes.
- Backup retention, restore verification, and recovery ownership.

## Interim design constraints

Keep app stateless where practical; externalize durable data; configure secrets per environment; run backward-compatible migrations; separate deployment credentials from runtime credentials. Do not invent production targets until product owners agree on them.
