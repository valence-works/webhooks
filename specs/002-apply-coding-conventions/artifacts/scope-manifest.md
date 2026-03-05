# Scope Manifest: Codebase Coding Conventions Review

**Created**: 2026-03-04  
**Rationale**: FR-001, FR-009, Research Decision 1

## Include Paths

| Path | Type | Description |
|------|------|-------------|
| `src/Webhooks.Core/**/*.cs` | Source | Core library — all authored C# |
| `src/WebhooksCore.Shared/**/*.cs` | Source | Shared models — all authored C# |
| `tests/Webhooks.Core.Tests/**/*.cs` | Test | Unit tests — all authored C# |
| `samples/WebhookEvents.Generator.Web/**/*.cs` | Sample | Generator web app — authored C# only |
| `samples/WebhooksEvents.Receiver.Web/**/*.cs` | Sample | Receiver web app — authored C# only |
| `samples/WebhookEvents.Shared/**/*.cs` | Sample | Shared sample models — authored C# only |

## Exclude Paths

| Path | Reason |
|------|--------|
| `**/bin/**` | Build output (generated) |
| `**/obj/**` | Build intermediate output (generated) |
| `**/*.Designer.cs` | Auto-generated designer files |
| `**/*.g.cs` | Source-generated files |
| `**/*.AssemblyInfo.cs` | Auto-generated assembly info |
| `**/GlobalUsings.g.cs` | Auto-generated global usings |
| External/vendor NuGet source | Not project-authored code |

## Scope Decisions

- **Convention baseline**: `docs/coding-conventions.md` — all rules apply unless excluded by ADR/architecture precedence.
- **Architecture precedence**: Any accepted ADR in `docs/adr/` takes precedence over convention rules; such findings are dispositioned as exceptions with rationale.
- **Samples scope**: Only manually-authored C# files in `samples/` are reviewed; `Program.cs` top-level statement files are included but evaluated contextually.
- **Generated code**: Files under `bin/`, `obj/`, and auto-generated source files are excluded from review and remediation.

## File Inventory

### src/Webhooks.Core/ (48 files)

- `Services/` — 8 files
- `Extensions/` — 3 files
- `HostedServices/` — 2 files
- `Models/` — 8 files (includes `DeliveryEnvelope.cs`)
- `Contracts/` — 15 files
- `Strategies/` — 5 files
- `SinkProviders/` — 1 file
- `Options/` — 3 files
- `Serialization/Converters/` — 1 file
- Root — `Webhooks.Core.csproj`, `Webhooks.Core.csproj.DotSettings`

### src/WebhooksCore.Shared/ (1 file)

- `Models/WebhookEvent.cs`

### tests/Webhooks.Core.Tests/ (26 files)

- `Usings.cs` — 1 file
- `Validation/` — 5 files
- `Routing/` — 4 files
- `Dispatch/` — 11 files
- `Middleware/` — 4 files

### samples/ (4 authored C# files)

- `WebhookEvents.Shared/Models/Heartbeat.cs`
- `WebhookEvents.Generator.Web/Program.cs`
- `WebhookEvents.Generator.Web/HostedServices/EventGenerator.cs`
- `WebhooksEvents.Receiver.Web/Program.cs`
