# Feature Specification: Baseline Webhook Broadcasting Behavior

**Feature Branch**: `001-initial-src-spec`  
**Created**: 2026-02-28  
**Status**: Draft  
**Input**: User description: "Create an initial specification based on the existing source code in src. FYI: the plan is to then enhance that spec and update the current source code, accordingly."

## Clarifications

### Session 2026-02-28

- Q: For sink subscriptions with multiple payload predicates, how should matching work? → A: Support both modes per subscription (configurable AND/OR).
- Q: If a subscription has payload predicates but no matching mode is specified, what should happen? → A: Reject configuration as invalid (mode required).
- Q: For dispatcher modules that use queued delivery, what should happen when the queue is at capacity? → A: Host app can configure policy; default is Fail.
- Q: How should outbound HTTP retry behavior be specified? → A: Host app configures attempts and backoff strategy, with library defaults.
- Q: Should webhook delivery be extensible/pluggable at dispatch time? → A: Yes, via one or more registered `IWebhookDispatcher` implementations, with exactly one dispatcher selected per sink delivery attempt through the dispatcher invocation coordinator.
- Q: Where should middleware extensibility be introduced? → A: Support broadcast middleware in the dispatch plane and an Endpoint Invoker middleware pipeline in the invoke plane.
- Q: How should execution policy abstraction evolve with dispatcher pluggability? → A: Execution policy is owned by broadcaster orchestration while dispatcher remains terminal transport; a standalone strategy abstraction is not required.
- Q: How should payload predicate evaluation evolve to avoid brittle string-only matching? → A: Payload predicate evaluation uses structured field comparison with a pluggable value comparison strategy; v1 defaults to string equality.
- Q: Should payload field addressing specify a default implementation? → A: Yes, default field addressing uses JsonPath (restricted subset), while allowing host-provided alternatives.
- Q: How should retry boundaries and transient detection be specified? → A: Retry is per delivery attempt at the Endpoint Invoker boundary within the selected dispatcher path, middleware executes per retry attempt, and transient detection is host-configurable.
- Q: Should the Webhook Source concept be part of this baseline? → A: No. Webhook Source is removed from this baseline specification as non-behavioral metadata.
- Q: What exact scope does broadcast middleware wrap? → A: Broadcast middleware wraps the full broadcast operation, including sink iteration and invocation of per-delivery-attempt middleware/dispatcher flows.
- Q: How should dispatcher registration be modeled at startup? → A: Support one or more registered dispatchers with sink-level optional dispatcher selection and an application-level default dispatcher; coordinator selects exactly one dispatcher per sink delivery attempt.
- Q: How should idempotency, signing extensibility, and backpressure concerns be handled? → A: Event IDs are required, deduplication is optional and policy-driven, delivery middleware can mutate/sign outbound delivery requests, and advanced backlog escalation (dead-letter/re-drive) is explicitly future-scope unless configured by extensions.
- Q: When must a generated EventId be assigned if the host does not provide one? → A: At broadcast API entry, before broadcast middleware, orchestration, and dispatch.

### Session 2026-03-01

- Q: For EventId-based deduplication, what should the default be when the application does not configure a policy? → A: Disabled by default.
- Q: If one enabled dispatcher is temporarily unavailable at runtime for a delivery attempt, what should the default behavior be? → A: Mark that delivery attempt failed and continue processing other sinks.
- Q: What is the minimum required observability scope for each delivery attempt? → A: Record status, attempt count, final failure reason, and EventId correlation.
- Q: Should delivery success be determined by dispatcher handoff or by endpoint invocation? → A: Endpoint invocation outcome is primary; dispatcher handoff is secondary telemetry.

## Canonical Vocabulary

Use the following terms consistently across specification, planning, implementation, and tests:

Source of truth: `docs/architecture/vocabulary.md`.

- **Publish Request**: Host-provided input submitted to the broadcast API.
- **Delivery Envelope**: Normalized outbound message used by orchestration after input normalization.
- **Subscription Criteria**: Subscription-side matching rule (event type + optional payload predicates + matching mode).
- **Payload Predicate**: Field comparison condition used during subscription matching.
- **Coordinator Invocation Policy**: Policy controlling how the dispatcher coordinator selects exactly one dispatcher per sink delivery attempt.
- **Dispatch Handoff Result**: Secondary telemetry for accepted/enqueued/rejected dispatcher handoff outcomes.
- **Delivery Attempt**: One endpoint invocation attempt for one `(Delivery Envelope, Sink)` pair.
- **Delivery Result**: Structured outcome for a delivery attempt, including status and observability fields.
- **Overflow Policy**: Configurable behavior when extension-provided queued dispatcher modules reach capacity.
- **Dispatch Plane**: Components responsible for matching and dispatcher handoff.
- **Invoke Plane**: Components responsible for actual endpoint invocation.

