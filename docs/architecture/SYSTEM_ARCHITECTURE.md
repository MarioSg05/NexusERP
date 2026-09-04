# NexusERP System Architecture

## Overview

NexusERP is an ERP platform implemented as a **Modular Monolith** using:

- Domain-Driven Design (DDD)
- Clean Architecture
- Rich Domain Model
- Vertical Slice organization
- CQRS-style application flows
- Event-driven integration

The system keeps its business capabilities in a single application boundary
while using Domain Events, Integration Events and reliable asynchronous
messaging to maintain explicit integration boundaries.

NexusERP v1 is intentionally **not a microservices architecture**.

---

## Solution Structure

The backend contains seven projects:

```text
backend/
|
+-- src/
|   +-- NexusERP.Api
|   +-- NexusERP.Application
|   +-- NexusERP.Domain
|   +-- NexusERP.Infrastructure
|   +-- NexusERP.Worker
|
+-- tests/
    +-- NexusERP.UnitTests
    +-- NexusERP.IntegrationTests
```

### Domain

Contains the business model:

- Aggregate Roots
- Entities
- Value Objects
- Domain Events
- Business invariants

The Domain project has no project-to-project dependencies.

### Application

Contains application use cases and orchestration:

- Commands and Queries
- Handlers
- Validators
- Application interfaces
- Query abstractions
- Domain Event processing
- Integration Event contracts and handlers

Application depends on Domain and remains independent from the concrete
Infrastructure implementation.

### Infrastructure

Implements technical concerns:

- Entity Framework Core
- SQL Server persistence
- Query implementations
- JWT generation
- Password hashing
- Ollama integration
- Transactional Outbox
- Transactional Inbox
- RabbitMQ
- Retry and dead-letter handling
- Infrastructure health checks

### API

`NexusERP.Api` is the HTTP host and composition root.

It provides:

- ASP.NET Core Minimal APIs
- JWT authentication
- Role-based authorization
- Swagger/OpenAPI
- CORS
- Global exception handling
- Health endpoints

### Worker

`NexusERP.Worker` hosts asynchronous background processing separately from
the HTTP request pipeline.

It is responsible for:

- Outbox processing
- Integration Event publication
- RabbitMQ consumption
- Retry and dead-letter processing

---

## Dependency Direction

The main dependency direction is:

```text
                   Domain
                     ^
                     |
                 Application
                     ^
                     |
                Infrastructure
                  ^       ^
                  |       |
                 API    Worker
```

Key rules:

- Domain does not depend on outer layers.
- Application does not depend on the concrete Infrastructure project.
- Infrastructure implements abstractions defined by Application.
- API and Worker act as outer hosts and composition boundaries.

Application uses selected Entity Framework Core abstractions and query
extensions as an explicit trade-off while keeping the concrete DbContext,
SQL Server provider and migrations inside Infrastructure.

---

## Business Modules

NexusERP v1 includes:

- Identity
- Customers
- Products
- Inventory
- Suppliers
- Purchasing
- Sales
- Reports
- Dashboard
- AI Business Insights

Domain modules remain internally separated and avoid direct dependencies on
other domain modules.

---

## Persistence

NexusERP v1 uses SQL Server, Entity Framework Core and a shared
`ApplicationDbContext`.

A shared database is consistent with the current Modular Monolith
architecture. Database-per-service boundaries are intentionally deferred
unless future service extraction requires them.

---

## Event and Messaging Architecture

Domain Events represent meaningful changes inside the business model.

Integration Events represent asynchronous integration boundaries.

The messaging pipeline is:

```text
Domain Event
     |
     v
Integration Event
     |
     v
Transactional Outbox
     |
     v
Outbox Worker
     |
     v
RabbitMQ
     |
     v
Transactional Inbox
     |
     v
Integration Event Handler
```

Business state and corresponding Outbox messages are persisted through the
same database transaction.

The Worker publishes pending Outbox messages to RabbitMQ. Publisher Confirms
are enabled before messages are marked as successfully processed.

Consumer failures support bounded retries and dead-letter handling.
Retry and dead-letter forwarding are also confirmed before the original
RabbitMQ message is acknowledged.

The Integration Event identifier is used as the Inbox primary key to protect
against duplicate deliveries.

The resulting messaging model is:

**at-least-once delivery with idempotent consumption**

NexusERP does not claim exactly-once distributed delivery.

---

## Observability and Health

Messaging uses structured diagnostics with `MessageId` correlation across
publication, consumption, retry and dead-letter flows.

The API exposes:

```text
GET /health/live
GET /health/ready
```

Readiness checks include:

- SQL Server
- RabbitMQ

---

## Security

NexusERP uses:

- JWT Bearer authentication
- Role-based authorization
- Authorization policies
- Authenticated fallback policy

Endpoints require authentication unless they are explicitly configured for
anonymous access.

Current roles include:

- Administrator
- Manager
- User

---

## AI Boundary

AI Business Insights are isolated behind an Application abstraction and use
Ollama as the local provider.

Authoritative ERP calculations remain deterministic.

```text
ERP Data
   |
   v
Deterministic Analysis
   |
   v
Verified Business Facts
   |
   v
AI Abstraction
   |
   v
Ollama
   |
   v
Optional Narrative Summary
```

The language model is not a source of truth, has no direct database access,
and does not receive database credentials.

NexusERP remains functional when the AI provider is unavailable.

---

## Frontend

The frontend uses:

- React
- TypeScript
- Vite
- Tailwind CSS
- TanStack Query
- React Router
- Axios

It communicates with NexusERP through the HTTP API and remains separate from
backend domain logic.

---

## Testing

NexusERP separates unit and integration testing.

Integration tests exercise real infrastructure using:

- xUnit
- WebApplicationFactory
- SQL Server Testcontainers
- RabbitMQ Testcontainers

Current v1 baseline:

```text
Tests:   166
Passed:  166
Failed:  0
Skipped: 0
```

---

## Architectural Trade-offs

### Entity Framework Core in Application

Application uses selected EF Core abstractions and query extensions.

This keeps use cases simple while the concrete persistence implementation
remains inside Infrastructure.

### Shared Database

NexusERP v1 uses one application database because it is a Modular Monolith.

Independent data stores would only be introduced as part of a justified
service extraction.

### Messaging Semantics

Reliable messaging uses at-least-once delivery with idempotent Inbox
processing rather than claiming exactly-once distributed delivery.

### RabbitMQ Connections

The current publisher favors implementation simplicity over connection and
channel reuse.

Long-lived connection optimization is considered post-v1 work.

---

## Evolution

NexusERP v1 stops before microservice extraction.

Its current boundaries provide a foundation for future evolution through:

- Domain modules
- Integration Events
- Transactional Outbox
- Transactional Inbox
- RabbitMQ
- Independent Worker host

Any future service extraction should be driven by business and operational
requirements and must define data ownership, contracts, consistency and
deployment boundaries explicitly.

Potential post-v1 work includes:

- Microservices evolution
- RabbitMQ connection/channel reuse
- Outbox poison-message retry limits
- Generic Integration Event routing
- OpenTelemetry and distributed tracing
- Production monitoring and alerting