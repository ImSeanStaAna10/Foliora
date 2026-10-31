---
title: Roadmap
status: proposed sequencing; dates uncommitted
updated: 2026-10-31
tags: [foliora, planning, roadmap]
---

# Roadmap

No calendar commitments have been approved. The phases below are dependency-based, not promises or estimates.

## Phase 0 — Decisions and foundation

- Resolve authentication integration and Google OIDC flow.
- Select hosting, file storage, email, subscription and AI provider strategy as needed.
- Agree on privacy jurisdiction, data retention, accessibility target, and launch SLOs.
- Establish repo structure, CI checks, environment configuration, EF Core/Npgsql connectivity to Supabase PostgreSQL, migrations, observability, and security baseline.
- Define module contracts and initial domain model.

**Exit:** team can deploy a minimal secure service and database change through a repeatable path; owners are clear.

## Phase 1 — Creator foundation

- Account lifecycle and authorization.
- Professional profile and reusable records.
- Portfolio creation and template catalog.
- Draft configuration persistence and editor shell.

**Exit:** authenticated creator can save profile and portfolio draft; ownership and validation tests pass.

## Phase 2 — Builder and publication

- Block editing, drag-and-drop, accessible alternatives, preview.
- Save status, conflict handling, configuration schema versioning.
- Publish snapshots, slug routing, unpublish and rollback/republish workflow.

**Exit:** publish guarantees last-good public state; draft/public separation verified.

## Phase 3 — Trust and launch readiness

- Feedback, moderation, notifications.
- Privacy-conscious analytics and retention jobs.
- Abuse prevention, account export/closure, backups and restore exercise.
- Accessibility, load, security, and operational review.

**Exit:** MVP launch gate in [[MVP Scope|MVP scope]] is met.

## Phase 4 — Premium and growth (conditional)

- Validate AI use cases and consent copy.
- Implement premium entitlements, quotas, provider abstraction, safety review and audit.
- Add subscription billing only after plan and provider decisions.
- Evaluate templates, custom domains, integrations, or collaboration from evidence.

**Exit:** paid features have measured value, cost limits, clear user control, and operational support.

## Roadmap governance

Review at phase boundaries. Record architectural changes in [[../02 Architecture/Architecture Decision Records|ADRs]]. Do not turn this sequence into a deadline without resourcing and estimation.