Deprecated terms that should not be reintroduced:

- `Broadcast Input`, `Broadcast Request`, `Broadcast Event`
- `Webhook Event Filter`, `Subscription Match Rule`, `Payload Filter`
- `Sink Delivery Attempt`, `Delivery Outcome Record`
- `Delivery Orchestration Policy`, `queue-full policy`
- `Dispatch Mode` (when used for coordinator policy semantics)

## Component Diagram

- Spec-local diagram: `specs/001-initial-src-spec/system-components.md`
- Canonical docs diagram: `docs/architecture/system-components.md`

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Deliver events to subscribed sinks (Priority: P1)

As an application owner, I can define webhook sinks and event subscriptions so that outgoing webhook events are delivered only to interested destinations.

**Why this priority**: This is the core product value; without correct routing and delivery, the system does not provide usable webhook functionality.

**Independent Test**: Configure multiple sinks with different event subscriptions, publish one event type, and verify delivery occurs only to sinks subscribed to that event type.

**Acceptance Scenarios**:

1. **Given** two sinks where only one subscribes to `OrderCreated`, **When** an `OrderCreated` event is broadcast, **Then** only the subscribed sink receives the event.
2. **Given** a sink that subscribes to all currently used event types, **When** each event type is broadcast once, **Then** the sink receives one delivery attempt per event broadcast.

---

### User Story 2 - Route events by payload rules (Priority: P2)

As an application owner, I can define payload-based rules for a sink subscription so that sinks receive only events relevant to specific payload attributes.

**Why this priority**: Payload predicate evaluation prevents noisy deliveries and allows finer-grained routing without introducing separate event types for every business variant.

**Independent Test**: Configure one sink with payload rules and publish two events of the same type with different payload values; verify only matching payloads are delivered.

**Acceptance Scenarios**:

1. **Given** a sink subscribed to `InvoiceCreated` with a payload rule `Region=EU`, **When** one `InvoiceCreated` event is broadcast with `Region=EU` and another with `Region=US`, **Then** only the EU-matching event is delivered to that sink.
2. **Given** a sink subscription with no payload rules, **When** events of the subscribed type are broadcast with varied payloads, **Then** all events of that type are eligible for delivery.

---

### User Story 3 - Plug in delivery dispatcher and middleware (Priority: P3)

As an application owner, I can select or replace the delivery dispatcher and compose middleware so transport behavior and cross-cutting concerns can be customized without replacing the broadcaster.

**Why this priority**: Pluggability is required for production transport integrations and for adding concerns like logging, tracing, and persistence without forking core behavior.

**Independent Test**: Register the default dispatcher and then a custom dispatcher in separate runs, broadcast the same event set, and verify broadcaster behavior remains consistent while dispatch transport behavior changes via DI.

**Acceptance Scenarios**:

1. **Given** the default dispatcher is registered, **When** a broadcast targets matching sinks, **Then** the broadcaster routes sink deliveries through the configured middleware pipeline and terminal dispatcher invocation.
2. **Given** a custom dispatcher implementation is registered, **When** events are broadcast, **Then** delivery uses the custom dispatcher without replacing broadcaster logic.
3. **Given** broadcast middleware and Endpoint Invoker middleware are configured, **When** events are broadcast, **Then** middleware executes in deterministic configured order before terminal dispatch.

### Edge Cases

