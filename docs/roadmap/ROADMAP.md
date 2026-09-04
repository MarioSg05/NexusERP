# NexusERP Roadmap

NexusERP has completed its planned v1 functional scope and is currently
in **v1.0 release preparation**.

The project is under Feature Freeze. No new business modules will be added
before the v1.0.0 release.

## Completed for v1

### Platform

- Clean Architecture and Domain-Driven Design foundation
- Modular Monolith
- SQL Server persistence with Entity Framework Core
- Docker development environment
- Authentication and role-based authorization
- Unit and integration testing
- Health checks and structured diagnostics

### ERP Capabilities

- Identity and User Management
- Customers
- Products
- Inventory
- Suppliers
- Purchasing
- Sales
- Reports
- Dashboard
- AI Business Insights

### Frontend

- React and TypeScript application
- Authentication flow
- ERP navigation
- Dashboard and business module interfaces
- Server-state integration with TanStack Query

### Messaging

- Domain Events
- Integration Events
- Transactional Outbox
- Background Worker
- RabbitMQ publishing and consumption
- Transactional Inbox
- Retry and dead-letter handling
- Publisher Confirms
- Messaging health checks and diagnostics

## Current Phase — NexusERP v1.0

Current work is limited to release preparation:

- Architecture stabilization
- Documentation synchronization
- Security and configuration review
- Code and test quality review
- Frontend release polish
- Portfolio presentation
- Release candidate validation

No new business functionality is planned during this phase.

## Post-v1

Potential future work includes:

- Warehouse Management
- Accounting
- Notifications
- Microservices evolution
- Additional observability and distributed tracing
- Production deployment improvements

Post-v1 work will be prioritized only after the v1.0.0 release and should
be driven by concrete product or operational requirements.