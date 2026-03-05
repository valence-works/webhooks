# Handover: Wildcard Event-Type Matching & Pluggable Subscription Predicate

**Date:** 2026-03-04  
**Status:** Investigation complete — no code changes made yet  
**Next step:** Spec + design the feature, then implement  

---

## Problem Summary

**Receiver 1 (`ExternalApplication1`) never receives `Heartbeat` events**, even though it subscribes using the wildcard `"*"` event type in `samples/WebhookEvents.Generator.Web/appsettings.json`:

```json
{
  "Id": "ExternalApplication1",
  "Name": "External Application 1",
  "Subscriptions": [{ "EventType": "*" }],
  "Url": "https://localhost:6001/webhooks"
}
```

**Root cause:** Event-type matching in `DefaultWebhookEventBroadcaster` is a plain string equality check:

```csharp
// src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs ~line 73
where eventFilter.EventType == webhookEvent.EventType
```

`"*"` never equals `"Heartbeat"`, so sink A is never matched. Receiver 2 (`ExternalApplication2`, subscribed to `"Heartbeat"`) works correctly.

---

## Confirmed Spec Gap

Analysis run against `specs/002-initial-src-spec/` revealed the following:

| ID | Severity | Summary |
|----|----------|---------|
| C1 | CRITICAL | Constitution Principle II requires replaceable dispatch/comparison behavior. Event-type matching has no strategy interface, unlike payload comparison (`IPayloadValueComparisonStrategy`, `IPayloadFieldSelectorStrategy`). |
| I1 | HIGH | `README.md` + sample config publicly show `"*"` wildcard syntax, but spec FR-004 says **exact event type matching only** — direct contradiction. |
| U1 | HIGH | `spec.md` line 79 says a sink can subscribe to "all currently used event types" but specifies no mechanism (explicit list, `*`, glob, regex). |
| G1 | HIGH | No FR, no task, and no test exists for wildcard or pattern-based event-type matching. |
| G2 | MEDIUM | Pluggable predicate strategy exists for **payload** criteria but not for **subscription / event-type** matching. |

Relevant spec file: `specs/002-initial-src-spec/spec.md` — FR-004 (exact match), FR-005b/c/d (pluggable payload strategies, but no equivalent for event-type).

---

## Relevant Files

| File | Role |
|------|------|
| `src/Webhooks.Core/Services/DefaultWebhookEventBroadcaster.cs` | Contains the hardcoded `==` event-type match (lines ~71–74). The fix goes here. |
| `src/Webhooks.Core/Contracts/` | All pluggable strategy interfaces live here. New `IEventTypeMatcherStrategy` belongs here. |
| `src/Webhooks.Core/Strategies/` | Default strategy implementations. New `WildcardEventTypeMatcherStrategy` (and possibly `ExactEventTypeMatcherStrategy`) go here. |
| `src/Webhooks.Core/Extensions/ServiceCollectionExtensions.cs` | DI registration for all strategies. New strategy must be registered here. |
| `src/Webhooks.Core/Models/WebhookEventFilter.cs` | `SubscriptionCriteria` — the event-type field lives here. No changes needed unless the data model changes. |
| `tests/Webhooks.Core.Tests/Routing/EventTypeRoutingTests.cs` | Existing routing tests (exact match only). New wildcard tests should go here (or a new file alongside it). |
| `samples/WebhookEvents.Generator.Web/appsettings.json` | Already configured with `"*"` for sink 1 and `"Heartbeat"` for sink 2 — the test scenario is ready. |
| `samples/WebhooksEvents.Receiver.Web/Program.cs` | Receiver 1 listens at `POST /webhooks`, prints `EventType + Timestamp`. Receiver 2 at `/webhooks/heartbeat` prints the typed `Heartbeat` payload. |
| `specs/002-initial-src-spec/spec.md` | Contains all FRs. Needs a new/updated FR for wildcard + pluggable event-type matcher. |
| `specs/002-initial-src-spec/tasks.md` | Needs new tasks: contract, default impl, DI wiring, tests, README alignment. |

---

## Proposed Design

### New strategy interface

```csharp
// src/Webhooks.Core/Contracts/IEventTypeMatcherStrategy.cs
namespace Webhooks.Core;

public interface IEventTypeMatcherStrategy
{
    bool IsMatch(string subscriptionEventType, string incomingEventType);
}
```

### Default implementations

| Class | Behavior |
|-------|----------|
| `ExactEventTypeMatcherStrategy` | Current behavior: case-sensitive exact equality. |
| `WildcardEventTypeMatcherStrategy` | `"*"` matches any event type; otherwise falls back to exact equality. |

The wildcard strategy should be the **new default** registered in DI (replacing the hardcoded equality predicate).

### Broadcaster change

Replace the hardcoded equality predicate in `DefaultWebhookEventBroadcaster` with a call to the injected `IEventTypeMatcherStrategy`:

```csharp
// Before
where eventFilter.EventType == webhookEvent.EventType

// After
where _eventTypeMatcher.IsMatch(eventFilter.EventType, webhookEvent.EventType)
```

The strategy is resolved from DI (injected via constructor, same pattern as `IPayloadFieldSelectorStrategy` and `IPayloadValueComparisonStrategy`).

### DI registration

Register `WildcardEventTypeMatcherStrategy` as the default in `AddWebhooksCore()`, allowing hosts to replace it.

---

## Suggested Next Steps

1. **`/speckit.specify`** — Add/refine spec requirements:
   - Update FR-004 to support wildcard and make event-type matcher pluggable.
   - Add acceptance criteria and edge cases (null, empty, casing, `*`, literal strings).
   - Align spec language with `README.md` example.

2. **`/speckit.plan`** — Define the `IEventTypeMatcherStrategy` contract, ownership, and DI wiring.

3. **`/speckit.tasks`** — Generate implementation tasks:
   - Contract file (`IEventTypeMatcherStrategy`)
   - `WildcardEventTypeMatcherStrategy` + `ExactEventTypeMatcherStrategy`
   - Update `DefaultWebhookEventBroadcaster` constructor + matching predicate
   - DI registration in `AddWebhooksCore()`
   - Tests: wildcard match, exact match, no match, edge cases
   - Align `README.md` / docs

4. **`/speckit.implement`** — Execute tasks once spec + plan + tasks are approved.

---

## Quick Smoke Test (once implemented)

Run both sample projects together:

```bash
# Terminal 1 - Receiver (Receiver 1 on 6001, Receiver 2 on 7001)
dotnet run --project samples/WebhooksEvents.Receiver.Web

# Terminal 2 - Generator
dotnet run --project samples/WebhookEvents.Generator.Web
```

Expected: Receiver 1 (`/webhooks`) prints `Heartbeat event received at: ...` every 5 seconds. Receiver 2 (`/webhooks/heartbeat`) prints `Heartbeat at ...` every 5 seconds.

---

## Notes

- No code has been changed — this is a spec gap, not a runtime bug.
- The existing assumption in `spec.md` ("Event type matching is case-sensitive exact matching") must be explicitly updated to reflect wildcard/pluggable behavior.
- The wildcard strategy must remain opt-in via DI replacement if strict exact-match behavior is desired by hosts.