- What happens when an event has no matching sink subscriptions? The broadcast completes without endpoint delivery attempts.
- What happens when the same EventId is broadcast more than once? Behavior follows configured deduplication policy; baseline behavior allows duplicates unless deduplication is enabled.
- What happens when dispatcher handoff succeeds but endpoint invocation fails later? Delivery is recorded as failed because Endpoint Invoker outcome is authoritative.
- What happens when a delivery attempt fails transiently? The default HTTP dispatcher retries delivery attempts before final failure.
- What happens when a delivery attempt ultimately fails? Failure is recorded as an error and does not stop attempts to other sinks.
- What happens when a dispatcher is temporarily unavailable for one delivery attempt? That delivery attempt is recorded as failed and other eligible sinks continue by default.
- What happens when retry behavior differs across dispatchers? Dispatchers must follow the common retry contract: per-delivery-attempt retry boundaries, dispatcher-owned retries, and host-configurable transient detection.
- What happens to middleware during retries? Endpoint Invoker middleware executes for each retry attempt, not only for the initial attempt.
- What happens when a payload rule references a field path missing in the event payload? That sink is not considered a successful match for that rule set.
- What happens when payload value type and comparator expectations do not align? The filter evaluation returns non-match unless a host-provided comparison strategy handles that case.
- What happens when a payload predicate uses an invalid field path expression? Configuration validation fails for that rule definition.
- What happens when a selected dispatcher module uses queued execution and receives more work than immediate processing capacity? Behavior follows host-configured overflow policy; default behavior is fail-fast.
- What happens when transient failures and retries cause sustained queued backlog growth in a dispatcher module? Overflow policy applies immediately; dead-letter and re-drive behavior are extension-defined unless explicitly configured.
- What happens when no dispatcher is registered? Startup/configuration validation fails.
- What happens when multiple dispatchers are registered? The dispatcher invocation coordinator selects exactly one dispatcher for each sink delivery attempt using sink-level override when configured, otherwise the application default dispatcher.
- What happens when middleware throws for one sink delivery? That sink delivery fails and is recorded, while other matching sinks continue unless host policy explicitly changes this behavior.
- What happens when broadcast middleware short-circuits? No dispatch-plane delivery middleware, Endpoint Invoker middleware, or dispatcher invocation occurs for that broadcast.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST allow registering webhook sinks with a unique identifier, destination target, and one or more event subscriptions.
- **FR-003**: System MUST accept new outbound webhook events consisting of an event type and optional payload.
- **FR-003a**: System MUST require an EventId for each outbound webhook event (host-supplied or library-generated GUID when absent).
- **FR-003b**: When EventId is library-generated, it MUST be assigned at broadcast API entry before middleware execution and delivery orchestration.
- **FR-004**: System MUST match events to sinks by exact event type subscription.
- **FR-005**: System MUST support optional payload-based filtering per event subscription using structured field comparisons with per-subscription matching mode (AND or OR).
- **FR-005a**: System MUST require explicit payload matching mode for any subscription that defines one or more payload predicates.
- **FR-005b**: System MUST provide a pluggable payload value comparison strategy used during payload predicate evaluation.
- **FR-005c**: The baseline default comparison strategy MUST support string-equality comparison for scalar values.
- **FR-005d**: System MUST provide a pluggable payload field selector strategy; the baseline default selector MUST support a restricted JsonPath subset.
- **FR-006**: System MUST hand off each matched event/sink pair through the dispatcher invocation coordinator to exactly one selected dispatcher.
- **FR-007**: System MUST include event metadata in outgoing deliveries, including event type, payload, and dispatch timestamp.
- **FR-007a**: System MUST include EventId in outgoing deliveries.
- **FR-008**: System MUST provide a pluggable dispatcher abstraction (`IWebhookDispatcher`) responsible for terminal delivery transport of matched sink invocations.
- **FR-009**: System MUST support resolving one or more dispatcher implementations from DI at runtime.
- **FR-009a**: System MUST provide a dispatcher invocation coordinator service responsible for invoking resolved dispatchers for terminal delivery.
- **FR-009b**: System MUST allow a sink to optionally specify dispatcher selection for its deliveries.
- **FR-009c**: System MUST support an application-level default dispatcher used when a sink does not specify a dispatcher.
- **FR-010**: Retry behavior MUST be scoped per delivery attempt in the invoke plane.
- **FR-010a**: Endpoint invocation retries MUST execute against the Endpoint Invoker HTTP attempt boundary.
- **FR-010b**: Transient-failure detection used for retry decisions MUST be host-configurable.
- **FR-010c**: Endpoint Invoker middleware MUST execute for each retry attempt.
- **FR-010d**: For queued dispatcher modules/integrations, retry execution MUST occur within the worker/consumer-owned delivery attempt lifecycle and MUST NOT rely on implicit whole-message requeue as the retry mechanism.
- **FR-011**: System MUST continue processing other sinks when a single sink delivery fails.
- **FR-011a**: When dispatcher unavailability causes an attempt failure, default behavior MUST record that delivery attempt as failed and continue processing other eligible sinks.
- **FR-012**: Queued dispatcher modules/integrations MUST support configurable queue capacity and worker parallelism where applicable.
- **FR-012a**: Webhooks Core MUST NOT require or embed queue/worker runtime infrastructure; queue/worker capabilities MAY be provided by dispatcher extension modules.
- **FR-013**: System MUST provide a default dispatcher implementation in core that performs direct HTTP endpoint invocation through `IWebhookEndpointInvoker`.
- **FR-014**: System MUST reject subscription configurations that define payload predicates without an explicit payload matching mode.
- **FR-014a**: System MUST support an optional deduplication policy based on EventId with host-configurable scope/retention.
- **FR-014b**: EventId-based deduplication MUST be disabled by default when no host policy is configured.
- **FR-015**: Queued dispatcher modules/integrations MUST allow host applications to configure overflow policy for queued processing.
- **FR-016**: Queued dispatcher modules/integrations MUST default overflow policy to immediate failure when the host application does not override it.
- **FR-017**: The default HTTP dispatcher MUST allow host applications to configure outbound HTTP retry attempts and backoff strategy.
- **FR-018**: The default HTTP dispatcher MUST provide default outbound retry attempts and backoff strategy when host applications do not override retry settings.
- **FR-019**: System MUST allow external modules to add or replace dispatcher implementations (for example Wolverine, RabbitMQ, or MassTransit-based dispatchers).
- **FR-020**: System MUST support a broadcast middleware pipeline that executes once per broadcast operation in the dispatch plane.
- **FR-020a**: Broadcast middleware MUST wrap the full dispatch operation boundary, including matched-sink iteration and dispatcher handoff.
- **FR-021**: System MUST support an Endpoint Invoker middleware pipeline that executes once per endpoint invocation attempt in the invoke plane.
- **FR-021a**: Endpoint Invoker middleware MUST be able to access and mutate outbound HTTP request metadata before invocation.
- **FR-021b**: Endpoint Invoker middleware MUST support pluggable signing/authentication extensions for outbound HTTP requests.
- **FR-022**: System MUST execute middleware components in deterministic host-configured order for both middleware scopes.
- **FR-022a**: Minimum observability per delivery attempt MUST include delivery status, attempt count, final failure reason (when failed), and EventId-based correlation.
- **FR-023**: System MUST route terminal delivery through the dispatcher invocation coordinator after middleware execution.
- **FR-023a**: Delivery success/failure MUST be determined by Endpoint Invoker outcome, not dispatcher handoff outcome.
- **FR-023b**: Dispatcher handoff outcomes MUST be recorded as secondary telemetry and MUST NOT be treated as final delivery success.
- **FR-023c**: For async queued dispatcher modules/integrations, delivery status MAY be `Pending` after handoff and MUST transition to `Succeeded`/`Failed` when Endpoint Invoker outcome is known.
- **FR-024**: System MUST validate dispatcher registration and fail startup/configuration when no dispatchers are configured or coordinator resolution is invalid.
- **FR-025**: System MUST support a Coordinator Invocation Policy (Coordinator-owned) that governs single-dispatcher selection precedence (sink-selected dispatcher when configured, otherwise application default dispatcher).
- **FR-026**: Dispatcher implementations MUST NOT be required to implement coordinator invocation policy semantics.
- **FR-027**: The baseline scope MUST remain webhook broadcasting; generalized event-stream processing features (arbitrary stream processing, stateful workflow orchestration) are out of scope unless explicitly specified in a future feature.

