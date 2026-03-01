# Tasks: Baseline Webhook Broadcasting Behavior

**Input**: Design documents from `/specs/001-initial-src-spec/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/

**Tests**: Included because `spec.md` defines mandatory user-scenario testing and measurable conformance outcomes.

**Organization**: Tasks are grouped by user story so each story can be implemented and verified independently.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependency on incomplete tasks)
- **[Story]**: User story label (`[US1]`, `[US2]`, `[US3]`)
- Every task includes an explicit file path

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create implementation scaffolding for testable delivery increments.

- [ ] T001 Create test project file at `tests/Webhooks.Core.Tests/Webhooks.Core.Tests.csproj`
- [ ] T002 Add test project to solution in `Webhooks.sln`
- [ ] T003 [P] Create test folders and baseline test bootstrap in `tests/Webhooks.Core.Tests/Usings.cs`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core contracts, options, validation, and DI wiring required before any user story work.

**⚠️ CRITICAL**: No user story implementation starts before this phase is complete.

- [ ] T004 Create dispatcher contracts in `src/Webhooks.Core/Contracts/IWebhookDispatcher.cs` and `src/Webhooks.Core/Contracts/IDispatcherInvocationCoordinator.cs`
- [ ] T005 Create middleware contracts in `src/Webhooks.Core/Contracts/IBroadcastMiddleware.cs` and `src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs`
- [ ] T006 Create core delivery models in `src/Webhooks.Core/Models/DeliveryEnvelope.cs`, `src/Webhooks.Core/Models/DeliveryAttempt.cs`, and `src/Webhooks.Core/Models/DeliveryResult.cs`
- [ ] T007 Create handoff telemetry model in `src/Webhooks.Core/Models/DispatchHandoffResult.cs`
- [ ] T008 Extend broadcaster options for dispatcher selection/defaults in `src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs`
- [ ] T009 Add startup validation rules in `src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs`
- [ ] T010 Wire coordinator and middleware registrations in `src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs`

**Checkpoint**: Foundational contracts/options/DI are ready for story implementation.

---

## Phase 3: User Story 1 - Deliver events to subscribed sinks (Priority: P1) 🎯 MVP

**Goal**: Deliver events only to sinks that subscribe to the matching event type.

**Independent Test**: Configure multiple sinks with different subscriptions and verify only subscribed sinks receive each event type.

### Tests for User Story 1

- [ ] T011 [P] [US1] Add exact event-type routing tests in `tests/Webhooks.Core.Tests/Routing/EventTypeRoutingTests.cs`
- [ ] T012 [P] [US1] Add no-subscriber/no-delivery tests in `tests/Webhooks.Core.Tests/Routing/NoMatchingSinksTests.cs`

### Implementation for User Story 1

- [ ] T013 [US1] Implement publish-request normalization and EventId assignment at API entry in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T064 [US1] Ensure outgoing delivery metadata includes dispatch timestamp and EventId in `src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs` and `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T014 [US1] Implement event-type sink matching using subscription criteria in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T015 [US1] Implement coordinator selection flow (exactly one dispatcher per sink attempt) in `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T016 [US1] Add default dispatcher implementation path in `src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs`
- [ ] T017 [US1] Ensure one-sink failure does not block other sinks in `src/Webhooks.Core/Strategies/SequentialBroadcasterStrategy.cs` and `src/Webhooks.Core/Strategies/ParallelTaskBroadcasterStrategy.cs`

**Checkpoint**: US1 is independently functional and testable.

---

## Phase 4: User Story 2 - Route events by payload rules (Priority: P2)

**Goal**: Support payload-based sink routing with explicit AND/OR matching semantics.

**Independent Test**: Publish same event type with different payloads and verify only predicate-matching events are delivered.

### Tests for User Story 2

- [ ] T018 [P] [US2] Add AND/OR payload matching tests in `tests/Webhooks.Core.Tests/Routing/PayloadMatchingModeTests.cs`
- [ ] T019 [P] [US2] Add missing payload field non-match tests in `tests/Webhooks.Core.Tests/Routing/MissingPayloadFieldTests.cs`
- [ ] T020 [P] [US2] Add invalid payload selector startup-failure tests in `tests/Webhooks.Core.Tests/Validation/PayloadSelectorValidationTests.cs`

### Implementation for User Story 2

- [ ] T021 [US2] Replace event filter terminology with subscription criteria model updates in `src/Webhooks.Core/Models/WebhookEventFilter.cs`
- [ ] T022 [US2] Replace payload filter terminology with payload predicate model updates in `src/Webhooks.Core/Models/PayloadFilter.cs`
- [ ] T023 [US2] Add selector/comparator contracts in `src/Webhooks.Core/Contracts/IPayloadFieldSelectorStrategy.cs` and `src/Webhooks.Core/Contracts/IPayloadValueComparisonStrategy.cs`
- [ ] T024 [US2] Implement restricted JsonPath selector and default scalar comparator in `src/Webhooks.Core/Strategies/JsonPathPayloadFieldSelectorStrategy.cs` and `src/Webhooks.Core/Strategies/ScalarStringEqualityComparisonStrategy.cs`
- [ ] T025 [US2] Integrate payload predicate evaluation into matching pipeline in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T026 [US2] Enforce payload predicate configuration validation rules in `src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs`
- [ ] T040 [US2] Add optional deduplication policy options with default-disabled behavior in `src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs`
- [ ] T041 [US2] Implement EventId-based deduplication checks in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T042 [P] [US2] Add deduplication policy conformance tests in `tests/Webhooks.Core.Tests/Dispatch/DeduplicationPolicyTests.cs`
- [ ] T043 [US2] Define selector syntax constraints for restricted JsonPath subset in `src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs`
- [ ] T044 [P] [US2] Add restricted JsonPath conformance tests in `tests/Webhooks.Core.Tests/Validation/JsonPathSubsetValidationTests.cs`

**Checkpoint**: US2 is independently functional and testable.

---

## Phase 5: User Story 3 - Plug in delivery dispatcher and middleware (Priority: P3)

**Goal**: Enable pluggable dispatcher/middleware extension points while keeping broadcaster orchestration stable.

**Independent Test**: Swap default and custom dispatchers through DI and verify behavior changes only in transport path; verify middleware ordering and invocation semantics.

### Tests for User Story 3

- [ ] T027 [P] [US3] Add dispatcher replacement and selection precedence tests in `tests/Webhooks.Core.Tests/Dispatch/DispatcherSelectionPrecedenceTests.cs`
- [ ] T028 [P] [US3] Add broadcast middleware ordering tests in `tests/Webhooks.Core.Tests/Middleware/BroadcastMiddlewareOrderingTests.cs`
- [ ] T029 [P] [US3] Add endpoint-invoker middleware per-retry tests in `tests/Webhooks.Core.Tests/Middleware/EndpointInvokerMiddlewareRetryTests.cs`
- [ ] T030 [P] [US3] Add delivery-result source-of-truth tests (invoker outcome vs handoff telemetry) in `tests/Webhooks.Core.Tests/Dispatch/DeliveryOutcomeSemanticsTests.cs`
- [ ] T045 [P] [US3] Add queue capacity and worker parallelism behavior tests in `tests/Webhooks.Core.Tests/Dispatch/QueueCapacityAndParallelismTests.cs`
- [ ] T046 [P] [US3] Add overflow policy default/override tests in `tests/Webhooks.Core.Tests/Dispatch/OverflowPolicyTests.cs`
- [ ] T047 [P] [US3] Add retry configuration default/override tests in `tests/Webhooks.Core.Tests/Dispatch/RetryConfigurationTests.cs`
- [ ] T048 [P] [US3] Add transient-failure detection configurability tests in `tests/Webhooks.Core.Tests/Dispatch/TransientDetectionStrategyTests.cs`
- [ ] T049 [P] [US3] Add queued dispatcher pending-to-final status transition tests in `tests/Webhooks.Core.Tests/Dispatch/PendingStatusTransitionTests.cs`
- [ ] T050 [P] [US3] Add signing/authentication middleware extension tests in `tests/Webhooks.Core.Tests/Middleware/SigningAuthenticationMiddlewareTests.cs`
- [ ] T051 [P] [US3] Add middleware context shape conformance tests in `tests/Webhooks.Core.Tests/Middleware/MiddlewareContextShapeTests.cs`
- [ ] T052 [P] [US3] Add runtime exception taxonomy conformance tests in `tests/Webhooks.Core.Tests/Dispatch/DispatchExceptionTaxonomyTests.cs`
- [ ] T065 [P] [US3] Add minimum observability field-set conformance tests (status, attempt count, failure reason, EventId correlation) in `tests/Webhooks.Core.Tests/Dispatch/DeliveryObservabilityFieldsTests.cs`

### Implementation for User Story 3

- [ ] T031 [US3] Implement broadcast middleware pipeline execution in `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs`
- [ ] T032 [US3] Implement endpoint invoker middleware pipeline in `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T033 [US3] Add sink-level dispatcher override and app default selection resolution in `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T034 [US3] Add dispatch handoff telemetry recording in `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T035 [US3] Ensure final delivery status is sourced from invoker outcome in `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T036 [US3] Register host-extensible middleware and strategy services in `src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs`
- [ ] T053 [US3] Implement queue capacity and worker parallelism options wiring in `src/Webhooks.Core/Options/BackgroundTaskProcessorOptions.cs` and `src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs`
- [ ] T054 [US3] Implement overflow policy default fail-fast and override behavior in `src/Webhooks.Core/Services/BackgroundTaskChannel.cs`
- [ ] T055 [US3] Implement host-configurable retry policy defaults/overrides in `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T056 [US3] Implement host-configurable transient detection strategy hooks in `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T057 [US3] Implement queued dispatcher `Pending` -> final status transition handling in `src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs`
- [ ] T058 [US3] Implement signing/authentication middleware extension point in `src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs` and `src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs`
- [ ] T059 [US3] Define middleware context shape contract in `src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs`
- [ ] T060 [US3] Define runtime dispatch exception taxonomy in `src/Webhooks.Core/Models/DispatchException.cs`

**Checkpoint**: US3 is independently functional and testable.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final consistency, docs, and end-to-end validation.

- [ ] T061 [P] [US3] Update architecture notes for final implementation behavior in `docs/architecture/system-components.md`
- [ ] T062 [P] [US3] Update feature quickstart verification checklist in `specs/001-initial-src-spec/quickstart.md`
- [ ] T063 [US3] Run end-to-end build and tests per quickstart in `specs/001-initial-src-spec/quickstart.md`

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
- **US3 (P3)**: No dependency on other stories (integrates with shared dispatch/invocation components only).

### Within Each User Story

- Tests are written before implementation tasks.
- Model/contract updates precede service logic updates.
- Service logic updates precede DI/final integration updates.

### Parallel Opportunities

- Phase 1 task `T003` can run in parallel with `T001`/`T002`.
- In US1, `T011` and `T012` can run in parallel.
- In US2, `T018`, `T019`, `T020`, `T042`, and `T044` can run in parallel.
- In US3, `T027`–`T030` and `T045`–`T052` can run in parallel.
- In Polish, `T061` and `T062` can run in parallel.

---

## Parallel Example: User Story 1

```bash
Task: "T011 [US1] Add exact event-type routing tests in tests/Webhooks.Core.Tests/Routing/EventTypeRoutingTests.cs"
Task: "T012 [US1] Add no-subscriber/no-delivery tests in tests/Webhooks.Core.Tests/Routing/NoMatchingSinksTests.cs"
```

## Parallel Example: User Story 2

```bash
Task: "T018 [US2] Add AND/OR payload matching tests in tests/Webhooks.Core.Tests/Routing/PayloadMatchingModeTests.cs"
Task: "T019 [US2] Add missing payload field non-match tests in tests/Webhooks.Core.Tests/Routing/MissingPayloadFieldTests.cs"
Task: "T020 [US2] Add invalid payload selector startup-failure tests in tests/Webhooks.Core.Tests/Validation/PayloadSelectorValidationTests.cs"
```

## Parallel Example: User Story 3

```bash
Task: "T027 [US3] Add dispatcher replacement and selection precedence tests in tests/Webhooks.Core.Tests/Dispatch/DispatcherSelectionPrecedenceTests.cs"
Task: "T028 [US3] Add broadcast middleware ordering tests in tests/Webhooks.Core.Tests/Middleware/BroadcastMiddlewareOrderingTests.cs"
Task: "T029 [US3] Add endpoint-invoker middleware per-retry tests in tests/Webhooks.Core.Tests/Middleware/EndpointInvokerMiddlewareRetryTests.cs"
Task: "T030 [US3] Add delivery-result source-of-truth tests in tests/Webhooks.Core.Tests/Dispatch/DeliveryOutcomeSemanticsTests.cs"
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

- **MVP**: Through Phase 3 (US1), including `T001`–`T017`.
