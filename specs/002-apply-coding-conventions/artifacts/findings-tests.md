# Findings: tests/

**Scope**: `tests/Webhooks.Core.Tests/**/*.cs`  
**Reviewed**: 2026-03-04  
**Convention Baseline**: `docs/coding-conventions.md`

---

## Systemic Findings

### F-065: Test classes — missing sealed (bulk)

| Field | Value |
|-------|-------|
| Rule ID | CS-005 |
| Location | tests/Webhooks.Core.Tests/**/*.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | All 24 test classes are not sealed. Classes: DispatcherRegistrationValidationTests, PayloadSelectorValidationTests, JsonPathSubsetValidationTests, SinkRegistrationValidationTests, CoordinatorResolutionValidationTests, EventTypeRoutingTests, MissingPayloadFieldTests, PayloadMatchingModeTests, NoMatchingSinksTests, DispatcherSelectionPrecedenceTests, QueueCapacityAndParallelismTests, OverflowPolicyTests, DispatchExceptionTaxonomyTests, RetryConfigurationTests, DeliveryObservabilityFieldsTests, DeduplicationPolicyTests, CoordinatorPolicyOwnershipTests, PendingStatusTransitionTests, TransientDetectionStrategyTests, DeliveryOutcomeSemanticsTests, DispatcherUnavailableBehaviorTests, BroadcastMiddlewareOrderingTests, MiddlewareContextShapeTests, EndpointInvokerMiddlewareRetryTests, SigningAuthenticationMiddlewareTests |

### F-066: SequenceHandler — underscore-prefixed private field

| Field | Value |
|-------|-------|
| Rule ID | NC-007 |
| Location | tests/Webhooks.Core.Tests/Middleware/EndpointInvokerMiddlewareRetryTests.cs |
| Status | non_compliant |
| Priority | high |
| Applicability | applicable |
| Impact | Inner class `SequenceHandler` has `_index` field with underscore prefix |

---

## Compliant Areas (tests/)

| Rule ID | Area | Status |
|---------|------|--------|
| CO-001 | All files | compliant — file-scoped namespaces |
| CS-001 | All files | compliant — explicit access modifiers |
| TS-001 | All test classes | compliant — end in `Tests`, reflect capability |
| TS-002 | All test methods | compliant — follow `Method_Scenario_ExpectedBehavior` |
| TS-003 | All tests | compliant — Arrange/Act/Assert structure |
| TS-004 | All tests | compliant — focused on one behavior |
| TS-005 | All tests | compliant — explicit assertions |
| CO-003 | All files | compliant — using directive order correct |
| NC-011 | All methods | compliant — PascalCase verb phrases |
