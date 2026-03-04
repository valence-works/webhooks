# Webhooks Coding Conventions

This document defines coding standards and conventions for the Webhooks project. All contributors should follow these guidelines to keep the codebase consistent, maintainable, and safe for package consumers.

## Table of Contents

- [General Principles](#general-principles)
- [Naming Conventions](#naming-conventions)
- [Code Organization](#code-organization)
- [XML Documentation](#xml-documentation)
- [Dependency Injection](#dependency-injection)
- [Configuration & Options](#configuration--options)
- [Error Handling & Resilience](#error-handling--resilience)
- [Webhook Dispatch Patterns](#webhook-dispatch-patterns)
- [Testing](#testing)
- [Build & Packaging](#build--packaging)
- [CancellationToken](#cancellationtoken)
- [Code Style Quick Reference](#code-style-quick-reference)

---

## General Principles

1. **Deterministic behavior where possible** — Given the same inputs (event, sink configuration, strategy), dispatch behavior should be predictable.
2. **Safe defaults** — Default options should favor reliability and correctness (for example, conservative queue overflow behavior and bounded retry attempts).
3. **Consumer-first API design** — Public types and extension methods should remain clear, stable, and backwards compatible.
4. **No secrets in logs** — Never log credentials, tokens, signatures, or sensitive payload data.
5. **Fail fast for invalid configuration** — Invalid options must surface at startup through option validation.

---

## Naming Conventions

### Types

| Kind | Convention | Example |
|------|-----------|---------|
| Interface | `I` prefix, PascalCase | `IWebhookEventBroadcaster`, `IWebhookSinkProvider` |
| Class | PascalCase, no prefix | `DefaultWebhookEventBroadcaster`, `DispatcherInvocationCoordinator` |
| Record | PascalCase | `WebhookEvent`, `DeliveryEnvelope` |
| Enum | PascalCase, singular | `OverflowPolicy`, `DispatchExceptionKind` |
| Enum member | PascalCase | `OverflowPolicy.FailFast` |

### Members

| Kind | Convention | Example |
|------|-----------|---------|
| Public property | PascalCase | `RetryAttempts`, `QueueCapacity` |
| Private field | camelCase (no underscore prefix) | `sinkProvider`, `middlewares` |
| Parameter | camelCase | `cancellationToken`, `webhookSink` |
| Local variable | camelCase | `deliveryEnvelope`, `dispatchResult` |
| Constant | PascalCase | `DefaultRetryAttempts` |
| Method | PascalCase, verb phrase | `BroadcastAsync`, `DispatchAsync` |
| Async method | `Async` suffix | `InvokeAsync`, `ListAsync` |

### Files

- One public type per file (except tightly related helper types).
- File name matches primary type name (for example `DefaultWebhookDispatcher.cs`).
- Keep domain folders focused (`Contracts/`, `Services/`, `Options/`, `Strategies/`, `HostedServices/`, `SinkProviders/`).
- Place extension methods in `Extensions/` and use clear names (for example `ServiceCollectionExtensions.cs`).

---

## Code Organization

### Namespace Layout

Use file-scoped namespaces (`namespace X;`). Namespaces should mirror folder intent:

```text
Webhooks.Core                 — DI entry points and core runtime behavior
Webhooks.Core.Contracts       — Public and internal interfaces
Webhooks.Core.Services        — Service implementations
Webhooks.Core.Options         — Options and validators
Webhooks.Core.Strategies      — Broadcast and comparison strategies
Webhooks.Core.HostedServices  — Startup/background hosted services
Webhooks.Core.SinkProviders   — Sink resolution providers
Webhooks.Core.Extensions      — Extension methods
WebhooksCore                  — Shared event contracts (from WebhooksCore.Shared)
Webhooks.Core.Tests.*         — Capability-oriented test namespaces
```

### Using Directives Order

Order `using` directives as follows:

1. `System.*` namespaces
2. `Microsoft.*` namespaces
3. `Webhooks.*` / `WebhooksCore` namespaces
4. Other third-party namespaces

No unnecessary blank lines or unused `using` directives.

---

## XML Documentation

- Public API surface should include XML documentation, especially package-facing contracts, options, and extension methods.
- Use `<see cref="..."/>` for type/member cross-references.
- Use `<inheritdoc/>` when implementation docs are fully inherited and clear.
- Document thrown exceptions where behavior is not obvious.
- Internal/private members may omit docs when intent is self-evident.

> Note: `CS1591` is currently suppressed in build settings, but documentation quality is still a project standard for public APIs.

---

## Dependency Injection

### Registration Pattern

- Register services through `AddWebhooksCore` and related extension methods.
- Prefer interface-based registrations for consumers (`IWebhookEventBroadcaster`, `IWebhookDispatcher`, etc.).
- Register hosted services explicitly (`AddHostedService<T>()`) when behavior depends on application lifetime.

Example:

```csharp
services.AddSingleton<IWebhookEventBroadcaster, DefaultWebhookEventBroadcaster>();
services.AddSingleton<IWebhookDispatcher, DefaultWebhookDispatcher>();
services.AddHostedService<ValidateOptionsOnStart>();
```

### Lifetime Rules

| Type | Lifetime | Reason |
|------|----------|--------|
| Stateless services / coordinators | `Singleton` | Shared runtime components |
| Options validation | `Singleton` | Validation logic reused by options system |
| Hosted services | `AddHostedService<T>` | Framework-managed startup/background behavior |

### Extension Method Conventions

- Keep DI setup in focused `IServiceCollection` extension methods.
- Return `IServiceCollection` for fluent chaining.
- Accept optional delegates only when needed for customization (for example custom `HttpClient` builder configuration).

---

## Configuration & Options

- Options classes belong in `Options/` and are data containers.
- Define defaults directly on properties.
- Validate options with `IValidateOptions<T>` implementations.
- Trigger validation during startup through hosted startup validation (`ValidateOptionsOnStart`) or equivalent startup checks.
- Do not hide option validation in ad-hoc runtime checks when it can be validated once at startup.

Example expectations:

- `RetryAttempts` must be greater than `0`.
- `QueueCapacity` and `WorkerParallelism` must be positive when configured.
- `BroadcasterStrategy` must implement `IBroadcasterStrategy`.

---

## Error Handling & Resilience

### Validation & Guard Clauses

- Use argument guards for public entry points (`ArgumentNullException.ThrowIfNull(...)`).
- Throw `ArgumentException` for invalid argument values.
- Throw `InvalidOperationException` when current state does not allow an operation.

### Dispatch Exception Taxonomy

- Use `DispatchException` with `DispatchExceptionKind` to classify webhook dispatch failures.
- Keep failure type information machine-readable for caller behavior and telemetry.
- Preserve inner exceptions where appropriate.

### Retry & Failure Strategy

- Apply retry behavior consistently using configured retry attempts.
- Treat transient network failures differently from permanent configuration errors.
- Keep overflow and queue behavior explicit via `OverflowPolicy`.

---

## Webhook Dispatch Patterns

### Middleware Pipeline

- Middleware components (`IBroadcastMiddleware`, `IWebhookEndpointInvokerMiddleware`) must be composable and order-aware.
- Middleware must call `next(...)` exactly once unless intentionally short-circuiting.
- Middleware should not mutate shared state unsafely.

### Strategy-Based Execution

- Dispatch execution mode is selected through `IBroadcasterStrategy` implementations (for example sequential, parallel, background processor).
- Strategy selection must be configuration-driven and validated.

### Routing & Filtering

- Sink matching should remain deterministic and easy to reason about.
- Payload selector/comparison strategies must be isolated in strategy interfaces (`IPayloadFieldSelectorStrategy`, `IPayloadValueComparisonStrategy`).

---

## Testing

### Test Project Layout

```text
tests/
	Webhooks.Core.Tests/
		Dispatch/
		Middleware/
		Routing/
		Validation/
```

### Test Naming

- Test classes should end in `Tests` and reflect the capability under test (for example `BroadcastMiddlewareOrderingTests`).
- Test methods should follow `Method_Scenario_ExpectedBehavior` and may use underscores for readability.

### Test Structure

- Use Arrange / Act / Assert.
- Keep each test focused on one behavior.
- Prefer explicit assertions (`Assert.Equal`, `Assert.Single`, `Assert.Empty`, `Assert.ThrowsAsync<T>`).

---

## Build & Packaging

- The project currently targets multiple frameworks (`net6.0` through `net10.0`) in source projects where configured.
- `Nullable` and `ImplicitUsings` are enabled.
- Package versions in `src/` are centrally managed via `Directory.Packages.props`; do not add `Version` attributes in `src` project `PackageReference` items.
- Keep source projects packable only when intended as distributable packages.
- Use conditional package version entries only when framework-specific dependency versions are required.

---

## CancellationToken

- Accept `CancellationToken` as the last parameter for async methods.
- Propagate the token through async call chains.
- In potentially long loops, check cancellation (`ThrowIfCancellationRequested()`).
- Use `CancellationToken.None` only at boundaries where no token is available (commonly in tests).

---

## Code Style Quick Reference

- Use explicit access modifiers.
- Use `var` when the right-hand side makes the type obvious.
- Prefer expression-bodied members for trivial one-line members.
- Use file-scoped namespaces.
- Keep nullability annotations accurate and intentional.
- Mark types `sealed` when inheritance is not a design requirement.
- Mark fields `readonly` when assigned only in constructors.
- Avoid unnecessary comments; code should be self-explanatory.
