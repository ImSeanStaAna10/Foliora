---
title: "ADR-003: Draft and Published Snapshots"
status: accepted
decided: 2026-10-31
tags: [foliora, adr, publishing]
---

# ADR-003: Draft and Published Snapshots

## Status

Accepted product and data direction.

## Context

Creators must edit without silently changing their live portfolio. Publishing needs a reliable rollback point and a public representation that is independent from private source records.

## Decision

Store editable draft configuration separately from immutable published snapshot records. A portfolio points to its current published snapshot. Publish validates and materializes a complete public document, then atomically advances the pointer. Unpublish changes public availability; it does not delete the creator's draft.

## Consequences

- Snapshot payloads are self-contained render data with schema version and references to eligible public assets.
- Profile edits do not leak into a public page until a new snapshot is published.
- Snapshot retention/rollback duration and asset lifecycle need explicit operational policy.
- Cache keys include publication/snapshot version; unpublish invalidates or bypasses cache.

See [[../04 Portfolio Builder/Publishing Workflow|publishing workflow]].
