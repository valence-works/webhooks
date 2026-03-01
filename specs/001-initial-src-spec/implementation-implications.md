# Implementation Implications Checklist

Purpose: Translate clarified spec requirements into concrete planning inputs for `/speckit.plan`.

Terminology baseline: `docs/architecture/vocabulary.md`.

## 1) Contracts & Interfaces

- [ ] Define `IWebhookDispatcher` terminal contract (single-sink delivery invocation boundary).
- [ ] Define broadcast middleware contract (executes once per broadcast operation).
- [ ] Define delivery middleware contract (executes once per delivery attempt, including retries).
- [ ] Define payload field selector strategy contract (default restricted JsonPath selector).
- [ ] Define payload value comparison strategy contract (default scalar string-equality comparator).
- [ ] Define retry classification/transient-detection contract (host-configurable decision point).
- [ ] Define overflow policy contract for broadcaster orchestration.

## 2) Ownership Boundaries

- [ ] Broadcaster owns: sink matching, orchestration mode (sequential/concurrent/queued), middleware pipeline composition.
- [ ] Dispatcher owns: terminal transport, retry loop per delivery attempt, transient classification integration.
- [ ] Ensure dispatcher implementations are not required to implement orchestration policy semantics.
- [ ] Ensure queued-mode retry semantics stay within worker-owned delivery attempt lifecycle (no implicit whole-message requeue retries).

## 3) Configuration Model

- [ ] Add options for Dispatch Mode (`sequential|concurrent|queued`).
- [ ] Add options for queue capacity, worker parallelism, overflow policy (default fail-fast).
- [ ] Add options for default HTTP dispatcher retry attempts/backoff/transient detection.
- [ ] Add options for payload field selector strategy + selector defaults (restricted JsonPath).
- [ ] Add options for payload value comparison strategy + comparator defaults.
- [ ] Add validation rules for:
  - [ ] Exactly one active dispatcher registration.
  - [ ] Payload predicate with explicit matching mode requirement.
  - [ ] Invalid payload field-path expressions fail configuration validation.

## 4) DI & Extensibility

- [ ] Register exactly one active `IWebhookDispatcher` by default (HTTP dispatcher).
- [ ] Provide clear replacement path for extension packages (Wolverine/RabbitMQ/MassTransit).
- [ ] Support host registration of custom field selector/value comparator/transient detection strategies.
- [ ] Ensure middleware ordering is deterministic and host-configurable.

## 5) Pipeline Semantics

- [ ] Broadcast middleware wraps broadcast operation once.
- [ ] Delivery middleware wraps each delivery attempt and each retry attempt.
- [ ] Terminal call always routes through installed dispatcher after middleware execution.
- [ ] Error behavior: one sink failure should not block other matching sinks by default.

## 6) Test Plan Inputs

### Routing & Filtering
- [ ] Exact event-type matching tests for `Subscription Criteria`.
- [ ] AND/OR payload matching mode tests.
- [ ] Missing payload field-path -> non-match tests.
- [ ] Invalid payload field-path -> startup/configuration validation failure tests.

### Dispatcher & Extensibility
- [ ] Default dispatcher conformance tests.
- [ ] Custom dispatcher substitution via DI tests (no broadcaster replacement required).
- [ ] Single active dispatcher validation tests (missing/multiple invalid).

### Retry Semantics
- [ ] Per-delivery-attempt retry scope tests.
- [ ] Middleware executes per retry attempt tests.
- [ ] Host-configurable transient detection tests.
- [ ] Queued-mode retry inside worker lifecycle tests.

### Orchestration
- [ ] Sequential/concurrent/queued mode behavior tests.
- [ ] Overflow policy default fail-fast and override behavior tests.

### Observability & Middleware
- [ ] Deterministic middleware ordering tests (broadcast + delivery).
- [ ] Middleware short-circuit/error propagation tests.

## 7) Migration / Compatibility Notes

- [ ] Maintain backward-compatible default behavior where feasible.
- [ ] Document terminology changes (`Dispatch Mode (Broadcaster-owned)`).
- [ ] Add migration notes for strategy-based code paths to orchestration-policy model.

## 8) Open Decisions to Confirm During Planning

- [ ] Restricted JsonPath subset definition (supported operators/expressions).
- [ ] Default transient detection heuristic for HTTP dispatcher.
- [ ] Standardized middleware context payload shape.
- [ ] Exception taxonomy for validation vs runtime dispatch failures.

## 9) Recommended Defaults (Planning Baseline)

- **JsonPath subset (v1)**: Support root `$`, dot notation (`$.a.b`), array index (`[0]`), and wildcard (`[*]`); exclude filters/functions/scripts.
- **Transient detection (default HTTP dispatcher)**: Treat network errors, timeouts, and HTTP `5xx`/`429` as transient; treat other `4xx` as non-transient.
- **Middleware context shape**: Use a single immutable logical context object plus mutable per-attempt metadata bag (attempt number, sink identifier, correlation id, timestamp).
- **Exception taxonomy**: Use configuration-validation exceptions for startup invalid state, and runtime dispatch exceptions categorized as transient/non-transient delivery failures.

These defaults are intended to reduce planning churn and can be overridden by explicit host or extension-package decisions.
