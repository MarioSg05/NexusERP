# NexusERP - Current Project State

Last updated: 2026-08-27

## Project Status

NexusERP has entered **v1.0 Feature Freeze**.

The functional scope for NexusERP v1 is considered complete.

No new business features will be introduced before v1.0.0.

Allowed changes during the release preparation phase:

- Bug fixes
- Security fixes
- Test improvements
- Documentation
- Developer experience improvements
- Release preparation
- Portfolio presentation improvements

Out of scope until after v1.0:

- Warehouse
- Accounting
- Notifications
- New AI capabilities
- Microservice extraction
- New business modules

---

## Current Baseline

- Branch: `develop`
- Remote branch: `origin/develop`
- Working tree: clean
- Last completed issue: `ID-035 - Add messaging health checks and observability`
- Last merged pull request: `#74`
- Last completed milestone: `Sprint 12 - Microservices`
- Build: passing
- Automated tests: 166 passed
- Failed tests: 0
- Skipped tests: 0

---

## Architectural Style

NexusERP currently follows:

- Domain-Driven Design (DDD)
- Clean Architecture
- Rich Domain Model
- Modular Monolith
- Vertical Slice Architecture
- CQRS-style Application Layer
- Dependency Inversion
- In-process Domain Events
- Integration Events
- Reliable asynchronous messaging

NexusERP is currently a **Modular Monolith**, not a fully distributed
microservices architecture.

The messaging infrastructure establishes evolutionary boundaries that may
support future service extraction without requiring premature decomposition
of the current system.

---

## Backend Projects

The backend solution currently contains:

- `NexusERP.Api`
- `NexusERP.Application`
- `NexusERP.Domain`
- `NexusERP.Infrastructure`
- `NexusERP.Shared`
- `NexusERP.Worker`
- `NexusERP.UnitTests`
- `NexusERP.IntegrationTests`

---

## Implemented Business Capabilities

### Identity and Security

- User registration
- Login
- JWT authentication
- Role-based authorization
- User management

### Customers

- Customer management

### Products

- Product management

### Inventory

- Inventory management
- Stock adjustments

### Suppliers

- Supplier management

### Purchasing

- Purchase order management

### Sales

- Sales order management

### Reports

- Inventory report
- Low-stock report
- Sales report
- Purchasing report

### Dashboard

- Inventory metrics
- Sales metrics
- Purchasing metrics

### AI Business Insights

- Deterministic ERP business analysis
- Local AI summaries using Ollama
- Provider-independent AI abstraction
- Graceful degradation when the AI provider is unavailable

The language model is not a source of truth for ERP business data.

---

## Messaging Architecture

The distributed messaging foundation currently follows this flow:

```text
Domain Operation
       |
       v
Domain Event
       |
       v
Domain Event Dispatcher
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
Integration Event Consumer
       |
       v
Transactional Inbox
       |
       v
Integration Event Handler

Reliability

The messaging pipeline includes:

Transactional Outbox
Transactional Inbox
Idempotent event consumption
Retry handling
Dead-letter handling
Background processing through NexusERP.Worker
Observability

Current operational diagnostics include:

SQL Server health check
RabbitMQ health check
Liveness endpoint
Readiness endpoint
Structured Outbox logging
Structured Integration Event consumer logging
Retry diagnostics
Dead-letter diagnostics
Message correlation using MessageId

Health endpoints:

GET /health/live
GET /health/ready
Data and Infrastructure

Current infrastructure includes:

SQL Server
Entity Framework Core
Entity Framework Core migrations
Docker
RabbitMQ
.NET Worker Service
Ollama for local AI inference
Testcontainers for integration testing
Testing Strategy

NexusERP uses separate unit and integration testing strategies.

Unit Tests

Unit tests cover domain and application behavior in isolation.

Integration Tests

Integration tests exercise the real backend stack using:

xUnit
WebApplicationFactory
ASP.NET Core
Authentication and authorization
Application layer
Infrastructure layer
Entity Framework Core
SQL Server Testcontainers
Real database migrations
Current Test Baseline
Total:   166
Passed:  166
Failed:  0
Skipped: 0
Frontend

Frontend Phase 1 is complete.

Current frontend technologies include:

React
TypeScript
Vite
Tailwind CSS
TanStack Query
React Router
Axios

The frontend consumes the NexusERP HTTP API.

Known Documentation Debt

The following documentation requires synchronization before v1.0.0:

SYSTEM_ARCHITECTURE.md predates the current messaging architecture.
ROADMAP.md represents the original roadmap and is obsolete.
DEVELOPMENT_GUIDE.md does not yet describe the complete development environment.
README.md contains outdated automated test counts.
README.md project structure does not yet include NexusERP.Worker.
ADR coverage predates the distributed messaging architecture.
Known Review Items

The v1 release review must verify:

Architecture boundaries
Domain model consistency
Application layer consistency
Infrastructure dependencies
Authentication and authorization
Secret management
CORS configuration
Exception handling
Database migrations
Messaging reliability
Retry and dead-letter behavior
Health checks
Automated test coverage
Frontend quality
Docker developer experience
Repository cleanliness
Documentation accuracy

No refactoring should be performed solely for stylistic reasons during
Feature Freeze.

Changes require a concrete correctness, security, maintainability,
documentation, or release-readiness justification.

NexusERP v1 Release Goal

NexusERP v1.0.0 is intended to represent a stable portfolio-ready release
demonstrating the design and implementation of a modern ERP platform using
enterprise software engineering practices.

The release should demonstrate:

Business-oriented domain modeling
Maintainable application architecture
Reliable persistence
Authentication and authorization
Automated testing
Asynchronous messaging
Failure handling
Operational health monitoring
Local AI integration with deterministic ERP data boundaries
Full-stack integration
Post-v1 Roadmap

Potential work after NexusERP v1.0.0 includes:

Microservices evolution
Warehouse management
Accounting
Notifications
Additional observability infrastructure
Distributed tracing
Production deployment improvements

These items are intentionally excluded from the v1 Feature Freeze.

Immediate Next Step

Perform the NexusERP v1 Architecture Review.

The architecture review will determine the concrete stabilization issues
required before the v1.0.0 release candidate.