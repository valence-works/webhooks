# Research: Wildcard Event-Type Matching

## Decision 1: Wildcard token semantics
- **Decision**: Treat `*` as wildcard only when it is the full subscription event-type value, and match all incoming event types (including null/empty/whitespace).
- **Rationale**: Matches clarified specification and current sample intent while keeping behavior deterministic.
- **Alternatives considered**:
  - Match only non-empty incoming values: rejected because it conflicts with clarified requirement.
  - Add glob/pattern syntax (`prefix*`, regex): rejected as out of current scope.

## Decision 2: Literal event-type matching semantics
- **Decision**: Keep non-wildcard matching as case-sensitive exact comparison.
- **Rationale**: Preserves backward compatibility and existing routing expectations.
- **Alternatives considered**:
  - Case-insensitive matching: rejected because it changes existing behavior and could expand routing unexpectedly.

## Decision 3: Invalid subscription value handling
- **Decision**: Subscription event-type values with leading/trailing whitespace are invalid; system logs warning and treats subscription as non-match at runtime.
- **Rationale**: Avoids ambiguous silent normalization while maintaining service availability.
- **Alternatives considered**:
  - Startup fail-fast: rejected for this feature because clarified behavior requires continue-startup semantics.
  - Auto-trim normalization: rejected because it hides configuration mistakes.

## Decision 4: Pluggability boundary for matching policy
- **Decision**: Event-type matching is owned by broadcaster orchestration via replaceable matcher policy contract.
- **Rationale**: Aligns with constitution Principle II and existing strategy-based extension model.
- **Alternatives considered**:
  - Keep hardcoded comparison in broadcaster: rejected because it violates pluggability requirement.
  - Embed matching in sink provider: rejected because matching is broadcaster-owned routing logic.

## Decision 5: Scope boundary with payload criteria
- **Decision**: Payload selector/comparison behavior remains unchanged.
- **Rationale**: Feature addresses event-type matching only; avoids unnecessary risk and regressions.
- **Alternatives considered**:
  - Refactor payload strategy stack in same change: rejected as unrelated scope expansion.

## Decision 6: Verification strategy
- **Decision**: Add focused routing tests for wildcard match, exact match, invalid subscription non-match + warning behavior, and host matcher override behavior.
- **Rationale**: Covers all clarified requirements with minimal, high-signal regression protection.
- **Alternatives considered**:
  - Sample-only manual validation: rejected because automated guarantees are required by the spec.
