# Implementation Plan: Wildcard Event-Type Matching

**Branch**: `003-wildcard-event-matching` | **Date**: 2026-03-05 | **Spec**: `/specs/003-wildcard-event-matching/spec.md`
**Input**: Feature specification from `/specs/003-wildcard-event-matching/spec.md`

## Summary

Introduce pluggable event-type matching in Webhooks Core by replacing hardcoded event-type equality with a host-replaceable matcher contract and a default wildcard-capable implementation. The baseline behavior adds support for `*` subscriptions, preserves exact case-sensitive literal matching, and handles invalid whitespace-padded subscription values with warning + runtime non-match semantics.

## Technical Context

**Language/Version**: C# with multi-targeted .NET (`net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`)  
**Primary Dependencies**: `Microsoft.Extensions.Hosting.Abstractions`, `Microsoft.Extensions.Http`, `Microsoft.Extensions.Logging`, `Microsoft.Extensions.Http.Polly`  
**Storage**: N/A (in-memory routing/matching evaluation only)  
**Testing**: `dotnet test` using xUnit (`Webhooks.Core.Tests`)  
**Target Platform**: Cross-platform .NET host applications (Linux/macOS/Windows)
**Project Type**: Reusable .NET library with sample host applications  
**Performance Goals**: No material routing throughput regression from current exact-match baseline; keep matching evaluation O(1) per subscription check  
**Constraints**: Preserve backward compatibility for literal subscriptions; wildcard and invalid-value behavior must match clarified spec; payload filtering behavior unchanged  
**Scale/Scope**: Changes limited to event subscription matching in `src/Webhooks.Core`, plus tests/docs/spec artifacts

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Spec-first gate: PASS (`spec.md` contains clarified requirements, edge cases, and measurable outcomes).
- Contract boundary gate: PASS (matching ownership remains in broadcaster orchestration with explicit replaceable matcher policy).
- Simplicity/compatibility gate: PASS (single default matcher extension point; exact literal behavior preserved).
- Verification gate: PASS (plan includes targeted and solution-level build/test validation in quickstart).

Status before Phase 0: PASS

## Project Structure

### Documentation (this feature)

```text
specs/003-wildcard-event-matching/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── event-type-matching-contract.md
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
│   ├── Models/
│   ├── Services/
│   └── Strategies/
└── WebhooksCore.Shared/
    └── Models/

tests/
└── Webhooks.Core.Tests/
    └── Routing/

samples/
├── WebhookEvents.Generator.Web/
└── WebhooksEvents.Receiver.Web/
```

**Structure Decision**: Keep existing single-library + tests structure and implement this feature inside `src/Webhooks.Core` contracts/strategies/services/DI registration, with routing tests in `tests/Webhooks.Core.Tests/Routing` and no schema changes to shared models.

## Phase 0: Research Output

- Confirmed wildcard semantics: `*` matches any incoming event type, including null/empty/whitespace.
- Confirmed invalid subscription semantics: leading/trailing whitespace in subscription event type is invalid, logged as warning, treated as runtime non-match.
- Confirmed pluggability boundary: event-type matching policy is host-replaceable and owned by broadcaster orchestration.
- Confirmed compatibility: literal matching remains case-sensitive exact; payload criteria behavior unchanged.

See: `/specs/003-wildcard-event-matching/research.md`

## Phase 1: Design Output

- Data model captures subscription event type normalization/validity states and matching outcomes.
- Contract document defines matcher behavior, ownership, defaults, override rules, and warning obligations.
- Quickstart defines focused verification workflow for routing behavior and docs alignment.

See:
- `/specs/003-wildcard-event-matching/data-model.md`
- `/specs/003-wildcard-event-matching/contracts/`
- `/specs/003-wildcard-event-matching/quickstart.md`

## Post-Design Constitution Check

- Spec-first gate: PASS (clarifications embedded and reflected in requirements/edge cases).
- Contract boundary gate: PASS (orchestration owner and extension points explicit in spec + contract artifact).
- Simplicity/compatibility gate: PASS (single matcher abstraction with default behavior and host override).
- Verification gate: PASS (quickstart includes build + targeted routing tests + optional sample smoke test).

Status after Phase 1: PASS

## Phase 2: Task Planning Approach

Task generation (`/speckit.tasks`) should be grouped by:

1. Contract and default strategy implementation
2. Broadcaster integration and DI registration
3. Routing and edge-case test coverage
4. Documentation alignment (README + sample expectation)
5. Polish: solution build/test validation and regression check

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |
