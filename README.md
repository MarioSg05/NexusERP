# NexusERP

A modern Enterprise Resource Planning (ERP) system built with Domain-Driven Design (DDD), Clean Architecture and a Modular Monolith architecture using .NET 9.

> Project status: Active development.

---

# Overview

NexusERP is designed as a long-term ERP platform focused on maintainability, scalability and business-driven software design.

The project follows Domain-Driven Design principles and emphasizes:

- Rich Domain Model
- Clean Architecture
- Test-Driven Development (TDD)
- Modular Monolith
- Explicit Architecture Decision Records (ADR)

The objective is to build an ERP that can evolve over time without sacrificing code quality.

---

# Architecture

The solution follows a layered architecture.

```text
Presentation (API)
        |
        v
Application
        |
        v
Domain
        ^
        |
Infrastructure
```

Core architectural principles:

- Domain-Driven Design (DDD)
- Clean Architecture
- Rich Domain Model
- Dependency Inversion
- CQRS-style Application Layer
- In-process Domain Event dispatching
- Transactional Outbox persistence
- Entity Framework Core
- Minimal APIs

---

# Technology Stack

## Backend

- .NET 9
- ASP.NET Core Minimal APIs
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- xUnit

## Frontend

- React
- TypeScript
- Vite
- Tailwind CSS
- TanStack Query
- React Router
- Axios

## AI

- Ollama
- Gemma 3 1B
- Local LLM inference
- Provider-independent AI abstraction

## Infrastructure

- Docker
- SQL Server container
- Redis (planned)
- RabbitMQ (planned)

---

# Implemented Modules

## Authentication and Authorization

- User registration
- Login
- JWT authentication
- Role-based authorization
- User management

## Customers

- Customer management

## Products

- Product management

## Inventory

- Inventory management
- Stock adjustments

## Suppliers

- Supplier management

## Purchasing

- Purchase order management

## Sales

- Sales order management

## Reporting

- Inventory report
- Low-stock report
- Sales report
- Purchasing report

## Dashboard

- Inventory metrics
- Sales metrics
- Purchasing metrics

## AI Business Insights

- Deterministic ERP business analysis
- Locally generated AI summaries
- Ollama integration
- Graceful degradation when the AI provider is unavailable

---

# AI Business Insights

NexusERP includes a local AI Business Insights feature designed to provide business-oriented summaries without making the language model the source of truth for ERP data.

The feature uses a hybrid deterministic/LLM architecture:

```text
ERP Data
   |
   v
BusinessInsightsAnalyzer
   |
   +------------------------+
   |                        |
   v                        v
Verified Business Facts   AI-Safe Signals
   |                        |
   |                        v
   |                IAiInsightsGenerator
   |                        |
   |                        v
   |                      Ollama
   |                        |
   |                        v
   |                   Gemma 3 1B
   |                        |
   v                        v
Business Snapshot       AI Summary
   |                        |
   +-----------+------------+
               |
               v
            Frontend
```

Critical ERP values such as sales totals, purchasing totals, inventory counts and pending order counts are calculated deterministically by NexusERP.

The local language model does not act as the source of truth for those values. It receives only controlled qualitative signals and generates an optional short summary.

If Ollama is unavailable, the deterministic business snapshot remains available and the API returns the AI summary as `null`.

## Local AI Setup

NexusERP uses Ollama as the development AI provider.

Install Ollama and download the configured model:

```bash
ollama pull gemma3:1b
```

Start Ollama:

```bash
ollama serve
```

The default NexusERP configuration expects:

```text
Base URL: http://localhost:11434
Model: gemma3:1b
```

No paid AI API or external AI credentials are required.

### Hardware Compatibility

Ollama selects the available inference backend according to the host hardware.

On systems where GPU acceleration is unavailable or incompatible, CPU inference can be used as a fallback. Hardware-specific Ollama configuration is an environment concern and is not coupled to the NexusERP application layers.

### AI Limitations

- AI summaries may contain inaccuracies.
- AI output is not authoritative ERP data.
- The model has no direct database access.
- The model does not receive authentication credentials.
- The model does not receive customer, supplier or user information in the current Business Insights implementation.
- AI-generated actions cannot mutate ERP state.
- Business Insights currently operates on aggregate inventory, sales and purchasing signals.

---

# Project Structure

