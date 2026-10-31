---
title: Deployment and CI-CD
status: proposed deployment baseline; providers open
updated: 2026-10-31
tags: [foliora, deployment, operations, cicd]
---

# Deployment, CI/CD and Environment Separation

## Environments

Keep local, development/test, staging/preproduction, and production isolated. Each uses separate database projects/credentials, storage buckets, OAuth callback URLs, email sending modes, AI keys/quotas, analytics, and secrets. Never point local development at production data.

## Pipeline (proposed)

1. Pull request: format, static checks, frontend typecheck/build, backend build, tests, dependency/container scanning, migration review.
2. Merge: build immutable artifact/container, attach commit and software inventory metadata, publish to registry.
3. Development deployment: apply backward-compatible database migration, deploy app/worker, run smoke checks.
4. Staging: production-like configuration with synthetic data; verify auth callbacks, public routes, publish/unpublish, queue, backup/restore procedure.
5. Production: controlled promotion, serialized migration runner, health checks, monitored rollout, rollback plan.

The specific CI/CD vendor and hosting provider are open.

## Supabase PostgreSQL migrations

- EF Core/Npgsql migration files are reviewed and committed with code.
- One controlled deployment job applies migrations; app replicas must not race to migrate.
- Prefer expand/contract: add compatible column/table, deploy dual-compatible code, backfill, switch reads/writes, then remove obsolete schema in later release.
- Take/verify backup according to approved operational policy before risky migration.
- Never use ad hoc production SQL without reviewed change record and recovery plan.
- Validate connection/pooling settings for selected Supabase endpoint; monitor pool utilization and limits.

## Configuration and secrets

Inject secrets at runtime through managed secret storage. Separate public frontend configuration (non-secret) from server secrets. Rotate DB and provider credentials; restrict CI deployment identity. Do not print environment variables in build logs.

## Health and observability

- Liveness indicates process health; readiness checks essential dependencies without causing cascading failure.
- Trace requests across API, database and jobs with correlation IDs.
- Monitor error rate/latency, DB connections/pool wait, publish success, stale-cache invalidation, queue age/retries, email delivery, storage failures, AI usage/cost, auth errors and rate limiting.
- Alert on user-impacting changes; define on-call owner and runbook.

## Backup, restore and recovery

Choose backups and point-in-time recovery based on RPO/RTO requirements. Verify restoration into an isolated environment regularly; a backup without a restore exercise is not a proven recovery path. Include object storage references/retention in recovery testing.

## Rollback

Application rollback must remain compatible with current schema during deployment window. Do not automatically reverse destructive migrations. Publishing rollback reselects or republishes an immutable snapshot; it is separate from code rollback.

See [[../03 Database/Database Architecture and ERD|database operational notes]] and [[../02 Architecture/ADR-Hosting-and-Operations|open hosting ADR]].
