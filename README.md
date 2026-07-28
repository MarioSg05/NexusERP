# NexusERP

A modern Enterprise Resource Planning (ERP) system built with Domain-Driven Design (DDD), Clean Architecture and a Modular Monolith architecture using .NET 9.

> ⚠️ Project status: Active development.

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

↓

Application

↓

Domain

↓

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

Backend

- .NET 9
- ASP.NET Core Minimal APIs
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- xUnit

Frontend

- React
- Next.js

Infrastructure

- Docker
- Redis (planned)
- RabbitMQ (planned)

---

# Implemented Modules

Authentication

- User Registration
- Login
- JWT Authentication

Customers

- Customer management

Products

- Product management

Inventory

- Inventory management

Suppliers

- Supplier management

Purchasing

- Purchase Orders

---

# Project Structure

```text
backend
│
├── src
│   ├── NexusERP.Api
│   ├── NexusERP.Application
│   ├── NexusERP.Domain
│   ├── NexusERP.Infrastructure
│   └── NexusERP.Shared
│
├── tests
│   ├── NexusERP.UnitTests
│   └── NexusERP.IntegrationTests
│
└── docs
```

---

# Testing

Current testing strategy:

- xUnit
- Arrange / Act / Assert
- Aggregate Tests
- Value Object Tests
- Integration Tests

Current project status:

- 92 Unit Tests
- 0 Failing Tests

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

↓

Domain

↓

Unit Tests

↓

Application

↓

Infrastructure

↓

Migration

↓

API

↓

Swagger

↓

Manual Tests

↓

Git Review

↓

Pull Request

↓

Merge
```

---

# Roadmap

Completed

- Authentication
- Customers
- Products
- Inventory
- Suppliers
- Purchasing

Planned

- Sales
- Warehouse
- Accounting
- Reporting
- Notifications

---

# Contributing

Please read:

- CONTRIBUTING.md
- CODE_OF_CONDUCT.md

before contributing to the project.

---

# License

This project is licensed under the MIT License.