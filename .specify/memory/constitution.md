<!--
Sync Impact Report
- Version change: template -> 1.0.0
- Modified principles:
	- Principle slot 1 -> I. Spec-First Delivery (NON-NEGOTIABLE)
	- Principle slot 2 -> II. Library-First, Pluggable Design
	- Principle slot 3 -> III. Explicit Behavior Contracts
	- Principle slot 4 -> IV. Verification Before Merge
	- Principle slot 5 -> V. Simplicity and Backward Compatibility
- Added sections:
	- Engineering Constraints
	- Workflow & Quality Gates
- Removed sections: none
- Templates requiring updates:
	- ✅ updated: .specify/templates/plan-template.md
	- ✅ updated: .specify/templates/spec-template.md
	- ✅ updated: .specify/templates/tasks-template.md
	- ⚠ pending: .specify/templates/commands/*.md (directory not present)
- Runtime docs reviewed:
	- ✅ updated: README.md (added constitution reference)
- Deferred TODOs: none
-->

# Webhooks Core Constitution

## Core Principles

### I. Spec-First Delivery (NON-NEGOTIABLE)
Every non-trivial change MUST begin with a spec in `specs/` and MUST be clarified before
planning or implementation. Requirements, edge cases, and measurable outcomes MUST be
captured first; code and tasks MUST trace back to approved requirements.

Rationale: The project is intentionally run with Spec Kit workflows. Spec-first discipline
prevents architectural drift and hidden assumptions.

### II. Library-First, Pluggable Design
Core behavior MUST be implemented as reusable library contracts in `src/` and exposed through
dependency injection extension points. Transport, dispatch, and comparison behavior MUST be
replaceable by host applications or extension packages without forking core orchestration.

Rationale: Webhooks Core is a framework-style library; extensibility is a product requirement,
not a convenience.

### III. Explicit Behavior Contracts
Behavioral boundaries MUST be explicit and testable: ownership of orchestration vs transport,
retry scope, middleware execution points, and configuration validation rules. New features MUST
define defaults and override points, and MUST use deterministic wording (`MUST`/`MUST NOT`/
`SHOULD`) in specs.

Rationale: Ambiguous contracts lead to divergent implementations across integrations.

### IV. Verification Before Merge
Changes MUST be validated at the smallest relevant scope first, then at broader scope. At a
minimum, modified projects MUST build successfully; when tests exist for changed behavior, those
tests MUST pass before merge. Warnings introduced by a change MUST be resolved or explicitly
justified in the PR.

Rationale: Fast, scoped verification reduces regressions and keeps delivery predictable.

### V. Simplicity and Backward Compatibility
Designs MUST prefer the simplest viable mechanism, with explicit defaults and minimal new
surface area. Existing public behavior SHOULD remain backward compatible unless a breaking
change is intentionally approved and documented in spec and ADR updates.

Rationale: This library is consumed by host applications; stability and clarity are essential.

## Engineering Constraints

- Runtime platform: .NET multi-targeted library and sample apps.
- Architectural style: DI-first composition with replaceable contracts.
- Configuration: host-driven options with explicit validation for invalid states.
- Documentation: architectural decisions MUST be tracked in `docs/adr/`.

## Workflow & Quality Gates

1. Specification Gate
	 - Start from `/speckit.specify` semantics: create/update `spec.md` with clear user stories,
		 requirements, edge cases, and measurable outcomes.
	 - Run clarification pass for all high-impact ambiguities before planning.

2. Planning Gate
	 - Implementation plan MUST include Constitution Check outcomes.
	 - Contract ownership (orchestration vs dispatcher, middleware boundaries, retry semantics,
		 filtering semantics) MUST be explicit before task generation.

3. Implementation Gate
	 - Tasks MUST be story-scoped and independently testable.
	 - Code changes MUST map to spec requirements and avoid unrelated refactors.

4. Verification Gate
	 - Build changed projects and relevant solution scope.
	 - Execute applicable tests for changed behavior.
	 - Resolve or document warnings introduced by the change.

## Governance

This constitution supersedes conflicting local process notes for specification, planning, and
implementation workflow in this repository.

Amendment process:
- Amendments MUST be proposed in a PR that includes:
	- the constitution diff,
	- rationale and impact,
	- required template/document sync updates.
- At least one maintainer approval is REQUIRED before merge.

Versioning policy for this constitution:
- MAJOR: incompatible principle or governance changes.
- MINOR: new principle/section or materially expanded guidance.
- PATCH: clarifications, wording improvements, typo fixes.

Compliance review expectations:
- Every implementation PR MUST verify Constitution Check expectations in planning artifacts.
- Reviewers MUST reject PRs that bypass spec/clarification/planning gates for non-trivial work.
- Deviations MUST be explicitly justified in spec/plan and tracked as follow-up work.

**Version**: 1.0.0 | **Ratified**: 2026-03-01 | **Last Amended**: 2026-03-01