### Key Entities *(include if feature involves data)*

- **Delivery Envelope**: The normalized outbound message used by orchestration, containing EventId, event type, optional payload, and dispatch timestamp.
- **Publish Request**: The host-provided input submitted for broadcasting before normalization into a `Delivery Envelope`.
- **Webhook Sink**: A destination target definition with identifier, name, address details (for example URL for HTTP), and subscription filters.
- **Subscription Criteria**: A sink subscription rule containing event type, optional payload predicate set, and payload matching mode (AND/OR).
- **Payload Predicate**: A structured field comparison condition used to determine sink eligibility for an event, evaluated through the configured field selector and comparison strategies.
- **Webhook Dispatcher**: A pluggable terminal delivery component that performs sink dispatch for matched webhook events.
- **Dispatcher Invocation Coordinator**: A service that receives the resolved dispatcher set and selects exactly one dispatcher for each sink delivery attempt according to configured selection precedence.
- **Dispatch Handoff Result**: Secondary record describing whether a dispatcher accepted/enqueued/rejected a handoff.
- **Broadcast Middleware Component**: A middleware unit executed once per broadcast operation for cross-cutting behavior.
- **Endpoint Invoker Middleware Component**: A middleware unit executed per endpoint invocation attempt for invoke-plane cross-cutting behavior.
- **Coordinator Invocation Policy (Coordinator-owned)**: The selected behavior for choosing exactly one dispatcher per sink delivery attempt using sink override and application default rules.
- **Deduplication Policy**: Optional EventId-based policy that determines duplicate handling behavior and retention scope.
- **Delivery Result**: Structured result for observability that captures delivery status, attempt count, retry progression, final failure reason (when failed), and EventId correlation.

