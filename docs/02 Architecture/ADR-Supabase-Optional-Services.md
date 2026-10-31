---
title: "ADR-006: Supabase Optional Services"
status: proposed; review per capability
updated: 2026-10-31
tags: [foliora, adr, supabase]
---

# ADR-006: Supabase Optional Services

## Status

PostgreSQL is accepted. Supabase Auth, Storage and Realtime are optional services and are not implied by using Supabase PostgreSQL. Choose each only when the use case justifies it.

## Auth

Supabase Auth is a valid option but remains part of open [[ADR-Authentication-and-Google-OIDC|ADR-005]]. Do not operate two competing account/session systems.

## Storage

Object storage is appropriate for avatars, portfolio media, and uploaded CVs; PostgreSQL should hold metadata and ownership, not large binary payloads. Supabase Storage is a candidate, not finalized. Evaluate signed URL behavior, public vs private buckets, image transformation needs, region, size/type limits, malware scanning, deletion, retention, CDN/cache invalidation and provider contract. All writes are authorized by the backend or carefully designed policies. Never place CVs in a public bucket.

## Realtime

Not required for the first builder. Add Realtime only if live collaboration or immediate multi-session updates are a validated requirement. Ordinary editor autosave and notification polling can start with HTTP. If enabled, review channel authorization, RLS behavior, event payload minimization, disconnect/replay semantics, and rate limits.

## Database access and RLS

Initial design is a trusted backend using Npgsql. Do not expose a privileged Supabase key to the browser. If direct browser-to-Supabase access is later introduced, define table grants, RLS policies, JWT claims, and tests before release; keep privileged operations server-side. Backend authorization remains mandatory.

## Decision rule

For each optional service, write a short ADR covering need, alternatives, data classification, security model, cost/limits, failure mode, and exit/migration strategy.
