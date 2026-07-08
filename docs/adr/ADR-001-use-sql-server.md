# ADR-001: Use SQL Server

## Estado

Aceptado

## Contexto

NexusERP requiere una base de datos relacional robusta para soportar procesos empresariales como inventario, ventas, compras y facturación.

## Decisión

Se utilizará Microsoft SQL Server como base de datos principal.

## Justificación

- Excelente integración con .NET y Entity Framework Core.
- Amplia adopción en empresas de Guatemala y Latinoamérica.
- Ideal para fortalecer habilidades demandadas en el mercado laboral.
- Soporte para procedimientos almacenados, funciones y herramientas empresariales.

## Consecuencias

- El proyecto dependerá inicialmente de SQL Server.
- En el futuro podrá abstraerse mediante Entity Framework para facilitar cambios si fueran necesarios.