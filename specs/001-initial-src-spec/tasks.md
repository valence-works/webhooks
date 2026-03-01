# Tasks: Baseline Webhook Broadcasting Behavior

**Input**: Design documents from `/specs/001-initial-src-spec/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/

**Tests**: Included because `spec.md` defines mandatory user-scenario testing and measurable conformance outcomes.

**Organization**: Tasks are grouped by user story so each story can be implemented and verified independently.

**Scope Note**: Phase 1 and Phase 2 are intentionally shared cross-story prerequisites; story-scoped execution begins in Phase 3.

**Traceability Conventions**: FR traceability tables under each user story are quick reference summaries; task-level FR tags (for example `[FR-012]`) are authoritative when present.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependency on incomplete tasks)
- **[Story]**: User story label (`[US1]`, `[US2]`, `[US3]`)
- Every task includes an explicit file path

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and test scaffolding.

- [ ] T001 Create test project file in `tests/Webhooks.Core.Tests/Webhooks.Core.Tests.csproj`
- [ ] T002 Add test project to solution in `Webhooks.sln`
- [ ] T003 [P] Create baseline test bootstrap in `tests/Webhooks.Core.Tests/Usings.cs`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core contracts, models, options, and DI wiring required before any user story work.

**⚠️ CRITICAL**: No user story implementation starts before this phase is complete.

- [ ] T004 Create dispatcher contracts in `src/Webhooks.Core/Contracts/IWebhookDispatcher.cs` and `src/Webhooks.Core/Contracts/IDispatcherInvocationCoordinator.cs`
- [ ] T005 Create middleware contracts in `src/Webhooks.Core/Contracts/IBroadcastMiddleware.cs` and `src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs`
- [ ] T006 Create core delivery models in `src/Webhooks.Core/Models/DeliveryEnvelope.cs`, `src/Webhooks.Core/Models/DeliveryAttempt.cs`, and `src/Webhooks.Core/Models/DeliveryResult.cs`
- [ ] T007 Create handoff telemetry model in `src/Webhooks.Core/Models/DispatchHandoffResult.cs`
- [ ] T008 Extend broadcaster options for dispatcher selection and defaults in `src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs`
- [ ] T009 Add startup validation framework in `src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs`
- [ ] T010 Wire coordinator and middleware registrations in `src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs`
- [ ] T011 [P] Add dispatcher registration validation tests in `tests/Webhooks.Core.Tests/Validation/DispatcherRegistrationValidationTests.cs`
- [ ] T012 [P] Add coordinator registration/resolve tests in `tests/Webhooks.Core.Tests/Validation/CoordinatorResolutionValidationTests.cs`

**Checkpoint**: Foundational contracts/options/DI are ready for story implementation.

---

## Phase 3: User Story 1 - Deliver events to subscribed sinks (Priority: P1) 🎯 MVP

**Goal**: Deliver events only to sinks that subscribe to the matching event type.

**Independent Test**: Configure multiple sinks with different subscriptions and verify only subscribed sinks receive each event type.

**US1 FR Traceability**

| FR | Covered By Tasks |
|----|------------------|
| FR-001 | T015, T017 |
| FR-003 | T016 |
| FR-003a, FR-003b | T016 |
| FR-004 | T013, T018 |
| FR-006, FR-025 | T019 |
| FR-007, FR-007a | T021 |
| FR-011 | T022 |
| FR-013 | T020 |
| FR-023, FR-023a, FR-023b | T019, T020 |

### Tests for User Story 1

- [ ] T013 [P] [US1] Add exact event-type routing tests in `tests/Webhooks.Core.Tests/Routing/EventTypeRoutingTests.cs`
- [ ] T014 [P] [US1] Add no-subscriber/no-delivery tests in `tests/Webhooks.Core.Tests/Routing/NoMatchingSinksTests.cs`
- [ ] T015 [P] [US1] Add sink registration validation tests (unique SinkId, required destination, required subscriptions) in `tests/Webhooks.Core.Tests/Validation/SinkRegistrationValidationTests.cs`

### Implementation for User Story 1

- [ ] T016 [US1] Implement publish-request normalization and EventId assignment at API entry in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T017 [US1] Implement sink registration validation rules (unique SinkId, destination required, subscriptions required) in `src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs`
- [ ] T018 [US1] Implement event-type sink matching using subscription criteria in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T019 [US1] Implement coordinator selection flow (exactly one dispatcher per sink attempt) in `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T020 [US1] Add default dispatcher implementation path in `src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs`
- [ ] T021 [US1] Ensure outgoing delivery metadata includes dispatch timestamp and EventId in `src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs` and `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T022 [US1] Ensure one-sink failure does not block other sinks in `src/Webhooks.Core/Strategies/SequentialBroadcasterStrategy.cs` and `src/Webhooks.Core/Strategies/ParallelTaskBroadcasterStrategy.cs`

**Checkpoint**: US1 is independently functional and testable.

---

## Phase 4: User Story 2 - Route events by payload rules (Priority: P2)

**Goal**: Support payload-based sink routing with explicit AND/OR matching semantics.

**Independent Test**: Publish same event type with different payloads and verify only predicate-matching events are delivered.

**US2 FR Traceability**

| FR | Covered By Tasks |
|----|------------------|
| FR-005, FR-005a | T023, T032 |
| FR-005b, FR-005c, FR-005d | T030, T031, T027 |
| FR-014 | T025, T033 |
| FR-014a, FR-014b | T026, T034, T035 |

### Tests for User Story 2

- [ ] T023 [P] [US2] Add AND/OR payload matching tests in `tests/Webhooks.Core.Tests/Routing/PayloadMatchingModeTests.cs`
- [ ] T024 [P] [US2] Add missing payload field non-match tests in `tests/Webhooks.Core.Tests/Routing/MissingPayloadFieldTests.cs`
- [ ] T025 [P] [US2] Add invalid payload selector startup-failure tests in `tests/Webhooks.Core.Tests/Validation/PayloadSelectorValidationTests.cs`
- [ ] T026 [P] [US2] Add deduplication policy conformance tests in `tests/Webhooks.Core.Tests/Dispatch/DeduplicationPolicyTests.cs`
- [ ] T027 [P] [US2] Add restricted JsonPath conformance tests in `tests/Webhooks.Core.Tests/Validation/JsonPathSubsetValidationTests.cs`

### Implementation for User Story 2

- [ ] T028 [US2] Migrate legacy event-filter model naming to canonical subscription criteria terminology in `src/Webhooks.Core/Models/WebhookEventFilter.cs`
- [ ] T029 [US2] Migrate legacy payload-filter naming (including existing PayloadFilter file/type names) to canonical payload predicate terminology in `src/Webhooks.Core/Models/PayloadFilter.cs`
- [ ] T030 [US2] Add selector/comparator contracts in `src/Webhooks.Core/Contracts/IPayloadFieldSelectorStrategy.cs` and `src/Webhooks.Core/Contracts/IPayloadValueComparisonStrategy.cs`
- [ ] T031 [US2] Implement restricted JsonPath selector and default scalar comparator in `src/Webhooks.Core/Strategies/JsonPathPayloadFieldSelectorStrategy.cs` and `src/Webhooks.Core/Strategies/ScalarStringEqualityComparisonStrategy.cs`
- [ ] T032 [US2] Integrate payload predicate evaluation into matching pipeline in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T033 [US2] Enforce payload predicate configuration validation rules in `src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs`
- [ ] T034 [US2] Add optional deduplication policy options with default-disabled behavior in `src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs`
- [ ] T035 [US2] Implement EventId-based deduplication checks in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T036 [US2] Define selector syntax constraints for restricted JsonPath subset in `src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs`

**Checkpoint**: US2 is independently functional and testable.

---

## Phase 5: User Story 3 - Plug in delivery dispatcher and middleware (Priority: P3)

**Goal**: Enable pluggable dispatcher/middleware extension points in Webhooks Core while keeping optional queue/worker execution in dispatcher extension modules.

**Independent Test**: Swap default and custom dispatchers through DI and verify behavior changes only in transport path; if a queued dispatcher module is installed, verify queue/worker behavior remains module-owned while middleware ordering and invocation semantics remain stable.

**US3 FR Traceability**

| FR | Covered By Tasks |
|----|------------------|
| FR-008, FR-009, FR-009a, FR-009b, FR-009c | T037, T052, T054, T057 |
| FR-010, FR-010a, FR-010b, FR-010c, FR-010d | T039, T043, T044, T053, T060, T061 |
| FR-011a | T050, T066 |
| FR-012, FR-012a | T041, T058 |
| FR-015, FR-016 | T042, T059 |
| FR-017, FR-018 | T043, T060 |
| FR-019 | T037, T057 |
| FR-020, FR-020a, FR-021, FR-021a, FR-021b, FR-022 | T038, T039, T046, T052, T053, T063 |
| FR-022a | T049 |
| FR-023c | T045, T062 |
| FR-024 | T009, T011, T012 |
| FR-026 | T051, T067 |
| FR-027 | T070 |

### Tests for User Story 3

- [ ] T037 [P] [US3] Add dispatcher replacement and selection precedence tests in `tests/Webhooks.Core.Tests/Dispatch/DispatcherSelectionPrecedenceTests.cs`
- [ ] T038 [P] [US3] Add broadcast middleware ordering tests in `tests/Webhooks.Core.Tests/Middleware/BroadcastMiddlewareOrderingTests.cs`
- [ ] T039 [P] [US3] Add endpoint-invoker middleware per-retry tests in `tests/Webhooks.Core.Tests/Middleware/EndpointInvokerMiddlewareRetryTests.cs`
- [ ] T040 [P] [US3] Add delivery-result source-of-truth tests (invoker outcome vs handoff telemetry) in `tests/Webhooks.Core.Tests/Dispatch/DeliveryOutcomeSemanticsTests.cs`
- [ ] T041 [P] [US3] Add queued dispatcher module queue-capacity/worker-parallelism integration behavior tests in `tests/Webhooks.Core.Tests/Dispatch/QueueCapacityAndParallelismTests.cs`
- [ ] T042 [P] [US3] Add queued dispatcher module overflow policy default/override integration tests in `tests/Webhooks.Core.Tests/Dispatch/OverflowPolicyTests.cs`
- [ ] T043 [P] [US3] Add retry configuration default/override tests in `tests/Webhooks.Core.Tests/Dispatch/RetryConfigurationTests.cs`
- [ ] T044 [P] [US3] Add transient-failure detection configurability tests in `tests/Webhooks.Core.Tests/Dispatch/TransientDetectionStrategyTests.cs`
- [ ] T045 [P] [US3] Add queued dispatcher pending-to-final status transition tests in `tests/Webhooks.Core.Tests/Dispatch/PendingStatusTransitionTests.cs`
- [ ] T046 [P] [US3] Add signing/authentication middleware extension tests in `tests/Webhooks.Core.Tests/Middleware/SigningAuthenticationMiddlewareTests.cs`
- [ ] T047 [P] [US3] Add middleware context shape conformance tests in `tests/Webhooks.Core.Tests/Middleware/MiddlewareContextShapeTests.cs`
- [ ] T048 [P] [US3] Add runtime exception taxonomy conformance tests in `tests/Webhooks.Core.Tests/Dispatch/DispatchExceptionTaxonomyTests.cs`
- [ ] T049 [P] [US3] Add minimum observability field-set conformance tests (status, attempt count, failure reason, EventId correlation) in `tests/Webhooks.Core.Tests/Dispatch/DeliveryObservabilityFieldsTests.cs`
- [ ] T050 [P] [US3] Add dispatcher-unavailable default behavior tests (mark failed, continue other sinks) in `tests/Webhooks.Core.Tests/Dispatch/DispatcherUnavailableBehaviorTests.cs`
- [ ] T051 [P] [US3] Add coordinator-policy ownership conformance tests (dispatcher transport-only, policy in coordinator) in `tests/Webhooks.Core.Tests/Dispatch/CoordinatorPolicyOwnershipTests.cs`

### Implementation for User Story 3

- [ ] T052 [US3] Implement broadcast middleware pipeline execution in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T053 [US3] Implement endpoint invoker middleware pipeline in `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T054 [US3] Add sink-level dispatcher override and app default selection resolution in `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T055 [US3] Add dispatch handoff telemetry recording in `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T056 [US3] Ensure final delivery status is sourced from invoker outcome in `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T057 [US3] Register host-extensible middleware and strategy services in `src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs`
- [ ] T058 [US3] Implement queued dispatcher module options pass-through contract (queue capacity, worker parallelism) while preserving module-owned queue/worker runtime boundary in `src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs` and `src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs`
- [ ] T059 [US3] Implement overflow policy default fail-fast and override behavior contract for queued dispatcher integrations in `src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs` and `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T060 [US3] Implement host-configurable retry policy defaults/overrides in `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T061 [US3] Implement host-configurable transient detection strategy hooks in `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T062 [US3] Implement queued dispatcher pending-to-final status transition handling in `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T063 [US3] Implement signing/authentication middleware extension point in `src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs` and `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T064 [US3] Define middleware context shape contract in `src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs`
- [ ] T065 [US3] Define runtime dispatch exception taxonomy in `src/Webhooks.Core/Models/DispatchException.cs`
- [ ] T066 [US3] Implement coordinator handling for dispatcher-unavailable attempts (record failed, continue eligible sinks) in `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T067 [US3] Enforce coordinator-owned selection policy boundary in dispatcher contracts/docs and coordinator flow in `src/Webhooks.Core/Contracts/IWebhookDispatcher.cs` and `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`

**Checkpoint**: US3 is independently functional and testable.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final consistency, docs, and end-to-end validation.

- [ ] T068 [P] [US3] Update architecture notes for final implementation behavior in `docs/architecture/system-components.md`
- [ ] T069 [P] [US3] Update feature quickstart verification checklist in `specs/001-initial-src-spec/quickstart.md`
- [ ] T070 [US3] Add scope-guard documentation/conformance check to ensure non-webhook stream/workflow features remain out of baseline scope in `specs/001-initial-src-spec/quickstart.md`
- [ ] T071 [US3] Run end-to-end build and tests per quickstart in `specs/001-initial-src-spec/quickstart.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup)**: No dependencies.
- **Phase 2 (Foundational)**: Depends on Phase 1 and blocks all user stories.
- **Phase 3 (US1)**: Depends on Phase 2 completion.
- **Phase 4 (US2)**: Depends on Phase 2 completion; can run in parallel with US1 after foundation.
- **Phase 5 (US3)**: Depends on Phase 2 completion; can run in parallel with US1/US2 after foundation.
- **Phase 6 (Polish)**: Depends on completion of desired user stories.

