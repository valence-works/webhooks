# Research: Baseline Webhook Broadcasting Behavior

## Decision 1: Dispatcher invocation is coordinator-based
- **Decision**: Broadcaster routes terminal delivery through a dispatcher invocation coordinator, not directly to a single dispatcher.
- **Rationale**: This supports one-or-more dispatchers while keeping orchestration transport-neutral.
- **Alternatives considered**:
  - Single active dispatcher only: rejected because it prevents multi-dispatch use cases and introduces avoidable startup rigidity.
  - Dispatcher-owned orchestration: rejected because it couples scheduling policy to transport.

## Decision 2: Retry ownership is dispatcher-scoped per sink attempt
- **Decision**: Retries run inside dispatcher implementation for each sink delivery attempt.
- **Rationale**: Keeps transport semantics localized and supports transient-failure logic that can vary by transport.
- **Alternatives considered**:
  - Global broadcaster retry loop: rejected because it over-couples orchestrator to transport specifics.

## Decision 3: Queue-full behavior is host-configurable with fail-fast default
- **Decision**: Queue-full policy is configurable by host; baseline default is immediate failure.
- **Rationale**: Fail-fast is deterministic and avoids hidden latency/backpressure side effects when host does not opt into alternate policy.

## Decision 4: EventId deduplication is optional and default-disabled
- **Decision**: Deduplication policy is optional and disabled by default when unspecified.
- **Rationale**: Prevents accidental suppression of legitimate retry/replay scenarios while still enabling idempotency where required.

## Decision 5: Minimum observability contract per sink attempt
- **Decision**: Record delivery status, attempt count, final failure reason (when failed), and EventId correlation.
- **Rationale**: Provides enough signal for conformance and operational triage without over-prescribing telemetry implementation.

## Decision 6: Payload filtering defaults
- **Decision**: Field selection via restricted JsonPath subset and default scalar string-equality comparison.
- **Rationale**: Keeps baseline predictable while allowing host-provided selector/comparator strategies.

## Open items intentionally deferred to implementation
- Exact middleware context object shape.
- Concrete exception taxonomy naming.
- Any dead-letter/re-drive extension package behavior.
