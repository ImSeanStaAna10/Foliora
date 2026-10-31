---
title: Analytics and Privacy
status: proposed privacy-by-design baseline
updated: 2026-10-31
tags: [foliora, privacy, analytics]
---

# Analytics and Privacy

## Product promise

Foliora reports aggregate activity to portfolio owners. Anonymous activity must not be described as a named recruiter, employer, or identifiable visitor unless the platform later adds a separate disclosed and authenticated feature.

## Minimal event model (proposed)

- Event types: portfolio view and selected coarse interactions only if useful.
- Fields: portfolio ID, event time, published snapshot ID or version, coarse path/section key, coarse device category, referrer category, short-lived pseudonymous session hash.
- Do not persist raw IP addresses, full user-agent strings, page form content, or cross-site identifiers in analytics events.
- If transient network signals are used for rate limiting or bot filtering, process in memory or in short-lived security logs with separate retention and access policy.

## Estimated unique sessions

Unique counts are estimates within an explicitly described time window, based on a first-party short-lived identifier or similarly minimized method. Account for cookie consent rules, blocked storage, shared devices, bot traffic and reset windows. Label metric as estimated; never imply person-level identity.

## Privacy controls

- Present clear notice and consent where required by jurisdiction and tracking method.
- Respect browser privacy signals and user choices as required by policy.
- Minimize collection, retention, access, and event granularity.
- Suppress detailed breakdowns for low counts where re-identification risk exists.
- Define analytics opt-out if required or chosen by product policy.
- Provide privacy notice covering public-page visits and creator analytics.

## Retention and access

Exact retention is an open operational/legal decision. Propose short-lived raw events with daily aggregates retained only as long as product value and policy require. Restrict owner queries to their portfolios and aggregate only. Admin access should be limited, audited, and avoid curiosity access to individual visitor records.

## Abuse and quality

Rate limit event intake; validate referer/origin as a signal but do not treat it as an identity proof. Filter known bots conservatively and disclose methodology. Monitor event volume spikes and avoid letting analytics failures slow public page rendering.

## Open decisions

- Jurisdictions and consent requirements.
- Cookie/local storage behavior and notice UX.
- Raw event and aggregate retention periods.
- Public page analytics opt-out policy.
- Bot detection method and operational logs separation.

See [[Security and Non-Functional Requirements|security/privacy controls]] and [[../05 Backend/API Conventions and Endpoint Catalog|analytics endpoints]].
