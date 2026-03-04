# System Components Diagram

Canonical architecture reference: [docs/architecture/system-components.md](../../docs/architecture/system-components.md).

This local copy is kept in the spec folder for implementation-phase convenience.

```mermaid
flowchart LR
  A[Host Application] --> B[Publish Request]
  B --> C

  subgraph Dispatch_Plane
    C[Broadcast Orchestrator]
    C --> D[Broadcast Middleware Pipeline]
    D --> E[Sink Registry / Sink Provider]
    E --> F[Subscription Criteria Evaluation]
    F --> H[Delivery Attempt Handoff Pipeline]
    H --> J[Dispatcher Invocation Coordinator]
    J --> G{Sink Dispatcher Override?}
    G -->|yes| K1[Selected IWebhookDispatcher]
    G -->|no| K2[App Default IWebhookDispatcher]

    K2 --> K1
    K1 --> X1[Dispatch Handoff Result]

    subgraph Registered_Dispatchers
      RD1[DefaultWebhookDispatcher]
      RD2[WolverineWebhookDispatcher]
      RD3[MassTransitWebhookDispatcher]
    end

    J -.resolves from DI.-> RD1
    J -.resolves from DI.-> RD2
    J -.resolves from DI.-> RD3
  end

  subgraph Invoke_Plane
    Y[Endpoint Invoker Middleware Pipeline]
    Z[Endpoint Invoker HTTP Client]
    M[Webhook Sink Endpoints]
  end

  K1 -->|direct invoke or enqueue delivery envelope| Q1[Extension Module Async Path]
  Q1 --> W1[Module-Owned Worker / Consumer]
  Q1 --> Y
  W1 --> Y
  Y --> Z --> M

  subgraph Policies_and_Strategies
    N[Payload Field Selector Strategy]
    O[Payload Value Comparison Strategy]
    P[Transient Detection Strategy]
    R[Overflow Policy]
    S[Deduplication Policy]
  end

  F -.uses.-> N
  F -.uses.-> O
  Z -.uses.-> P
  Q1 -.governed by.-> R
  C -.optional.-> S

  subgraph Observability
    T[Delivery Result (Primary)]
    TH[Dispatch Handoff Result (Secondary)]
    U[EventId Correlation]
    V[Attempt Count / Failure Reason]
  end

  X1 --> TH
  Z --> T
  T --> U
  T --> V
```

Queue and worker/consumer components shown in the async path are optional and provided by dispatcher extension modules, not by Webhooks Core runtime components.
