# ADR-004 - Aggregate Lifecycle Standard

## Status

Accepted

---

## Context

As NexusERP evolved, multiple bounded contexts introduced Aggregate Roots representing different business concepts such as Users, Customers, Products, Inventory, Suppliers and Purchase Orders.

Although each aggregate models a different business process, they all share the same lifecycle and structural characteristics.

Without a common standard, future modules could gradually diverge in style and design, making the domain model harder to understand and maintain.

A consistent Aggregate pattern improves readability, predictability and long-term maintainability.

---

## Decision

All Aggregate Roots in NexusERP shall follow the same lifecycle pattern.

### Constructor

Aggregate constructors must be private.

Aggregates cannot be instantiated directly outside the domain.

Creation must always occur through a factory method.

---

### Factory Methods

Aggregate creation shall occur through static factory methods.

The method name must express the business language.

Examples:

- Register()
- Create()

Use:

- Register() for master data.
- Create() for transactional documents.

---

### State

Aggregate state must be encapsulated.

Properties expose:

```csharp
public Xxx Value { get; private set; }
```

Public setters are not allowed.

---

### Behavior

Business operations must be expressed through behavior methods.

Examples:

- ChangeEmail()
- ChangePrice()
- Activate()
- Deactivate()
- Approve()
- Cancel()

Property assignment from the Application layer is not allowed.

---

### Guard Methods

Repeated business validations should be extracted into private guard methods.

Examples:

- EnsurePending()
- EnsureHasItems()

Guard methods improve readability while centralizing business invariants.

---

### Audit

Every state-changing operation must call:

```csharp
UpdateAudit();
```

Audit updates are the responsibility of the Aggregate.

---

### Domain Events

Domain Events should be raised only when a meaningful business event occurs.

Examples:

- CustomerRegisteredEvent
- SupplierRegisteredEvent
- PurchaseOrderCreatedEvent

Events should not be created for every property change.

---

### Collections

Aggregate collections must always be encapsulated.

Use:

```csharp
private readonly List<T> _items = [];

public IReadOnlyCollection<T> Items =>
    _items.AsReadOnly();
```

Collections must never be publicly mutable.

---

## Consequences

Benefits

- Consistent Aggregate design.
- Predictable modeling across bounded contexts.
- Strong encapsulation.
- Better maintainability.
- Clear separation between Application and Domain.

Tradeoffs

- Slightly more code than using public setters.
- Factory methods require additional implementation.
- Developers must understand the Aggregate pattern before contributing.