# NexusERP

**Full-stack Enterprise Resource Planning platform built with .NET 9, React, Domain-Driven Design and reliable asynchronous messaging.**

NexusERP is a portfolio-focused ERP project designed to demonstrate the architecture and implementation of a modern business application beyond basic CRUD operations.

**Current status:** NexusERP v1.0 release preparation.

---

## Overview

NexusERP combines common ERP capabilities with enterprise application patterns including:

- Domain-Driven Design
- Clean Architecture
- Modular Monolith
- Rich Domain Model
- CQRS-style application flows
- JWT authentication and role-based authorization
- Transactional Outbox and Inbox
- RabbitMQ asynchronous messaging
- Retry and dead-letter handling
- Publisher Confirms
- Automated integration testing with real infrastructure
- Local AI-assisted business insights

NexusERP v1 is intentionally a **Modular Monolith**, not a microservices architecture.

Its integration boundaries allow future service extraction if concrete business or operational requirements justify it.

---

## Technology Stack

### Backend

- .NET 9
- ASP.NET Core Minimal APIs
- Entity Framework Core
- SQL Server
- FluentValidation
- JWT Bearer Authentication
- BCrypt
- xUnit

### Frontend

- React
- TypeScript
- Vite
- Tailwind CSS
- TanStack Query
- React Router
- Axios

### Messaging and Infrastructure

- RabbitMQ
- .NET Worker Service
- Transactional Outbox
- Transactional Inbox
- Publisher Confirms
- Retry and Dead-Letter Queues
- Docker
- Testcontainers

### AI

- Ollama
- Local language model integration
- Provider-independent AI abstraction

---

## Implemented Capabilities

### Identity and Security

- User registration and authentication
- JWT authentication
- Role-based authorization
- User management
- Account activation and deactivation

### Sales Master Data

- Customer management
- Product management
- Inventory management
- Supplier management

### Business Operations

- Purchase Orders
- Sales Orders
- Inventory stock operations

### Analytics

- Inventory reports
- Low-stock reporting
- Sales reporting
- Purchasing reporting
- Business dashboard
- AI-assisted business insights

---

## Architecture

NexusERP follows a Clean Architecture dependency model:

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

The backend is organized as a **Modular Monolith** with independent business modules inside the Domain and use-case-oriented slices in Application.

The HTTP API and background Worker are separate hosts over the same application and infrastructure foundation.

For the detailed architecture and trade-offs, see:

[`docs/architecture/SYSTEM_ARCHITECTURE.md`](docs/architecture/SYSTEM_ARCHITECTURE.md)

---

## Reliable Messaging

NexusERP includes a complete asynchronous messaging foundation:

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

Key reliability characteristics:

- Business changes and Outbox messages are persisted atomically.
- RabbitMQ Publisher Confirms protect publication before messages are considered processed.
- Consumers use manual acknowledgements.
- Retry attempts are bounded.
- Permanent failures can be moved to a dead-letter queue.
- Inbox persistence protects against duplicate message processing.
- `MessageId` is preserved for correlation.

Messaging semantics are:

**at-least-once delivery with idempotent consumption**

NexusERP does not claim exactly-once distributed delivery.

---

## AI Business Insights

NexusERP integrates local AI through Ollama while keeping ERP calculations deterministic.

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

The language model:

- is not the source of truth for business data;
- does not calculate authoritative ERP values;
- has no direct database access;
- does not receive database credentials.

The ERP remains functional when the AI provider is unavailable.

---

## Project Structure

```text
NexusERP/
|
+-- backend/
|   |
|   +-- src/
|   |   +-- NexusERP.Api
|   |   +-- NexusERP.Application
|   |   +-- NexusERP.Domain
|   |   +-- NexusERP.Infrastructure
|   |   +-- NexusERP.Worker
|   |
|   +-- tests/
|       +-- NexusERP.UnitTests
|       +-- NexusERP.IntegrationTests
|
+-- frontend/
+-- docker/
+-- docs/
```

---

## Testing

NexusERP uses both unit and integration testing.

Integration tests exercise real infrastructure through:

- ASP.NET Core `WebApplicationFactory`
- SQL Server Testcontainers
- RabbitMQ Testcontainers
- Entity Framework Core migrations
- Authentication and authorization
- Messaging and persistence flows

Current v1 baseline:

```text
Total:   166
Passed:  166
Failed:  0
Skipped: 0
```

Examples of integration coverage include:

- Authentication and authorization
- Sales and Inventory transactions
- Transactional Outbox persistence
- RabbitMQ publishing and consumption
- Transactional Inbox processing
- Retry and dead-letter behavior
- Infrastructure health checks

---

## Health Checks

The API exposes:

```text
GET /health/live
GET /health/ready
```

Readiness includes SQL Server and RabbitMQ connectivity.

---

## Local Development

Main requirements:

- .NET 9 SDK
- Docker Desktop
- Node.js and npm
- Git

Optional:

- Ollama for AI-generated summaries

The local environment uses Docker for SQL Server and RabbitMQ and .NET User Secrets for sensitive API and Worker configuration.

For complete setup and execution instructions, see:

[`docs/onboarding/DEVELOPMENT_GUIDE.md`](docs/onboarding/DEVELOPMENT_GUIDE.md)

---

## Documentation

Additional project documentation:

- [System Architecture](docs/architecture/SYSTEM_ARCHITECTURE.md)
- [Development Guide](docs/onboarding/DEVELOPMENT_GUIDE.md)
- [Current Project State](docs/project/CURRENT_STATE.md)
- [Roadmap](docs/roadmap/ROADMAP.md)
- [Architecture Decision Records](docs/adr/)

---

## Roadmap

The functional scope for NexusERP v1 is complete.

Current work focuses on:

- release stabilization;
- security and configuration review;
- quality verification;
- frontend polish;
- documentation;
- portfolio presentation.

Potential post-v1 work includes:

- Warehouse Management
- Accounting
- Notifications
- Microservices evolution
- Additional production observability

See the complete [Roadmap](docs/roadmap/ROADMAP.md).

---

## Development Workflow

Development follows:

```text
feature/* -> develop -> main
```

Changes are reviewed through Pull Requests and use Conventional Commit-style messages.

See the [Development Guide](docs/onboarding/DEVELOPMENT_GUIDE.md) and [Contributing Guide](CONTRIBUTING.md).

---

## Project Goals

NexusERP was built to demonstrate practical experience with:

- business-oriented domain modeling;
- backend and frontend integration;
- authentication and authorization;
- relational persistence;
- automated testing;
- asynchronous messaging;
- reliability patterns;
- failure handling;
- local AI integration;
- architecture evolution and technical trade-offs.

The project favors justified engineering decisions over adding technologies solely for complexity.

---

## Contributing

Contribution guidelines are available in:

[CONTRIBUTING.md](CONTRIBUTING.md)

---

## License

See [LICENSE](LICENSE).