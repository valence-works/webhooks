# Validation Gate Checklist

**Feature**: `001-apply-coding-conventions`  
**Created**: 2026-03-04  
**Rationale**: FR-007, FR-010, Research Decision 2

## Purpose

Track build and impacted test validation outcomes. A remediation batch is complete only when both gates pass.

## Gate Criteria

| Gate | Requirement | Status |
|------|-------------|--------|
| Build | Solution builds successfully (`dotnet build Webhooks.sln`) | ⬜ Pending |
| Impacted Tests | All existing impacted automated tests pass (`dotnet test`) | ⬜ Pending |

## Validation Protocol

1. **Scope**: Verify at smallest relevant scope first (changed project), then broader solution.
2. **Build**: Run `dotnet build` on affected projects, then full solution.
3. **Tests**: Run `dotnet test` on `tests/Webhooks.Core.Tests/` (the impacted test project).
4. **Record**: Log pass/fail with timestamp, command output summary, and any failures.

## Validation Records

_Populated during Phase 4 (US2) — T022 and T023_

### Build Validation

| Run | Date | Command | Result | Notes |
|-----|------|---------|--------|-------|
| _TBD_ | | `dotnet build Webhooks.sln` | | |

### Test Validation

| Run | Date | Command | Result | Tests Passed | Tests Failed | Notes |
|-----|------|---------|--------|--------------|--------------|-------|
| _TBD_ | | `dotnet test tests/Webhooks.Core.Tests/` | | | | |

## Completion Criteria

- ✅ Build passed: solution compiles without errors
- ✅ Tests passed: all existing impacted automated tests pass
- Both gates MUST pass before remediation is considered complete (per FR-010)
