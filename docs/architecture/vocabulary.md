# Webhooks Core Vocabulary

Purpose: Provide a single repository-level source of truth for domain terminology used across specs, plans, tasks, and implementation notes.

## Canonical Terms

- **Publish Request**: Host-provided input submitted to the broadcast API.
- **Delivery Envelope**: Normalized outbound message used by orchestration after input normalization.
- **Subscription Criteria**: Subscription-side matching rule (event type + optional payload predicates + matching mode).
- **Payload Predicate**: Field comparison condition used during subscription matching.
- **Coordinator Invocation Policy**: Dispatcher-coordinator policy that determines how exactly one dispatcher is selected per sink delivery attempt (sink override when configured, otherwise application default dispatcher).
- **Dispatch Handoff Result**: Secondary telemetry indicating dispatcher handoff outcome (accepted/enqueued/rejected).
- **Delivery Attempt**: One HTTP invoke attempt for one `(Delivery Envelope, Sink)` pair executed by the Endpoint Invoker.
- **Delivery Result**: Primary outcome derived from Endpoint Invoker execution (success/failure, attempt count, failure reason, correlation).
- **Overflow Policy**: Configurable behavior when extension-provided queued dispatcher modules reach capacity.
- **Dispatch Plane**: Components responsible for matching and handoff (`Broadcaster`, `Dispatcher Invocation Coordinator`, dispatchers).
- **Invoke Plane**: Components responsible for actual endpoint invocation (`Endpoint Invoker` + invoker middleware + HTTP transport).

## Deprecated Terms

These terms should not be reintroduced in new specs or implementation artifacts:

- `Broadcast Input`, `Broadcast Request`, `Broadcast Event`
- `Webhook Event Filter`, `Subscription Match Rule`, `Payload Filter`
- `Sink Delivery Attempt`, `Delivery Outcome Record`
- `Delivery Orchestration Policy`, `queue-full policy`
- `Dispatch Mode` (when used for dispatcher invocation policy)

## Usage Rules

1. New feature specs SHOULD use canonical terms from this document.
2. Feature-specific terms MAY be introduced only when needed and MUST be documented in the feature spec.
3. When legacy terms appear in historical docs, new edits SHOULD replace them with canonical equivalents.
