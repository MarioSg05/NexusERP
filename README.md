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

Current testing strategy:

- xUnit
- Arrange / Act / Assert
- Aggregate tests
- Value Object tests
- Application tests
- Deterministic AI analysis tests
- AI provider fallback tests
- Integration test project

Current project status:

- 109 Unit Tests
- 0 Failing Unit Tests

AI unit tests do not require Ollama to be running. The AI provider is abstracted through `IAiInsightsGenerator`, allowing the Application layer to be tested deterministically.

---

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

## Planned

- Additional AI capabilities
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