### User Story Dependencies

- **US1 (P1)**: No dependency on other stories.
- **US2 (P2)**: No dependency on other stories (integrates with shared matching components only).
- **US3 (P3)**: No dependency on other stories (integrates with shared dispatch/invocation components and optional dispatcher extension modules).

### Within Each User Story

- Tests are written before implementation tasks.
- Model/contract updates precede service logic updates.
- Service logic updates precede DI and final integration updates.

### Parallel Opportunities

- Phase 1 task `T003` can run in parallel with `T001`/`T002`.
- In Phase 2, `T011` and `T012` can run in parallel.
- In US1, `T013`, `T014`, and `T015` can run in parallel.
- In US2, `T023`–`T027` can run in parallel.
- In US3, `T037`–`T051` can run in parallel where files differ.
- In Polish, `T068` and `T069` can run in parallel.

---

## Parallel Example: User Story 1

```bash
Task: "T013 [US1] Add exact event-type routing tests in tests/Webhooks.Core.Tests/Routing/EventTypeRoutingTests.cs"
Task: "T014 [US1] Add no-subscriber/no-delivery tests in tests/Webhooks.Core.Tests/Routing/NoMatchingSinksTests.cs"
Task: "T015 [US1] Add sink registration validation tests in tests/Webhooks.Core.Tests/Validation/SinkRegistrationValidationTests.cs"
```

