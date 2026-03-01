# Implementation Plan: Baseline Webhook Broadcasting Behavior

**Branch**: `001-initial-src-spec` | **Date**: 2026-03-01 | **Spec**: `/specs/001-initial-src-spec/spec.md`
**Input**: Feature specification from `/specs/001-initial-src-spec/spec.md`

## Summary

Implement a contract-driven refinement of Webhooks Core using a two-plane architecture (dispatch plane + invoke plane) so dispatcher selection policy, dispatcher handoff, endpoint invocation, retry semantics, and observability are explicit and testable. The implementation preserves direct HTTP behavior by default, adds pluggable dispatcher/middleware/strategy contracts, and keeps queued queue/worker execution in extension modules rather than Webhooks Core runtime.

## Technical Context

**Language/Version**: C# with multi-targeted .NET (`net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`)  
**Primary Dependencies**: `Microsoft.Extensions.Hosting.Abstractions`, `Microsoft.Extensions.Http`, `Microsoft.Extensions.Http.Polly`, `Microsoft.Extensions.Logging`, `Polly`/`Polly.Extensions.Http`  
**Storage**: In-memory runtime configuration/state in core; queue/outbox infrastructure only when provided by dispatcher extension modules  
**Testing**: `dotnet test` (unit/integration/conformance coverage in solution test projects)  
**Target Platform**: Cross-platform .NET host applications (Linux/macOS/Windows)
**Project Type**: Reusable .NET library  
**Performance Goals**: Deterministic coordinator invocation behavior and reliable endpoint invocation outcomes with no cross-sink blocking on single-sink failure  
**Constraints**: Preserve backward-compatible defaults, avoid transport-specific orchestration coupling, fail startup on invalid configuration  
**Scale/Scope**: Webhook broadcast and delivery orchestration in `src/Webhooks.Core` with shared models in `src/WebhooksCore.Shared`

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Spec-first gate: `spec.md` includes clarified requirements, edge cases, and measurable outcomes.
- Contract boundary gate: ownership and extension points are explicit (orchestration vs transport/invocation).
- Simplicity/compatibility gate: defaults and backward-compatibility impact are documented.
- Verification gate: plan, quickstart, and tasks include project-level build/test validation steps.

Status before Phase 0: PASS

## Project Structure

### Documentation (this feature)

```text
specs/001-initial-src-spec/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── dispatcher-contract.md
│   ├── middleware-contracts.md
│   └── configuration-validation-contract.md
├── implementation-implications.md
├── checklists/
│   └── requirements.md
└── tasks.md
```

### Source Code (repository root)

```text
src/
├── Webhooks.Core/
│   ├── Contracts/
│   ├── Extensions/
│   ├── HostedServices/
│   ├── Models/
│   ├── Options/
│   ├── Serialization/
│   ├── Services/
│   ├── SinkProviders/
│   └── Strategies/
└── WebhooksCore.Shared/
    └── Models/

samples/
├── WebhookEvents.Generator.Web/
└── WebhooksEvents.Receiver.Web/
```

**Structure Decision**: Keep the existing single-library architecture and implement refinements primarily in `src/Webhooks.Core` contracts/options/services/strategies, with shared model adjustments only when required in `src/WebhooksCore.Shared`.

## Phase 0: Research Output

- Dispatch plane vs invoke plane boundaries finalized.
- Invoker-outcome-first delivery semantics confirmed.
- Retry scope confirmed at invoke boundary with host-configurable transient detection.
- Overflow policy defaults/overrides and deduplication default-disabled behavior confirmed.
- Single-dispatcher-per-sink selection policy confirmed.

See: `/specs/001-initial-src-spec/research.md`

## Phase 1: Design Output

- Data model defined for `Delivery Envelope`, `Delivery Attempt`, `Delivery Result`, and policy entities.
- Contracts defined for dispatcher/coordinator behavior, middleware boundaries, and configuration validation.
- Quickstart captures build/test and verification outcomes for implementation handoff.

See:
- `/specs/001-initial-src-spec/data-model.md`
- `/specs/001-initial-src-spec/contracts/`
- `/specs/001-initial-src-spec/quickstart.md`

## Post-Design Constitution Check

- Spec-first gate: PASS (`spec.md` + clarifications + measurable outcomes present).
- Contract boundary gate: PASS (coordinator/dispatcher/invoker ownership and module queue boundaries explicit).
- Simplicity/compatibility gate: PASS (default behaviors explicit; no forced breaking runtime model).
- Verification gate: PASS (`quickstart.md` and `tasks.md` provide build/test/conformance path).

Status after Phase 1: PASS

## Phase 2: Task Planning Approach

Task generation (`/speckit.tasks`) is grouped by:

1. Setup + foundational contracts/options/DI
2. US1 event subscription routing
3. US2 payload predicate routing
4. US3 dispatcher/middleware pluggability and queue-boundary integrations
5. Polish/cross-cutting validation and scope guard

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |
