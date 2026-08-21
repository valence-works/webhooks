# Remediation Plan

**Feature**: `002-apply-coding-conventions`  
**Created**: 2026-03-04  
**Input**: [finding-register.md](finding-register.md)

## Remediation Strategy

Apply safe, behavior-preserving convention fixes. Prioritize by:
1. **High-priority applicable** findings first (constitution-aligned)
2. Group by change type (mechanical fixes that can be batched)
3. **Defer** only findings that require breaking API changes or deeper refactoring
4. **Waive** only findings with explicit ADR/architecture precedence

## Remediation Batches

### Batch 1: Add `sealed` modifier (CS-005) — SAFE

**Risk**: None — adding `sealed` to classes with no derived types is behavior-preserving.  
**Scope**: All non-sealed classes/records across src/, tests/, samples/  
**Findings**: F-002, F-005, F-010, F-012, F-014, F-017, F-019, F-021, F-028, F-031, F-033, F-036, F-042, F-043, F-044, F-050 (5 strategies), F-053, F-055 (5 options), F-060, F-062 (2 records), F-065 (24 test classes), F-068, F-071

### Batch 2: Rename underscore-prefixed private fields (NC-007) — SAFE

**Risk**: Low — rename private fields within same class; no public API change.  
**Scope**: DefaultWebhookEventBroadcaster, ChannelBackgroundTaskProcessor, EventGenerator, SequenceHandler  
**Findings**: F-001, F-004, F-066, F-067

### Batch 3: Add `readonly` modifier to applicable fields (CS-006) — SAFE

**Risk**: None — compile-time enforcement only.  
**Scope**: DefaultWebhookEventBroadcaster, ChannelBackgroundTaskProcessor  
**Findings**: F-003, F-009

### Batch 4: Fix malformed XML docs (XD-005) — SAFE

**Risk**: None — documentation change only.  
**Scope**: 6 files with bare `///` comments that need `<summary>` wrapping  
**Findings**: F-024, F-035, F-046, F-054, F-056, F-063

### Batch 5: Add missing XML docs on public API (XD-001) — SAFE

**Risk**: None — documentation addition only.  
**Scope**: Contracts, services, models, options, strategies, extensions  
**Findings**: F-007, F-011, F-013, F-016, F-018, F-020, F-022, F-023, F-025, F-026, F-029, F-032, F-034, F-045, F-051, F-061, F-069, F-072

**Note**: This is the largest batch. Prioritize contracts and public-facing services first; internal/helper types can receive minimal docs.

## Deferred Items

### Batch D1: Rename async methods (NC-012) — DEFERRED (Breaking Change)

**Risk**: HIGH — Renaming `EnqueueWork` to `EnqueueWorkAsync` and `Wait` to `WaitAsync` would be breaking API changes for any consumer of `IBackgroundTaskScheduler` or `IBackgroundTaskProcessor`.  
**Findings**: F-006, F-015, F-047  
**Decision**: Defer to a major version bump or coordinated breaking-change release.

### Batch D2: Add CancellationToken to interface methods (CT-001) — DEFERRED (Breaking Change)

**Risk**: HIGH — Adding parameters to `IBackgroundTaskProcessor.StartAsync/StopAsync` would break all implementors.  
**Findings**: F-008, F-048  
**Decision**: Defer to a major version bump.

### Batch D3: Propagate CancellationToken (CT-002) — PARTIAL

**Risk**: Medium — Some propagation is safe (internal calls), some requires interface changes.  
**Findings**: F-030, F-052, F-070  
**Decision**: Apply where possible without interface changes. Defer interface-dependent portions.

### Batch D4: Split multiple types per file (FO-001) — DEFERRED

**Risk**: Medium — File splits are safe but create noisy diffs and complicate git blame history.  
**Findings**: F-027, F-037, F-038, F-039, F-040, F-041, F-049, F-057, F-058, F-059, F-064  
**Decision**: Defer to a dedicated housekeeping pass. Document as accepted debt with contextual exceptions for tightly-related types.

## Execution Order

1. Batch 1 (sealed) — mechanical, very low risk
2. Batch 2 (field rename) — low risk, localized
3. Batch 3 (readonly) — zero risk
4. Batch 4 (fix XML) — zero risk
5. Batch 5 (add XML) — zero risk, largest effort
6. Build + test validation gate
7. Record deferred/waived decisions
