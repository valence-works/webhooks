# Data Model: Codebase Coding Conventions Review

## Entity: ConventionRule
- **Description**: A codified convention requirement used during review.
- **Fields**:
  - `ruleId` (string, required, unique)
  - `title` (string, required)
  - `description` (string, required)
  - `sourcePath` (string, required; e.g., `docs/coding-conventions.md`)
  - `applicability` (enum: `always`, `contextual`, `excluded`)
  - `priorityDefault` (enum: `high`, `medium`, `low`)
- **Validation Rules**:
  - `ruleId` must be stable across review cycles.
  - `sourcePath` must reference an existing conventions source.

## Entity: ComplianceFinding
- **Description**: Review result for one rule at one scoped code location.
- **Fields**:
  - `findingId` (string, required, unique)
  - `ruleId` (string, required, FK -> ConventionRule.ruleId)
  - `location` (string, required; repository-relative path)
  - `status` (enum: `compliant`, `non_compliant`, `not_applicable`)
  - `priority` (enum: `high`, `medium`, `low`)
  - `impact` (string, required when `status=non_compliant`)
  - `evidence` (string, optional)
- **Validation Rules**:
  - `ruleId` must exist.
  - `impact` is mandatory for non-compliant findings.

## Entity: RemediationDecision
- **Description**: Final disposition and action plan for a finding.
- **Fields**:
  - `decisionId` (string, required, unique)
  - `findingId` (string, required, FK -> ComplianceFinding.findingId)
  - `disposition` (enum: `resolved`, `deferred`, `waived`, `not_applicable`)
  - `rationale` (string, required)
  - `owner` (string, required for `deferred`/`waived`)
  - `targetRelease` (string, required for `deferred`/`waived`)
  - `approval` (string, optional reviewer identifier)
- **Validation Rules**:
  - `rationale` always required.
  - `owner` and `targetRelease` required when not immediately resolved.

## Entity: ComplianceSummary
- **Description**: Feature-level rollup used for sign-off.
- **Fields**:
  - `summaryId` (string, required)
  - `scope` (object, required)
  - `counts` (object, required; totals by status/disposition/priority)
  - `validationGate` (object, required; build + impacted tests result)
  - `openItems` (array<RemediationDecision reference>, required)
  - `approvedBy` (string, optional)
  - `approvedAt` (datetime, optional)
- **Validation Rules**:
  - Every finding must map to one disposition.
  - All high-priority applicable findings must be `resolved` before summary can be marked complete.

## Relationships
- `ConventionRule` 1 -> many `ComplianceFinding`
- `ComplianceFinding` 1 -> 1 `RemediationDecision`
- `ComplianceSummary` aggregates `ComplianceFinding` and `RemediationDecision`

## State Transitions

### ComplianceFinding.status
- `non_compliant` -> `compliant` (after resolved remediation)
- `non_compliant` -> `not_applicable` (when exception approved with rationale)
- `compliant` is terminal for current cycle.

### RemediationDecision.disposition
- Initial: `resolved` | `deferred` | `waived` | `not_applicable`
- `deferred` may transition to `resolved` in a future cycle.
- `waived` may transition to `resolved` if policy changes.
