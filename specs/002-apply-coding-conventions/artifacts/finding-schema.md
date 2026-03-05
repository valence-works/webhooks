# ComplianceFinding Capture Schema

**Entity**: `ComplianceFinding` (per [data-model.md](../data-model.md))  
**Created**: 2026-03-04

## Purpose

Define the standard format for recording individual compliance review findings. Every in-scope rule evaluation produces one finding per scoped code location.

## Schema

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `findingId` | string | Yes | Unique identifier (F-NNN) |
| `ruleId` | string | Yes | FK → `convention-rules.md` rule ID |
| `location` | string | Yes | Repository-relative file path |
| `status` | enum | Yes | `compliant`, `non_compliant`, `not_applicable` |
| `priority` | enum | Yes | `high`, `medium`, `low` |
| `applicability` | enum | Yes | `applicable`, `not_applicable`, `contextual` |
| `impact` | string | Conditional | Required when `status=non_compliant`; describes effect of the deviation |
| `evidence` | string | No | Code excerpt, line reference, or description supporting the finding |
| `rationale` | string | No | Reasoning for status classification |

## Validation Rules

1. `findingId` must be unique within the feature.
2. `ruleId` must reference a valid rule in `convention-rules.md`.
3. `impact` is mandatory when `status=non_compliant`.
4. `location` must be a repository-relative path within scope (see `scope-manifest.md`).

## Finding Record Template

```markdown
### F-NNN: [Short description]

| Field | Value |
|-------|-------|
| Rule ID | XX-NNN |
| Location | path/to/file.cs |
| Status | compliant / non_compliant / not_applicable |
| Priority | high / medium / low |
| Applicability | applicable / not_applicable / contextual |
| Impact | [Description of deviation impact, if non_compliant] |
| Evidence | [Code excerpt or line ref] |
| Rationale | [Why this status was assigned] |
```

## Status Transitions

- `non_compliant` → `compliant` (after resolved remediation)
- `non_compliant` → `not_applicable` (when exception approved with rationale)
- `compliant` is terminal for the current review cycle.
