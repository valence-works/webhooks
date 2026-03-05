# Remediation Decision Register

**Feature**: `002-apply-coding-conventions`  
**Created**: 2026-03-04  
**Schema**: [decision-schema.md](decision-schema.md)

## Summary

| Disposition | Count |
|-------------|-------|
| resolved | 71 |
| deferred | 0 |
| waived | 1 |
| not_applicable | 0 |
| **Total** | **72** |

## Resolved Decisions

All safe remediation batches were applied successfully.

### Batch 1 — Sealed Modifier (CS-005)

| Decision ID | Finding ID | Disposition | Change Ref |
|-------------|------------|-------------|------------|
| D-001 | F-002 | resolved | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs |
| D-002 | F-005 | resolved | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs |
| D-003 | F-010 | resolved | src/Webhooks.Core/Services/BackgroundTaskChannel.cs |
| D-004 | F-012 | resolved | src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs |
| D-005 | F-014 | resolved | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs |
| D-006 | F-017 | resolved | src/Webhooks.Core/Services/DefaultSystemClock.cs |
| D-007 | F-019 | resolved | src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs |
| D-008 | F-021 | resolved | src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs |
| D-009 | F-028 | resolved | src/Webhooks.Core/Models/WebhookSink.cs |
| D-010 | F-031 | resolved | src/Webhooks.Core/Models/DispatchException.cs |
| D-011 | F-033 | resolved | src/Webhooks.Core/Models/WebhookEventFilter.cs (WebhookEventFilter only) |
| D-012 | F-036 | resolved | src/Webhooks.Core/Strategies/* (all 6 strategy classes) |
| D-013 | F-042 | resolved | src/Webhooks.Core/SinkProviders/OptionsWebhookSinkProvider.cs |
| D-014 | F-043 | resolved | src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs (ValidateWebhookSinksOptions) |
| D-015 | F-044 | resolved | src/Webhooks.Core/Options/* (all 6 option/configure classes) |
| D-016 | F-050 | resolved | src/Webhooks.Core/Serialization/Converters/TypeTypeConverter.cs |
| D-017 | F-053 | resolved | src/Webhooks.Core/HostedServices/* (both classes) |
| D-018 | F-055 | resolved | src/Webhooks.Core/Models/* (records: PayloadPredicate, PayloadFilter, NewWebhookEvent) |
| D-019 | F-060 | resolved | src/WebhooksCore.Shared/Models/WebhookEvent.cs (both records) |
| D-020 | F-062 | resolved | src/Webhooks.Core/Models/DeliveryEnvelope.cs, DeliveryResult.cs, DispatchHandoffResult.cs |
| D-021 | F-065 | resolved | tests/ — all 25 test classes sealed |
| D-022 | F-068 | resolved | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs |
| D-023 | F-071 | resolved | (no additional classes to seal in samples receiver) |

### Batch 2 — Field Name Renames (NC-007)

| Decision ID | Finding ID | Disposition | Change Ref |
|-------------|------------|-------------|------------|
| D-024 | F-001 | resolved | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs (12 fields) |
| D-025 | F-004 | resolved | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs (3 fields) |
| D-026 | F-066 | resolved | (no underscore fields found in tests/) |
| D-027 | F-067 | resolved | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs (2 fields) |

### Batch 3 — Readonly Modifier (CS-006)

| Decision ID | Finding ID | Disposition | Change Ref |
|-------------|------------|-------------|------------|
| D-028 | F-003 | resolved | Already readonly; no additional changes needed |
| D-029 | F-009 | resolved | Already readonly; no additional changes needed |

### Batch 4 — Fix Malformed XML Docs (XD-005)

| Decision ID | Finding ID | Disposition | Change Ref |
|-------------|------------|-------------|------------|
| D-030 | F-024 | resolved | src/Webhooks.Core/Contracts/IWebhookEventBroadcaster.cs |
| D-031 | F-035 | resolved | src/Webhooks.Core/Models/WebhookSink.cs |
| D-032 | F-046 | resolved | src/Webhooks.Core/SinkProviders/OptionsWebhookSinkProvider.cs |
| D-033 | F-054 | resolved | src/Webhooks.Core/Options/BackgroundTaskProcessorOptions.cs |
| D-034 | F-056 | resolved | src/Webhooks.Core/Extensions/WebhookEventBroadcasterExtensions.cs |
| D-035 | F-063 | resolved | src/WebhooksCore.Shared/Models/WebhookEvent.cs |

### Batch 5 — Add Missing XML Docs (XD-001)

| Decision ID | Finding ID | Disposition | Change Ref |
|-------------|------------|-------------|------------|
| D-036 | F-007 | resolved | src/Webhooks.Core/Contracts/IBackgroundTaskChannel.cs |
| D-037 | F-011 | resolved | src/Webhooks.Core/Contracts/IWebhookEndpointInvoker.cs |
| D-038 | F-013 | resolved | src/Webhooks.Core/Contracts/IPayloadFieldSelectorStrategy.cs |
| D-039 | F-016 | resolved | src/Webhooks.Core/Contracts/ITransientFailureDetectionStrategy.cs |
| D-040 | F-018 | resolved | src/Webhooks.Core/Contracts/IBackgroundTaskScheduler.cs |
| D-041 | F-020 | resolved | src/Webhooks.Core/Contracts/IDispatcherInvocationCoordinator.cs |
| D-042 | F-022 | resolved | src/Webhooks.Core/Contracts/ISystemClock.cs |
| D-043 | F-023 | resolved | src/Webhooks.Core/Contracts/IBroadcasterStrategy.cs |
| D-044 | F-025 | resolved | src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs |
| D-045 | F-026 | resolved | src/Webhooks.Core/Contracts/IBroadcastMiddleware.cs |
| D-046 | F-029 | resolved | src/Webhooks.Core/Contracts/IPayloadValueComparisonStrategy.cs |
| D-047 | F-032 | resolved | src/Webhooks.Core/Contracts/IWebhookDispatcher.cs |
| D-048 | F-034 | resolved | src/Webhooks.Core/Contracts/IBackgroundTaskProcessor.cs |
| D-049 | F-045 | resolved | src/Webhooks.Core/Contracts/IWebhookSinkProvider.cs (ListAsync) |
| D-050 | F-051 | resolved | src/Webhooks.Core/Models/* (all model types) |
| D-051 | F-061 | resolved | src/Webhooks.Core/Services/* + Strategies/* + HostedServices/* + Serialization/* |
| D-052 | F-069 | resolved | samples/ (EventGenerator) |
| D-053 | F-072 | resolved | src/Webhooks.Core/Extensions/* + Options/* |

## Resolved — Previously Deferred (Breaking Changes Authorized)

### Batch 6 — Async Method Naming (NC-012)

| Decision ID | Finding ID | Disposition | Change Ref |
|-------------|------------|-------------|------------|
| D-054 | F-006 | resolved | src/Webhooks.Core/Contracts/IBackgroundTaskScheduler.cs — `EnqueueWork` → `EnqueueWorkAsync` |
| D-055 | F-015 | resolved | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs — `EnqueueWork` → `EnqueueWorkAsync` |
| D-056 | F-047 | resolved | src/Webhooks.Core/Strategies/BackgroundProcessorBroadcasterStrategy.cs — call site updated |

### Batch 7 — CancellationToken on Interface Methods (CT-001)

| Decision ID | Finding ID | Disposition | Change Ref |
|-------------|------------|-------------|------------|
| D-057 | F-008 | resolved | src/Webhooks.Core/Contracts/IBackgroundTaskProcessor.cs — added CancellationToken to StartAsync |
| D-058 | F-048 | resolved | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs — `Wait` → `WaitAsync`, CancellationToken support |

### Batch 8 — CancellationToken Propagation (CT-002)

| Decision ID | Finding ID | Disposition | Change Ref |
|-------------|------------|-------------|------------|
| D-059 | F-030 | resolved | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs — propagates cancellationToken |
| D-060 | F-052 | resolved | src/Webhooks.Core/Strategies/ParallelTaskBroadcasterStrategy.cs — `.WaitAsync(cancellationToken)` |
| D-061 | F-070 | resolved | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs — captures and propagates stoppingToken |

### Batch 9 — Multi-Type File Splits (FO-001)

| Decision ID | Finding ID | Disposition | Change Ref |
|-------------|------------|-------------|------------|
| D-062 | F-027 | resolved | Extracted `ValidateWebhookSinksOptions` → Extensions/ValidateWebhookSinksOptions.cs |
| D-063 | F-037 | resolved | Extracted `PayloadMatchingMode` → Models/PayloadMatchingMode.cs, `SubscriptionCriteria` → Models/SubscriptionCriteria.cs |
| D-064 | F-038 | resolved | Extracted `DispatchExceptionKind` → Models/DispatchExceptionKind.cs |
| D-065 | F-039 | resolved | Extracted `PayloadPredicate` → Models/PayloadPredicate.cs |
| D-066 | F-040 | resolved | Extracted `DeliveryStatus` → Models/DeliveryStatus.cs |
| D-067 | F-041 | resolved | Extracted `DispatchHandoffStatus` → Models/DispatchHandoffStatus.cs |
| D-068 | F-049 | resolved | Extracted `WebhookEndpointInvocationContext` → Contracts/WebhookEndpointInvocationContext.cs |
| D-069 | F-057 | resolved | Extracted `ConfigureBackgroundTaskProcessorOptions` → Options/ConfigureBackgroundTaskProcessorOptions.cs |
| D-070 | F-058 | resolved | Extracted `ConfigureWebhookSinksOptions` → Options/ConfigureWebhookSinksOptions.cs |
| D-071 | F-059 | resolved | Extracted `OverflowPolicy` → Options/OverflowPolicy.cs, `ConfigureWebhookEventBroadcasterOptions` → Options/ConfigureWebhookEventBroadcasterOptions.cs |

## Waived Decisions

### D-072: WebhookEvent generic variants (FO-001)

| Field | Value |
|-------|-------|
| Finding ID | F-064 |
| Disposition | waived |
| Rationale | `WebhookEvent` and `WebhookEvent<T>` are generic/non-generic variants of the same record. C# convention (cf. `Task`/`Task<T>`, `ILogger`/`ILogger<T>`) co-locates these in a single file. |
| Owner | Core team |
| Target Release | N/A — intentional co-location |
