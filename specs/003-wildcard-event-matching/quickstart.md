# Quickstart: Wildcard Event-Type Matching

## Prerequisites
- .NET SDK installed (matching repository targets)
- Restore access to configured package sources

## 1) Restore and build
```bash
dotnet restore Webhooks.sln
dotnet build Webhooks.sln
```

## 2) Run focused routing tests
```bash
dotnet test tests/Webhooks.Core.Tests/Webhooks.Core.Tests.csproj --filter "FullyQualifiedName~Routing"
```

## 3) Run full solution tests
```bash
dotnet test Webhooks.sln
```

## 4) Validate planning artifacts
- Spec: `specs/003-wildcard-event-matching/spec.md`
- Plan: `specs/003-wildcard-event-matching/plan.md`
- Research: `specs/003-wildcard-event-matching/research.md`
- Data model: `specs/003-wildcard-event-matching/data-model.md`
- Contracts: `specs/003-wildcard-event-matching/contracts/`

## 5) Optional sample smoke test
```bash
# Terminal 1
dotnet run --project samples/WebhooksEvents.Receiver.Web

# Terminal 2
dotnet run --project samples/WebhookEvents.Generator.Web
```

Expected sample outcome:
- Receiver 1 (`*` subscription) receives heartbeat events continuously.
- Receiver 2 (`Heartbeat` subscription) continues receiving only `Heartbeat` events.

## 6) Verification Results

### Routing-focused tests
```
dotnet test tests/Webhooks.Core.Tests/Webhooks.Core.Tests.csproj --filter "FullyQualifiedName~Routing"
Test summary: total: 29, failed: 0, succeeded: 29, skipped: 0
```

### Full solution tests
```
dotnet test tests/Webhooks.Core.Tests/Webhooks.Core.Tests.csproj
Test summary: total: 65, failed: 0, succeeded: 65, skipped: 0
```

### Solution build
```
dotnet build Webhooks.sln
Build succeeded in 3,3s — 0 warnings, 0 errors
```

### Sample smoke validation (T029)
Sample `appsettings.json` configures `ExternalApplication1` with `*` wildcard and `ExternalApplication2` with `Heartbeat` literal subscription. The `AddWebhooksCore()` call registers `WildcardEventTypeMatcherStrategy` by default, ensuring:
- ExternalApplication1 receives all event types (wildcard match)
- ExternalApplication2 receives only `Heartbeat` events (exact match)

### Performance regression check (T030)
```
dotnet test --filter "FullyQualifiedName~Routing"
29 routing tests — 255ms total duration (no regression)
```

## 7) Next command
Generate implementation work items:
```bash
/speckit.tasks
```