## Parallel Example: User Story 2

```bash
Task: "T023 [US2] Add AND/OR payload matching tests in tests/Webhooks.Core.Tests/Routing/PayloadMatchingModeTests.cs"
Task: "T024 [US2] Add missing payload field non-match tests in tests/Webhooks.Core.Tests/Routing/MissingPayloadFieldTests.cs"
Task: "T025 [US2] Add invalid payload selector startup-failure tests in tests/Webhooks.Core.Tests/Validation/PayloadSelectorValidationTests.cs"
```

## Parallel Example: User Story 3

```bash
Task: "T037 [US3] Add dispatcher replacement and selection precedence tests in tests/Webhooks.Core.Tests/Dispatch/DispatcherSelectionPrecedenceTests.cs"
Task: "T038 [US3] Add broadcast middleware ordering tests in tests/Webhooks.Core.Tests/Middleware/BroadcastMiddlewareOrderingTests.cs"
Task: "T039 [US3] Add endpoint-invoker middleware per-retry tests in tests/Webhooks.Core.Tests/Middleware/EndpointInvokerMiddlewareRetryTests.cs"
```

---

## Implementation Strategy

### MVP First (US1 only)

1. Complete Phase 1 (Setup).
2. Complete Phase 2 (Foundational).
3. Complete Phase 3 (US1).
4. Validate US1 independently before expanding scope.

### Incremental Delivery

1. Deliver US1 as MVP.
2. Add US2 payload criteria routing.
3. Add US3 pluggability/middleware semantics.
4. Run polish validation and quickstart checks.

### Suggested MVP Scope

- **MVP**: Through Phase 3 (US1), including `T001`–`T022`.