```text
NexusERP
|
+-- backend
|   |
|   +-- src
|   |   +-- NexusERP.Api
|   |   +-- NexusERP.Application
|   |   +-- NexusERP.Domain
|   |   +-- NexusERP.Infrastructure
|   |   +-- NexusERP.Shared
|   |
|   +-- tests
|       +-- NexusERP.UnitTests
|       +-- NexusERP.IntegrationTests
|
+-- frontend
|
+-- docker
|
+-- docs
|   +-- adr
|
+-- scripts
```

---

# Testing

NexusERP uses separate unit and integration testing strategies.

## Unit Tests

Unit tests validate domain and application behavior in isolation.

Current coverage includes:

- Aggregates
- Value Objects
- Domain business rules
- Application handlers
- Deterministic AI analysis
- AI provider fallback behavior

Unit tests use xUnit and follow the Arrange / Act / Assert pattern.

AI unit tests do not require Ollama to be running. The AI provider is abstracted through `IAiInsightsGenerator`, allowing the Application layer to be tested deterministically.

## Integration Tests

Backend integration tests exercise NexusERP through the real application stack:

```text
xUnit
  |
  v
WebApplicationFactory
  |
  v
NexusERP.Api
  |
  v
Authentication / Authorization
  |
  v
Application
  |
  v
Infrastructure
  |
  v
Entity Framework Core
  |
  v
SQL Server Testcontainer
```

Integration tests use:

- xUnit
- `Microsoft.AspNetCore.Mvc.Testing`
- `WebApplicationFactory`
- Testcontainers for .NET
- Real SQL Server
- Real Entity Framework Core migrations

The integration test suite currently covers:

- API startup
- Protected endpoint behavior
- Valid and invalid authentication
- Inactive user authentication
- JWT authentication through the real HTTP pipeline
- Role-based authorization
- Customer write/read persistence
- Successful sales order confirmation
- Inventory reduction after sales confirmation
- Insufficient-stock consistency
- Multi-item sales confirmation without partial inventory changes

## Integration Test Isolation

Integration tests run against a disposable SQL Server container.

They do not use the normal NexusERP development database or development database credentials.

The test infrastructure:

1. Starts a SQL Server container.
2. Starts NexusERP through `WebApplicationFactory`.
3. Overrides the database and JWT configuration with test-only values.
4. Applies the real Entity Framework Core migrations.
5. Executes the integration test suite.
6. Disposes the API test host and SQL Server container.

Test data uses isolated identifiers and does not depend on execution order.

## Running Tests

Run the complete backend test suite:

```bash
cd backend
dotnet test
```

Run only unit tests:

```bash
dotnet test tests/NexusERP.UnitTests
```

Run only integration tests:

```bash
dotnet test tests/NexusERP.IntegrationTests
```

### Integration Test Requirement

Docker must be running when executing `NexusERP.IntegrationTests`.

Testcontainers automatically manages the temporary SQL Server instance, so no manually configured integration-test database is required.

Docker is not required to run the unit test suite.

## Current Test Status

- 109 unit tests
- 17 integration tests
- 126 total automated backend tests
- 0 failing tests

# Documentation

Project documentation is available under:

```text
docs/
```

Including:

- Architecture
- ADRs
- Domain documentation
- Roadmap
- Development guide

---

# Development Workflow

Each feature follows the same engineering workflow.

```text
Architecture Workshop
        |
        v
Domain
        |
        v
Unit Tests
        |
        v
Application
        |
        v
Infrastructure
        |
        v
Migration (when required)
        |
        v
API
        |
        v
Swagger / API Verification
        |
        v
Manual Tests
        |
        v
Git Review
        |
        v
Pull Request
        |
        v
Merge
```

Features that do not require persistence changes do not introduce database migrations.

---

# Roadmap

## Completed

- Authentication and authorization
- Customers
- Products
- Inventory
- Suppliers
- Purchasing
- Sales
- Reporting
- Dashboard
- Frontend Phase 1
- AI Business Insights foundation
- Backend integration testing foundation
- In-process Domain Event dispatching
- Transactional Outbox persistence

## Planned

- Microservices evolution
- Warehouse
- Accounting
- Notifications

---

# Contributing

Please read:

- `CONTRIBUTING.md`
- `CODE_OF_CONDUCT.md`

before contributing to the project.

---

# License

This project is licensed under the MIT License.