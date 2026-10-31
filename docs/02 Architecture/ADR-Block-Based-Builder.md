---
title: "ADR-004: Drag-and-Drop Block Builder"
status: accepted product direction
decided: 2026-10-31
tags: [foliora, adr, builder]
---

# ADR-004: Drag-and-Drop Block Builder

## Status

Accepted product direction; exact UI library and initial block catalog remain implementation choices.

## Context

Professionals need to arrange portfolios visually without code, while the public renderer must remain safe, responsive, and maintainable.

## Decision

Provide a full drag-and-drop editor over typed, versioned, schema-validated content blocks. The first release is a structured layout editor, not an unrestricted arbitrary-code or pixel-perfect canvas. Provide keyboard/touch alternatives and safe styling tokens.

## Consequences

- Block configuration has a versioned JSON schema and server validation.
- Templates compose supported blocks and theme tokens.
- New block types require renderer, schema, editor, accessibility and migration/compatibility work.
- The UI may use a suitable maintained drag-and-drop library after package/security review; the library is not selected here.

See [[../04 Portfolio Builder/Builder Architecture|builder architecture]].
