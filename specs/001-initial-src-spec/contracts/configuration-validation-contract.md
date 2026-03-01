# Contract: Configuration and Validation

## Required validations
- At least one dispatcher is registered and coordinator resolution succeeds.
- Subscriptions with payload predicates must declare explicit `AND`/`OR` matching mode.
- Payload field-path expressions must conform to supported selector syntax.

## Defaults
- Overflow policy defaults to fail-fast when unset.
- Deduplication defaults to disabled when unset.
- HTTP dispatcher retry behavior has library defaults and host overrides.

## Failure model
- Invalid configuration fails startup/configuration validation.
- Runtime delivery failures are captured in delivery results.
