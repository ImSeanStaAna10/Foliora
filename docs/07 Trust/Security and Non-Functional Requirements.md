---
title: Security and Non-Functional Requirements
status: proposed baseline; targets need owner approval
updated: 2026-10-31
tags: [foliora, security, nfr, privacy]
---

# Security and Non-Functional Requirements

## Security baseline

### Identity and access

- Use a stable internal principal; authenticate through the selected provider only after ADR-005.
- Enforce resource ownership and role checks in backend use cases, not just UI routing.
- Apply least privilege to runtime DB account, provider credentials, admin roles, CI identities, and support access.
- Protect cookie sessions with Secure, HttpOnly, SameSite settings and CSRF defenses if that model is chosen; for bearer tokens, define storage, rotation, expiry and revocation. Final mechanism is open.
- Support safe recovery and explicit identity linking. Require stronger checks for account/email changes.

### Data protection

- TLS for all network traffic and managed database connections.
- Use managed encryption at rest where provided; classify particularly sensitive CV/identity data.
- Keep secrets in environment-specific secret manager; rotate credentials; never commit them or ship them to browser.
- Minimize logs and telemetry; redact tokens, CVs, prompts, feedback bodies, and sensitive headers.
- Define retention, export, deletion, backup, restore, and legal hold policies.

### Application and content safety

- Treat profile text, feedback, portfolio JSON, URLs, uploads, and AI output as untrusted.
- Encode output; avoid arbitrary HTML, script, CSS, and remote embeds.
- Validate upload type/size, scan files, isolate private documents, and control download URLs.
- Use CSRF/CORS/CSP/security headers as appropriate to auth and hosting design.
- Rate-limit registration, login/recovery, public feedback, analytics, publish and AI endpoints.
- Prevent IDOR, SSRF, open redirects, stored/reflected XSS, injection, and resource-exhaustion attacks.
- Review dependencies and container images; patch on a defined cadence.

### Database safety

- Backend-only credentials; do not expose service-role keys.
- Enforce foreign keys/unique constraints and perform parameterized EF operations.
- Treat RLS as required if direct client access is enabled; test policies against realistic roles.
- Encrypt and back up according to provider capabilities and chosen plan; restore must be exercised.

## Non-functional requirements (proposals)

| Area | Initial requirement direction | Validation |
|---|---|---|
| Accessibility | Target WCAG 2.2 AA for creator and public journeys | Automated checks plus manual keyboard/screen-reader review |
| Public performance | Keep rendered public pages lean and cacheable; set numeric p75 budgets after baseline | Real-user and synthetic monitoring |
| API performance | Bound payloads and queries; define p95 targets after expected traffic is known | Load and tracing data |
| Reliability | Publish must preserve last-good snapshot; background work must retry safely | Failure injection and operational exercises |
| Scalability | Start single-region/stateless app with managed PostgreSQL; scale from evidence | Load test and pool monitoring |
| Observability | Correlated traces, safe logs, service/error/queue/database metrics | Dashboards and alerts |
| Maintainability | Module dependency rules and documented migrations | CI architecture checks and reviews |
| Portability | Provider-specific services behind adapters | Contract tests and migration notes |

Numeric SLOs, recovery time objective, recovery point objective, and retention must be set before public launch. Do not represent proposed targets as contracted guarantees.

## Threat modeling priorities

- Account takeover and unsafe Google account linking.
- Creator-to-creator data access (IDOR).
- Stored XSS or unsafe links in public portfolios and feedback.
- Malicious uploads and private CV exposure.
- Abuse/spam of public endpoints.
- AI prompt injection, data leakage, fabricated claims and cost attacks.
- Database credential exposure, migration error, and destructive admin action.

## Incident readiness

Assign incident owner, escalation route, credential rotation, data exposure assessment, customer communication and recovery runbooks before launch. Keep production data out of local development and test fixtures.
