# Finding Register: Master Consolidated List

**Feature**: `002-apply-coding-conventions`  
**Created**: 2026-03-04  
**Sources**: [findings-src.md](findings-src.md), [findings-tests.md](findings-tests.md), [findings-samples.md](findings-samples.md)

## Summary Statistics

| Metric | Count |
|--------|-------|
| Total Findings | 72 |
| Non-Compliant | 64 |
| Compliant Areas | 8 (documented in source findings) |
| Not Applicable | 0 |

### By Priority

| Priority | Count |
|----------|-------|
| High | 59 |
| Medium | 13 |
| Low | 0 |

### By Category

| Category | Rule IDs | Findings | Priority |
|----------|----------|----------|----------|
| Missing `sealed` | CS-005 | F-002, F-005, F-010, F-012, F-014, F-017, F-019, F-021, F-028, F-031, F-033, F-036, F-042, F-043, F-044, F-050 (×5), F-053, F-055 (×5), F-060, F-062 (×2), F-065 (×24), F-068, F-071 | High |
| Missing XML docs | XD-001 | F-007, F-011, F-013, F-016, F-018, F-020, F-022, F-023, F-025, F-026, F-029, F-032, F-034, F-045, F-051, F-061, F-069, F-072 | High |
| Malformed XML docs | XD-005 | F-024, F-035, F-046, F-054, F-056, F-063 | High |
| Underscore private fields | NC-007 | F-001, F-004, F-066, F-067 | High |
| Missing Async suffix | NC-012 | F-006, F-015, F-047 | High |
| Multiple types per file | FO-001 | F-027, F-037, F-038, F-039, F-040, F-041, F-049, F-057, F-058, F-059, F-064 | Medium |
| Missing CancellationToken | CT-001 | F-008, F-048 | High |
| CancellationToken not propagated | CT-002 | F-030, F-052, F-070 | High |
| Readonly fields | CS-006 | F-003, F-009 | Medium |

---

## Master Register

### High-Priority Applicable Findings

