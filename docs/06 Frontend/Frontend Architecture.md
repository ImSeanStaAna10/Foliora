---
title: Frontend Architecture
status: proposed frontend structure
updated: 2026-10-31
tags: [foliora, frontend, react, typescript]
---

# Frontend Architecture

## Stack

React + TypeScript is the selected frontend direction. Exact framework, router, styling system, form library, data-fetching library, and drag-and-drop package are open implementation choices; select maintained tools after accessibility, security, and bundle-size review. Do not assume Tailwind or a particular state library.

## Proposed feature organization

```text
src/
  app/                 # app shell, routing, providers, error boundaries
  features/
    auth/              # adapter around chosen auth flow
    profile/
    portfolios/
    builder/
    publishing/
    analytics/
    feedback/
    notifications/
    premium-ai/
  shared/
    api/               # typed HTTP client, errors, auth token/session adapter
    components/        # accessible primitives
    validation/
    types/
```

This is a starting layout, not a required directory structure.

## Responsibilities

- Components render; hooks or feature services coordinate user flows.
- API client owns transport, correlation headers, problem responses, cancellation and retry policy.
- Feature components do not call arbitrary endpoints or duplicate backend business rules.
- UI validation mirrors server constraints for usability; server remains authoritative.
- Separate editor state (selection, drag session, undo history) from persisted portfolio JSON.
- Public renderer consumes a narrow published DTO and is independent from private creator APIs.

## Builder interaction model

Represent each block as a discriminated TypeScript union keyed by block `type`. Keep schemas/version adapters close to block feature code or in a shared generated contract. Render only registered types. Drag-and-drop changes `order` through a command that can be undone, saved and keyboard-operated. Do not use raw stored HTML as a rendering shortcut.

## Data and error handling

- Prefer server cache/query behavior that invalidates after writes; avoid stale optimistic publish state.
- Draft saves carry expected version and preserve unsaved edits if a request fails.
- Handle `401`, `403/404`, validation, conflict, throttling and server errors distinctly.
- Never store long-lived credentials in `localStorage` by default. Final session design depends on open auth ADR.
- Lazy-load editor-only code where it improves public page performance.

## Accessibility and responsive behavior

- Semantic landmarks, labels, keyboard navigation, focus management, contrast, reduced motion.
- Dragging must have buttons/keyboard alternative and live announcement for reorder.
- Preview at defined responsive breakpoints; do not treat device preview as separate content model.
- Public portfolio should remain readable with zoom, text resizing, and screen readers.

## Design system

Use named theme tokens and a small set of accessible presets. Restrict user-controlled values to validated tokens. Keep component patterns consistent; avoid one-off styling controls until demand warrants them.
