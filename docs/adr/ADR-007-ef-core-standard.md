# ADR-007 - Entity Framework Core Standard

## Status

Accepted

---

## Context

NexusERP uses Entity Framework Core as its Object-Relational Mapper (ORM).

As additional bounded contexts were implemented, several persistence patterns emerged, including Aggregate mappings, Value Object conversions, encapsulated collections and entity relationships.

Without a common persistence standard, each module could adopt different mapping strategies, reducing consistency and increasing maintenance costs.

A unified persistence strategy ensures predictable behavior across the entire solution.

---

## Decision

Entity Framework Core is responsible only for persistence.

The domain model must never be modified solely to simplify persistence concerns.

Persistence must adapt to the Domain Model, not the opposite.

---

### Configurations

Every Aggregate Root and Entity must have its own configuration class implementing:

```csharp
IEntityTypeConfiguration<T>
```

Configurations must be located under:

```text
Persistence/
└── Configurations/
```

---

### Configuration Order

All Entity Framework configurations must follow the same structure.

```text
ToTable()

↓

HasKey()

↓

Properties

↓

Relationships

↓

Indexes
```

This ordering improves readability and creates a consistent persistence model across all bounded contexts.

---

### Value Objects

Value Objects must be persisted using dedicated ValueConverters.

Primitive persistence types must never leak into the Domain Model.

Examples include:

- ProductPrice
- CustomerEmail
- PurchaseUnitPrice
- PurchaseOrderTotal

---

### Relationships

Relationships must reflect the Domain Model.

Aggregate boundaries determine persistence relationships.

Examples:

- PurchaseOrder
    - PurchaseOrderItem

Entity Framework relationships must never introduce behaviors that contradict the Domain Model.

---

### Collections

Aggregate collections remain encapsulated.

Use:

```csharp
private readonly List<T> _items = [];

public IReadOnlyCollection<T> Items =>
    _items.AsReadOnly();
```

Entity Framework is configured to work with the Aggregate rather than exposing mutable collections.

---

### Shadow Properties

Shadow properties may be used when they simplify persistence without polluting the Domain Model.

Example:

```text
PurchaseOrderId
```

exists only in persistence.

---

### Cascade Delete

Cascade deletion is allowed only when it represents Aggregate ownership.

Example:

```text
PurchaseOrder

↓

PurchaseOrderItems
```

Deleting the Aggregate Root removes its child entities.

---

### Constructors

Additional constructors required exclusively for Entity Framework materialization are acceptable.

These constructors exist only to reconstruct persisted state and must not weaken the business rules exposed by the public API.

---

## Consequences

Benefits

- Persistence remains isolated from business rules.
- Aggregate boundaries are preserved.
- Value Objects remain first-class domain concepts.
- Consistent mappings across every bounded context.
- Predictable Entity Framework configuration.

Tradeoffs

- More configuration classes are required.
- Additional ValueConverters increase implementation effort.
- Developers must understand both DDD and Entity Framework Core.

---

## Guiding Principle

The Domain Model defines the architecture.

Entity Framework adapts to it.