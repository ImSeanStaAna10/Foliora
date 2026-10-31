---
title: API Conventions and Endpoint Catalog
status: proposed API contract
updated: 2026-10-31
tags: [foliora, backend, api]
---

# API Conventions and Endpoint Catalog

## Conventions

- Base path: `/api/v1`; public portfolio reads may use `/public/v1` or a separately cached public host (hosting decision open).
- JSON request/response DTOs use camelCase; dates are ISO 8601 UTC timestamps unless date-only domain value.
- Use resource-oriented nouns and HTTP semantics. `POST` creates/commands, `GET` reads, `PUT` replaces, `PATCH` partial update, `DELETE` removes/deactivates.
- Return a consistent `application/problem+json` error shape with stable error code, correlation ID, safe detail, and field errors where helpful.
- Validate DTOs at boundaries and revalidate critical invariants in application/domain logic.
- Pagination is bounded; use cursor pagination for large collections. Sort order must be explicit and stable.
- Use ETag/`If-Match` or expected version for concurrent draft edits. Return `409 Conflict` on stale writes.
- Require idempotency key for publish and other costly/retryable commands where duplicate effects matter.
- Use cancellation tokens and async I/O. Apply rate limits by user/resource/IP-derived transient key, with privacy controls.
- Never return EF entities, credentials, internal provider errors, or private profile data through public endpoints.

## Endpoint catalog (proposed)

Authentication endpoints depend on open ADR-005; these are logical capabilities, not final URLs.

| Method and path | Access | Purpose |
|---|---|---|
| `GET /api/v1/me` | Signed-in | Current internal account summary |
| `PATCH /api/v1/me` | Signed-in | Update account preferences |
| `GET /api/v1/profile` | Owner | Read own profile |
| `PUT /api/v1/profile` | Owner | Create/update own profile |
| `GET /api/v1/profile/projects` | Owner | List own projects |
| `POST /api/v1/profile/projects` | Owner | Add project |
| `PATCH /api/v1/profile/projects/{id}` | Owner | Update owned project |
| `DELETE /api/v1/profile/projects/{id}` | Owner | Delete/archive owned project |
| `GET /api/v1/templates` | Public/authenticated catalog | List active templates |
| `GET /api/v1/portfolios` | Owner | List own portfolios |
| `POST /api/v1/portfolios` | Owner | Create portfolio/draft |
| `GET /api/v1/portfolios/{id}` | Owner | Read portfolio metadata |
| `PATCH /api/v1/portfolios/{id}` | Owner | Rename or update metadata |
| `GET /api/v1/portfolios/{id}/draft` | Owner | Load draft and version |
| `PUT /api/v1/portfolios/{id}/draft` | Owner | Save validated draft with expected version |
| `POST /api/v1/portfolios/{id}/publish` | Owner | Validate and publish snapshot |
| `POST /api/v1/portfolios/{id}/unpublish` | Owner | Disable public resolution |
| `GET /api/v1/portfolios/{id}/snapshots` | Owner | List publish history metadata |
| `POST /api/v1/portfolios/{id}/snapshots/{snapshotId}/restore` | Owner | Restore via a new publish event |
| `GET /public/v1/portfolios/{slug}` | Anonymous | Read published snapshot only |
| `POST /public/v1/portfolios/{slug}/feedback` | Anonymous | Submit rate-limited feedback if enabled |
| `POST /public/v1/portfolios/{slug}/analytics` | Anonymous/limited | Record minimal page event (may be beacon) |
| `GET /api/v1/portfolios/{id}/analytics` | Owner | Aggregate analytics within retention window |
| `GET /api/v1/portfolios/{id}/feedback` | Owner | List owner feedback |
| `PATCH /api/v1/feedback/{id}` | Owner | Update status/public approval |
| `DELETE /api/v1/feedback/{id}` | Owner | Delete/archive feedback |
| `GET /api/v1/notifications` | Signed-in | List notifications |
| `PATCH /api/v1/notifications/{id}` | Owner | Mark read/unread |
| `GET /api/v1/me/entitlements` | Signed-in | Read display-level entitlements (never authoritative for writes) |
| `POST /api/v1/ai/{feature}` | Premium, server checked | Run eligible AI assistance |

## Authorization expectations

For every `{id}` route, load through an owner-scoped query or authorize before disclosure. Avoid IDOR by returning not-found for inaccessible private resources where appropriate. Public endpoint queries only the current published snapshot. Admin operations use separate policy and audit.

## Versioning and compatibility

Version the HTTP contract for breaking public API changes. JSON portfolio schema has its own version. Additive API fields should be safe for clients. Keep generated OpenAPI contract reviewed and avoid undocumented behavior.

## Example problem response

```json
{
  "type": "https://foliora.example/problems/validation",
  "title": "The request is not valid.",
  "status": 400,
  "code": "portfolio.invalid_document",
  "correlationId": "01J8EXAMPLE",
  "errors": { "sections[1].content.title": ["A title is required."] }
}
```
