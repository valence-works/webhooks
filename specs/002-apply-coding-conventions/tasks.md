# Tasks: Codebase Coding Conventions Review

**Input**: Design documents from `/specs/002-apply-coding-conventions/`
**Prerequisites**: plan.md (required), spec.md (required), research.md, data-model.md, contracts/, quickstart.md

**Tests**: No new test-authoring tasks are included; this feature requires running existing impacted automated tests as validation gates.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Every task includes an exact file path for output or change target

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Establish review artifacts and scope controls used by all stories

- [x] T001 Create feature artifact folder index in `specs/002-apply-coding-conventions/artifacts/README.md`
- [x] T002 Create rule catalog extracted from conventions in `specs/002-apply-coding-conventions/artifacts/convention-rules.md`
- [x] T003 [P] Create scope include/exclude manifest in `specs/002-apply-coding-conventions/artifacts/scope-manifest.md`
- [x] T004 [P] Create exception decision log template in `specs/002-apply-coding-conventions/artifacts/exception-log.md`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Define the shared data/validation structures that all user stories depend on

**⚠️ CRITICAL**: No user story work begins until this phase is complete

- [x] T005 Define `ComplianceFinding` capture schema in `specs/002-apply-coding-conventions/artifacts/finding-schema.md`
- [x] T006 [P] Define `RemediationDecision` capture schema in `specs/002-apply-coding-conventions/artifacts/decision-schema.md`
- [x] T007 [P] Define build+tests validation gate checklist in `specs/002-apply-coding-conventions/artifacts/validation-gate.md`
- [x] T008 Define finding-to-change traceability matrix template in `specs/002-apply-coding-conventions/artifacts/traceability-matrix.md`
- [x] T009 Define summary assembly template aligned to contract in `specs/002-apply-coding-conventions/artifacts/compliance-summary.md`

**Checkpoint**: Foundation complete — user stories can proceed

---

## Phase 3: User Story 1 - Assess Convention Compliance (Priority: P1) 🎯 MVP

**Goal**: Produce a complete, prioritized, and traceable compliance finding set for in-scope areas

