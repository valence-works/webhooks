# Tasks: Wildcard Event-Type Matching

**Input**: Design documents from `/specs/003-wildcard-event-matching/`
**Prerequisites**: plan.md (required), spec.md (required), research.md, data-model.md, contracts/, quickstart.md

**Tests**: Include automated routing and validation tests because `FR-008` explicitly requires wildcard, exact, non-match, and host-override coverage.

**Organization**: Tasks are grouped by user story so each story can be implemented and tested independently.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependency on unfinished tasks)
- **[Story]**: User story label (`[US1]`, `[US2]`, `[US3]`)
- All tasks include explicit file paths

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Introduce matcher extension-point scaffolding used by all stories.

- [x] T001 Create matcher contract `IEventTypeMatcherStrategy` in `src/Webhooks.Core/Contracts/IEventTypeMatcherStrategy.cs`
- [x] T002 Create matcher strategy scaffold and interface implementation skeleton in `src/Webhooks.Core/Strategies/WildcardEventTypeMatcherStrategy.cs`
- [x] T003 [P] Create exact-only matcher strategy for host opt-in behavior in `src/Webhooks.Core/Strategies/ExactEventTypeMatcherStrategy.cs`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Wire matcher strategy into options, validation, DI, and broadcaster flow before story behaviors.

**⚠️ CRITICAL**: User story work depends on this phase.

- [x] T004 Add `EventTypeMatcherStrategy` option property (without default assignment) to `src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs`
- [x] T005 Validate matcher strategy assignability in `src/Webhooks.Core/Options/ConfigureWebhookEventBroadcasterOptions.cs`
- [x] T006 Add fluent matcher configuration extensions in `src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs`
- [x] T007 Register `IEventTypeMatcherStrategy` factory/default in `src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs`
- [x] T008 Replace hardcoded event-type equality with matcher invocation in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`

**Checkpoint**: Routing uses strategy-based event-type matching and hosts can configure matcher type.

---

## Phase 3: User Story 1 - Deliver events to wildcard subscribers (Priority: P1) 🎯 MVP

**Goal**: A subscription configured with `*` receives all incoming event types.

**Independent Test**: Configure a sink with `*` and broadcast multiple event types; the sink is selected for every event.

### Tests for User Story 1

- [x] T009 [P] [US1] Add wildcard routing tests for multiple incoming event types in `tests/Webhooks.Core.Tests/Routing/WildcardEventTypeRoutingTests.cs`
- [x] T010 [P] [US1] Add wildcard tests for null/empty/whitespace incoming event types in `tests/Webhooks.Core.Tests/Routing/WildcardEventTypeRoutingEdgeCasesTests.cs`

### Implementation for User Story 1

- [x] T011 [US1] Implement `*` full-token wildcard semantics in `src/Webhooks.Core/Strategies/WildcardEventTypeMatcherStrategy.cs`
- [x] T012 [US1] Set default `EventTypeMatcherStrategy` to wildcard matcher in `src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs`

**Checkpoint**: Wildcard subscriptions route all events and story-level tests pass independently.

---

## Phase 4: User Story 2 - Preserve exact-match routing behavior (Priority: P2)

**Goal**: Literal subscriptions stay case-sensitive exact, with no unintended fan-out.

**Independent Test**: A sink subscribed to `Heartbeat` receives `Heartbeat` and does not receive non-matching or case-variant event types.

### Tests for User Story 2

- [x] T013 [P] [US2] Add literal exact-match and non-match regression tests in `tests/Webhooks.Core.Tests/Routing/EventTypeRoutingTests.cs`
- [x] T014 [P] [US2] Add case-sensitivity regression tests for literal subscriptions in `tests/Webhooks.Core.Tests/Routing/LiteralEventTypeCaseSensitivityTests.cs`
- [x] T015 [P] [US2] Add invalid-subscription-value warning/non-match tests in `tests/Webhooks.Core.Tests/Routing/InvalidSubscriptionEventTypeTests.cs`

### Implementation for User Story 2

- [x] T016 [US2] Enforce case-sensitive exact matching for non-wildcard values in `src/Webhooks.Core/Strategies/WildcardEventTypeMatcherStrategy.cs`
- [x] T017 [US2] Treat leading/trailing-whitespace subscription values as invalid non-match with warning in `src/Webhooks.Core/Strategies/WildcardEventTypeMatcherStrategy.cs`
- [x] T018 [US2] Keep payload filter behavior unchanged while using matcher-based routing in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`

**Checkpoint**: Exact-match behavior remains stable and invalid subscription handling is covered by tests.

---

## Phase 5: User Story 3 - Allow host-controlled matching policy (Priority: P3)

**Goal**: Hosts can replace default matching behavior without changing library source.

**Independent Test**: Register a custom matcher strategy and verify sink selection follows custom logic.

### Tests for User Story 3

