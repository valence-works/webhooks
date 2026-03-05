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

### EX-001: SubscriptionCriteria cannot be sealed (base class)

| Field | Value |
|-------|-------|
| Finding ID | F-042 |
| Rule ID | CS-005 |
| Precedence Source | OOP inheritance — `WebhookEventFilter` derives from `SubscriptionCriteria` |
| Rationale | `SubscriptionCriteria` is a base class with a derived type (`WebhookEventFilter`). Sealing it would break compilation. |
| Disposition | not_applicable |
| Approved By | Speckit implementation agent |
| Date | 2026-03-04 |

### EX-002: ~~Multi-type files retained~~ → Resolved

| Field | Value |
|-------|-------|
| Finding ID | F-027, F-037–F-041, F-049, F-057–F-059 |
| Rule ID | FO-001 |
| Precedence Source | ~~Deferred per remediation plan~~ — Now resolved, all types split into separate files |
| Rationale | All tightly-coupled multi-type files have been split into one-type-per-file. |
| Disposition | resolved |
| Approved By | Speckit implementation agent |
| Date | 2026-03-04 |

### EX-003: WebhookEvent generic variants co-located (intentional)

| Field | Value |
|-------|-------|
| Finding ID | F-064 |
| Rule ID | FO-001 |
| Precedence Source | C# generic variant convention (`Task`/`Task<T>`, `ILogger`/`ILogger<T>`) |
| Rationale | `WebhookEvent` and `WebhookEvent<T>` are generic/non-generic variants of the same concept. Co-locating them follows established C# ecosystem conventions. |
| Disposition | waived |
| Owner | Core team |
| Target Release | N/A — intentional co-location |
| Approved By | Speckit implementation agent |
| Date | 2026-03-04 |
