# Research: Baseline Webhook Broadcasting Behavior

## Decision 1: Dispatch and invoke are split into two planes
- **Decision**: Architecture is split into dispatch plane (matching + coordinator handoff) and invoke plane (endpoint invocation + final outcome).
- **Rationale**: This cleanly separates handoff mechanics from actual delivery semantics.
- **Alternatives considered**:
  - Single-plane model: rejected because it conflates dispatcher handoff with delivery completion.
  - Dispatcher-owned orchestration: rejected because it couples orchestration policy to transport.

## Decision 2: Delivery success is invoker-outcome-first
- **Decision**: Final delivery success/failure is derived from Endpoint Invoker outcome, while dispatcher handoff is secondary telemetry.
- **Rationale**: A dispatcher enqueue/accept result does not prove endpoint delivery.
- **Alternatives considered**:
  - Handoff-success-as-delivery-success: rejected because it hides async invoke failures.

## Decision 3: Retry ownership is invoke-plane scoped per delivery attempt
- **Decision**: Retries are evaluated at endpoint invocation boundary per delivery attempt.
- **Rationale**: Retry semantics align with the outcome source of truth (HTTP invoke attempts).
- **Alternatives considered**:
  - Global broadcaster retry loop: rejected because it over-couples orchestrator to transport specifics.

## Decision 4: Overflow behavior is host-configurable with fail-fast default
- **Decision**: Overflow policy is configurable by host; baseline default is immediate failure.
- **Rationale**: Fail-fast is deterministic and avoids hidden latency/backpressure side effects when host does not opt into alternate policy.

## Decision 5: EventId deduplication is optional and default-disabled
- **Decision**: Deduplication policy is optional and disabled by default when unspecified.
- **Rationale**: Prevents accidental suppression of legitimate retry/replay scenarios while still enabling idempotency where required.

## Decision 6: Minimum observability contract per delivery attempt
- **Decision**: Record delivery status, attempt count, final failure reason (when failed), and EventId correlation.
- **Rationale**: Provides enough signal for conformance and operational triage without over-prescribing telemetry implementation.

## Decision 7: Payload predicate defaults
- **Decision**: Field selection via restricted JsonPath subset and default scalar string-equality comparison.
- **Rationale**: Keeps baseline predictable while allowing host-provided selector/comparator strategies.

## Decision 8: Dispatcher selection is per-sink single-dispatch
- **Decision**: Allow multiple dispatcher registrations globally, but select exactly one dispatcher for each sink delivery attempt.
- **Rationale**: Avoids ambiguous fan-out semantics while preserving extensibility.
- **Alternatives considered**:
  - Fan-out to all dispatchers for a sink delivery: rejected because it duplicates delivery paths and obscures transport intent.

## Open items intentionally deferred to implementation
- Exact middleware context object shape.
- Concrete exception taxonomy naming.
- Any dead-letter/re-drive extension package behavior.
