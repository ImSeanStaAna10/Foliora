---
title: Block Configuration Schema
status: proposed schema v1 example
updated: 2026-10-31
tags: [foliora, portfolio, json-schema]
---

# Block Configuration Schema

This is a concrete v1 shape example, not a final generated JSON Schema document. Implement an authoritative shared contract or equivalent client/server validators before release. Persist as PostgreSQL `jsonb` with a separate schema version.

## Document shape

```json
{
  "schemaVersion": 1,
  "templateKey": "professional-clean",
  "theme": {
    "colorScheme": "light",
    "accentColor": "#2F5D8C",
    "fontPreset": "modern-sans",
    "contentWidth": "wide"
  },
  "sections": [
    {
      "id": "sec_01J8HEROEXAMPLE",
      "type": "hero",
      "visible": true,
      "order": 1000,
      "content": {
        "headline": "Jordan Lee",
        "subheadline": "Operations leader focused on better customer experiences",
        "imageAssetId": null,
        "primaryAction": { "label": "Contact me", "href": "mailto:jordan@example.com" }
      },
      "style": { "alignment": "center", "density": "comfortable" }
    },
    {
      "id": "sec_01J8PROJECTEXAMPLE",
      "type": "projects",
      "visible": true,
      "order": 2000,
      "content": {
        "title": "Selected work",
        "source": "profile-projects",
        "itemIds": ["project-id-1"],
        "layout": "cards"
      },
      "style": { "columns": "responsive" }
    }
  ]
}
```

## Example JSON Schema excerpt

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://foliora.example/schemas/portfolio-v1.json",
  "type": "object",
  "additionalProperties": false,
  "required": ["schemaVersion", "templateKey", "theme", "sections"],
  "properties": {
    "schemaVersion": { "const": 1 },
    "templateKey": { "type": "string", "minLength": 1, "maxLength": 80 },
    "theme": { "$ref": "#/$defs/theme" },
    "sections": {
      "type": "array",
      "maxItems": 50,
      "items": { "$ref": "#/$defs/section" }
    }
  },
  "$defs": {
    "theme": {
      "type": "object",
      "additionalProperties": false,
      "properties": {
        "colorScheme": { "enum": ["light", "dark", "system"] },
        "accentColor": { "type": "string", "pattern": "^#[0-9A-Fa-f]{6}$" },
        "fontPreset": { "enum": ["modern-sans", "classic-serif", "humanist"] },
        "contentWidth": { "enum": ["standard", "wide"] }
      }
    },
    "section": {
      "type": "object",
      "required": ["id", "type", "visible", "order", "content"],
      "properties": {
        "id": { "type": "string", "maxLength": 64 },
        "type": { "enum": ["hero", "about", "skills", "projects", "experience", "education", "credentials", "gallery", "contact", "customText", "footer"] },
        "visible": { "type": "boolean" },
        "order": { "type": "integer", "minimum": 0 },
        "content": { "type": "object" },
        "style": { "type": "object" }
      }
    }
  }
}
```

Schema validators must also enforce unique section IDs, unique order or a deterministic tie-break, per-type content schemas, reference ownership, URL policy, serialized size limits, and allowed asset status. JSON Schema's basic shape alone does not enforce all cross-field/domain invariants.

## Block contract checklist

Each block type defines: `type`, `schemaVersion`, content fields and limits, style tokens, profile data source/reference policy, editor controls, renderer, empty state, accessibility behavior, public-field allowlist, and migration rules. Do not permit arbitrary component names, code, CSS, or component props from stored JSON.

## Versioning example

An incompatible Hero content change introduces document version 2 and an explicit migration from v1. Draft migration can occur on load/save with a backup. Published snapshots remain on their original version until republished or migrated through an approved operation. Keep schema tests and representative fixtures for each supported version.

See [[Builder Architecture|builder and renderer design]].
