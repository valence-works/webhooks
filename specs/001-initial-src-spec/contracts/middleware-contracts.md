# Contract: Middleware Pipelines

## Broadcast middleware
- Executes once per broadcast operation.
- Wraps dispatch-plane boundary: validation, matching, and dispatcher handoff orchestration.

## Dispatch-plane delivery middleware
- Executes per dispatcher handoff attempt.
- Can mutate envelope metadata before dispatcher handoff.

## Endpoint Invoker middleware
- Executes per endpoint invocation attempt.
- Executes per retry attempt boundary.
- Can mutate outbound HTTP request metadata (including signing/auth extensions) before endpoint invocation.

## Ordering
- Middleware execution order is deterministic and host-configured.

## Error semantics
- Middleware exceptions fail the current operation scope unless host policy overrides behavior.
- Sink failure does not block other eligible sinks by default.
