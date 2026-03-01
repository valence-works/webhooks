# Contract: Dispatcher and Coordinator

## IWebhookDispatcher (terminal transport contract)
- Handles terminal delivery for a single delivery attempt.
- Owns retry loop for that attempt.
- Uses host-configurable transient-failure classification.
- Must surface final success/failure outcome to coordinator.

## Dispatcher Invocation Coordinator
- Resolves one-or-more dispatcher implementations.
- Applies deterministic invocation policy/order.
- Invokes dispatchers for each delivery attempt after delivery middleware.
- Aggregates dispatcher-level outcomes into delivery result.

## Invariants
- Orchestration policy remains broadcaster-owned.
- Coordinator does not own scheduling semantics.
- Dispatcher does not own broadcast-wide orchestration.
