# Artifacts: Codebase Coding Conventions Review

**Feature**: `001-apply-coding-conventions`  
**Created**: 2026-03-04  
**Spec**: [../spec.md](../spec.md) | **Plan**: [../plan.md](../plan.md) | **Tasks**: [../tasks.md](../tasks.md)

## Artifact Index

### Phase 1 — Setup

| Artifact | Description |
|----------|-------------|
| [convention-rules.md](convention-rules.md) | Rule catalog extracted from `docs/coding-conventions.md` |
| [scope-manifest.md](scope-manifest.md) | Include/exclude scope boundaries for review |
| [exception-log.md](exception-log.md) | ADR/architecture precedence exception decision log |

### Phase 2 — Foundational Schemas

| Artifact | Description |
|----------|-------------|
| [finding-schema.md](finding-schema.md) | `ComplianceFinding` capture format |
| [decision-schema.md](decision-schema.md) | `RemediationDecision` capture format |
| [validation-gate.md](validation-gate.md) | Build + tests validation gate checklist |
| [traceability-matrix.md](traceability-matrix.md) | Finding-to-rule traceability matrix |
| [compliance-summary.md](compliance-summary.md) | Summary assembly template aligned to contract |

### Phase 3 — US1: Assess Compliance

| Artifact | Description |
|----------|-------------|
| [findings-src.md](findings-src.md) | Findings for `src/` |
| [findings-tests.md](findings-tests.md) | Findings for `tests/` |
| [findings-samples.md](findings-samples.md) | Findings for `samples/` |
| [finding-register.md](finding-register.md) | Consolidated master finding register |

### Phase 4 — US2: Apply Conventions

| Artifact | Description |
|----------|-------------|
| [remediation-plan.md](remediation-plan.md) | Prioritized remediation plan |
| [remediation-log-src.md](remediation-log-src.md) | Remediation log for `src/` changes |
| [remediation-log-tests.md](remediation-log-tests.md) | Remediation log for `tests/` changes |
| [remediation-log-samples.md](remediation-log-samples.md) | Remediation log for `samples/` changes |
| [decision-register.md](decision-register.md) | Final per-finding dispositions |

### Phase 5 — US3: Validate & Sign Off

| Artifact | Description |
|----------|-------------|
| [compliance-summary.instance.yaml](compliance-summary.instance.yaml) | Contract-aligned summary instance |
| [open-items.md](open-items.md) | Unresolved deferred/waived items with ownership |
| [sign-off.md](sign-off.md) | Reviewer sign-off record |

## Conventions Baseline

- **Source**: [`docs/coding-conventions.md`](../../docs/coding-conventions.md)
- **Architecture Precedence**: [`docs/adr/`](../../docs/adr/)
- **Vocabulary**: [`docs/architecture/vocabulary.md`](../../docs/architecture/vocabulary.md)