| ID | Rule | Location | Category | Applicability |
|----|------|----------|----------|---------------|
| F-001 | NC-007 | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs | Underscore fields | applicable |
| F-002 | CS-005 | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs | Missing sealed | applicable |
| F-004 | NC-007 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | Underscore fields | applicable |
| F-005 | CS-005 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | Missing sealed | applicable |
| F-006 | NC-012 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | Missing Async suffix | applicable |
| F-007 | XD-001 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | Missing XML docs | applicable |
| F-008 | CT-001 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | Missing CancellationToken | applicable |
| F-010 | CS-005 | src/Webhooks.Core/Services/BackgroundTaskChannel.cs | Missing sealed | applicable |
| F-011 | XD-001 | src/Webhooks.Core/Services/BackgroundTaskChannel.cs | Missing XML docs | applicable |
| F-012 | CS-005 | src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs | Missing sealed | applicable |
| F-013 | XD-001 | src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs | Missing XML docs | applicable |
| F-014 | CS-005 | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs | Missing sealed | applicable |
| F-015 | NC-012 | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs | Missing Async suffix | applicable |
| F-016 | XD-001 | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs | Missing XML docs | applicable |
| F-017 | CS-005 | src/Webhooks.Core/Services/DefaultSystemClock.cs | Missing sealed | applicable |
| F-018 | XD-001 | src/Webhooks.Core/Services/DefaultSystemClock.cs | Missing XML docs | applicable |
| F-019 | CS-005 | src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs | Missing sealed | applicable |
| F-020 | XD-001 | src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs | Missing XML docs | applicable |
| F-021 | CS-005 | src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs | Missing sealed | applicable |
| F-022 | XD-001 | src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs | Missing XML docs | applicable |
| F-023 | XD-001 | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs | Missing XML docs | applicable |
| F-024 | XD-005 | src/Webhooks.Core/Extensions/WebhookEventBroadcasterExtensions.cs | Malformed XML | applicable |
| F-025 | XD-001 | src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs | Missing XML docs | applicable |
| F-026 | XD-001 | src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs | Missing XML docs | applicable |
| F-028 | CS-005 | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs | Missing sealed | applicable |
| F-029 | XD-001 | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs | Missing XML docs | applicable |
| F-030 | CT-002 | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs | CT not propagated | applicable |
| F-031 | CS-005 | src/Webhooks.Core/HostedServices/ValidateOptionsOnStart.cs | Missing sealed | applicable |
| F-032 | XD-001 | src/Webhooks.Core/HostedServices/ValidateOptionsOnStart.cs | Missing XML docs | applicable |
| F-033 | CS-005 | src/Webhooks.Core/Models/NewWebhookEvent.cs | Missing sealed | applicable |
| F-034 | XD-001 | src/Webhooks.Core/Models/*.cs | Missing XML docs | applicable |
| F-035 | XD-005 | src/Webhooks.Core/Models/WebhookSink.cs | Malformed XML | applicable |
| F-036 | CS-005 | src/Webhooks.Core/Models/WebhookSink.cs | Missing sealed | applicable |
| F-042 | CS-005 | src/Webhooks.Core/Models/WebhookEventFilter.cs | Missing sealed | applicable |
| F-043 | CS-005 | src/Webhooks.Core/Models/PayloadFilter.cs | Missing sealed | applicable |
| F-044 | CS-005 | src/Webhooks.Core/Models/DispatchException.cs | Missing sealed | applicable |
| F-045 | XD-001 | src/Webhooks.Core/Contracts/*.cs | Missing XML docs | applicable |
| F-046 | XD-005 | src/Webhooks.Core/Contracts/IWebhookEventBroadcaster.cs + 2 | Malformed XML | applicable |
| F-047 | NC-012 | src/Webhooks.Core/Contracts/IBackgroundTaskScheduler.cs | Missing Async suffix | applicable |
| F-048 | CT-001 | src/Webhooks.Core/Contracts/IBackgroundTaskProcessor.cs | Missing CancellationToken | applicable |
| F-050 | CS-005 | src/Webhooks.Core/Strategies/*.cs | Missing sealed | applicable |
| F-051 | XD-001 | src/Webhooks.Core/Strategies/*.cs | Missing XML docs | applicable |
| F-052 | CT-002 | src/Webhooks.Core/Strategies/ParallelTaskBroadcasterStrategy.cs | CT not propagated | applicable |
| F-053 | CS-005 | src/Webhooks.Core/SinkProviders/OptionsWebhookSinkProvider.cs | Missing sealed | applicable |
| F-054 | XD-005 | src/Webhooks.Core/SinkProviders/OptionsWebhookSinkProvider.cs | Malformed XML | applicable |
| F-055 | CS-005 | src/Webhooks.Core/Options/*.cs | Missing sealed | applicable |
| F-056 | XD-005 | src/Webhooks.Core/Options/BackgroundTaskProcessorOptions.cs | Malformed XML | applicable |
| F-060 | CS-005 | src/Webhooks.Core/Serialization/Converters/TypeTypeConverter.cs | Missing sealed | applicable |
| F-061 | XD-001 | src/Webhooks.Core/Serialization/Converters/TypeTypeConverter.cs | Missing XML docs | applicable |
| F-062 | CS-005 | src/WebhooksCore.Shared/Models/WebhookEvent.cs | Missing sealed | applicable |
| F-063 | XD-005 | src/WebhooksCore.Shared/Models/WebhookEvent.cs | Malformed XML | applicable |
| F-065 | CS-005 | tests/Webhooks.Core.Tests/**/*.cs | Missing sealed (24 classes) | applicable |
| F-066 | NC-007 | tests/Webhooks.Core.Tests/Middleware/EndpointInvokerMiddlewareRetryTests.cs | Underscore fields | applicable |
| F-067 | NC-007 | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs | Underscore fields | applicable |
| F-068 | CS-005 | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs | Missing sealed | applicable |
| F-069 | XD-001 | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs | Missing XML docs | applicable |
| F-070 | CT-002 | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs | CT not propagated | applicable |
| F-071 | CS-005 | samples/WebhookEvents.Shared/Models/Heartbeat.cs | Missing sealed | applicable |
| F-072 | XD-001 | samples/WebhookEvents.Shared/Models/Heartbeat.cs | Missing XML docs | applicable |

### Medium-Priority Findings

| ID | Rule | Location | Category | Applicability |
|----|------|----------|----------|---------------|
| F-003 | CS-006 | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs | Readonly fields | applicable |
| F-009 | CS-006 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | Readonly fields | applicable |
| F-027 | FO-001 | src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs | Multiple types | applicable |
| F-037 | FO-001 | src/Webhooks.Core/Models/WebhookEventFilter.cs | Multiple types | applicable |
| F-038 | FO-001 | src/Webhooks.Core/Models/DispatchException.cs | Multiple types | contextual |
| F-039 | FO-001 | src/Webhooks.Core/Models/PayloadFilter.cs | Multiple types | applicable |
| F-040 | FO-001 | src/Webhooks.Core/Models/DeliveryResult.cs | Multiple types | contextual |
| F-041 | FO-001 | src/Webhooks.Core/Models/DispatchHandoffResult.cs | Multiple types | contextual |
| F-049 | FO-001 | src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs | Multiple types | contextual |
| F-057 | FO-001 | src/Webhooks.Core/Options/BackgroundTaskProcessorOptions.cs | Multiple types | applicable |
| F-058 | FO-001 | src/Webhooks.Core/Options/WebhookSinksOptions.cs | Multiple types | applicable |
| F-059 | FO-001 | src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs | Multiple types | applicable |
| F-064 | FO-001 | src/WebhooksCore.Shared/Models/WebhookEvent.cs | Multiple types | contextual |
