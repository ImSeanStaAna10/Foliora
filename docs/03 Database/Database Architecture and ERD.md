---
title: Database Architecture and ERD
status: proposed conceptual model on accepted PostgreSQL platform
updated: 2026-10-31
tags: [foliora, database, supabase, postgres, erd]
---

# Database Architecture and ERD

## Platform and access

The accepted platform is Supabase PostgreSQL, accessed by the ASP.NET Core backend with EF Core/Npgsql. See [[../02 Architecture/ADR-Supabase-PostgreSQL|ADR-001]]. Supabase Auth, Storage, and Realtime are optional; the auth ownership decision is still open.

Use relational tables for ownership, lifecycle, authorization-relevant state, and query dimensions. Use `jsonb` for validated, schema-versioned builder documents and immutable snapshots. Files live in object storage; PostgreSQL stores metadata and storage keys.

## Proposed conceptual ERD

```mermaid
erDiagram
  USERS ||--o| PROFESSIONAL_PROFILES : owns
  USERS ||--o{ PORTFOLIOS : creates
  USERS ||--o{ NOTIFICATIONS : receives
  USERS ||--o{ SUBSCRIPTIONS : has
  USERS ||--o{ AI_USAGE_RECORDS : incurs
  USERS ||--o{ EXTERNAL_IDENTITIES : maps
  PROFESSIONAL_PROFILES ||--o{ PROFILE_SKILLS : lists
  SKILLS ||--o{ PROFILE_SKILLS : categorizes
  PROFESSIONAL_PROFILES ||--o{ PROJECTS : owns
  PROFESSIONAL_PROFILES ||--o{ EXPERIENCES : records
  PROFESSIONAL_PROFILES ||--o{ EDUCATIONS : records
  PROFESSIONAL_PROFILES ||--o{ CREDENTIALS : records
  PROFESSIONAL_PROFILES ||--o{ SOCIAL_LINKS : exposes
  USERS ||--o{ PORTFOLIOS : owns
  TEMPLATES ||--o{ PORTFOLIOS : starts
  PORTFOLIOS ||--o{ PORTFOLIO_DRAFTS : edits
  PORTFOLIOS ||--o{ PUBLISHED_SNAPSHOTS : publishes
  PORTFOLIOS ||--o{ PORTFOLIO_ASSETS : references
  PORTFOLIOS ||--o{ ANALYTICS_EVENTS : receives
  PORTFOLIOS ||--o{ FEEDBACK : receives
  PLANS ||--o{ SUBSCRIPTIONS : governs
  PLANS ||--o{ ENTITLEMENTS : grants
  FEEDBACK ||--o| NOTIFICATIONS : triggers

  USERS {
    uuid id PK
    timestamptz created_at
    timestamptz deleted_at
  }
  PROFESSIONAL_PROFILES {
    uuid id PK
    uuid user_id FK
    text headline
    text biography
    text location
    text avatar_asset_id
    bigint version
    timestamptz updated_at
  }
  PORTFOLIOS {
    uuid id PK
    uuid owner_user_id FK
    uuid template_id FK
    text name
    text slug UK
    text status
    uuid current_snapshot_id FK
    bigint draft_version
    timestamptz updated_at
  }
  PORTFOLIO_DRAFTS {
    uuid id PK
    uuid portfolio_id FK
    integer schema_version
    jsonb document
    bigint version
    timestamptz updated_at
  }
  PUBLISHED_SNAPSHOTS {
    uuid id PK
    uuid portfolio_id FK
    integer schema_version
    jsonb document
    text content_hash
    timestamptz published_at
  }
  FEEDBACK {
    uuid id PK
    uuid portfolio_id FK
    text message
    text status
    boolean public_approved
    timestamptz created_at
  }
  ANALYTICS_EVENTS {
    uuid id PK
    uuid portfolio_id FK
    text event_type
    text session_key_hash
    text path_key
    text referrer_class
    text device_class
    timestamptz occurred_at
  }
```

The diagram is conceptual. Details are proposals and must be refined before migrations. Exact names may differ; ownership and snapshot invariants must not.

## Key constraints and indexes (proposed)

- UUID primary keys; UTC `timestamptz`; created/updated timestamps.
- `users` identity mapping depends on ADR-005; `external_identities(provider, subject)` unique if used.
- `professional_profiles.user_id` unique for one-profile-per-user baseline; reconsider only with a product requirement.
- `portfolios.slug` unique with case-insensitive normalization; reserved slugs excluded.
- A portfolio's `current_snapshot_id` must reference a snapshot of that same portfolio (enforce in service/constraint strategy).
- `portfolio_drafts.portfolio_id` unique for current draft, plus version concurrency token.
- `published_snapshots(portfolio_id, published_at)` index; immutable rows.
- Analytics indexes begin with portfolio and date; partitioning is deferred until volume justifies it.
- Feedback and notification owner/status/date indexes support inbox queries.
- Foreign-key delete behavior is explicit; do not cascade-delete evidence needed for audit or retention.

## Transaction boundaries

- Draft save: validate schema, check owner and expected version, update draft and version atomically.
- Publish: validate, create snapshot, move current pointer and set published status atomically; after commit enqueue cache invalidation/event work.
- Feedback: persist submission and durable notification intent atomically, then send email asynchronously.
- AI usage: reserve/check quota before provider call and record outcome without storing sensitive prompt content unnecessarily.

## Supabase operational notes

Use a server-side connection secret with least privilege appropriate to backend operations. Determine whether the chosen connection endpoint is direct or pooled and configure Npgsql pooling and statement behavior accordingly. Confirm connection caps, backups, PITR availability, TLS, region, migrations, and restore procedure for the selected plan before production. These are deployment decisions, not assumed product guarantees.

See [[Data Dictionary|entity definitions]], [[../02 Architecture/ADR-Supabase-Optional-Services|optional services]], and [[../09 Quality/Deployment and CI-CD|migration and environment controls]].
