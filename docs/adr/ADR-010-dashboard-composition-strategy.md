# ADR-010: Dashboard Composition Strategy

## Status

Accepted

---

## Context

After introducing the reporting architecture (ADR-009), NexusERP required a mechanism to provide executive-level information without exposing operational reports directly.

Operational reports answer business questions by returning detailed datasets such as inventory items, purchase orders or sales orders.

A dashboard has different requirements.

Its purpose is to present high-level business indicators (KPIs) that summarize the current state of the ERP rather than detailed operational information.

This introduces a different read model focused on aggregation rather than reporting.

---

## Decision

NexusERP introduces a dedicated Dashboard composition layer.

The Dashboard is not considered part of the Domain Model.

It is an application-level composition module responsible for aggregating business indicators obtained from the persistence layer.

Dashboard requests never modify application state.

---

## Architecture

Dashboard follows the architecture below.

API

↓

Dashboard Handler

↓

IDashboardQueries

↓

DashboardQueries

↓

Entity Framework Core

↓

SQL Server

Dashboard handlers never access Entity Framework Core directly.

All KPI retrieval is encapsulated inside DashboardQueries.

---

## Dashboard Queries

DashboardQueries are independent from ReportQueries.

Although both belong to the read side of the application, they have different responsibilities.

ReportQueries provide operational reports.

DashboardQueries provide executive KPIs.

DashboardQueries never return collections of business entities.

Instead, each query returns a strongly typed dashboard widget.

---

## Dashboard Widgets

The dashboard is composed of independent widgets.

Each widget represents a cohesive business area.

Current widgets include:

- Inventory
- Sales
- Purchasing

Each widget is responsible only for its own business indicators.

This allows the dashboard to grow without modifying existing widgets.

Future widgets may include:

- Finance
- CRM
- Manufacturing
- Human Resources

---

## Query Strategy

Dashboard queries prioritize aggregation inside SQL Server.

Typical operations include:

- COUNT
- SUM
- AVG
- MIN
- MAX

Dashboard never materializes complete collections simply to calculate KPIs.

Aggregation is delegated to the database whenever possible.

---

## Separation of Responsibilities

ReportQueries answer:

"What happened?"

DashboardQueries answer:

"How is the business performing?"

Both modules belong to the read side of the application but serve different purposes.

---

## Benefits

- Clear separation between operational reporting and executive dashboards.
- Efficient KPI generation through SQL aggregation.
- Dashboard remains independent from the Domain Model.
- Widgets are cohesive and independently extensible.
- Future dashboard sections can be added without breaking existing contracts.
- Read-side architecture remains modular and scalable.

---

## Tradeoffs

- Dashboard introduces a second read-query abstraction alongside ReportQueries.
- Some business information exists in both reports and dashboard widgets, but with different granularity.
- Dashboard queries rely primarily on SQL aggregation rather than object materialization.

These tradeoffs are accepted because they preserve separation of concerns while optimizing dashboard performance.