# Contract: Event-Type Matching Policy

## Contract Name
`IEventTypeMatcherStrategy`

## Purpose
Define the single extensibility boundary responsible for deciding whether a subscription event type matches an incoming event type during sink routing.

## Ownership Boundaries

- **Orchestration owner**: Broadcaster routing flow invokes this contract when evaluating subscriptions.
- **Terminal executor**: Sink dispatch pipeline executes only after route selection and is out of scope for this contract.
- **Retry ownership**: Unchanged by this contract; retries remain in existing delivery pipeline behavior.

## Required Behavior

1. Evaluate subscription value and incoming value to determine match/no-match.
2. Default policy MUST implement:
   - `*` matches any incoming event type value (including null/empty/whitespace).
   - Non-wildcard values use case-sensitive exact comparison.
   - Subscription values with leading/trailing whitespace are invalid and evaluate as no-match.
3. Invalid subscription evaluation MUST emit warning signal while allowing runtime to continue.
4. Host applications MUST be able to replace the default policy through standard DI configuration.

## Input/Output Semantics

- **Input**:
  - `subscriptionEventType` (configured subscription value)
  - `incomingEventType` (event value)
- **Output**:
  - Boolean match result (`true` when subscription matches incoming event type)

## Determinism

For identical inputs, the contract implementation MUST return identical outputs.

## Non-Goals

- Payload predicate evaluation.
- Transport dispatch selection and retry mechanics.
- Pattern syntaxes beyond full-token wildcard `*`.
