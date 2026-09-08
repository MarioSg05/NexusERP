# ADR-008 - Testing Standard

## Status

Accepted

---

## Context

Testing is a fundamental part of the NexusERP development workflow.

As the project evolved, unit tests were introduced for Aggregates, Entities and Value Objects across multiple bounded contexts.

Without a common testing strategy, different modules could gradually adopt inconsistent styles, naming conventions and project structures, making the test suite harder to understand and maintain.

A unified testing standard ensures consistency, readability and long-term maintainability.

---

## Decision

NexusERP adopts a consistent testing strategy across the entire solution.

Testing is considered a first-class citizen and is part of the development workflow rather than an optional activity.

---

### Testing Framework

The project uses:

- xUnit

Assertions are performed using:

```csharp
Assert
```

No additional assertion libraries are required.

---

### Test Structure

Every test follows the same structure.

```text
Arrange

↓

Act

↓

Assert
```

Sections should be explicitly identified using comments.

Example:

```csharp
// Arrange

// Act

// Assert
```

---

### Naming Convention

Test methods follow the format:

```text
Method_Should_Result_When_Condition
```

Examples:

- Register_Should_Create_Customer()
- ChangePrice_Should_Update_Price()
- Approve_Should_Change_Status()

Method names should describe observable business behavior.

---

### Test Organization

Unit tests mirror the Domain structure.

Each bounded context follows:

```text
BoundedContext

├── Aggregates
│     └── AggregateTests.cs

├── Entities
│     └── EntityTests.cs

└── ValueObjects
      └── ValueObjectTests.cs
```

Folders are created only when the corresponding domain concept exists.

---

### Aggregate Tests

Aggregate tests validate:

- Creation
- State transitions
- Business behavior
- Domain invariants

Tests should never verify persistence concerns.

---

### Entity Tests

Entities with business behavior must have their own test suite.

Examples include:

- PurchaseOrderItem

---

### Value Object Tests

Every Value Object must have dedicated tests validating:

- Valid values
- Invalid values
- Boundary conditions
- Normalization (when applicable)

---

### Business-Oriented Tests

Tests should verify business behavior rather than implementation details.

Examples:

Good:

```text
Approve_Should_Change_Status()
```

Avoid:

```text
Status_Should_Be_Set_To_Approved()
```

The test should describe business intent instead of internal implementation.

---

### Test Independence

Each test must be independent.

Tests must not rely on execution order or shared mutable state.

---

### Coverage Philosophy

The objective is not achieving a specific percentage of code coverage.

Instead, tests focus on protecting:

- Business rules
- Domain invariants
- Aggregate behavior
- Value Object validation

Coverage is a consequence of good testing rather than a primary goal.

---

## Consequences

Benefits

- Consistent test structure across all bounded contexts.
- Easier onboarding for new developers.
- Predictable test organization.
- Better readability.
- Reliable regression protection.

Tradeoffs

- More test files are required.
- Slightly higher implementation effort.
- Developers must follow the agreed naming and organization conventions.

---

## Guiding Principle

Tests document business behavior.

They protect the Domain Model from regression.