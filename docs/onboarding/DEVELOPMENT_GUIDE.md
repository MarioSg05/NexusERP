# NexusERP Development Guide

This guide describes the local development environment for NexusERP.

## Prerequisites

Install:

- .NET 9 SDK
- Docker Desktop
- Node.js and npm
- Git

Optional:

- Ollama, required only for local AI-generated summaries

## Repository Structure

```text
NexusERP/
├── backend/
├── frontend/
├── docker/
└── docs/
```

## 1. Infrastructure

NexusERP uses Docker for:

- SQL Server
- RabbitMQ

From the `docker` directory, create the local environment file:

```cmd
copy .env.example .env
```

Update `.env` with local development credentials:

```text
MSSQL_SA_PASSWORD=<your-local-password>
RABBITMQ_DEFAULT_USER=nexuserp
RABBITMQ_DEFAULT_PASS=<your-local-password>
```

The `.env` file is ignored by Git and must not be committed.

Start the containers:

```cmd
docker compose up -d
```

Verify them:

```cmd
docker ps
```

Default local ports:

| Service | Port |
|---|---:|
| SQL Server | 14330 |
| RabbitMQ | 5672 |
| RabbitMQ Management UI | 15672 |

## 2. Backend Configuration

Sensitive local .NET configuration is stored with **User Secrets** rather
than committed to `appsettings` files.

### API

From the repository root:

```cmd
cd backend\src\NexusERP.Api
```

Configure the SQL Server connection:

```cmd
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,14330;Database=NexusERPDb;User Id=sa;Password=<your-sql-password>;TrustServerCertificate=True;"
```

Configure the JWT signing key:

```cmd
dotnet user-secrets set "Jwt:Key" "<your-development-jwt-signing-key>"
```

Configure RabbitMQ:

```cmd
dotnet user-secrets set "RabbitMq:ConnectionString" "amqp://nexuserp:<your-rabbitmq-password>@localhost:5672/"
```

Non-sensitive JWT settings and other defaults remain in the application
configuration files.

### Worker

From:

```cmd
cd backend\src\NexusERP.Worker
```

Configure SQL Server:

```cmd
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,14330;Database=NexusERPDb;User Id=sa;Password=<your-sql-password>;TrustServerCertificate=True;"
```

Configure RabbitMQ:

```cmd
dotnet user-secrets set "RabbitMq:ConnectionString" "amqp://nexuserp:<your-rabbitmq-password>@localhost:5672/"
```

Do not commit development credentials or User Secrets.

## 3. Database

From the `backend` directory, apply migrations with:

```cmd
dotnet ef database update --project src/NexusERP.Infrastructure --startup-project src/NexusERP.Api
```

This creates or updates the local `NexusERPDb` database.

## 4. Run the API

From `backend`:

```cmd
dotnet run --project src/NexusERP.Api
```

The development launch profile determines the local API URL.

Swagger is available in the Development environment at:

```text
/swagger
```

## 5. Run the Worker

Open a second terminal in `backend`:

```cmd
dotnet run --project src/NexusERP.Worker
```

The Worker processes:

- Transactional Outbox messages
- RabbitMQ Integration Events
- Retry and dead-letter flows

For normal messaging development, run both the API and Worker.

## 6. Frontend

From the repository root:

```cmd
cd frontend
npm install
npm run dev
```

The Vite development server normally runs at:

```text
http://localhost:5173
```

Available frontend commands:

```cmd
npm run dev
npm run build
npm run lint
npm run preview
```

## 7. Ollama

Ollama is optional.

Without Ollama, the core ERP remains functional; only AI-generated narrative
summaries are unavailable.

The default local configuration expects:

```text
Base URL: http://localhost:11434
Model: gemma3:1b
```

After installing Ollama, ensure the configured model is available locally
before testing AI Business Insights.

## 8. Health Checks

With the API running:

```text
GET /health/live
GET /health/ready
```

`/health/live` checks application liveness.

`/health/ready` includes required infrastructure dependencies such as:

- SQL Server
- RabbitMQ

## 9. Build

From `backend`:

```cmd
dotnet build
```

The complete backend solution should build successfully before committing.

## 10. Automated Tests

From `backend`:

```cmd
dotnet test
```

Integration tests use Testcontainers and require Docker to be available.

Current v1 baseline:

```text
166 tests
166 passed
0 failed
0 skipped
```

## 11. Frontend Verification

Before submitting frontend changes:

```cmd
cd frontend
npm run lint
npm run build
```

Both commands should complete successfully.

## 12. Git Workflow

Primary branches:

```text
main
develop
feature/*
```

New work starts from an updated `develop` branch.

Example:

```cmd
git checkout develop
git pull origin develop
git checkout -b feature/ID-XXX-short-description
```

Changes are merged into `develop` through Pull Requests.

`main` is reserved for stable release integration.

## 13. Commit Convention

Use Conventional Commit-style prefixes:

```text
feat
fix
docs
refactor
test
chore
```

Examples:

```text
feat(sales): add sales order confirmation
fix(messaging): enable RabbitMQ publisher confirms
docs: update system architecture
test(integration): cover outbox processing
```

## 14. Definition of Done

Before considering a change complete:

- The intended behavior is implemented.
- Architecture boundaries remain respected.
- No secrets are committed.
- `dotnet build` succeeds.
- `dotnet test` succeeds.
- Frontend lint/build succeeds when frontend code changes.
- Relevant documentation is updated.
- The working tree contains no accidental files.
- The change is reviewed through a Pull Request.

## 15. Stop Local Infrastructure

When finished:

```cmd
cd docker
docker compose down
```

Docker volumes are preserved unless they are explicitly removed.