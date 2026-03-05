# Open Items: Deferred & Waived Findings

**Feature**: `001-apply-coding-conventions`  
**Created**: 2026-03-04  
**Updated**: 2026-03-04  
**Source**: [decision-register.md](decision-register.md)

## Summary

| Category | Count | Status |
|----------|-------|--------|
| ~~Deferred — Breaking API changes~~ | ~~5 findings~~ | ✅ Resolved |
| ~~Deferred — CancellationToken propagation~~ | ~~3 findings~~ | ✅ Resolved |
| ~~Waived — Multi-type file splits~~ | ~~10 findings~~ | ✅ Resolved |
| Waived — Generic variant co-location | 1 finding | Intentional — waived |
| **Total Open** | **1 finding** | |

---

## Closed Items (Previously Deferred/Waived — Now Resolved)

### ~~OI-001: Async Method Naming (NC-012)~~ ✅ Resolved

**Findings**: F-006, F-015, F-047 — All resolved. Breaking changes authorized and applied.

### ~~OI-002: CancellationToken on Interface Methods (CT-001)~~ ✅ Resolved

**Findings**: F-008, F-048 — All resolved. Breaking changes authorized and applied.

### ~~OI-003: CancellationToken Propagation (CT-002)~~ ✅ Resolved

**Findings**: F-030, F-052, F-070 — All resolved. Propagation applied end-to-end.

### ~~OI-004: Multi-Type File Organization (FO-001)~~ ✅ Resolved (10 of 11)

**Findings**: F-027, F-037–F-041, F-049, F-057–F-059 — All resolved. Types extracted to individual files.

---

## Remaining Waived Items

### OI-005: WebhookEvent Generic Variant Co-Location (FO-001)

**Finding**: F-064  
**Owner**: Core team  
**Target Release**: N/A — intentional co-location  
**Rationale**: `WebhookEvent` and `WebhookEvent<T>` are generic/non-generic variants of the same concept. Co-locating them in a single file follows established C# ecosystem conventions (cf. `Task`/`Task<T>`, `ILogger`/`ILogger<T>`).

**Action Required**: None — this is an intentional design choice, not a deferral.
