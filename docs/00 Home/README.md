---
title: Foliora Documentation Home
aliases: [Foliora, Documentation Home]
status: living specification
updated: 2026-10-31
tags: [foliora, index, documentation]
---

# Foliora Documentation Home

Foliora is a professional identity and portfolio publishing platform for people across industries. This Obsidian vault is the product and engineering reference for planning and future implementation. It describes a target design; it does not imply that a code repository or working product already exists.

## Start here

- [[Product Vision|Product vision and principles]]
- [[Product Requirements|Complete product requirements]]
- [[../01 Planning/MVP Scope|MVP boundaries and definition of done]]
- [[../01 Planning/Roadmap|Phased delivery roadmap]]
- [[../01 Planning/User Stories and Acceptance Criteria|User stories and testable acceptance criteria]]
- [[../10 Reference/AI Coding Assistant Instructions|Instructions for Cline and other coding agents]]
- [[Importing Into Obsidian|Import this vault into Obsidian]]

## Product and planning

- [[../00 Home/Product Vision|Product Vision]]
- [[../00 Home/Product Requirements|Product Requirements Document]]
- [[../01 Planning/MVP Scope|MVP Scope]]
- [[../01 Planning/Roadmap|Roadmap]]
- [[../01 Planning/User Stories and Acceptance Criteria|User Stories and Acceptance Criteria]]

## Architecture and implementation

- [[../02 Architecture/System Context and Components|System context and components]]
- [[../02 Architecture/Module Boundaries|Modular monolith module boundaries]]
- [[../02 Architecture/Engineering Principles|OOP, SOLID, DRY, KISS, YAGNI and clean architecture]]
- [[../02 Architecture/Architecture Decision Records|Decision register]]
- [[../03 Database/Database Architecture and ERD|Supabase PostgreSQL architecture and ERD]]
- [[../03 Database/Data Dictionary|Data dictionary]]
- [[../04 Portfolio Builder/Builder Architecture|Portfolio builder architecture]]
- [[../04 Portfolio Builder/Block Configuration Schema|Block JSON schema and examples]]
- [[../04 Portfolio Builder/Publishing Workflow|Draft, snapshot and publishing workflow]]
- [[../05 Backend/API Conventions and Endpoint Catalog|API conventions and endpoint catalog]]
- [[../05 Backend/Backend Conventions|Backend coding conventions]]
- [[../06 Frontend/Frontend Architecture|React frontend architecture]]
- [[../07 Trust/Analytics and Privacy|Analytics and privacy]]
- [[../07 Trust/Feedback, Moderation and Notifications|Feedback, moderation and notifications]]
- [[../07 Trust/Security and Non-Functional Requirements|Security and non-functional requirements]]
- [[../08 AI/Premium AI Architecture|Premium AI architecture and safeguards]]
- [[../09 Quality/Testing Strategy|Testing strategy]]
- [[../09 Quality/Deployment and CI-CD|Deployment, CI/CD and environments]]
- [[../10 Reference/Glossary|Glossary]]

## Decision snapshot

| Topic | State | Current direction |
|---|---|---|
| Product audience | Accepted | Professionals across industries; not developer-only |
| Frontend | Accepted | React + TypeScript |
| Backend | Accepted | ASP.NET Core + C# |
| Architecture | Accepted | Modular monolith |
| Database | Accepted | Supabase-hosted PostgreSQL; EF Core + Npgsql from the backend |
| Builder | Accepted product direction | Full drag-and-drop block editor; controlled, schema-validated blocks |
| AI | Accepted product direction | Premium, optional, user-reviewed assistance |
| Auth integration | Open ADR | Decide between backend-owned identity and Supabase Auth before implementation |
| Supabase Storage / Realtime | Optional proposal | Use only when concrete requirements justify them |

Accepted does not mean implemented. See [[../02 Architecture/Architecture Decision Records|the decision register]] and linked ADRs for rationale and unresolved items.

## How to read the vault

Each note has YAML frontmatter. Obsidian links use vault-relative paths so that duplicate note names can be resolved unambiguously. Mermaid diagrams are included as source blocks. Treat notes marked `proposed` or `open` as recommendations or questions, not approved implementation instructions.

For a quick import, unzip the bundle and open the extracted `Foliora-Obsidian` folder as a vault. See [[Importing Into Obsidian|the import note]].

## Scope and source of truth

This specification captures the supplied product direction and fills gaps with explicitly labeled proposals. It is not a substitute for implementation-specific decisions, threat modeling, provider contracts, accessibility review, or operational validation. When an approved decision changes, update the relevant ADR and affected notes together.
