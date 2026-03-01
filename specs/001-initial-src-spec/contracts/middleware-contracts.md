# Contract: Middleware Pipelines

## Broadcast middleware
- Executes once per broadcast operation.
- Wraps full operation boundary: validation, matching, scheduling, and per-delivery-attempt invocation.

## Delivery middleware
- Executes per delivery attempt.
- Executes per retry attempt boundary.
- Can mutate outbound metadata (including signing/auth extensions) before terminal dispatch.

## Ordering
- Middleware execution order is deterministic and host-configured.

## Error semantics
- Middleware exceptions fail the current delivery attempt unless host policy overrides behavior.
- Sink failure does not block other eligible sinks by default.
