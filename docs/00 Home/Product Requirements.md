---
title: Product Requirements Document
aliases: [PRD]
status: baseline requirements; scope proposals labeled
updated: 2026-10-31
tags: [foliora, product, requirements]
---

# Product Requirements Document

## 1. Product summary

Foliora provides a private professional workspace and a public portfolio publishing experience. Users manage reusable professional records, configure a portfolio using typed blocks, preview it, then publish a versioned snapshot. Premium AI features are optional and require explicit user action and server-side entitlement checks.

## 2. Goals

- Let a broad range of professionals create a credible online portfolio without coding.
- Keep profile data reusable while allowing portfolios to curate it.
- Make draft editing and public publishing safely independent.
- Provide accessible, responsive portfolio presentation.
- Build a maintainable product with a modular monolith and a PostgreSQL database hosted by Supabase.

## 3. Non-goals for initial delivery

- Social feeds, follows, messaging, or a recruiting marketplace.
- Arbitrary executable HTML, JavaScript, or user-defined server code.
- Full freeform pixel-canvas design.
- Automated job applications or claims that AI guarantees employment.
- Multi-tenant enterprise teams, marketplace, custom-domain purchasing, or native mobile apps.

## 4. Users and permissions

| Persona | Needs | Access |
|---|---|---|
| Visitor | View a public portfolio and optionally leave feedback | Published, public content only; rate-limited feedback submission |
| Creator | Manage a profile, portfolio drafts, publication and feedback | Own resources only |
| Premium creator | Use eligible AI tools | Creator permissions plus server-verified AI entitlements and quotas |
| Moderator/support | Handle reports and abuse | Least-privilege, audited operational access; exact role model proposed |
| Administrator | Operate templates and platform settings | Restricted administrative actions with audit trail |

Authorization is enforced by the backend on every resource operation, regardless of frontend visibility.

## 5. Functional requirements

### 5.1 Account and identity

- Account registration and sign-in, account recovery, sign-out, and account closure.
- Google OIDC sign-in is a desired supported flow; provider integration is an open ADR.
- Link identity providers only after proving control of both identities; prevent account takeover by email collision.
- Maintain stable internal user IDs independent of an external identity provider.

### 5.2 Professional profile

- Store a headline, biography, location (optional), avatar, contact preferences, links, skills, experience, education, projects, and credentials.
- Permit incomplete drafts; validate field constraints without forcing every profession into one profile template.
- Distinguish private profile records from content selected for a published portfolio.
- Support ordering, visibility, soft deletion/restore where appropriate, and user export/deletion flows.

### 5.3 Portfolio management and builder

- Create one or more portfolios from a template or starter layout.
- Add, remove, duplicate, hide/show, configure, and reorder supported blocks through drag-and-drop.
- Offer undo/redo, autosave or explicit save with clear save status, and responsive preview.
- Validate configuration against a versioned schema on client and server; server validation is authoritative.
- Support accessible pointer, keyboard, and touch interactions for reordering.
- Reject executable markup and unsafe resource URLs.

### 5.4 Publishing

- Assign a unique public slug; allow availability checking and safe change flow.
- Publish only after validation and explicit confirmation.
- Freeze a complete renderable snapshot; draft edits do not change the currently published snapshot.
- Republish atomically; failed publish leaves the last good snapshot available.
- Unpublish immediately disables public access while retaining owner-accessible drafts and eligible history.

### 5.5 Analytics

- Report aggregate page views and estimated unique sessions over a date range.
- Optionally report page/section engagement, referrer category, and coarse device class when policy and sample size permit.
- Never identify a visitor as a recruiter or specific person without an authenticated, disclosed mechanism.
- Minimize retention and avoid storing raw IP addresses or full user-agent strings in analytics records.

### 5.6 Feedback and moderation

- Portfolio owner can enable/disable feedback and choose which fields to request.
- Visitor can submit a bounded message with abuse controls and clear privacy disclosure.
- Feedback is private by default; public display requires owner approval and separate consent from the submitter where identity is shown.
- Owner can mark new/read/resolved, archive, report, or delete feedback.
- Moderation actions are auditable; abusive or illegal material has a documented escalation path.

### 5.7 Notifications

- In-app notification for meaningful events such as new feedback and account/security changes.
- Email notifications are optional and governed by user preferences and applicable consent rules.
- Delivery is asynchronous, idempotent, retryable, observable, and does not block the primary transaction.

### 5.8 Premium AI

- AI is an add-on; the non-AI product remains fully usable.
- Initial candidate features: writing assistant, portfolio critique, CV extraction, and job-description alignment.
- Check subscription/entitlement and usage limits on the server before provider calls.
- Show generated suggestions separately; require user review before modifying profile or publishing.
- Disclose relevant processing and provide a way to cancel or discard output.

## 6. Quality attributes

Detailed targets are unapproved until load, availability, budget, and compliance requirements are known. Initial service objectives are proposals in [[../07 Trust/Security and Non-Functional Requirements|Security and NFRs]].

- Security: owner isolation, safe rendering, encrypted transport, secrets management, abuse controls.
- Availability: preserve prior published state through editor or publication failures.
- Performance: fast public pages, paginated owner lists, bounded API payloads.
- Accessibility: target WCAG 2.2 AA as a design and verification objective (proposed).
- Maintainability: explicit module ownership, automated checks, migration discipline.
- Portability: avoid unnecessary lock-in; keep provider-specific operations behind interfaces.

## 7. Assumptions and open questions

- Whether authentication is ASP.NET Core Identity + Google OIDC or Supabase Auth + Google is open; see [[../02 Architecture/ADR-Authentication-and-Google-OIDC|ADR: Authentication and Google OIDC]].
- Subscription provider, exact plan matrix, AI provider, email provider, hosting provider, legal jurisdiction, retention periods, and quantitative SLOs are open.
- Supabase Storage and Realtime are optional rather than assumed; see [[../02 Architecture/ADR-Supabase-Optional-Services|ADR: Supabase optional services]].
- This is a requirements baseline, not a signed commercial or legal specification.
