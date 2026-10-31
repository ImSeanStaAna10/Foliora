---
title: Premium AI Architecture
status: proposed premium capability; provider and launch open
updated: 2026-10-31
tags: [foliora, ai, premium, safety]
---

# Premium AI Architecture and Safeguards

## Positioning

AI features are premium enhancements, not a dependency for core profile, builder, publishing, feedback, or analytics workflows. Candidate features include rewriting supplied text, portfolio critique, CV extraction, and matching user-provided material to a job description.

## Request architecture

```mermaid
sequenceDiagram
  actor User
  participant UI as React UI
  participant API as ASP.NET Core
  participant Ent as Entitlements module
  participant AI as AI application service
  participant Guard as Safety/validation layer
  participant Provider as External model provider
  participant DB as PostgreSQL
  User->>UI: Request a specific suggestion
  UI->>API: Feature request + selected source content
  API->>Ent: Check plan, quota and eligibility
  Ent-->>API: Allow / deny
  API->>DB: Reserve usage and record correlation
  API->>Guard: Minimize, label untrusted text, validate task
  Guard->>Provider: Structured task request
  Provider-->>Guard: Candidate response
  Guard->>Guard: Validate schema, safety and unsupported claims
  Guard-->>API: Suggestion + caveats
  API->>DB: Record outcome and usage units
  API-->>UI: Suggestion for review
  User->>UI: Edit, accept, or discard
```

## Controls

- Server-side entitlement and quota checks; UI flags are not authorization.
- User initiates each task and sees what source data will be processed.
- Send only selected, relevant content. Do not send entire account history by default.
- Treat resumes, job descriptions, and portfolio text as untrusted instructions/data; isolate task instructions from document content and resist prompt injection.
- Require structured output schema and validate all fields, lengths, and referenced facts.
- Ask the model to preserve uncertainty and avoid unsupported claims; show source-grounded suggestions and require human review.
- Never auto-edit canonical profile or publish AI output.
- No provider training or retention assumptions are promised until contracts/settings are verified. Choose a provider with acceptable processing terms, retention, region and deletion controls.
- Avoid retaining prompts/completions by default. Store feature, status, model/provider class, cost/usage estimate, and correlation ID; if content retention is needed for support, make it explicit and time-bound.
- Apply timeout, retry, concurrency, request-size and cost ceilings; avoid retrying expensive non-idempotent requests blindly.
- Provide graceful refusal/failure, cancellation, and accessible explanation.
- Scan and safely extract uploaded CV files; do not execute embedded content.

## Suggested feature contracts

- `ImproveText(sourceText, audience, tone)` returns a suggestion, changes summary, and uncertainty flags.
- `ReviewPortfolio(snapshotOrDraftSelection, rubric)` returns prioritized issues and evidence references.
- `ExtractCv(file)` returns candidate structured facts with confidence and page/source references for user confirmation.
- `CompareToJobDescription(selectedProfileFacts, jobDescription)` returns matched evidence and gaps; not a hiring prediction or guarantee.

Exact prompts, providers, model versions, plan limits, pricing and legal disclosures are open decisions. Keep provider SDK calls behind `IAiProvider` and version task contracts.

## Abuse and safety

- Prevent prompt injection from user documents from overriding system policy or causing tool use.
- No autonomous external actions, account changes, messaging, or publication.
- Do not produce fabricated credentials or employment history; flag missing evidence.
- Add per-user quotas and global budget alerts; protect public endpoints from triggering model calls.
- Review demographic fairness and avoid employment eligibility/ranking claims.

## Release gate

Premium AI may launch only after processor/retention review, pricing/cost controls, evaluation set for factuality and harmful output, safe file handling, entitlement test coverage, user disclosure, observability, and support/refund process are ready.
