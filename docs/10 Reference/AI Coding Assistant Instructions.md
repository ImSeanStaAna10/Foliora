---
title: AI Coding Assistant Instructions
aliases: [Cline Instructions, Coding Agent Guide]
status: proposed repository guidance
updated: 2026-10-31
tags: [foliora, ai-assistant, engineering]
---

# AI Coding Assistant Instructions

Use this note as a future repository-facing guide for Cline and other coding agents. Copy/adapt it into the repository's agent instruction file when implementation begins. This vault is a specification; repository instructions should link to its committed equivalent rather than assume this external folder is always mounted.

## Before changing code

1. Read the repository's `AGENTS.md`/assistant instructions and relevant module documentation.
2. Inspect the actual repository structure and existing conventions; do not assume this proposal is already implemented.
3. Identify affected modules, API contracts, schema versions, security/privacy impacts, and user-visible changes.
4. Check ADR status. Accepted decisions are constraints; proposed/open ADRs are not final. Do not silently close an open ADR through code.
5. For ambiguous choices, proceed only with reversible work that does not depend on the answer; surface the specific open decision.

## Product and architecture constraints

- Frontend: React + TypeScript.
- Backend: ASP.NET Core + C#.
- Database: Supabase PostgreSQL through EF Core/Npgsql.
- Architecture: modular monolith with clear module ownership and no circular dependencies.
- Builder: drag-and-drop over typed, validated blocks; never execute arbitrary portfolio HTML/JavaScript.
- Draft and published snapshots remain separate. Draft edits must not change the live version.
- AI is premium, user initiated, server-entitlement checked, privacy-aware, and suggestion-only until human approval.
- Authentication integration and Supabase optional-service use remain open until their ADRs are accepted.

## Engineering expectations

- Apply OOP, SOLID, DRY, KISS, YAGNI, separation of concerns, dependency inversion and pragmatic clean architecture.
- Prefer the smallest maintainable change that fulfills a concrete requirement.
- Keep controllers/endpoints thin; keep module internals private; avoid leaking EF entities.
- Validate all untrusted input server-side; enforce ownership on every private resource access.
- Keep secrets out of source, client bundles, logs, and examples.
- Do not invent user facts or present proposals as finalized decisions.
- Avoid speculative frameworks, microservices, event sourcing, generic repositories and unrelated refactors.

## Database and migration care

- Check existing migrations and database state before proposing schema changes.
- Use EF Core/Npgsql and PostgreSQL-compatible types/constraints; test PostgreSQL-specific behavior.
- Never run destructive production actions or alter real user data without explicit, scoped authorization.
- Make migration steps reviewable and compatible with deployment order; explain rollback/forward-fix path.
- Do not place binary files or sensitive CV contents into JSONB or logs.

## Completion report

When an implementation task is complete, summarize what changed, why, and relevant limitations. State exactly which verification was run and its outcome; never claim a test passed if it was not run. Update the affected documentation when an approved design changes. Leave unrelated pre-existing work untouched.

## Repository bootstrap checklist

- Copy approved requirements and ADRs into repo docs or link them in root instructions.
- Add real build/test commands after the repository exists; this vault intentionally does not invent commands.
- Pin and review dependencies, setup secrets locally through documented examples, and separate environment configuration.
- Add module ownership and CI checks as implementation makes boundaries concrete.
