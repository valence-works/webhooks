# Data Model: Baseline Webhook Broadcasting Behavior

## Entities

### 1) Broadcast Event
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

### 3) Webhook Event Filter
- **Purpose**: Event type + optional payload filters with explicit matching mode.
- **Core fields**:
  - `EventType`
  - `PayloadFilters[]`
  - `PayloadMatchingMode` (`AND` or `OR`, required when filters exist)
- **Constraints**:
  - Missing matching mode with filters is invalid configuration.

### 4) Sink Delivery Attempt
- **Purpose**: One delivery attempt for one `(Broadcast Event, Sink)` pair.
- **Core fields**:
  - `SinkId`
  - `EventId`
  - `DispatcherInvocationSet`
  - `AttemptMetadata`

### 5) Delivery Outcome Record
- **Purpose**: Observability and conformance result for each sink attempt.
- **Core fields**:
  - `Status`
  - `AttemptCount`
  - `FinalFailureReason` (nullable)
  - `EventIdCorrelation`
  - `RetryProgression`

### 6) Delivery Orchestration Policy
- **Purpose**: Broadcaster-owned scheduling behavior.
- **Values**:
  - `Sequential`
  - `Concurrent`
  - `Queued`
- **Related settings**:
  - `QueueCapacity`
  - `WorkerParallelism`
  - `QueueFullPolicy` (host-configurable; default fail-fast)

### 7) Deduplication Policy
- **Purpose**: Optional EventId duplicate handling.
- **Defaults**:
  - Disabled when not configured.

## Relationships
- One `Broadcast Event` can match zero-to-many `Webhook Sinks`.
- Each matched sink yields one `Sink Delivery Attempt`.
- One sink attempt can invoke one-to-many dispatchers through coordinator policy.
- Each sink attempt emits one `Delivery Outcome Record`.

## Validation Rules (startup/config)
- At least one dispatcher must be resolvable.
- Payload-filtered subscriptions require explicit matching mode.
- Invalid payload field-path expressions fail validation.
