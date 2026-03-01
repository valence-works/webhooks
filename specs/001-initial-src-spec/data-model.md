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
  - `PayloadMatchingMode` (`AND` or `OR`, required when filters exist)
- **Constraints**:
  - Missing matching mode with filters is invalid configuration.

### 4) Delivery Attempt
- **Purpose**: One delivery attempt for one `(Delivery Envelope, Sink)` pair.
- **Core fields**:
  - `SinkId`
  - `EventId`
  - `DispatcherInvocationSet`
  - `AttemptMetadata`

### 5) Delivery Result
- **Purpose**: Observability and conformance result for each delivery attempt.
- **Core fields**:
  - `Status`
  - `AttemptCount`
  - `FinalFailureReason` (nullable)
  - `EventIdCorrelation`
  - `RetryProgression`

### 6) Dispatch Mode
- **Purpose**: Broadcaster-owned scheduling behavior.
- **Values**:
  - `Sequential`
  - `Concurrent`
  - `Queued`
- **Related settings**:
  - `QueueCapacity`
  - `WorkerParallelism`
  - `OverflowPolicy` (host-configurable; default fail-fast)

### 7) Deduplication Policy
- **Purpose**: Optional EventId duplicate handling.
- **Defaults**:
  - Disabled when not configured.

## Relationships
- One `Delivery Envelope` can match zero-to-many `Webhook Sinks`.
- Each matched sink yields one `Delivery Attempt`.
- One delivery attempt can invoke one-to-many dispatchers through coordinator policy.
- Each delivery attempt emits one `Delivery Result`.

## Validation Rules (startup/config)
- At least one dispatcher must be resolvable.
- Predicate-based subscriptions require explicit matching mode.
- Invalid payload field-path expressions fail validation.
