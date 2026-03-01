# Webhooks Core System Components

This diagram describes the current baseline component architecture for webhook broadcasting, delivery orchestration, and pluggable dispatch.

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

  K1 -->|direct invoke or enqueue delivery envelope| Q1[Invoke Path / Queue]
  Q1 --> W1[Worker / Consumer (when queued)]
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

## Notes

- Dispatch plane handles matching and handoff; invoke plane handles actual endpoint invocation.
- Delivery success/failure is determined by Endpoint Invoker outcome and captured as primary `Delivery Result`.
- Dispatcher handoff status is secondary telemetry (`Dispatch Handoff Result`) and does not define business delivery success.
- Coordinator owns dispatcher selection precedence (sink override, then app default), with one dispatcher selected per sink delivery attempt.
