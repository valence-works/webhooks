# Finding-to-Rule Traceability Matrix

**Feature**: `001-apply-coding-conventions`  
**Created**: 2026-03-04  
**Rationale**: FR-008

## Purpose

Provide bidirectional traceability from each compliance finding to its originating convention rule, and from each rule to all findings that reference it.

## Matrix Format

### By Finding

| Finding ID | Rule ID | Location | Status | Disposition |
|------------|---------|----------|--------|-------------|
| F-001 | NC-007 | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs | non_compliant | _pending_ |
| F-002 | CS-005 | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs | non_compliant | _pending_ |
| F-003 | CS-006 | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs | non_compliant | _pending_ |
| F-004 | NC-007 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | non_compliant | _pending_ |
| F-005 | CS-005 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | non_compliant | _pending_ |
| F-006 | NC-012 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | non_compliant | _pending_ |
| F-007 | XD-001 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | non_compliant | _pending_ |
| F-008 | CT-001 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | non_compliant | _pending_ |
| F-009 | CS-006 | src/Webhooks.Core/Services/ChannelBackgroundTaskProcessor.cs | non_compliant | _pending_ |
| F-010 | CS-005 | src/Webhooks.Core/Services/BackgroundTaskChannel.cs | non_compliant | _pending_ |
| F-011 | XD-001 | src/Webhooks.Core/Services/BackgroundTaskChannel.cs | non_compliant | _pending_ |
| F-012 | CS-005 | src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs | non_compliant | _pending_ |
| F-013 | XD-001 | src/Webhooks.Core/Services/DefaultWebhookDispatcher.cs | non_compliant | _pending_ |
| F-014 | CS-005 | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs | non_compliant | _pending_ |
| F-015 | NC-012 | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs | non_compliant | _pending_ |
| F-016 | XD-001 | src/Webhooks.Core/Services/ChannelBackgroundTaskScheduler.cs | non_compliant | _pending_ |
| F-017 | CS-005 | src/Webhooks.Core/Services/DefaultSystemClock.cs | non_compliant | _pending_ |
| F-018 | XD-001 | src/Webhooks.Core/Services/DefaultSystemClock.cs | non_compliant | _pending_ |
| F-019 | CS-005 | src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs | non_compliant | _pending_ |
| F-020 | XD-001 | src/Webhooks.Core/Services/DispatcherInvocationCoordinator.cs | non_compliant | _pending_ |
| F-021 | CS-005 | src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs | non_compliant | _pending_ |
| F-022 | XD-001 | src/Webhooks.Core/Services/HttpWebhookEndpointInvoker.cs | non_compliant | _pending_ |
| F-023 | XD-001 | src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs | non_compliant | _pending_ |
| F-024 | XD-005 | src/Webhooks.Core/Extensions/WebhookEventBroadcasterExtensions.cs | non_compliant | _pending_ |
| F-025 | XD-001 | src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs | non_compliant | _pending_ |
| F-026 | XD-001 | src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs | non_compliant | _pending_ |
| F-027 | FO-001 | src/Webhooks.Core/Extensions/WebhookEventBroadcasterOptionsExtensions.cs | non_compliant | _pending_ |
| F-028 | CS-005 | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs | non_compliant | _pending_ |
| F-029 | XD-001 | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs | non_compliant | _pending_ |
| F-030 | CT-002 | src/Webhooks.Core/HostedServices/StartBackgroundProcessor.cs | non_compliant | _pending_ |
| F-031 | CS-005 | src/Webhooks.Core/HostedServices/ValidateOptionsOnStart.cs | non_compliant | _pending_ |
| F-032 | XD-001 | src/Webhooks.Core/HostedServices/ValidateOptionsOnStart.cs | non_compliant | _pending_ |
| F-033 | CS-005 | src/Webhooks.Core/Models/NewWebhookEvent.cs | non_compliant | _pending_ |
| F-034 | XD-001 | src/Webhooks.Core/Models/*.cs | non_compliant | _pending_ |
| F-035 | XD-005 | src/Webhooks.Core/Models/WebhookSink.cs | non_compliant | _pending_ |
| F-036 | CS-005 | src/Webhooks.Core/Models/WebhookSink.cs | non_compliant | _pending_ |
| F-037 | FO-001 | src/Webhooks.Core/Models/WebhookEventFilter.cs | non_compliant | _pending_ |
| F-038 | FO-001 | src/Webhooks.Core/Models/DispatchException.cs | non_compliant | _pending_ |
| F-039 | FO-001 | src/Webhooks.Core/Models/PayloadFilter.cs | non_compliant | _pending_ |
| F-040 | FO-001 | src/Webhooks.Core/Models/DeliveryResult.cs | non_compliant | _pending_ |
| F-041 | FO-001 | src/Webhooks.Core/Models/DispatchHandoffResult.cs | non_compliant | _pending_ |
| F-042 | CS-005 | src/Webhooks.Core/Models/WebhookEventFilter.cs | non_compliant | _pending_ |
| F-043 | CS-005 | src/Webhooks.Core/Models/PayloadFilter.cs | non_compliant | _pending_ |
| F-044 | CS-005 | src/Webhooks.Core/Models/DispatchException.cs | non_compliant | _pending_ |
| F-045 | XD-001 | src/Webhooks.Core/Contracts/*.cs | non_compliant | _pending_ |
| F-046 | XD-005 | src/Webhooks.Core/Contracts/IWebhookEventBroadcaster.cs+2 | non_compliant | _pending_ |
| F-047 | NC-012 | src/Webhooks.Core/Contracts/IBackgroundTaskScheduler.cs | non_compliant | _pending_ |
| F-048 | CT-001 | src/Webhooks.Core/Contracts/IBackgroundTaskProcessor.cs | non_compliant | _pending_ |
| F-049 | FO-001 | src/Webhooks.Core/Contracts/IWebhookEndpointInvokerMiddleware.cs | non_compliant | _pending_ |
| F-050 | CS-005 | src/Webhooks.Core/Strategies/*.cs | non_compliant | _pending_ |
| F-051 | XD-001 | src/Webhooks.Core/Strategies/*.cs | non_compliant | _pending_ |
| F-052 | CT-002 | src/Webhooks.Core/Strategies/ParallelTaskBroadcasterStrategy.cs | non_compliant | _pending_ |
| F-053 | CS-005 | src/Webhooks.Core/SinkProviders/OptionsWebhookSinkProvider.cs | non_compliant | _pending_ |
| F-054 | XD-005 | src/Webhooks.Core/SinkProviders/OptionsWebhookSinkProvider.cs | non_compliant | _pending_ |
| F-055 | CS-005 | src/Webhooks.Core/Options/*.cs | non_compliant | _pending_ |
| F-056 | XD-005 | src/Webhooks.Core/Options/BackgroundTaskProcessorOptions.cs | non_compliant | _pending_ |
| F-057 | FO-001 | src/Webhooks.Core/Options/BackgroundTaskProcessorOptions.cs | non_compliant | _pending_ |
| F-058 | FO-001 | src/Webhooks.Core/Options/WebhookSinksOptions.cs | non_compliant | _pending_ |
| F-059 | FO-001 | src/Webhooks.Core/Options/WebhookBroadcasterOptions.cs | non_compliant | _pending_ |
| F-060 | CS-005 | src/Webhooks.Core/Serialization/Converters/TypeTypeConverter.cs | non_compliant | _pending_ |
| F-061 | XD-001 | src/Webhooks.Core/Serialization/Converters/TypeTypeConverter.cs | non_compliant | _pending_ |
| F-062 | CS-005 | src/WebhooksCore.Shared/Models/WebhookEvent.cs | non_compliant | _pending_ |
| F-063 | XD-005 | src/WebhooksCore.Shared/Models/WebhookEvent.cs | non_compliant | _pending_ |
| F-064 | FO-001 | src/WebhooksCore.Shared/Models/WebhookEvent.cs | non_compliant | _pending_ |
| F-065 | CS-005 | tests/Webhooks.Core.Tests/**/*.cs (24 classes) | non_compliant | _pending_ |
| F-066 | NC-007 | tests/Webhooks.Core.Tests/Middleware/EndpointInvokerMiddlewareRetryTests.cs | non_compliant | _pending_ |
| F-067 | NC-007 | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs | non_compliant | _pending_ |
| F-068 | CS-005 | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs | non_compliant | _pending_ |
| F-069 | XD-001 | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs | non_compliant | _pending_ |
| F-070 | CT-002 | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs | non_compliant | _pending_ |
| F-071 | CS-005 | samples/WebhookEvents.Shared/Models/Heartbeat.cs | non_compliant | _pending_ |
| F-072 | XD-001 | samples/WebhookEvents.Shared/Models/Heartbeat.cs | non_compliant | _pending_ |

