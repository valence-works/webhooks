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

## 6) Next command
Generate implementation work items:
```bash
/speckit.tasks
```