**Independent Test**: Confirm all in-scope areas (`src/`, `tests/`, authored C# in `samples/`) have findings recorded with status, applicability, priority, and rationale

- [x] T010 [P] [US1] Review `src/` conventions compliance and record findings in `specs/002-apply-coding-conventions/artifacts/findings-src.md`
- [x] T011 [P] [US1] Review `tests/` conventions compliance and record findings in `specs/002-apply-coding-conventions/artifacts/findings-tests.md`
- [x] T012 [P] [US1] Review authored C# in `samples/` and record findings in `specs/002-apply-coding-conventions/artifacts/findings-samples.md`
- [x] T013 [US1] Consolidate all findings into master register in `specs/002-apply-coding-conventions/artifacts/finding-register.md`
- [x] T014 [US1] Assign applicability, priority, and impact for each finding in `specs/002-apply-coding-conventions/artifacts/finding-register.md`
- [x] T015 [US1] Link each finding to source rule IDs in `specs/002-apply-coding-conventions/artifacts/traceability-matrix.md`

**Checkpoint**: User Story 1 is independently testable and provides MVP value

---

## Phase 4: User Story 2 - Apply Applicable Conventions (Priority: P2)

**Goal**: Apply safe convention remediations for applicable findings while preserving behavior

**Independent Test**: Verify applicable high-priority findings are remediated, exceptions are justified, and validation gates pass

- [x] T016 [US2] Create prioritized remediation plan from finding register in `specs/002-apply-coding-conventions/artifacts/remediation-plan.md`
- [x] T017 [P] [US2] Apply conventions remediations in `src/Webhooks.Core/**/*.cs` and log results in `specs/002-apply-coding-conventions/artifacts/remediation-log-src.md`
- [x] T018 [P] [US2] Apply conventions remediations in `tests/Webhooks.Core.Tests/**/*.cs` and log results in `specs/002-apply-coding-conventions/artifacts/remediation-log-tests.md`
- [x] T019 [P] [US2] Apply conventions remediations in `samples/**/*.cs` (authored files only) and log results in `specs/002-apply-coding-conventions/artifacts/remediation-log-samples.md`
- [x] T020 [US2] Record ADR/architecture precedence exceptions with rationale/approval in `specs/002-apply-coding-conventions/artifacts/exception-log.md`
- [x] T021 [US2] Update final per-finding dispositions and ownership targets in `specs/002-apply-coding-conventions/artifacts/decision-register.md`
- [x] T022 [US2] Capture build validation outcome in `specs/002-apply-coding-conventions/artifacts/validation-gate.md`
- [x] T023 [US2] Capture impacted automated test results in `specs/002-apply-coding-conventions/artifacts/validation-gate.md`

**Checkpoint**: User Story 2 remediations are complete and behavior-safety gate is satisfied

---

## Phase 5: User Story 3 - Validate and Sign Off Compliance Outcomes (Priority: P3)

**Goal**: Publish final contract-aligned compliance summary with complete dispositions and sign-off metadata

**Independent Test**: Verify summary includes all findings/dispositions, validation gate status, unresolved owner/target-release details, and reviewer sign-off

- [x] T024 [P] [US3] Assemble final compliance summary narrative in `specs/002-apply-coding-conventions/artifacts/compliance-summary.md`
- [x] T025 [US3] Produce contract-aligned summary instance in `specs/002-apply-coding-conventions/artifacts/compliance-summary.instance.yaml`
- [x] T026 [US3] Validate completion rules against decision register in `specs/002-apply-coding-conventions/artifacts/decision-register.md`
- [x] T027 [P] [US3] Record unresolved deferred/waived ownership and target release in `specs/002-apply-coding-conventions/artifacts/open-items.md`
- [x] T028 [US3] Record reviewer sign-off outcome in `specs/002-apply-coding-conventions/artifacts/sign-off.md`

**Checkpoint**: User Story 3 completes auditable closure of the feature

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final consistency, docs alignment, and handoff readiness

- [x] T029 [P] Cross-check `quickstart.md` steps against executed workflow in `specs/002-apply-coding-conventions/quickstart.md`
- [x] T030 [P] Verify contract/summary alignment in `specs/002-apply-coding-conventions/contracts/compliance-summary.contract.yaml`
- [x] T031 Finalize implementation handoff notes in `specs/002-apply-coding-conventions/artifacts/README.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup)**: starts immediately
- **Phase 2 (Foundational)**: depends on Phase 1 and blocks all user stories
- **Phase 3 (US1)**: depends on Phase 2
- **Phase 4 (US2)**: depends on Phase 3 outputs (`finding-register`, `traceability`)
- **Phase 5 (US3)**: depends on Phase 4 outputs (`decision-register`, `validation-gate`)
- **Phase 6 (Polish)**: depends on completion of selected user stories

### User Story Dependencies

- **US1 (P1)**: independent after Foundational
- **US2 (P2)**: depends on US1 finding outputs
- **US3 (P3)**: depends on US2 remediation and validation outputs

### Within Each User Story

- Capture/review outputs before consolidation
- Consolidation before disposition and sign-off
- Validation-gate recording before final summary completion

### Parallel Opportunities

- Setup: T003 and T004 run in parallel
- Foundational: T006 and T007 run in parallel after T005
- US1: T010, T011, T012 run in parallel
- US2: T017, T018, T019 run in parallel after T016
- Polish: T029 and T030 run in parallel

---

## Parallel Example: User Story 1

```bash
# Parallel review streams
T010 [US1] -> specs/002-apply-coding-conventions/artifacts/findings-src.md
T011 [US1] -> specs/002-apply-coding-conventions/artifacts/findings-tests.md
T012 [US1] -> specs/002-apply-coding-conventions/artifacts/findings-samples.md
```

## Parallel Example: User Story 2

```bash
# Parallel remediation streams
T017 [US2] -> src/Webhooks.Core/**/*.cs + remediation-log-src.md
T018 [US2] -> tests/Webhooks.Core.Tests/**/*.cs + remediation-log-tests.md
T019 [US2] -> samples/**/*.cs + remediation-log-samples.md
```

## Parallel Example: User Story 3

```bash
# Parallel closing artifacts once decisions are complete
T024 [US3] -> artifacts/compliance-summary.md
T027 [US3] -> artifacts/open-items.md
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 and Phase 2
2. Complete Phase 3 (US1)
3. Validate US1 independent test criteria (complete findings coverage)
4. Review and approve findings baseline before remediation work

### Incremental Delivery

1. Setup + Foundational establish shared schema/templates
2. Deliver US1 findings baseline
3. Deliver US2 safe remediation + validation gate
4. Deliver US3 summary/sign-off package
5. Apply Phase 6 polish for handoff quality

### Parallel Team Strategy

1. One maintainer owns schema/templates (Phases 1-2)
2. Reviewers split US1 by folder (`src`, `tests`, `samples`)
3. Remediators split US2 by folder while one owner tracks exceptions/decisions
4. Final reviewer and maintainer close US3 sign-off artifacts
