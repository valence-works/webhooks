# Feature Specification: Baseline Webhook Broadcasting Behavior

**Feature Branch**: `001-initial-src-spec`  
**Created**: 2026-02-28  
**Status**: Draft  
**Input**: User description: "Create an initial specification based on the existing source code in src. FYI: the plan is to then enhance that spec and update the current source code, accordingly."

## Clarifications

### Session 2026-02-28

- Q: For sink subscriptions with multiple payload filters, how should matching work? → A: Support both modes per subscription (configurable AND/OR).
- Q: If a subscription has payload filters but no matching mode is specified, what should happen? → A: Reject configuration as invalid (mode required).
- Q: In queued background mode, what should happen when the queue is at capacity? → A: Host app can configure policy; default is Fail.
- Q: How should outbound HTTP retry behavior be specified? → A: Host app configures attempts and backoff strategy, with library defaults.

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

**Why this priority**: Payload filtering prevents noisy deliveries and allows finer-grained routing without introducing separate event types for every business variant.

**Independent Test**: Configure one sink with payload rules and publish two events of the same type with different payload values; verify only matching payloads are delivered.

**Acceptance Scenarios**:

1. **Given** a sink subscribed to `InvoiceCreated` with a payload rule `Region=EU`, **When** one `InvoiceCreated` event is broadcast with `Region=EU` and another with `Region=US`, **Then** only the EU-matching event is delivered to that sink.
2. **Given** a sink subscription with no payload rules, **When** events of the subscribed type are broadcast with varied payloads, **Then** all events of that type are eligible for delivery.

---

### User Story 3 - Choose delivery execution mode (Priority: P3)

As an application owner, I can select how deliveries are executed (in order, concurrently, or queued) so behavior can be tuned for throughput and operational control.

**Why this priority**: Execution mode affects throughput, ordering expectations, and operational resilience for different environments.

**Independent Test**: Run the same broadcast workload under each execution mode and verify that delivery attempts are performed according to the selected mode.

**Acceptance Scenarios**:

1. **Given** sequential execution mode, **When** a broadcast targets multiple sinks, **Then** delivery attempts are performed one sink at a time.
2. **Given** concurrent execution mode, **When** a broadcast targets multiple sinks, **Then** delivery attempts are performed in parallel.
3. **Given** queued execution mode with background processing enabled, **When** a broadcast targets multiple sinks, **Then** delivery attempts are enqueued and processed by background workers.

### Edge Cases

- What happens when an event has no matching sink subscriptions? The broadcast completes without endpoint delivery attempts.
- What happens when a sink endpoint fails transiently? The system retries delivery attempts before final failure.
- What happens when a sink endpoint ultimately fails? Failure is recorded as an error and does not stop attempts to other sinks.
- What happens when a payload rule references a field missing in the event payload? That sink is not considered a successful match for that rule set.
- What happens when queued execution receives more work than immediate processing capacity? Behavior follows host-configured queue-full policy; default behavior is fail-fast.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST allow registering webhook sinks with a unique identifier, destination URL, and one or more event subscriptions.
- **FR-002**: System MUST allow defining webhook sources with supported event types for discovery and governance.
- **FR-003**: System MUST accept new outbound webhook events consisting of an event type and optional payload.
- **FR-004**: System MUST match events to sinks by exact event type subscription.
- **FR-005**: System MUST support optional payload-based filtering per event subscription using key/value match rules with per-subscription matching mode (AND or OR).
- **FR-005a**: System MUST require explicit payload matching mode for any subscription that defines one or more payload filters.
- **FR-006**: System MUST attempt HTTP delivery of each matched event to each matched sink URL.
- **FR-007**: System MUST include event metadata in outgoing deliveries, including event type, payload, and dispatch timestamp.
- **FR-008**: System MUST support three delivery execution modes: sequential, concurrent, and queued background processing.
- **FR-009**: System MUST allow selecting the delivery execution mode through configuration.
- **FR-010**: System MUST retry transient delivery failures before reporting final failure.
- **FR-011**: System MUST continue processing other sinks when a single sink delivery fails.
- **FR-012**: System MUST provide configurable queue capacity and worker parallelism for queued background processing.
- **FR-013**: System MUST validate configured delivery mode and reject invalid mode selections during startup/configuration validation.
- **FR-014**: System MUST reject subscription configurations that define payload filters without an explicit payload matching mode.
- **FR-015**: System MUST allow host applications to configure queue-full handling policy for queued background processing.
- **FR-016**: System MUST default queue-full handling policy to immediate failure when the host application does not override it.
- **FR-017**: System MUST allow host applications to configure outbound HTTP retry attempts and backoff strategy.
- **FR-018**: System MUST provide default outbound retry attempts and backoff strategy when host applications do not override retry settings.

### Key Entities *(include if feature involves data)*

- **Webhook Event (Outbound Request)**: A message containing event type, optional payload, and dispatch timestamp sent to sinks.
- **New Webhook Event**: The input message submitted by the source application for broadcasting.
- **Webhook Sink**: A destination endpoint with identifier, name, URL, and subscription filters.
- **Webhook Event Filter**: A sink subscription rule containing event type, optional payload filter set, and payload matching mode (AND/OR).
- **Payload Filter**: A key/value match condition used to determine sink eligibility for an event.
- **Webhook Source**: A source definition with identifier, origin, and declared event types.
- **Broadcaster Mode**: The selected execution behavior for sink invocation (sequential, concurrent, queued).

### Dependencies

- Destination endpoints must be reachable over HTTP from the broadcasting application environment.
- Sink and source configuration must be available to the application at runtime.
- Background execution mode depends on a running background processor lifecycle.

### Assumptions

- Event type matching is case-sensitive exact matching.
- Payload filtering compares string representations of payload values.
- The initial baseline scope covers outbound delivery behavior and configuration-driven routing only.
- Authentication, authorization, and endpoint signature verification are handled outside this baseline behavior unless added in future enhancements.
- Retry defaults are safe baseline values intended for broad compatibility and can be overridden by host applications.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% of events with at least one valid subscription produce at least one delivery attempt to each matching sink.
- **SC-002**: 100% of events with no matching subscriptions result in zero sink delivery attempts.
- **SC-003**: In test runs covering all three execution modes, each mode exhibits its expected behavior (ordered single-attempt flow, parallel attempt flow, or queued background flow) in 100% of sampled broadcasts.
- **SC-004**: For transient endpoint failures introduced in controlled tests, the system performs retry attempts before final failure in 100% of affected deliveries.
- **SC-005**: A single sink failure does not prevent attempted delivery to other matching sinks in 100% of multi-sink failure test runs.
