# main Development Guidelines

Auto-generated from all feature plans. Last updated: 2026-03-01

## Active Technologies
- C# with multi-targeted .NET (`net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`) + `Microsoft.Extensions.Hosting.Abstractions`, `Microsoft.Extensions.Http`, `Microsoft.Extensions.Http.Polly`, `Microsoft.Extensions.Logging`, `Polly`/`Polly.Extensions.Http` (001-initial-src-spec)
- In-memory runtime configuration/state in core; queue/outbox infrastructure only when provided by dispatcher extension modules (001-initial-src-spec)
- C# on .NET (`src` multi-targets `net6.0;net7.0;net8.0;net9.0;net10.0`; tests on `net8.0`; samples on `net10.0`) + `Microsoft.Extensions.*` hosting/http/logging, Polly integration via `Microsoft.Extensions.Http.Polly`, `JetBrains.Annotations`, xUnit test stack (002-apply-coding-conventions)
- N/A (file-based planning and reporting artifacts only) (002-apply-coding-conventions)
- C# with multi-targeted .NET (`net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`) + `Microsoft.Extensions.Hosting.Abstractions`, `Microsoft.Extensions.Http`, `Microsoft.Extensions.Logging`, `Microsoft.Extensions.Http.Polly` (003-wildcard-event-matching)
- N/A (in-memory routing/matching evaluation only) (003-wildcard-event-matching)

- (001-initial-src-spec)

## Project Structure

```text
src/
tests/
```

## Commands

# Add commands for 

## Code Style

: Follow standard conventions

## Recent Changes
- 003-wildcard-event-matching: Added C# with multi-targeted .NET (`net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`) + `Microsoft.Extensions.Hosting.Abstractions`, `Microsoft.Extensions.Http`, `Microsoft.Extensions.Logging`, `Microsoft.Extensions.Http.Polly`
- 002-apply-coding-conventions: Added C# on .NET (`src` multi-targets `net6.0;net7.0;net8.0;net9.0;net10.0`; tests on `net8.0`; samples on `net10.0`) + `Microsoft.Extensions.*` hosting/http/logging, Polly integration via `Microsoft.Extensions.Http.Polly`, `JetBrains.Annotations`, xUnit test stack
- 001-initial-src-spec: Added C# with multi-targeted .NET (`net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`) + `Microsoft.Extensions.Hosting.Abstractions`, `Microsoft.Extensions.Http`, `Microsoft.Extensions.Http.Polly`, `Microsoft.Extensions.Logging`, `Polly`/`Polly.Extensions.Http`


<!-- MANUAL ADDITIONS START -->
<!-- MANUAL ADDITIONS END -->
