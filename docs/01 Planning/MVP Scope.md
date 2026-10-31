---
title: MVP Scope
status: proposed scope baseline
updated: 2026-10-31
tags: [foliora, planning, mvp]
---

# MVP Scope

## MVP outcome

A creator can maintain a basic professional profile, build a responsive portfolio with drag-and-drop blocks, preview it, publish a stable public URL, view simple privacy-conscious analytics, and receive/manage feedback.

## In scope (proposed)

1. Account lifecycle, with the chosen identity integration implemented only after the open ADR is accepted.
2. Profile headline, biography, skills, experience, education, projects, links, and profile image metadata.
3. A small set of inclusive templates and typed blocks: hero, about, skills, projects, experience, education, credentials, gallery, contact, and footer.
4. Full drag-and-drop ordering of supported blocks; keyboard-accessible reorder controls.
5. Draft saving, undo/redo within an editing session, desktop/mobile preview, publish validation.
6. Versioned immutable published snapshots; publish, republish, unpublish, slug uniqueness.
7. Basic aggregate page-view and estimated-session reporting.
8. Visitor feedback form, private inbox, owner moderation, rate limiting.
9. In-app notifications; email notifications if operationally ready.
10. Basic admin operations for templates, abuse reports, and support access with audit logging.

## Explicitly deferred

- Premium AI (architecture documented now; release only after provider, privacy, cost, and entitlement decisions).
- Payments/subscription automation beyond a manually controlled entitlement seam.
- Custom domains, marketplace, unrestricted design canvas, animation editor, arbitrary embeds/scripts.
- Team workspaces, collaboration, social network, job board, native mobile apps.
- Deep visitor attribution, identity resolution, heatmaps, session replay, cross-site tracking.
- Multi-region active-active operation.

## MVP acceptance gate

- Core creator flow works end to end in a production-like environment.
- Draft changes cannot leak to the public page before republish.
- A failed publish preserves the prior published snapshot.
- Object ownership is enforced in backend tests for every write and private read.
- Public rendering escapes content and blocks unsupported executable markup.
- Editor works at supported phone, tablet, and desktop widths with keyboard alternatives.
- Data export, account closure, backup, restore, and incident runbooks are documented and exercised before public launch.
- Product, privacy, accessibility, and operational open questions with launch impact are resolved.

## MVP exclusions are not permanent

Deferred capabilities can enter a later roadmap after validating demand, cost, risk, and operational readiness. Do not prebuild generalized frameworks for speculative features; preserve seams only where they reduce real coupling.

See [[Roadmap|roadmap]], [[User Stories and Acceptance Criteria|acceptance criteria]], and [[../02 Architecture/Architecture Decision Records|ADRs]].