### By Rule

| Rule ID | Rule Title | Findings | Non-Compliant |
|---------|-----------|----------|---------------|
| CS-005 | Sealed when no inheritance | F-002,005,010,012,014,017,019,021,028,031,033,036,042,043,044,050,053,055,060,062,065,068,071 | 23 entries (45+ classes) |
| XD-001 | Public API XML docs | F-007,011,013,016,018,020,022,023,025,026,029,032,034,045,051,061,069,072 | 18 entries |
| XD-005 | Well-formed XML | F-024,035,046,054,056,063 | 6 |
| NC-007 | Private field naming | F-001,004,066,067 | 4 (18 fields) |
| NC-012 | Async method suffix | F-006,015,047 | 3 |
| FO-001 | One type per file | F-027,037,038,039,040,041,049,057,058,059,064 | 11 |
| CT-001 | CT last parameter | F-008,048 | 2 |
| CT-002 | Propagate CT | F-030,052,070 | 3 |
| CS-006 | Readonly fields | F-003,009 | 2 |

## Coverage Summary

| Metric | Count |
|--------|-------|
| Total rules in catalog | 54 |
| Rules with findings | 9 |
| Rules fully compliant | 45 |
| Total findings | 72 |
| Findings linked to rules | 72 |
| Unlinked findings | 0 |

## Traceability Validation

1. Every finding MUST reference a valid rule ID from `convention-rules.md`.
2. Every rule that applies to in-scope code MUST have at least one finding.
3. No finding may exist without a rule linkage.