- [x] T019 [P] [US3] Add host override routing test with custom matcher strategy in `tests/Webhooks.Core.Tests/Routing/CustomEventTypeMatcherOverrideTests.cs`
- [x] T020 [P] [US3] Add options validation tests for invalid matcher type configuration in `tests/Webhooks.Core.Tests/Validation/EventTypeMatcherStrategyValidationTests.cs`

### Implementation for User Story 3

- [x] T021 [US3] Resolve matcher strategy type via DI activation in `src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs`
- [x] T022 [US3] Add matcher override extension methods for typed and runtime configuration in `src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs`
- [x] T023 [US3] Document default and override matcher behavior in `README.md`

**Checkpoint**: Host-provided matcher strategies drive routing when configured, with default behavior intact otherwise.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final alignment, verification, and handoff readiness.

- [x] T024 [P] Update feature verification notes and expected outcomes in `specs/003-wildcard-event-matching/quickstart.md`
- [x] T025 [P] Cross-check contract wording against implementation expectations in `specs/003-wildcard-event-matching/contracts/event-type-matching-contract.md`
- [x] T026 Execute routing-focused regression tests and record pass/fail plus command output summary in `specs/003-wildcard-event-matching/quickstart.md`
- [x] T027 Execute full solution tests and record pass/fail plus command output summary in `specs/003-wildcard-event-matching/quickstart.md`
- [x] T028 Run solution build validation (`dotnet build Webhooks.sln`) and capture warnings disposition in `specs/003-wildcard-event-matching/quickstart.md`
- [x] T029 Execute 5-minute sample smoke validation and record wildcard/literal delivery outcomes in `specs/003-wildcard-event-matching/quickstart.md`
- [x] T030 Execute focused routing throughput regression check and record findings against performance goals in `specs/003-wildcard-event-matching/quickstart.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup)**: Starts immediately.
- **Phase 2 (Foundational)**: Depends on Phase 1 and blocks all user stories.
- **Phase 3 (US1)**: Depends on Phase 2.
- **Phase 4 (US2)**: Depends on Phase 2 (independent of US1 code, but sequenced after P1 for delivery priority).
- **Phase 5 (US3)**: Depends on Phase 2.
- **Phase 6 (Polish)**: Depends on completion of the targeted user stories.

### User Story Dependency Graph

- `Foundational (Phase 2) -> US1`
- `Foundational (Phase 2) -> US2`
- `Foundational (Phase 2) -> US3`
- `US1 -> MVP release candidate`

### Within Each User Story

- Tests first (must fail before implementation changes).
- Story-specific implementation after failing tests exist.
- Story completes only when its independent test criteria pass.

### Parallel Opportunities

- **Setup**: `T003` parallel with `T001`/`T002` after contract shape is agreed.
- **US1**: `T009` and `T010` run in parallel.
- **US2**: `T013`, `T014`, and `T015` run in parallel.
- **US3**: `T019` and `T020` run in parallel.
- **Polish**: `T024`, `T025`, and `T030` run in parallel.

---

## Parallel Example: User Story 1

```bash
Task: "T009 [US1] Add wildcard routing tests in tests/Webhooks.Core.Tests/Routing/WildcardEventTypeRoutingTests.cs"
Task: "T010 [US1] Add wildcard edge-case tests in tests/Webhooks.Core.Tests/Routing/WildcardEventTypeRoutingEdgeCasesTests.cs"
```

## Parallel Example: User Story 2

```bash
Task: "T013 [US2] Extend EventTypeRoutingTests exact/non-match coverage"
Task: "T014 [US2] Add case-sensitivity test file"
Task: "T015 [US2] Add invalid-subscription warning/non-match tests"
```

## Parallel Example: User Story 3

```bash
Task: "T019 [US3] Add custom matcher override routing tests"
Task: "T020 [US3] Add matcher strategy options validation tests"
```

---

## Implementation Strategy

### MVP First (User Story 1 only)

1. Finish Phase 1 and Phase 2.
2. Deliver Phase 3 (US1) end-to-end.
3. Validate wildcard routing via US1 independent tests.
4. Demo/release MVP behavior.

### Incremental Delivery

1. Foundation (strategy contract + DI + broadcaster wiring).
2. US1 wildcard delivery behavior.
3. US2 exact-match compatibility + invalid-value warning behavior.
4. US3 host override behavior.
5. Polish and full regression execution.

### Parallel Team Strategy

1. One developer owns Phase 2 options/DI wiring.
2. Routing-test contributors split US1/US2/US3 test files in parallel.
3. Docs/contract alignment (Phase 6) runs in parallel with final regression checks.

---

## Completeness Check

- [ ] Each user story has: tests + implementation + independent verification criteria.
- [ ] Wildcard, exact-match, non-match, invalid-value warning, and host override are all covered.
- [ ] Task descriptions reference concrete repository paths.
- [ ] No task violates checklist format requirements.
