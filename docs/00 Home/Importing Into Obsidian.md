---
title: Importing Into Obsidian
status: user guide
updated: 2026-10-31
tags: [foliora, obsidian, import]
---

# Importing Into Obsidian

1. Download `Foliora-Obsidian.zip` and extract it to a folder where you keep your Obsidian vaults.
2. Open Obsidian and choose **Open folder as vault** (or **Manage vaults → Open folder as vault**, depending on your version).
3. Select the extracted `Foliora-Obsidian` folder. The top-level folder contains the note directories.
4. Open `00 Home/README.md` to navigate the documentation.
5. Optional: enable the **Templates** and **Backlinks** core plugins, or install a Mermaid renderer only if you want additional diagram controls. Obsidian supports Mermaid code blocks in notes.

The vault uses YAML frontmatter and relative Obsidian wikilinks. No community plugin is required. Keep the folder name and internal paths intact so links resolve. You may copy individual `.md` files into another vault, but linked references work only when their linked notes are also copied at the same paths.

All decisions marked open or proposed need review before implementation. The accepted database direction is Supabase PostgreSQL with backend EF Core/Npgsql. Authentication remains open in [[../02 Architecture/ADR-Authentication-and-Google-OIDC|ADR-005]].
