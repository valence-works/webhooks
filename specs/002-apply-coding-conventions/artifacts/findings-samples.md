# Findings: samples/

**Scope**: Manually-authored C# in `samples/`  
**Reviewed**: 2026-03-04  
**Convention Baseline**: `docs/coding-conventions.md`

---

## WebhookEvents.Generator.Web/

### F-067: EventGenerator — underscore-prefixed private fields

| Field | Value |
|-------|-------|
| Rule ID | NC-007 |
| Location | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `_interval` and `_timer` use underscore prefix |

### F-068: EventGenerator — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Class not sealed |

### F-069: EventGenerator — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation on public members |

### F-070: EventGenerator — CancellationToken not propagated

| Field | Value |
|-------|-------|
| Rule ID | CT-002 |
| Location | samples/WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | `BroadcastAsync` call inside `OnTimerTick` doesn't pass `CancellationToken`; `stoppingToken` from `ExecuteAsync` is not propagated |

## WebhookEvents.Shared/

### F-071: Heartbeat — missing sealed

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | samples/WebhookEvents.Shared/Models/Heartbeat.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Record not sealed |

### F-072: Heartbeat — missing XML docs

| Field | Value |
|-------|-------|
| Rule ID | XD-001 |
| Location | samples/WebhookEvents.Shared/Models/Heartbeat.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | No XML documentation |

---

## Compliant Areas (samples/)

| Rule ID | Area | Status |
|---------|------|--------|
| CO-001 | All files | compliant — file-scoped namespaces (or top-level statements) |
| CS-001 | All files | compliant — explicit access modifiers |
| FO-002 | All files | compliant — file names match types |
| CO-003 | Program.cs files | compliant — using order correct |
