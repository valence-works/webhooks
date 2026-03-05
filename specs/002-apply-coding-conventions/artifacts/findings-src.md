# Findings: src/

**Scope**: `src/Webhooks.Core/**/*.cs`, `src/WebhooksCore.Shared/**/*.cs`  
**Reviewed**: 2026-03-04  
**Convention Baseline**: `docs/coding-conventions.md`

---

## Services/ (8 files)

### F-001: DefaultWebhookEventBroadcaster — underscore-prefixed private fields

| Field | Value |
|-------|-------|
| Rule ID | NC-007 |
| Location | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | 12 private fields use `_` prefix (`_webhookSinkProvider`, `_dispatcherInvocationCoordinator`, `_systemClock`, `_strategy`, `_logger`, `_selector`, `_comparator`, `_broadcastMiddlewares`, `_broadcasterOptions`, `_seenEventIds`, `_seenEventIdOrder`, `_eventIdGate`), violating camelCase-no-underscore convention |
| Evidence | Fields declared with `_` prefix throughout the class |

### F-002: DefaultWebhookEventBroadcaster — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not marked `sealed`; no design need for inheritance |

### F-003: DefaultWebhookEventBroadcaster — readonly fields

| Field | Value |
|-------|-------|
| Rule ID | CS-006 |
| Location | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | applicable |
| Impact | `_seenEventIds`, `_seenEventIdOrder`, `_eventIdGate` are assigned only at declaration but not marked `readonly` |

### F-004: ChannelBackgroundTaskProcessor — underscore-prefixed private fields

| Field | Value |
|-------|-------|
| Rule ID | NC-007 |
| Location | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `_tasks`, `_isStarted`, `_cts` use underscore prefix |

### F-005: ChannelBackgroundTaskProcessor — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-006: ChannelBackgroundTaskProcessor — missing Async suffix on Wait

| Field | Value |
|-------|-------|
| Rule ID | NC-012 |
| Location | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `Wait()` returns async but lacks `Async` suffix |

### F-007: ChannelBackgroundTaskProcessor — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation on public members |

### F-008: ChannelBackgroundTaskProcessor — missing CancellationToken

| Field | Value |
|-------|-------|
| Rule ID | CT-001 |
| Location | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `StartAsync()` and `Wait()` do not accept CancellationToken |

### F-009: ChannelBackgroundTaskProcessor — readonly on _tasks

| Field | Value |
|-------|-------|
| Rule ID | CS-006 |
| Location | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | applicable |
| Impact | `_tasks` should be marked `readonly` |

### F-010: BackgroundTaskChannel — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Services/BackgroundTaskChannel.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-011: BackgroundTaskChannel — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Services/BackgroundTaskChannel.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation on public members |

### F-012: DefaultWebhookDispatcher — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-013: DefaultWebhookDispatcher — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation on public members |

### F-014: ChannelBackgroundTaskScheduler — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-015: ChannelBackgroundTaskScheduler — missing Async suffix on EnqueueWork

| Field | Value |
|-------|-------|
| Rule ID | NC-012 |
| Location | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `EnqueueWork` is async but lacks `Async` suffix |

### F-016: ChannelBackgroundTaskScheduler — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation on public members |

### F-017: DefaultSystemClock — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Services/DefaultSystemClock.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-018: DefaultSystemClock — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Services/DefaultSystemClock.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation on public members |

### F-019: DispatcherInvocationCoordinator — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-020: DispatcherInvocationCoordinator — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation on public members |

### F-021: HttpWebhookEndpointInvoker — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-022: HttpWebhookEndpointInvoker — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation on public members |

### F-023: DefaultWebhookEventBroadcaster — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Partial XML docs — class-level only, missing on constructor params and methods |

---

## Extensions/ (3 files)

### F-024: WebhookEventBroadcasterExtensions — malformed XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-005 |
| Location | src/Webhooks.Core/Extensions/WebhookEventBroadcasterExtensions.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Uses `///` without `<summary>` tags — bare `/// Broadcasts...` instead of `/// <summary>Broadcasts...</summary>` |

### F-025: ServiceCollectionExtensions — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML docs on `AddWebhooksCore` or `AddWebhooksBackgroundProcessor` public extension methods |

### F-026: WebhookEventBroadcasterOptionsExtensions — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML docs on public extension methods |

