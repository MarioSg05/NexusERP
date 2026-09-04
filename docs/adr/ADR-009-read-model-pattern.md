# ADR-009: Read Query Strategy

## Status

Accepted

---

## Context

NexusERP follows a Rich Domain Model where Aggregates and Value Objects enforce business rules and transactional consistency.

During the implementation of the Reports module, it became evident that read operations have different requirements than write operations.

In particular:

- Aggregates are optimized for business behavior, not reporting.
- Value Objects mapped through EF Core ValueConverters cannot always be translated into SQL expressions when filtering or aggregating data.
- Reports, dashboards, analytics and future AI features require optimized read operations that should not modify or compromise the Domain Model.

The application therefore requires a dedicated query strategy while preserving the existing Rich Domain Model.

---

## Decision

NexusERP adopts a hybrid query strategy.

### Write Side

Commands use:

API

↓

Application Handlers

↓

Domain Model

↓

IApplicationDbContext

↓

Entity Framework Core

↓

SQL Server

### Read Side

Queries use:

API

↓

Query Handlers

↓

IReportQueries

↓

ReportQueries

↓

ApplicationDbContext

↓

Entity Framework Core

↓

SQL Server

Query Handlers never access Entity Framework Core directly.

All read operations are encapsulated inside ReportQueries.

---

## Query Execution Levels

### Level 1 - EF Core Projection (Default)

Use Entity Framework Core with:

- AsNoTracking()
- LINQ
- Select()

This is the default implementation for approximately 90% of reports.

Suitable for:

- CRUD reports
- List endpoints
- Simple joins
- Simple projections

---

### Level 2 - Parameterized SQL

When Entity Framework Core cannot translate a LINQ expression because of ORM limitations (for example ValueConverters), use parameterized SQL through EF Core.

Typical scenarios include:

- filtering on Value Objects
- complex joins
- aggregations
- reporting queries where LINQ translation is not possible

Queries must use EF Core APIs for SQL execution.

String concatenation is not allowed.

SQL injection must always be prevented through parameterization.

---

### Level 3 - SQL Views

SQL Views are reserved for:

- analytical reports
- dashboard queries
- reusable reporting datasets
- performance-critical reports

Views should only be introduced when justified by complexity or performance requirements.

---

## Domain Preservation

The Domain Model must never be modified to satisfy reporting requirements.

Specifically:

- Aggregates remain optimized for write operations.
- Value Objects remain immutable.
- ValueConverters remain the persistence strategy.
- ORM limitations must never dictate Domain design.

If a report cannot be efficiently expressed through LINQ because of ValueConverter translation limitations, the reporting strategy changes - not the Domain Model.

---

## Dependency Rules

Application defines:

- Query Handlers
- IReportQueries
- Request
- Response

Infrastructure implements:

- ReportQueries
- Entity Framework Core queries
- SQL queries

Application never depends on Infrastructure.

Infrastructure depends on Application contracts.

---

## Benefits

- Complete separation between write and read concerns.
- Rich Domain Model remains focused on business behavior.
- Reports remain optimized for querying.
- ORM limitations are isolated inside Infrastructure.
- Dashboard and future AI modules reuse the same reporting infrastructure.
- Query implementation details remain hidden from Application.

---

## Tradeoffs

- Some reports may require SQL instead of LINQ.
- SQL queries require additional maintenance.
- Read-side implementations may differ depending on reporting complexity.

These tradeoffs are accepted in exchange for preserving the Domain Model and maintaining a scalable reporting architecture.