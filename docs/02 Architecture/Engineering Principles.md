---
title: Engineering Principles
status: accepted engineering guidance
updated: 2026-10-31
tags: [foliora, engineering, principles]
---

# Engineering Principles

Apply these principles pragmatically. They guide trade-offs; they are not a reason to add layers or abstractions without a use case.

## OOP and domain modeling

- Model behavior near the data and rules it protects when doing so makes invariants clearer.
- Use encapsulation to prevent invalid portfolio states, ownership changes, or publication transitions.
- Prefer composition and small cohesive types over deep inheritance.
- Use value objects for concepts with meaningful invariants (for example, validated slug or block ID) where they simplify rules.
- Keep persistence entities and API DTOs separate when their lifecycles or exposure differ.
- Use immutable records/value objects for published snapshots; snapshots must not be edited in place.

## SOLID

- **Single responsibility:** one module/class changes for one coherent reason.
- **Open/closed:** extend supported block types through explicit registration/schema versioning, not uncontrolled conditionals across the application.
- **Liskov substitution:** provider adapters honor the same timeout, error, and data-handling contract.
- **Interface segregation:** use narrow interfaces such as `IEmailSender` or `IAiProvider`, not broad service locators.
- **Dependency inversion:** policy code depends on abstractions; infrastructure details implement them.

## DRY

Share stable rules and repeated behavior when there is one underlying concept. Do not merge code merely because it looks similar; profile validation and public snapshot validation may have different policies. Centralize API error shape, slug validation, block schema rules, and authorization helpers only when they are genuinely common.

## KISS

Use one deployable backend, one PostgreSQL platform, and a small set of supported block types. Prefer explicit flows, boring persistence, and clear failure states over event buses, generic frameworks, and speculative extensibility.

## YAGNI

Do not implement billing automation, multi-region, arbitrary plugins, custom domains, microservices, event sourcing, or AI workflows until an approved requirement needs them. Preserve essential seams for identity, storage, email, and AI without building provider marketplaces.

## Separation of concerns and clean architecture

- HTTP transport handles binding, authentication middleware, response mapping, and status codes.
- Application use cases coordinate policy and module contracts.
- Domain types guard important business invariants.
- Infrastructure implements EF Core/Npgsql and provider adapters.
- React components render UI; hooks/application state coordinate flows; API clients own transport concerns.

This separation may be logical namespaces rather than a large project hierarchy. Make a boundary explicit when it lowers coupling or makes testing easier.

## Practical rules of thumb

- Keep controllers/endpoints thin and avoid leaking EF entities.
- Validate at trust boundaries; the browser validation improves usability but is not security.
- Use async I/O and cancellation for network/database operations.
- Prefer explicit transactions around publish and related state transitions.
- Make retries safe through idempotency keys or unique event identifiers.
- Log IDs and outcomes, not secrets, full CV text, or sensitive prompts.
- Add a dependency only for a concrete need and document why.

See [[Module Boundaries|module dependency rules]] and [[../05 Backend/Backend Conventions|backend conventions]].
