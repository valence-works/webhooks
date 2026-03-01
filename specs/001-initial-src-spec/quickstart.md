# Quickstart: Baseline Webhook Broadcasting Behavior

## Prerequisites
- .NET SDK installed (matching repository targets)
- Restore access to configured package sources

## 1) Restore and build
```bash
dotnet restore Webhooks.sln
dotnet build Webhooks.sln
```

## 2) Run tests
```bash
dotnet test Webhooks.sln
```

## 3) Validate planning artifacts
- Spec: `specs/001-initial-src-spec/spec.md`
- Plan: `specs/001-initial-src-spec/plan.md`
- Research: `specs/001-initial-src-spec/research.md`
- Data model: `specs/001-initial-src-spec/data-model.md`
- Contracts: `specs/001-initial-src-spec/contracts/`

## 4) Next command
Generate executable implementation work items:
```bash
/speckit.tasks
```

## Expected verification outcomes
- Build succeeds for solution.
- Tests covering routing, middleware, retry semantics, and (when using queued dispatcher modules) queue policy pass.
- Tests confirm exactly one dispatcher is selected per sink delivery attempt.
- Tests confirm sink-level dispatcher override takes precedence over application default dispatcher selection.
- Dispatcher registration and payload predicate validation failures surface at startup.
