---
title: System Context and Components
status: accepted architecture direction; implementation details proposed
updated: 2026-10-31
tags: [foliora, architecture, modular-monolith]
---

# System Context and Components

## System context

Foliora is a single product boundary: a React + TypeScript user interface and public portfolio renderer call an ASP.NET Core C# backend. The backend is a modular monolith that owns application rules and accesses Supabase-hosted PostgreSQL via EF Core/Npgsql. External identity, file storage, email and AI are behind adapters; Supabase Auth, Storage and Realtime are optional services, not required by the database decision.

```mermaid
flowchart LR
  Creator[Creator]
  Visitor[Portfolio visitor]
  Admin[Authorized operator]
  Web[React + TypeScript app]
  Public[Public portfolio renderer]
  API[ASP.NET Core API\nModular monolith]
  PG[(Supabase PostgreSQL)]
  Identity[Identity provider\nOpen ADR]
  Blob[Object storage\nProvider TBD]
  Mail[Transactional email\nProvider TBD]
  AI[AI provider\nPremium; provider TBD]

  Creator --> Web
  Admin --> Web
  Visitor --> Public
  Web --> API
  Public --> API
  API --> PG
  API -. OIDC adapter .-> Identity
  API -. signed asset URLs .-> Blob
  API -. queued messages .-> Mail
  API -. opt-in premium requests .-> AI
```

## Container/component view

```mermaid
flowchart TB
  subgraph Client[Browser clients]
    CreatorUI[Creator app: profile, editor, analytics, inbox]
    PublicUI[Published portfolio renderer]
  end
  subgraph Deploy[Foliora application deployment]
    Http[API host: routing, authn middleware, validation, errors]
    Identity[Identity module]
    Profiles[Profiles and work-history module]
    Portfolios[Portfolios module]
    Builder[Builder/templates module]
    Publishing[Publishing module]
    Analytics[Analytics module]
    Feedback[Feedback/moderation module]
    Notifications[Notifications module]
    Entitlements[Plans and entitlements module]
    AI[AI application module]
    Jobs[Background worker process, same codebase]
  end
  PG[(Supabase PostgreSQL)]
  Storage[(Optional object storage)]
  External[OIDC / email / AI providers]
  CreatorUI --> Http
  PublicUI --> Http
  Http --> Identity
  Http --> Profiles
  Http --> Portfolios
  Http --> Builder
  Http --> Publishing
  Http --> Analytics
  Http --> Feedback
  Http --> Notifications
  Http --> Entitlements
  Http --> AI
  Identity --> PG
  Profiles --> PG
  Portfolios --> PG
  Builder --> PG
  Publishing --> PG
  Analytics --> PG
  Feedback --> PG
  Notifications --> PG
  Entitlements --> PG
  AI --> PG
  Profiles -. asset metadata .-> Storage
  Publishing -. public assets .-> Storage
  Jobs --> PG
  Jobs --> External
  Identity -. optional integration .-> External
  AI -. premium only .-> External
```

## Runtime shape

The initial deployment can run API and background worker as separate processes built from one repository and sharing module contracts. They are not separate business services. A transactional outbox is a proposal if reliable asynchronous side effects are required; start with the smallest reliable job mechanism that meets delivery requirements.

## Data flows

1. Creator UI calls authenticated API; the backend resolves internal actor identity and authorizes every resource.
2. Profile and draft writes use module-owned persistence and validation.
3. Publish validates a draft, produces an immutable snapshot, and atomically moves the current publication pointer.
4. Public renderer reads only published records and permitted public assets.
5. Visitor analytics and feedback use constrained anonymous endpoints with rate limits and privacy controls.
6. Background delivery handles email and retention work; AI requests are synchronous or queued based on provider latency and user experience, with quotas either way.

## Supabase boundary

Supabase is the PostgreSQL platform. The trusted C# backend uses Npgsql. Do not expose database credentials, Supabase service-role keys, or privileged tokens in React. If a future client uses Supabase APIs directly, review RLS, JWT validation, and authorization boundaries in a new ADR before enabling it.

See [[Module Boundaries|module rules]], [[../03 Database/Database Architecture and ERD|database architecture]], and [[Architecture Decision Records|decision status]].
