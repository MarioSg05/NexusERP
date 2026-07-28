# ADR-001 - Use SQL Server

## Status

Accepted

---

## Context

NexusERP requires a robust relational database to support enterprise business processes such as inventory management, sales, purchasing and accounting.

The persistence layer must integrate seamlessly with the .NET ecosystem while providing reliability, performance and long-term maintainability.

---

## Decision

Microsoft SQL Server will be used as the primary relational database management system.

---

## Rationale

SQL Server was selected because it provides:

- Excellent integration with .NET and Entity Framework Core.
- Wide adoption across enterprise environments, particularly in Guatemala and Latin America.
- Strong support for enterprise workloads.
- Native support for advanced database features such as stored procedures, functions and indexing.

---

## Consequences

Benefits

- Tight integration with the Microsoft technology stack.
- Mature tooling and documentation.
- Reliable support for enterprise applications.

Tradeoffs

- The project initially depends on SQL Server.
- Database portability will rely on Entity Framework Core abstractions if another provider is required in the future.