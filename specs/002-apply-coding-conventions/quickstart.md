# Quickstart: Codebase Coding Conventions Review

## 1) Confirm scope and baseline
- Use `docs/coding-conventions.md` as baseline.
- Include `src/`, `tests/`, and manually-authored C# in `samples/`.
- Exclude `bin/`, `obj/`, generated files, and vendor/external code.

## 2) Execute review
- Evaluate in-scope files against convention rules.
- Record each result as a `ComplianceFinding` and classify priority/applicability.

## 3) Apply applicable remediation
- Implement convention-aligned updates only for applicable findings.
- If a finding conflicts with accepted ADR/architecture decisions, mark exception with rationale and reviewer approval.

## 4) Run verification gate
- Build changed projects/solution.
- Run all existing impacted automated tests.
- Only mark remediation complete when both checks pass.

## 5) Produce summary and sign-off
- Disposition every finding (`resolved`, `deferred`, `waived`, `not_applicable`).
- Ensure all high-priority applicable findings are resolved.
- For every deferred/waived item, capture owner and target release.
- Publish `ComplianceSummary` for review sign-off.

## 6) Handoff to task planning
- Use this plan plus data model/contracts to generate actionable story-scoped tasks via `/speckit.tasks`.
