---
title: Data Dictionary
status: proposed logical dictionary
updated: 2026-10-31
tags: [foliora, database, data-dictionary]
---

# Data Dictionary

This describes logical entities, not a final migration. Use `uuid`, `timestamptz`, `text`, `boolean`, `integer`/`bigint`, and `jsonb` where appropriate. Apply ownership constraints, indexes, retention, and delete policies after review.

| Entity | Owner / purpose | Important fields and rules |
|---|---|---|
| `users` | Identity module; stable internal principal | `id`, created/deleted times. Credential fields depend on open identity ADR. Do not use email as primary key. |
| `external_identities` | Identity mapping | `user_id`, provider, immutable provider subject, timestamps. Unique provider+subject; never rely on email alone for account linking. |
| `professional_profiles` | Profiles module | `user_id`, headline, biography, optional location/avatar asset, visibility preferences, version. Private by default. |
| `skills` | Curated or user-defined skill vocabulary | Name/category/normalization. Avoid making global taxonomy a launch blocker. |
| `profile_skills` | Profile-skill join | Profile, skill/name, proficiency optional, order, display choice. |
| `projects` | Work samples | Title, summary, role, dates, links, media references, ordering, visibility. Validate URL scheme and ownership. |
| `experiences` | Work history | Organization, title, description, dates, current flag, order. Avoid inferred employment claims. |
| `educations` | Education history | Institution, award/field, dates, order, optional visibility. |
| `credentials` | Certifications, licenses, awards | Issuer, issue/expiry dates, credential reference; user controls public display. |
| `social_links` | External professional links | Label, URL, visibility and order; restrict allowed schemes. |
| `templates` | Builder module | Key, active state, description, supported schema version, starter configuration, version. Changes do not mutate existing content. |
| `portfolios` | Portfolio ownership/lifecycle | Owner user, name, slug, status, template, current published snapshot pointer, timestamps. Unique normalized slug. |
| `portfolio_drafts` | Builder/portfolio editable state | Portfolio, schema version, JSON document, version/concurrency token, update time. One current draft per portfolio initially. |
| `published_snapshots` | Publishing module immutable public releases | Portfolio, schema version, full public document, content hash, publisher, timestamp. Never update payload in place. |
| `portfolio_assets` | Asset metadata | Portfolio/owner, storage key, media type, byte size, visibility, scan state, created/deleted times. Actual file in object storage. |
| `feedback` | Visitor messages | Portfolio, message, optional disclosed contact, moderation status, public approval, timestamps. Private by default; retention policy needed. |
| `analytics_events` | Minimal event intake | Portfolio, event type, timestamp, pseudonymous short-lived session hash, path key, coarse device/referrer. No raw IP or full UA. |
| `analytics_daily` | Aggregated reporting (optional materialization) | Portfolio/date, views, estimated unique sessions; no small-group breakdowns that identify visitors. |
| `notifications` | In-app inbox | User, type, safe title/body reference, read state, created time. Avoid duplicating sensitive message content. |
| `notification_preferences` | User preferences | Channel, event category, enabled state, updated time. Necessary security messages have separate policy. |
| `outbox_messages` | Durable background intent (proposed) | Event key, payload reference, status, attempt count, next attempt, created/delivered times. Idempotent dispatch. |
| `plans` | Entitlement catalog (future) | Plan key/status; billing provider details kept separate. |
| `entitlements` | Server-side feature rules | Plan/feature key, limit/window, effective dates. Never trust client plan claims. |
| `subscriptions` | Subscription state (future) | User, plan, provider reference, status and dates. Avoid storing payment card data. |
| `ai_usage_records` | AI quota/audit | User, feature key, provider/model class, status, units/cost estimate, timestamps, correlation ID. Do not store prompt/response by default. |
| `audit_events` | Security/admin actions | Actor, action, target reference, timestamp, correlation, minimal metadata. Append-only access policy. |

## Cross-cutting rules

- User-owned reads and writes filter by internal owner identity in the backend.
- Public projections are explicit allowlists. Never serialize profile entities directly.
- Normalize slugs before uniqueness checks; handle concurrent creation through a unique database constraint.
- Store dates without time as `date` when they represent a day; event times as UTC `timestamptz`.
- Store JSON only where flexible schema is useful; validate document structure and version.
- Define account deletion, asset deletion, snapshot retention, feedback retention, analytics aggregation and backups together.

See [[Database Architecture and ERD|ERD and transaction boundaries]].
