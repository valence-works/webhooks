# Contract: Dispatcher and Coordinator

## IWebhookDispatcher (terminal transport contract)
- Handles dispatch-plane handoff for a delivery envelope + sink pair.
- May perform direct invoke (default HTTP dispatcher) or enqueue for later invocation (Wolverine/MassTransit/etc.).
- Must emit a `Dispatch Handoff Result` (accepted/enqueued/rejected) as secondary telemetry.
- Must preserve correlation metadata (`EventId`, sink identity) into invoke-plane processing.

## Dispatcher Invocation Coordinator
- Resolves one-or-more dispatcher implementations.
- Applies deterministic dispatcher selection precedence (sink-selected dispatcher when configured, otherwise application default dispatcher).
- Selects and invokes exactly one dispatcher for each matched sink handoff after dispatch-plane middleware.
- Aggregates handoff telemetry only; does not decide final delivery success.

## Delivery Outcome Semantics
- Final business delivery success/failure is determined by Endpoint Invoker outcome in invoke plane.
- Dispatcher handoff success is not equivalent to delivery success.
- For async queued dispatchers, final delivery may remain `Pending` until invoke-plane completion.

## Invariants
- Orchestration policy remains broadcaster-owned.
- Coordinator owns dispatcher invocation policy, not endpoint invocation success semantics.
- Dispatcher does not own broadcast-wide orchestration.
