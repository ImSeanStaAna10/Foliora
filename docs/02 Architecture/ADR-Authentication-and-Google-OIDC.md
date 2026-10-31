---
title: "ADR-005: Authentication and Google OIDC"
status: open
updated: 2026-10-31
tags: [foliora, adr, authentication, oidc]
---

# ADR-005: Authentication and Google OIDC

## Status

**Open — implementation must not assume a final identity provider.** Stable internal user IDs and an identity-provider abstraction are recommended. The product wants Google sign-in; the owning auth system is not finalized.

## Context

Foliora uses Supabase PostgreSQL and ASP.NET Core. Supabase Auth is available as an optional service, but the database choice alone does not decide whether Supabase Auth or ASP.NET Core Identity owns credentials, sessions, and user lifecycle.

## Options

### A. ASP.NET Core Identity owns identity; Google is an external OIDC provider

Backend handles login/callback, maps external subject to an internal user, and issues an application session/token. This keeps auth policy inside the C# application and can use PostgreSQL for identity persistence. It requires careful cookie/CORS/CSRF design for browser sessions or secure token lifecycle if using bearer tokens.

### B. Supabase Auth owns identity; Google is configured as a Supabase provider

Frontend or backend initiates Supabase Auth flow; backend validates Supabase-issued JWTs and maps `sub` to internal user. This can reduce identity plumbing and integrate with optional Supabase services, but requires careful JWT verification, claim mapping, account linking, key rotation, and RLS alignment. Never trust client-supplied user IDs.

### C. External identity service

Use a dedicated identity provider and treat it as an external adapter. Adds vendor and operational cost; may be appropriate if enterprise identity needs appear.

## Proposed decision drivers

- Secure and supportable Google OIDC integration.
- Clear internal account ownership and recovery behavior.
- Avoid duplicate identities/account takeover on email matching.
- Browser session security, CSRF/CORS simplicity, and API-client needs.
- Supabase RLS/service integration requirements.
- Total operating cost and ability to test locally.

## Google OIDC architecture requirements (all options)

- Use Authorization Code flow with PKCE where applicable; validate state and nonce.
- Exact redirect URI allowlist; TLS; issuer/audience/signature/expiry validation.
- Request only `openid`, `profile`, and `email` as needed; do not request Gmail or Drive scopes for login.
- Use provider subject (`sub`) as external identity key; email is mutable and not a safe sole link key.
- Define account linking, unlinking, recovery, MFA, session expiration/revocation and account deletion.
- Keep client secrets and signing secrets on server/provider configuration, never in React bundle.
- Log auth outcomes and correlation IDs, not tokens or authorization codes.

## Interim rule

Implement domain use cases against an internal actor abstraction. Avoid auth tables, token assumptions, frontend SDK coupling, or provider-specific account-linking behavior until this ADR is accepted. A local development stub is acceptable only when clearly isolated.

## Decision record to complete

Decision owner: product/engineering. Record the selected option, session/token design, account-linking policy, RLS strategy, migration implications, threat review, and rollout plan, then change status to accepted.
