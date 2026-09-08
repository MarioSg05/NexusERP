# ADR-002 - Rich Domain Model

## Status

Accepted

---

## Context

NexusERP models business processes that contain complex business rules and invariants.

An anemic domain model would move business logic into services, making the domain difficult to maintain and reason about as the system grows.

---

## Decision

The project adopts a Rich Domain Model.

Business rules must live inside Aggregates, Entities and Value Objects.

The Domain layer is responsible for protecting its own invariants.

---

## Consequences

Benefits

- Business rules remain close to the data they govern.
- Domain invariants are protected.
- Application services remain thin and focused on orchestration.
- The domain model becomes easier to understand and evolve.

Tradeoffs

- Domain objects contain more behavior than traditional CRUD models.
- Developers must understand Domain-Driven Design concepts before contributing.