### F-027: WebhookEventBroadcasterOptionsExtensions — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | applicable |
| Impact | `ValidateWebhookSinksOptions` class defined alongside extension class. Convention: one public type per file |

---

## HostedServices/ (2 files)

### F-028: StartBackgroundProcessor — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-029: StartBackgroundProcessor — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation |

### F-030: StartBackgroundProcessor — CancellationToken not propagated

| Field | Value |
|-------|-------|
| Rule ID | CT-002 |
| Location | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `cancellationToken` parameter received but not propagated to `backgroundTaskProcessor.StartAsync()` or `StopAsync()` |

### F-031: ValidateOptionsOnStart — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/HostedServices/ValidateOptionsOnStart.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-032: ValidateOptionsOnStart — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/HostedServices/ValidateOptionsOnStart.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation |

---

## Models/ (8 files)

### F-033: NewWebhookEvent — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Models/NewWebhookEvent.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Record not marked `sealed` |

### F-034: Models — missing XML docs (bulk)

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Models/*.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | All model files lack XML documentation: DeliveryAttempt.cs, NewWebhookEvent.cs, DeliveryEnvelope.cs, DeliveryResult.cs, DispatchHandoffResult.cs, PayloadFilter.cs |

### F-035: WebhookSink — malformed XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-005 |
| Location | src/Webhooks.Core/Models/WebhookSink.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Uses `///` without `<summary>` tags |

### F-036: WebhookSink — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Models/WebhookSink.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-037: WebhookEventFilter.cs — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Models/WebhookEventFilter.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | applicable |
| Impact | `PayloadMatchingMode` enum, `SubscriptionCriteria` class, and `WebhookEventFilter` class in same file |

### F-038: DispatchException.cs — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Models/DispatchException.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | contextual |
| Impact | `DispatchExceptionKind` enum and `DispatchException` class in same file. Convention allows tightly related helper types — this is borderline |
| Rationale | The enum is tightly coupled to the exception class. Consider contextual exception per convention note. |

### F-039: PayloadFilter.cs — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Models/PayloadFilter.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | applicable |
| Impact | `PayloadPredicate` and `PayloadFilter` records in same file |

### F-040: DeliveryResult.cs — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Models/DeliveryResult.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | contextual |
| Impact | `DeliveryStatus` enum and `DeliveryResult` record in same file. Tightly related. |

### F-041: DispatchHandoffResult.cs — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Models/DispatchHandoffResult.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | contextual |
| Impact | `DispatchHandoffStatus` enum and `DispatchHandoffResult` record in same file. Tightly related. |

### F-042: WebhookEventFilter — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Models/WebhookEventFilter.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `SubscriptionCriteria` and `WebhookEventFilter` classes not sealed |

### F-043: PayloadFilter — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Models/PayloadFilter.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Records not marked `sealed` |

### F-044: DispatchException — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Models/DispatchException.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Exception class not sealed |

---

## Contracts/ (15 files)

### F-045: Contracts — missing XML docs (bulk)

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Contracts/*.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | 10 of 15 contracts have no XML documentation: IBackgroundTaskChannel, IWebhookEndpointInvoker, IPayloadFieldSelectorStrategy, ITransientFailureDetectionStrategy, IBackgroundTaskScheduler, IDispatcherInvocationCoordinator, ISystemClock, IBroadcasterStrategy, IWebhookEndpointInvokerMiddleware, IBroadcastMiddleware, IPayloadValueComparisonStrategy, IBackgroundTaskProcessor |

### F-046: Contracts — malformed XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-005 |
| Location | src/Webhooks.Core/Contracts/IWebhookEventBroadcaster.cs, IWebhookDispatcher.cs, IWebhookSinkProvider.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | 3 files use `///` without `<summary>` tags |

### F-047: IBackgroundTaskScheduler — missing Async suffix on EnqueueWork

| Field | Value |
|-------|-------|
| Rule ID | NC-012 |
| Location | src/Webhooks.Core/Contracts/IBackgroundTaskScheduler.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `EnqueueWork` returns `Task` but lacks `Async` suffix |

### F-048: IBackgroundTaskProcessor — missing CancellationToken

| Field | Value |
|-------|-------|
| Rule ID | CT-001 |
| Location | src/Webhooks.Core/Contracts/IBackgroundTaskProcessor.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `StartAsync()` and `StopAsync()` return `ValueTask` but don't accept `CancellationToken` |

### F-049: IWebhookEndpointInvokerMiddleware.cs — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | contextual |
| Impact | Contains interface + `WebhookEndpointInvocationContext` record. Tightly related — the context is the middleware's input. |

---

## Strategies/ (5 files)

### F-050: Strategies — missing sealed (bulk)

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Strategies/*.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | All 5 strategy classes not sealed: ScalarStringEqualityComparisonStrategy, DefaultTransientFailureDetectionStrategy, ParallelTaskBroadcasterStrategy, BackgroundProcessorBroadcasterStrategy, JsonPathPayloadFieldSelectorStrategy |

### F-051: Strategies — missing XML docs (bulk)

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Strategies/*.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | All 5 strategy classes lack complete XML documentation |

### F-052: ParallelTaskBroadcasterStrategy — CancellationToken not propagated

| Field | Value |
|-------|-------|
| Rule ID | CT-002 |
| Location | src/Webhooks.Core/Strategies/ParallelTaskBroadcasterStrategy.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `cancellationToken` accepted but not propagated to invocation calls |

---

## SinkProviders/ (1 file)

### F-053: OptionsWebhookSinkProvider — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/SinkProviders/OptionsWebhookSinkProvider.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-054: OptionsWebhookSinkProvider — malformed XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-005 |
| Location | src/Webhooks.Core/SinkProviders/OptionsWebhookSinkProvider.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Uses `///` without `<summary>` tags. Also uses `<see ref=...>` instead of `<see cref=...>` |

---

## Options/ (3 files)

### F-055: Options — missing sealed (bulk)

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Options/*.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | All option classes and their Configure companions not sealed |

### F-056: BackgroundTaskProcessorOptions — malformed XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-005 |
| Location | src/Webhooks.Core/Options/BackgroundTaskProcessorOptions.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Uses `///` without `<summary>` tags |

### F-057: BackgroundTaskProcessorOptions — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Options/BackgroundTaskProcessorOptions.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | applicable |
| Impact | Options class + Configure class in same file |

### F-058: WebhookSinksOptions — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Options/WebhookSinksOptions.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | applicable |
| Impact | Options class + Configure class in same file |

### F-059: WebhookBroadcasterOptions — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | applicable |
| Impact | Options + OverflowPolicy enum + Configure class in same file |

---

## Serialization/ (1 file)

### F-060: TypeTypeConverter — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/Webhooks.Core/Serialization/Converters/TypeTypeConverter.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-061: TypeTypeConverter — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | src/Webhooks.Core/Serialization/Converters/TypeTypeConverter.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation (has `[PublicAPI]` attribute but no docs) |

---

## WebhooksCore.Shared/ (1 file)

### F-062: WebhookEvent — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | src/WebhooksCore.Shared/Models/WebhookEvent.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Both `WebhookEvent` and `WebhookEvent<T>` records not sealed |

### F-063: WebhookEvent — malformed XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-005 |
| Location | src/WebhooksCore.Shared/Models/WebhookEvent.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Uses `///` without `<summary>` tags |

### F-064: WebhookEvent — multiple types in file

| Field | Value |
|-------|-------|
| Rule ID | FO-001 |
| Location | src/WebhooksCore.Shared/Models/WebhookEvent.cs |
| Status | non_compliant |
| Priority | medium |
| Applicability | contextual |
| Impact | Two records (`WebhookEvent` and `WebhookEvent<T>`) in same file. Tightly related generic/non-generic pair. |

---

## Compliant Areas (src/)

| Rule ID | Area | Status |
|---------|------|--------|
| CO-001 | All files | compliant — file-scoped namespaces used throughout |
| CO-003 | All files | compliant — using directive order correct |
| CS-001 | All files | compliant — explicit access modifiers used |
| NC-001 | All interfaces | compliant — `I` prefix, PascalCase |
| NC-002 | All classes | compliant — PascalCase, no prefix |
| NC-011 | All methods | compliant — PascalCase, verb phrases |
| FO-002 | All files | compliant — file names match primary types |
| FO-003 | Folder structure | compliant — domain folders focused |
| BP-001 | Package management | compliant — centrally managed |
| TS-002 | N/A (src) | N/A |
