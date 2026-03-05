# Convention Rules Catalog

**Source**: [`docs/coding-conventions.md`](../../docs/coding-conventions.md)  
**Extracted**: 2026-03-04

Each rule has a stable `ruleId`, title, description, applicability, and default priority per the data model.

---

## General Principles

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| GP-001 | Deterministic behavior | Given the same inputs, dispatch behavior should be predictable | always | high |
| GP-002 | Safe defaults | Default options should favor reliability and correctness | always | high |
| GP-003 | Consumer-first API design | Public types and extension methods should remain clear, stable, and backwards compatible | always | high |
| GP-004 | No secrets in logs | Never log credentials, tokens, signatures, or sensitive payload data | always | high |
| GP-005 | Fail fast for invalid configuration | Invalid options must surface at startup through option validation | always | high |

## Naming Conventions

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| NC-001 | Interface prefix | Interfaces use `I` prefix, PascalCase | always | medium |
| NC-002 | Class naming | Classes use PascalCase, no prefix | always | medium |
| NC-003 | Record naming | Records use PascalCase | always | medium |
| NC-004 | Enum naming | Enums use PascalCase, singular | always | medium |
| NC-005 | Enum member naming | Enum members use PascalCase | always | medium |
| NC-006 | Public property naming | Public properties use PascalCase | always | medium |
| NC-007 | Private field naming | Private fields use camelCase (no underscore prefix) | always | high |
| NC-008 | Parameter naming | Parameters use camelCase | always | medium |
| NC-009 | Local variable naming | Local variables use camelCase | always | low |
| NC-010 | Constant naming | Constants use PascalCase | always | medium |
| NC-011 | Method naming | Methods use PascalCase, verb phrase | always | medium |
| NC-012 | Async method suffix | Async methods use `Async` suffix | always | high |

## File Organization

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| FO-001 | One type per file | One public type per file (except tightly related helper types) | always | medium |
| FO-002 | File name matches type | File name matches primary type name | always | medium |
| FO-003 | Domain folder structure | Keep domain folders focused (Contracts/, Services/, Options/, etc.) | always | medium |
| FO-004 | Extension method location | Place extension methods in Extensions/ with clear names | always | low |

## Code Organization

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| CO-001 | File-scoped namespaces | Use `namespace X;` file-scoped namespaces | always | medium |
| CO-002 | Namespace mirrors folder | Namespaces should mirror folder intent | contextual | medium |
| CO-003 | Using directive order | System → Microsoft → Webhooks → third-party | always | low |
| CO-004 | No unused usings | No unnecessary blank lines or unused using directives | always | low |

## XML Documentation

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| XD-001 | Public API XML docs | Public API surface should include XML documentation | always | high |
| XD-002 | Use see cref | Use `<see cref="..."/>` for type/member cross-references | always | medium |
| XD-003 | Use inheritdoc | Use `<inheritdoc/>` when docs fully inherited and clear | contextual | low |
| XD-004 | Document exceptions | Document thrown exceptions where behavior is not obvious | contextual | medium |
| XD-005 | Well-formed XML | XML docs must use proper tags (`<summary>`, `<param>`, etc.) | always | high |

## Dependency Injection

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| DI-001 | Registration via extensions | Register services through `AddWebhooksCore` and related extension methods | always | medium |
| DI-002 | Interface-based registration | Prefer interface-based registrations for consumers | always | medium |
| DI-003 | Explicit hosted services | Register hosted services explicitly with `AddHostedService<T>()` | always | medium |
| DI-004 | Return IServiceCollection | Extension methods return `IServiceCollection` for fluent chaining | always | medium |

## Configuration & Options

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| CF-001 | Options in Options folder | Options classes belong in `Options/` and are data containers | always | medium |
| CF-002 | Property defaults | Define defaults directly on properties | always | medium |
| CF-003 | Validate with IValidateOptions | Validate options with `IValidateOptions<T>` implementations | always | high |
| CF-004 | Startup validation | Trigger validation during startup through hosted startup validation | always | high |

## Error Handling & Resilience

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| EH-001 | Argument guards | Use argument guards for public entry points | always | high |
| EH-002 | DispatchException taxonomy | Use DispatchException with DispatchExceptionKind to classify failures | always | high |
| EH-003 | Preserve inner exceptions | Preserve inner exceptions where appropriate | always | medium |
| EH-004 | Consistent retry behavior | Apply retry behavior consistently using configured retry attempts | always | high |
| EH-005 | Explicit overflow policy | Keep overflow and queue behavior explicit via OverflowPolicy | always | medium |

## Webhook Dispatch Patterns

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| WP-001 | Composable middleware | Middleware must be composable and order-aware | always | high |
| WP-002 | Middleware calls next once | Middleware must call `next(...)` exactly once unless short-circuiting | always | high |
| WP-003 | No unsafe shared state | Middleware should not mutate shared state unsafely | always | high |
| WP-004 | Strategy-driven execution | Dispatch execution mode is selected through IBroadcasterStrategy | always | medium |
| WP-005 | Deterministic routing | Sink matching should remain deterministic and easy to reason about | always | high |

## Testing

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| TS-001 | Test class naming | Test classes end in `Tests`, reflect capability under test | always | medium |
| TS-002 | Test method naming | Follow `Method_Scenario_ExpectedBehavior` pattern | always | medium |
| TS-003 | Arrange/Act/Assert | Use AAA structure | always | medium |
| TS-004 | Focused tests | Keep each test focused on one behavior | always | medium |
| TS-005 | Explicit assertions | Prefer explicit assertions (Assert.Equal, Assert.Single, etc.) | always | medium |

## Build & Packaging

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| BP-001 | Central package management | Package versions centrally managed via Directory.Packages.props | always | high |
| BP-002 | Nullable enabled | Nullable and ImplicitUsings enabled | always | medium |
| BP-003 | No Version in PackageReference | Do not add Version attributes in src PackageReference items | always | high |

## CancellationToken

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| CT-001 | Last parameter | Accept CancellationToken as last parameter for async methods | always | high |
| CT-002 | Propagate token | Propagate token through async call chains | always | high |
| CT-003 | Check in loops | In potentially long loops, check cancellation | contextual | medium |

## Code Style

| ID | Title | Description | Applicability | Priority |
|----|-------|-------------|---------------|----------|
| CS-001 | Explicit access modifiers | Use explicit access modifiers | always | medium |
| CS-002 | Use var when obvious | Use `var` when right-hand side makes type obvious | always | low |
| CS-003 | Expression-bodied members | Prefer expression-bodied members for trivial one-line members | always | low |
| CS-004 | Accurate nullability | Keep nullability annotations accurate and intentional | always | medium |
| CS-005 | Sealed when no inheritance | Mark types `sealed` when inheritance is not a design requirement | always | high |
| CS-006 | Readonly fields | Mark fields `readonly` when assigned only in constructors | always | medium |
| CS-007 | Avoid unnecessary comments | Code should be self-explanatory | always | low |
