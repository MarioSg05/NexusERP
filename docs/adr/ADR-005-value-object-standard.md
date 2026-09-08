# ADR-005 - Value Object Standard

## Status

Accepted

---

## Context

Value Objects are one of the core building blocks of the NexusERP domain model.

As the project evolved, multiple bounded contexts introduced Value Objects representing names, emails, identifiers, quantities, prices and monetary values.

Without a common standard, new Value Objects could gradually diverge in style, validation strategy and implementation details, reducing consistency across the domain.

A single implementation pattern improves readability, maintainability and predictability.

---

## Decision

All Value Objects in NexusERP must follow the same implementation standard.

### Type

Value Objects shall be implemented as:

```csharp
public sealed record Xxx
```

Using `record` provides value-based equality, while `sealed` prevents inheritance and preserves immutability.

---

### Immutability

Value Objects are immutable.

Their internal state cannot change after construction.

The exposed value must always use:

```csharp
public T Value { get; }
```

Public setters are not allowed.

---

### Validation

All validation rules must be executed inside the constructor.

Invalid values must throw:

```csharp
DomainException
```

The Domain layer is responsible for protecting its own invariants.

---

### Normalization

Whenever applicable, input values should be normalized before applying business rules.

Examples include:

- Trim()
- ToUpperInvariant()
- ToLowerInvariant()

Normalization must occur immediately after validation.

---

### Business Rules

Business rules must be evaluated after normalization.

Examples include:

- Maximum length
- Allowed characters
- Numeric ranges
- Positive quantities
- Monetary constraints

---

### Assignment

Only validated values may be assigned to the Value property.

---

### String Representation

Text-based Value Objects should override:

```csharp
public override string ToString() => Value;
```

This provides a consistent string representation across the domain.

---

### Persistence

If Entity Framework Core requires materialization support, an additional constructor may be introduced exclusively for persistence.

This constructor must not weaken the business rules exposed by the primary constructor.

---

### Standard Structure

Text-based Value Objects should follow this order:

```text
Validate

↓

Normalize

↓

Business Rules

↓

Assign Value
```

Numeric Value Objects generally follow:

```text
Validate

↓

Business Rules

↓

Assign Value
```

Money-related Value Objects follow:

```text
Validate

↓

Business Rules

↓

Round

↓

Assign Value
```

---

## Consequences

Benefits

- Consistent implementation across all bounded contexts.
- Strong protection of domain invariants.
- Predictable modeling for future modules.
- Reduced cognitive load for developers.
- Easier code reviews.

Tradeoffs

- Slightly more implementation effort than using primitive types.
- Developers must understand Value Object semantics before contributing.