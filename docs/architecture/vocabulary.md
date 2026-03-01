# Webhooks Core Vocabulary

Purpose: Provide a single repository-level source of truth for domain terminology used across specs, plans, tasks, and implementation notes.

## Canonical Terms

- **Publish Request**: Host-provided input submitted to the broadcast API.
- **Delivery Envelope**: Normalized outbound message used by orchestration after input normalization.
- **Subscription Criteria**: Subscription-side matching rule (event type + optional payload predicates + matching mode).
- **Payload Predicate**: Field comparison condition used during subscription matching.
- **Delivery Attempt**: One delivery execution for one `(Delivery Envelope, Sink)` pair.
- **Delivery Result**: Structured outcome for a delivery attempt, including status and observability fields.
- **Dispatch Mode**: Broadcaster-owned scheduling mode (`sequential`, `concurrent`, `queued`).
- **Overflow Policy**: Configurable behavior when queued mode reaches capacity.

## Deprecated Terms

These terms should not be reintroduced in new specs or implementation artifacts:

- `Broadcast Input`, `Broadcast Request`, `Broadcast Event`
- `Webhook Event Filter`, `Subscription Match Rule`, `Payload Filter`
- `Sink Delivery Attempt`, `Delivery Outcome Record`
- `Delivery Orchestration Policy`, `queue-full policy`

## Usage Rules

1. New feature specs SHOULD use canonical terms from this document.
2. Feature-specific terms MAY be introduced only when needed and MUST be documented in the feature spec.
3. When legacy terms appear in historical docs, new edits SHOULD replace them with canonical equivalents.
