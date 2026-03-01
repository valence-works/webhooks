# Data Model: Baseline Webhook Broadcasting Behavior

## Entities

### 1) Delivery Envelope
- **Purpose**: Represents one outbound publish request entering broadcaster orchestration.
- **Core fields**:
  - `EventId` (required; generated at API entry if omitted)
  - `EventType`
  - `Payload` (optional)
  - `DispatchTimestamp`
- **Constraints**:
  - `EventId` is immutable for the full broadcast lifecycle.

### 2) Webhook Sink
- **Purpose**: Destination configuration receiving matched events.
- **Core fields**:
  - `SinkId`
  - `Destination` (e.g., URL)
  - `Subscriptions`

### 3) Subscription Criteria
- **Purpose**: Event type + optional payload predicates with explicit matching mode.
- **Core fields**:
  - `EventType`
  - `PayloadPredicates[]`
  - `PayloadMatchingMode` (`AND` or `OR`, required when payload predicates exist)
- **Constraints**:
  - Missing matching mode with payload predicates is invalid configuration.

### 4) Delivery Attempt
- **Purpose**: One endpoint invocation attempt for one `(Delivery Envelope, Sink)` pair.
- **Core fields**:
  - `SinkId`
  - `EventId`
  - `SelectedDispatcher`
  - `AttemptMetadata`

### 5) Delivery Result
- **Purpose**: Observability and conformance result for each delivery attempt.
- **Core fields**:
  - `Status` (`Pending` | `Succeeded` | `Failed`)
  - `AttemptCount`
  - `FinalFailureReason` (nullable)
  - `EventIdCorrelation`
  - `RetryProgression`
  - `OutcomeSource` (`EndpointInvoker`)

### 6) Coordinator Invocation Policy
- **Purpose**: Coordinator-owned dispatcher invocation behavior.
- **Values**:
  - `SinkOverrideThenDefault`

### 7) Dispatch Handoff Result
- **Purpose**: Secondary telemetry for dispatcher handoff status.
- **Core fields**:
  - `DispatcherName`
  - `HandoffStatus` (`Accepted` | `Enqueued` | `Rejected`)
  - `HandoffReason` (nullable)
  - `EventIdCorrelation`

### 8) Overflow Policy
- **Purpose**: Behavior when an extension-provided queued dispatcher module reaches capacity.

### 9) Deduplication Policy
- **Purpose**: Optional EventId duplicate handling.
- **Defaults**:
  - Disabled when not configured.

## Delivery Planes

- **Dispatch Plane**: Broadcast orchestration, matching, coordinator invocation policy, and dispatcher handoff.
- **Invoke Plane**: Endpoint Invoker middleware + HTTP invocation + final delivery outcome.
- Queue/worker runtime infrastructure is not a Webhooks Core data-plane component; it is optional and module-owned when queued dispatcher integrations are used.

## Relationships
- One `Delivery Envelope` can match zero-to-many `Webhook Sinks`.
- Each matched sink yields one dispatch handoff through exactly one selected dispatcher.
- Dispatch handoff may be immediate (direct invoke) or queued via module-owned worker/consumer infrastructure.
- Each endpoint invocation attempt emits one primary `Delivery Result`.
- Optional `Dispatch Handoff Result` records are correlated but do not define final delivery success.

## Validation Rules (startup/config)
- At least one `IWebhookDispatcher` implementation must be resolvable.
- Predicate-based subscriptions require explicit matching mode.
- Invalid payload field-path expressions fail validation.
