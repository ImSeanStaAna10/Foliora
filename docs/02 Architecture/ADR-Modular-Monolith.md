---
title: "ADR-002: Modular Monolith"
status: accepted
decided: 2026-10-31
tags: [foliora, adr, architecture]
---

# ADR-002: Modular Monolith

## Status

Accepted architecture direction.

## Context

Foliora is an early product with cohesive workflows and a small initial engineering surface. Independent service deployment and distributed transactions would add cost before scale or team boundaries are known.

## Decision

Build one ASP.NET Core application organized into modules with explicit ownership, internal contracts, and dependency rules. A background worker may be a separate process from the same codebase when needed, but is not a separate business service by default.

## Consequences

- Local calls and transactional work remain straightforward.
- Boundaries need enforcement through code review, architecture checks, or project references.
- A module may be extracted later if a measurable operational or organizational reason emerges.
- Avoid cross-module database writes and circular references.

## Alternatives

- Microservices from day one: rejected as premature operational complexity.
- Unstructured monolith: rejected because it risks coupling and unclear ownership.

See [[Module Boundaries|module boundaries]].
