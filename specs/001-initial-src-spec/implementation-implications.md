# Implementation Implications Checklist

Purpose: Translate clarified spec requirements into concrete planning inputs for `/speckit.plan`.

Terminology baseline: `docs/architecture/vocabulary.md`.

## 1) Contracts & Interfaces

- [ ] Define `IWebhookDispatcher` handoff contract (direct invoke or queued handoff boundary).
- [ ] Define coordinator invocation policy contract (single dispatcher selection per sink delivery attempt).
- [ ] Define dispatch handoff result contract (accepted/enqueued/rejected telemetry).
- [ ] Define broadcast middleware contract (executes once per broadcast operation).
- [ ] Define dispatch-plane delivery middleware contract (executes per handoff attempt).
- [ ] Define Endpoint Invoker middleware contract (executes per HTTP invoke attempt, including retries).
- [ ] Define payload field selector strategy contract (default restricted JsonPath selector).
- [ ] Define payload value comparison strategy contract (default scalar string-equality comparator).
- [ ] Define invoke-plane retry classification/transient-detection contract (host-configurable decision point).
- [ ] Define overflow policy abstraction contract for queued dispatcher module integrations.

## 2) Ownership Boundaries

- [ ] Broadcaster owns: sink matching, dispatch-plane orchestration, middleware pipeline composition.
- [ ] Coordinator owns: dispatcher selection precedence and single-dispatcher resolution per sink delivery attempt.
- [ ] Dispatcher owns: handoff mechanics (direct invoke vs queued enqueue).
- [ ] Endpoint Invoker owns: HTTP invocation attempts and final delivery success/failure outcome.
- [ ] Ensure dispatcher implementations are not required to implement broadcast orchestration semantics.
- [ ] Ensure queued-mode retry semantics stay within extension module worker-owned delivery attempt lifecycle (no implicit whole-message requeue retries).

## 3) Configuration Model

- [ ] Add options for sink-level dispatcher selection and application-level default dispatcher selection.
- [ ] Add abstraction/contract options for queued dispatcher modules to expose queue capacity, worker parallelism, and overflow policy (default fail-fast).
- [ ] Add options for Endpoint Invoker retry attempts/backoff/transient detection.
- [ ] Add options for payload field selector strategy + selector defaults (restricted JsonPath).
- [ ] Add options for payload value comparison strategy + comparator defaults.
- [ ] Add validation rules for:
  - [ ] One or more dispatcher registrations with valid default dispatcher selection.
  - [ ] Sink-level dispatcher selections reference registered dispatchers.
  - [ ] Payload predicate with explicit matching mode requirement.
  - [ ] Invalid payload field-path expressions fail configuration validation.

## 4) DI & Extensibility

- [ ] Register default `DefaultWebhookDispatcher` and support additional named dispatchers.
- [ ] Provide clear replacement path for extension packages (Wolverine/RabbitMQ/MassTransit).
- [ ] Support host registration of custom field selector/value comparator/transient detection strategies.
- [ ] Support host registration of Endpoint Invoker middleware.
- [ ] Ensure middleware ordering is deterministic and host-configurable.

## 5) Pipeline Semantics

- [ ] Broadcast middleware wraps broadcast operation once.
- [ ] Dispatch-plane delivery middleware wraps each dispatcher handoff attempt.
- [ ] Endpoint Invoker middleware wraps each HTTP invoke attempt and each retry attempt.
- [ ] Terminal handoff always routes through dispatcher invocation coordinator after broadcast middleware execution.
- [ ] Final delivery result is determined by Endpoint Invoker outcome (not handoff result).
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
- [ ] Multiple dispatcher selection tests (sink override and application default precedence).

### Retry Semantics
- [ ] Per-delivery-attempt retry scope tests.
- [ ] Endpoint Invoker middleware executes per retry attempt tests.
- [ ] Host-configurable transient detection tests.
- [ ] Queued-mode retry inside extension module worker lifecycle tests.

### Orchestration
- [ ] Direct-dispatch and extension-provided queued-dispatch behavior tests.
- [ ] Overflow policy default fail-fast and override behavior tests.

### Observability & Middleware
- [ ] Deterministic middleware ordering tests (broadcast + delivery).
- [ ] Dispatcher handoff telemetry recorded as secondary outcome tests.
- [ ] Final delivery result sourced from Endpoint Invoker outcome tests.
- [ ] Middleware short-circuit/error propagation tests.

## 7) Migration / Compatibility Notes

- [ ] Maintain backward-compatible default behavior where feasible.
- [ ] Document terminology changes (`Coordinator Invocation Policy`, `Dispatch Plane`, `Invoke Plane`).
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
