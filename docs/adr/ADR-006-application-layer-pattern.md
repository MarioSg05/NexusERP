# ADR-006 - Application Layer Pattern

## Status

Accepted

---

## Context

As NexusERP evolved, multiple application use cases were implemented across different bounded contexts.

Examples include:

- RegisterCustomer
- RegisterProduct
- CreateInventory
- RegisterSupplier
- CreatePurchaseOrder
- RegisterUser
- LoginUser

Without a common pattern, application services could gradually accumulate business rules, duplicate validation logic and become difficult to maintain.

A consistent application workflow improves readability, maintainability and separation of concerns.

---

## Decision

The Application layer is responsible for orchestrating use cases.

Business rules must remain inside the Domain layer.

Application services coordinate the execution flow without implementing business decisions.

The standard workflow for every use case is:

```text
Request

↓

Validation

↓

Existence Checks

↓

Aggregate / Entity Creation

↓

Persistence

↓

Response
```

---

### Requests

Each use case must define a Request object containing only the information required to execute the operation.

Requests represent input data only.

---

### Validation

Input validation must be performed using FluentValidation.

Handlers must execute:

```csharp
await _validator.ValidateAndThrowAsync(
    request,
    cancellationToken);
```

Validation occurs before any business logic.

---

### Existence Checks

The Application layer is responsible for verifying external dependencies.

Examples include:

- Customer exists
- Supplier exists
- Product exists

These checks are orchestration concerns rather than domain rules.

---

### Aggregate Creation

Application creates Aggregates through their factory methods.

Examples:

```csharp
Customer.Register(...);

PurchaseOrder.Create(...);
```

Application must never instantiate Aggregates directly through constructors.

---

### Business Behavior

Application invokes behavior exposed by the Aggregate.

Examples:

```csharp
order.AddItem(...);

order.Approve();

customer.ChangeEmail(...);
```

Application must never modify Aggregate state directly.

---

### Persistence

Application persists Aggregates using the abstraction:

```csharp
IApplicationDbContext
```

The Domain layer must remain persistence-agnostic.

---

### Responses

Handlers return Response objects containing only the data required by the client.

Responses must not expose internal domain implementation details.

---

## Responsibilities

Application is responsible for:

- Coordinating the use case.
- Calling validators.
- Checking external dependencies.
- Creating Aggregates.
- Persisting changes.
- Returning responses.

The Domain is responsible for:

- Business rules.
- State transitions.
- Invariants.
- Derived values.
- Domain events.

---

## Consequences

Benefits

- Thin application services.
- Rich domain model.
- Clear separation of responsibilities.
- Easier testing.
- Predictable implementation across all modules.

Tradeoffs

- More classes are required for each use case.
- Developers must understand the distinction between orchestration and business logic.

---

## Guiding Principle

Application orchestrates.

Domain decides.