### Dependencies

- For the default HTTP dispatcher, sink URL destinations must be reachable over HTTP from the broadcasting application environment.
- Sink configuration must be available to the application at runtime.
- When a selected dispatcher module uses queued delivery, that module depends on its worker/consumer lifecycle being active.
- Custom dispatcher implementations may require additional runtime dependencies provided by host applications or extension packages.

### Assumptions

- Event type matching is case-sensitive exact matching.
- Payload predicate evaluation uses pluggable field selector and value comparison strategies; baseline defaults are restricted JsonPath field selection and scalar string-equality comparison.
- Event identity is represented by EventId for correlation, idempotency hooks, and duplicate-handling policies.
- Dispatcher retry behavior follows the shared retry contract in this specification to ensure consistent cross-dispatcher semantics.
- The initial baseline scope covers outbound delivery behavior and configuration-driven routing only.
- Authentication, authorization, and endpoint signature verification are handled outside this baseline behavior unless added in future enhancements.
- Retry defaults are safe baseline values intended for broad compatibility and can be overridden by host applications.
- At least one `IWebhookDispatcher` is present in the DI container for each host application instance.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% of events with at least one valid subscription produce at least one delivery attempt to each matching sink.
- **SC-002**: 100% of events with no matching subscriptions result in zero delivery attempts.
- **SC-002a**: 100% of outbound deliveries include EventId in payload/metadata envelope.
- **SC-002b**: In tests where EventId is omitted by the host, a single generated EventId is assigned at broadcast API entry and remains consistent across all sink deliveries and retry attempts for that broadcast.
- **SC-003**: In test runs covering configured dispatcher selection rules, each sink delivery attempt uses the expected dispatcher (sink override when configured, application default otherwise) in 100% of sampled broadcasts.
- **SC-004**: For transient endpoint failures introduced in controlled tests using the default HTTP dispatcher, the dispatcher performs retry attempts per delivery attempt before final failure in 100% of affected deliveries.
- **SC-008**: In conformance tests across dispatcher implementations, retry behavior follows the shared retry contract (per-delivery-attempt retry scope, dispatcher-owned retries, middleware per attempt, and host-configurable transient detection) in 100% of sampled failure scenarios.
- **SC-005**: A single sink failure does not prevent attempted delivery to other matching sinks in 100% of multi-sink failure test runs.
- **SC-006**: In conformance tests, adding or replacing dispatchers through DI changes terminal transport behavior without requiring changes to broadcaster orchestration logic.
- **SC-009**: In conformance tests with multiple registered dispatchers, the dispatcher invocation coordinator selects exactly one dispatcher per sink delivery attempt according to configured selection precedence in 100% of sampled broadcasts.
- **SC-007**: In pipeline conformance tests, configured broadcast middleware and Endpoint Invoker middleware execute in deterministic configured order in 100% of sampled broadcasts.
- **SC-010**: In conformance tests with deduplication enabled, duplicate EventIds are handled according to configured deduplication policy in 100% of sampled duplicate scenarios.
- **SC-011**: In conformance tests across direct and queued dispatcher modules/integrations, final delivery status is sourced from Endpoint Invoker outcomes in 100% of sampled scenarios, while dispatcher handoff is recorded only as secondary telemetry.
