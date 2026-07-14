# ADR-002: Rich Domain Model

## Estado

Aceptado

## Contexto

El proyecto requiere un dominio que encapsule reglas de negocio y evite modelos anémicos donde la lógica se concentra en servicios.

## Decisión

Las entidades del dominio serán modelos ricos (Rich Domain Model). Las reglas de negocio vivirán dentro de las entidades mediante métodos de dominio.

## Consecuencias

- Las entidades tendrán comportamiento además de datos.
- Se evitarán setters públicos.
- El dominio protegerá sus invariantes.
- Los cambios de estado siempre pasarán por métodos del Aggregate Root.