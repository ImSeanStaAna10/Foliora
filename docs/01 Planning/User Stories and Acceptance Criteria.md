---
title: User Stories and Acceptance Criteria
status: baseline stories; acceptance criteria proposed
updated: 2026-10-31
tags: [foliora, planning, user-stories]
---

# User Stories and Acceptance Criteria

Acceptance criteria are intentionally testable. Security and authorization apply to every story even where not repeated.

## Identity

### CV-001 — Create an account
As a new creator, I want to create an account so I can manage my professional presence.

- Valid input creates one account and a stable internal user ID.
- Duplicate identity is handled without revealing whether a private account exists beyond the minimum necessary response.
- Password rules and verification/recovery follow the accepted identity ADR.
- Secrets are never logged; failures are actionable but do not expose internals.

### CV-002 — Sign in with Google
As a creator, I want Google sign-in so I can access Foliora using a trusted identity.

- Redirect/callback validates issuer, audience, signature, nonce/state, expiry, and redirect URI.
- Only the minimum OIDC scopes are requested; Gmail or Drive access is not requested for sign-in.
- A Google identity is mapped to an internal account through a safe explicit linking policy.
- Invalid, replayed, or mismatched callbacks fail closed.
- Implementation is blocked until [[../02 Architecture/ADR-Authentication-and-Google-OIDC|the open identity ADR]] is accepted.

## Professional workspace

### CV-010 — Edit a professional profile
As a creator, I want to edit my professional profile so I can reuse accurate information.

- Creator can save partial records and receives field-level validation.
- A creator can read or mutate only their own profile.
- Private fields are omitted from public APIs and snapshots unless explicitly selected.
- Concurrent edits are detected through a version/ETag or equivalent mechanism.

### CV-011 — Manage portfolio items
As a creator, I want to manage projects, experience, education, skills, and credentials.

- Items can be created, edited, reordered, and deleted/archived.
- Dates, URLs, and required fields are validated consistently.
- A portfolio references only records owned by the creator.
- Existing published content remains unchanged until republished.

## Builder

### CV-020 — Create a portfolio
As a creator, I want a starter template so I can begin without designing from a blank page.

- Selected active template creates a draft with a supported configuration schema version.
- Default block IDs are unique and ordering is deterministic.
- Template updates do not silently mutate existing drafts or published snapshots.

### CV-021 — Arrange blocks by dragging
As a creator, I want to reorder portfolio blocks so the story reads in my preferred order.

- Dragging updates the visible order and accessible keyboard controls offer the same result.
- Reorder state is validated and saved with an optimistic version check.
- Reopening the editor preserves ordering.
- Hidden blocks remain in the draft but do not render publicly.
- Unsupported block IDs/types or duplicate IDs are rejected server-side.

### CV-022 — Edit and preview blocks
As a creator, I want immediate preview of content and style changes.

- Editor binds inputs to typed block configuration, not arbitrary HTML.
- Preview matches public renderer behavior at desktop and mobile breakpoints.
- Save state is clear; transient network failure preserves local edits and permits retry.
- Rich content is sanitized/encoded; unsafe URLs and executable content are blocked.

## Publishing

### CV-030 — Publish a portfolio
As a creator, I want to publish a validated version at a shareable URL.

- The server verifies ownership, publish readiness, slug availability, schema and asset references.
- Snapshot is committed atomically and becomes current only after full success.
- Public route serves snapshot content only; no private draft fields are returned.
- Idempotent retry does not create confusing duplicate live states.

### CV-031 — Edit without changing the live site
As a creator, I want to refine a draft while the current public version stays stable.

- Draft write never updates snapshot rows.
- Public page is byte/semantic stable until successful republish, except explicitly dynamic platform features.
- Failed save or publish leaves the last published snapshot intact.

### CV-032 — Unpublish or change URL
As a creator, I want to disable publication or manage my public slug.

- Unpublish makes public route unavailable and purges/invalidates relevant caches within the documented bound.
- Reserved and conflicting slugs are rejected.
- Slug change is atomic and has an explicit old URL policy (redirect or retire) before launch.

## Trust and engagement

### CV-040 — Review portfolio analytics
As a creator, I want aggregate engagement information.

- Only portfolio owner or authorized support role may read analytics.
- Unique visitor count is labeled as an estimate with its counting window.
- Raw IP and unnecessary personal data are not stored in event records.
- Small sample, bot traffic, retention, and consent policies are applied as documented.

### CV-050 — Submit feedback
As a visitor, I want to send a message to a creator when they allow feedback.

- The form clearly communicates optional identity fields and privacy use.
- Disabled forms do not accept submissions.
- Length/rate limits and anti-abuse controls apply.
- Feedback is private by default and cannot execute submitted markup.

### CV-051 — Moderate feedback
As a creator, I want to organize feedback and control what becomes public.

- Owner sees only own portfolio feedback.
- Status changes are recorded with timestamps.
- Public display requires explicit owner action and any required submitter consent.
- Delete/archive behavior follows retention and reporting policy.

### CV-060 — Receive notifications
As a creator, I want to hear about relevant events.

- In-app event is persisted idempotently.
- Email is queued after transaction commit and retried with bounded backoff.
- Unsubscribe/preferences are honored except strictly necessary security notices where policy permits.

## Premium AI

### CV-070 — Request a writing suggestion
As a premium creator, I want AI help improving supplied text.

- Server checks entitlement, quota, consent, payload size, and supported task.
- Only relevant user-provided content is sent to the configured provider.
- Output is clearly marked as a suggestion and linked to its source context.
- User must accept/edit before changing content; AI never publishes directly.
- Provider outage, timeout, refusal, and quota exhaustion fail gracefully.

### CV-071 — Analyze a CV
As a premium creator, I want a structured extraction and improvement checklist.

- File type, size, malware scanning, storage isolation, and retention controls apply.
- Extracted facts are shown for confirmation and not silently imported.
- Unsupported formats and extraction uncertainty are disclosed.
- User can delete uploaded file and derived data subject to documented retention limits.

## Platform operations

### CV-080 — Export and close account
As a creator, I want access to my data and a way to close my account.

- Export includes creator-owned profile, drafts, assets metadata, feedback, and portfolio history as defined by policy.
- Closure confirms the action and handles public pages, tokens, provider identity, analytics, and backups per retention policy.
- Deletion propagates to processors within contractual/legal periods and is observable.
