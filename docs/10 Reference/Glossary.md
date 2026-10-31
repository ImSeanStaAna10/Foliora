---
title: Glossary
status: reference
updated: 2026-10-31
tags: [foliora, glossary]
---

# Glossary

| Term | Meaning |
|---|---|
| AI entitlement | Server-verified permission and quota to use a premium AI feature. |
| Block | Typed portfolio section with validated content and style settings. |
| Creator | Authenticated Foliora user who owns professional data and portfolios. |
| Draft | Editable, private portfolio document. |
| External identity | Mapping between an internal Foliora user and a provider's immutable subject identifier. |
| Idempotency key | Client-supplied unique operation key used to make safe retries return one logical result. |
| Modular monolith | One deployable application divided into explicit business modules with dependency rules. |
| Npgsql | .NET data provider used by EF Core to connect to PostgreSQL. |
| OIDC | OpenID Connect, an identity layer on OAuth 2.0 used for sign-in. |
| Portfolio snapshot | Immutable, complete public representation created on publish. |
| Public projection | Explicitly selected public fields derived from private profile records. |
| RLS | PostgreSQL row-level security policies. Useful for direct client access when correctly designed; not a replacement for backend authorization. |
| Schema version | Version identifier for a portfolio JSON document and its renderer/validator contract. |
| Supabase Auth | Optional Supabase identity service; whether it owns Foliora authentication is an open ADR. |
| Supabase PostgreSQL | Managed PostgreSQL database platform selected for Foliora. |
| Supabase Realtime | Optional change-stream/realtime service; not required for initial editor. |
| Supabase Storage | Optional object storage service candidate for files and images. |
| Visitor | Anonymous user viewing a published portfolio or submitting feedback. |

See [[../02 Architecture/Architecture Decision Records|decision register]].
