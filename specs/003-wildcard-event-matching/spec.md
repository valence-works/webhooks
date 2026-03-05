# Feature Specification: Wildcard Event-Type Matching

**Feature Branch**: `003-wildcard-event-matching`  
**Created**: 2026-03-05  
**Status**: Draft  
**Input**: User description: "Handover: Wildcard Event-Type Matching & Pluggable Subscription Predicate"

## Clarifications

### Session 2026-03-05

- Q: Should `*` match null/empty incoming event types? → A: Yes, `*` matches any incoming event type, including null/empty/whitespace.
- Q: How should whitespace around subscription event type be handled? → A: Treat leading/trailing-whitespace subscription values as invalid configuration.
- Q: What should happen when an invalid subscription event type is present? → A: Log a warning and treat it as non-match at runtime; continue startup.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Deliver events to wildcard subscribers (Priority: P1)

As an integration owner, I want a subscription with event type `*` to receive every incoming event type so I can onboard downstream systems without maintaining a full event-type list.

**Why this priority**: Current behavior drops events for wildcard subscriptions, breaking a documented sample path and causing missed deliveries.

**Independent Test**: Configure one sink with subscription event type `*` and publish events of multiple event types; the sink receives all of them.

**Acceptance Scenarios**:

1. **Given** a sink subscribed with event type `*`, **When** an event with event type `Heartbeat` is broadcast, **Then** that sink is selected for delivery.
2. **Given** a sink subscribed with event type `*`, **When** events with differing event types are broadcast, **Then** that sink is selected for each event.

---

### User Story 2 - Preserve exact-match routing behavior (Priority: P2)

As an integration owner, I want literal event-type subscriptions to continue matching only their intended event type so existing precise routing stays stable.

**Why this priority**: Backward compatibility is required to avoid introducing unexpected fan-out.

**Independent Test**: Configure one sink with `Heartbeat` and publish `Heartbeat` plus another event type; only `Heartbeat` is routed to that sink.

**Acceptance Scenarios**:

1. **Given** a sink subscribed with event type `Heartbeat`, **When** a `Heartbeat` event is broadcast, **Then** the sink is selected for delivery.
2. **Given** a sink subscribed with event type `Heartbeat`, **When** a non-`Heartbeat` event is broadcast, **Then** the sink is not selected for delivery.

---

### User Story 3 - Allow host-controlled matching policy (Priority: P3)

As a host application owner, I want event-type matching to be replaceable so I can enforce strict exact behavior or custom rules without modifying core library code.

**Why this priority**: Extensibility consistency is required by architecture principles and enables host-specific policy control.

**Independent Test**: Replace the default event-type matcher in host configuration with a custom matcher and verify routing reflects the custom policy.

**Acceptance Scenarios**:

1. **Given** a host-provided event-type matching strategy, **When** events are broadcast, **Then** sink selection follows the host-provided strategy.
2. **Given** no host override, **When** events are broadcast, **Then** sink selection follows the default wildcard-capable strategy.

### Edge Cases

- A subscription event type value that is null, empty, or whitespace does not match any incoming event type.
- A subscription event type value with leading or trailing whitespace is invalid configuration and is treated as non-match at runtime.
- An incoming event type value that is null, empty, or whitespace is not matched by any literal subscription event type.
- A wildcard subscription event type value `*` matches incoming event types including null, empty, or whitespace.
- The wildcard token `*` is treated as wildcard only when used as the full subscription event type value.
- Matching remains case-sensitive for literal event-type comparisons.
- If a host replaces the default matching strategy, wildcard behavior may change according to host policy.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST select a sink for delivery when any of its subscriptions has event type `*`, regardless of the incoming event type value.
- **FR-002**: The system MUST continue to support case-sensitive exact matching for literal subscription event types.
- **FR-003**: The system MUST evaluate event-type matching through a replaceable matching policy rather than fixed comparison logic.
- **FR-004**: The system MUST provide a default event-type matching strategy where `*` matches all incoming event types and non-wildcard values use case-sensitive exact matching.
- **FR-005**: Host applications MUST be able to replace the default event-type matching policy through standard host configuration without modifying core behavior.
- **FR-006**: The system MUST keep payload-criteria filtering behavior unchanged by this feature.
- **FR-007**: The system MUST document wildcard event-type subscription behavior and host override behavior consistently across specification and public documentation.
- **FR-008**: The system MUST include automated routing tests that verify wildcard match, exact match, non-match, and host override behavior.
- **FR-009**: The system MUST treat subscription event type values with leading or trailing whitespace as invalid configuration.
- **FR-010**: When an invalid subscription event type value is encountered, the system MUST emit a warning and continue operation while treating that subscription as non-matching.

### Key Entities *(include if feature involves data)*

- **Subscription Event Type**: The event-type value configured on a sink subscription that determines candidate sink selection for an incoming event.
- **Incoming Event Type**: The event-type value attached to each broadcast event and evaluated against subscription event types.
- **Event-Type Matcher Strategy**: Replaceable policy object that determines whether a subscription event type matches an incoming event type.

### Contract Boundaries *(mandatory for extensible features)*

- **Orchestration Owner**: The event broadcaster coordinates sink selection and applies matching policies before dispatch.
- **Terminal Executor**: The sink dispatch pipeline performs actual delivery once a sink is selected.
- **Extension Points**: Event-type matching policy remains replaceable by host configuration, alongside existing payload-related policy extensions.
- **Retry/Failure Ownership**: Existing dispatch retry/failure ownership remains unchanged; this feature only changes pre-dispatch sink selection criteria.
- **Default vs Override Behavior**: Default behavior supports wildcard plus exact matching; host applications can override with stricter or custom matching policy.

### Assumptions

- Existing subscription schema remains unchanged and continues to use a single event-type field per subscription.
- The wildcard token for this feature is only `*`; additional pattern syntaxes are out of scope.
- Hosts requiring strict exact-only behavior will provide a replacement matcher strategy.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: In sample configuration, the wildcard subscriber receives 100% of broadcast `Heartbeat` events during a 5-minute run.
- **SC-002**: Literal event-type subscribers continue to receive only matching event types with 0 unintended deliveries in routing tests.
- **SC-003**: A host can switch routing policy through configuration and observe expected routing behavior in validation tests.
- **SC-004**: Documentation and specification contain no contradictions about wildcard support or default event-type matching behavior at feature sign-off.
