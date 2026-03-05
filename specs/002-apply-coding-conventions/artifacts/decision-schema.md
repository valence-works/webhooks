# RemediationDecision Capture Schema

**Entity**: `RemediationDecision` (per [data-model.md](../data-model.md))  
**Created**: 2026-03-04

## Purpose

Define the standard format for recording the final disposition and action plan for each compliance finding.

## Schema

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `decisionId` | string | Yes | Unique identifier (D-NNN) |
| `findingId` | string | Yes | FK → `ComplianceFinding.findingId` |
| `disposition` | enum | Yes | `resolved`, `deferred`, `waived`, `not_applicable` |
| `rationale` | string | Yes | Always required — explains the disposition |
| `owner` | string | Conditional | Required when `disposition` is `deferred` or `waived` |
| `targetRelease` | string | Conditional | Required when `disposition` is `deferred` or `waived` |
| `approval` | string | No | Reviewer identifier who approved the decision |
| `changeRef` | string | No | Reference to remediation change (file, commit, or PR) |

## Validation Rules

1. `decisionId` must be unique within the feature.
2. `findingId` must reference a valid finding in `finding-register.md`.
3. `rationale` is always required.
4. `owner` and `targetRelease` are required when `disposition` is `deferred` or `waived`.
5. Every finding must have exactly one corresponding decision.

## Decision Record Template

```markdown
### D-NNN: [Short description]

| Field | Value |
|-------|-------|
| Finding ID | F-NNN |
| Disposition | resolved / deferred / waived / not_applicable |
| Rationale | [Why this disposition was chosen] |
| Owner | [Who owns, if deferred/waived] |
| Target Release | [When to revisit, if deferred/waived] |
| Approval | [Reviewer ID] |
| Change Ref | [File/commit reference, if resolved] |
```

## Disposition Transitions

- Initial assignment: `resolved` | `deferred` | `waived` | `not_applicable`
- `deferred` → `resolved` in a future review cycle
- `waived` → `resolved` if policy changes
