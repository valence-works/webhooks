# Data Model: Wildcard Event-Type Matching

## Entities

### 1) Subscription Event Type
- **Purpose**: Configured value on each subscription used for candidate sink routing.
- **Core fields**:
  - `RawValue` (configured string from host options)
  - `MatchKind` (`Wildcard` | `Literal` | `Invalid`)
- **Classification rules**:
  - `Wildcard` when `RawValue == "*"`.
  - `Literal` when non-null/non-empty and no leading/trailing whitespace.
  - `Invalid` when null/empty/whitespace-only or when leading/trailing whitespace exists.

### 2) Incoming Event Type
- **Purpose**: Event-side value evaluated against each subscription event type.
- **Core fields**:
  - `Value` (string, may be null/empty/whitespace)
- **Notes**:
  - Wildcard subscriptions match any value including null/empty/whitespace.
  - Literal subscriptions match only case-sensitive exact values.

### 3) Event-Type Match Evaluation
- **Purpose**: Deterministic outcome of one `(Subscription Event Type, Incoming Event Type)` evaluation.
- **Core fields**:
  - `SubscriptionValue`
  - `IncomingValue`
  - `Outcome` (`Match` | `NoMatch`)
  - `Reason` (`Wildcard`, `LiteralExact`, `LiteralMismatch`, `InvalidSubscription`)

### 4) Event-Type Matcher Policy
- **Purpose**: Replaceable policy object that computes Event-Type Match Evaluation outcomes.
- **Core fields**:
  - `PolicyName` (default wildcard-capable or host-provided override)
  - `IsDefault` (boolean)

## Relationships

- One webhook event has one `Incoming Event Type`.
- Each subscription contributes one `Subscription Event Type` value.
- Broadcaster evaluates one `Event-Type Match Evaluation` per subscription.
- Broadcaster routes to sink when any subscription for that sink yields `Outcome = Match`.

## Validation Rules

- `RawValue` with leading/trailing whitespace is invalid configuration.
- Invalid subscription values MUST generate warning signal and evaluate as `NoMatch`.
- Wildcard classification requires exact `*` token only.
- Literal comparison remains case-sensitive exact equality.

## State/Outcome Transitions

- `Subscription Event Type (Wildcard)` + any incoming value -> `Match (Wildcard)`.
- `Subscription Event Type (Literal)` + exact incoming value -> `Match (LiteralExact)`.
- `Subscription Event Type (Literal)` + non-equal incoming value -> `NoMatch (LiteralMismatch)`.
- `Subscription Event Type (Invalid)` + any incoming value -> `NoMatch (InvalidSubscription)` with warning.
