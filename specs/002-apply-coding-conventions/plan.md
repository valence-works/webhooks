# Implementation Plan: Codebase Coding Conventions Review

**Branch**: `[002-apply-coding-conventions]` | **Date**: 2026-03-04 | **Spec**: `/specs/002-apply-coding-conventions/spec.md`
**Input**: Feature specification from `/specs/002-apply-coding-conventions/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

Review in-scope repository areas (`src/`, `tests/`, and manually-authored C# in `samples/`) against `docs/coding-conventions.md`, classify findings, apply only safe and applicable remediation, and produce a traceable compliance summary with explicit dispositions. The implementation approach is documentation- and process-centric: define repeatable review artifacts, clear finding/disposition data shape, deterministic precedence with ADRs, and verification gates (build + impacted tests).

## Technical Context

**Language/Version**: C# on .NET (`src` multi-targets `net6.0;net7.0;net8.0;net9.0;net10.0`; tests on `net8.0`; samples on `net10.0`)  
**Primary Dependencies**: `Microsoft.Extensions.*` hosting/http/logging, Polly integration via `Microsoft.Extensions.Http.Polly`, `JetBrains.Annotations`, xUnit test stack  
**Storage**: N/A (file-based planning and reporting artifacts only)  
**Testing**: `dotnet test` with xUnit (`tests/Webhooks.Core.Tests`)  
**Target Platform**: Cross-platform .NET library + ASP.NET sample applications (developer/CI environments)
**Project Type**: Library-first .NET solution with sample web apps and automated tests  
**Performance Goals**: N/A for this feature (process/documentation consistency feature, not runtime throughput change)  
**Constraints**: Must not introduce behavior changes; must exclude generated/output/vendor artifacts; must preserve backward compatibility and follow constitution gates  
**Scale/Scope**: In-scope review of `src/`, `tests/`, and manually-authored C# in `samples/` with full finding disposition coverage

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Spec-first gate**: PASS — `spec.md` contains clarified requirements, edge cases, and measurable outcomes.
- **Contract boundary gate**: PASS — this feature defines ownership boundaries for review, disposition, exception handling, and verification responsibilities via artifacts/contracts.
- **Simplicity/compatibility gate**: PASS — approach is additive/process-oriented with no intentional API/runtime behavioral changes.
- **Verification gate**: PASS — plan requires successful build and passing impacted automated tests for remediation completion.

**Post-Design Re-check (after Phase 1 artifacts)**: PASS — research, data model, contract artifacts, and quickstart preserve constitution principles and do not introduce unjustified complexity.

## Project Structure

### Documentation (this feature)

```text
specs/002-apply-coding-conventions/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
src/
├── Webhooks.Core/
└── WebhooksCore.Shared/

tests/
└── Webhooks.Core.Tests/

samples/
├── WebhookEvents.Generator.Web/
├── WebhooksEvents.Receiver.Web/
└── WebhookEvents.Shared/

docs/
├── coding-conventions.md
├── architecture/
└── adr/
```

**Structure Decision**: Use the existing library-first solution structure (`src` + `tests`) with sample applications (`samples`) as in-scope only for manually-authored C# convention review. Generated/build artifacts (`bin`, `obj`) and non-applicable files stay out of remediation scope.

## Complexity Tracking

No constitution violations identified; complexity tracking not required.
