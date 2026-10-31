---
title: Feedback, Moderation and Notifications
status: proposed product workflows
updated: 2026-10-31
tags: [foliora, feedback, moderation, notifications]
---

# Feedback, Moderation and Notifications

## Feedback flow

1. Portfolio owner enables feedback and configures optional name/email prompts.
2. Public form shows purpose, privacy use, length limits, and any anti-abuse challenge.
3. Anonymous endpoint validates portfolio is published and feedback-enabled, applies rate limits and spam checks, then stores message as private.
4. In-app notification intent is committed with feedback or through a transactional outbox.
5. Email is sent asynchronously only if creator preferences and policy allow.
6. Creator reviews and marks new/read/resolved, archives/deletes, reports, or separately approves a testimonial for public use.

## Moderation rules

- Private by default; do not display feedback publicly based only on visitor submission.
- If creator republishes a quote, require applicable submitter permission to display identifying details and provide takedown process.
- Escape message content; no executable HTML.
- Provide abuse/report controls and rate limits; protect creator email from direct exposure by default.
- Keep moderation/admin actions auditable while minimizing stored sensitive details.
- Define spam, harassment, threats, illegal content, escalation and account enforcement policy before public launch.

## Feedback states (proposed)

`new` → `read` → `resolved`; optional `archived`, `reported`, or `removed`. State changes should be timestamped. Public approval is separate from inbox status.

## Notifications

Channels: in-app first; email optional. Categories can include new feedback, publication/security events, account lifecycle, and AI usage. Product/marketing messages require separate consent and are out of initial scope.

### Reliability

- Persist notification and outbound delivery intent transactionally or through a durable outbox.
- Use unique event/delivery keys to avoid duplicate email on retries.
- Retry transient failures with bounded backoff; dead-letter and alert on exhausted retries.
- Do not retry permanent invalid-address or policy failures indefinitely.
- Keep in-app notification readable if email provider is down.
- Make unsubscribe/preferences easy; security notices may follow a separately documented policy.

## Data minimization

Avoid embedding full feedback message in an email subject/body unless user has a clear preference; a secure link to inbox is safer. Store only optional visitor contact information the form explains. Define deletion and retention before release.

See [[../05 Backend/API Conventions and Endpoint Catalog|API routes]], [[Analytics and Privacy|privacy controls]], and [[../09 Quality/Deployment and CI-CD|job operations]].
