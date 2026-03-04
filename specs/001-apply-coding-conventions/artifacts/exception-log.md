# Exception Decision Log

**Feature**: `001-apply-coding-conventions`  
**Created**: 2026-03-04  
**Rationale**: FR-005, FR-011, Research Decision 3

## Purpose

This log records findings that conflict with accepted ADR/architecture decisions or where conventions are explicitly waived with rationale and reviewer approval. Each exception must include the finding reference, conflicting rule, precedence source, rationale, and approval.

## Exception Format

| Field | Required | Description |
|-------|----------|-------------|
| Exception ID | Yes | Unique identifier (EX-NNN) |
| Finding ID | Yes | Reference to finding in finding-register.md |
| Rule ID | Yes | Convention rule that triggered the finding |
| Precedence Source | Yes | ADR or architecture decision that takes precedence |
| Rationale | Yes | Why the exception is justified |
| Disposition | Yes | `waived` or `not_applicable` |
| Owner | Conditional | Required for `waived` (who owns future remediation if any) |
| Target Release | Conditional | Required for `waived` (when it may be revisited) |
| Approved By | Yes | Reviewer who approved the exception |
| Date | Yes | Date of exception decision |

---

## Exceptions

_Populated during Phase 4 (US2) — T020_

<!-- 
Template:

### EX-001: [Short description]

| Field | Value |
|-------|-------|
| Finding ID | F-NNN |
| Rule ID | XX-NNN |
| Precedence Source | docs/adr/NNNN-xxx.md |
| Rationale | [Why this exception is justified] |
| Disposition | waived / not_applicable |
| Owner | [Who owns] |
| Target Release | [When to revisit] |
| Approved By | [Reviewer] |
| Date | YYYY-MM-DD |
-->
