# Identity - Event Storming

## Objetivo

Definir los eventos de negocio del módulo Identity.

---

## Eventos

- Usuario registrado.
- Usuario activado.
- Usuario desactivado.
- Correo confirmado.
- Contraseña cambiada.
- Rol asignado.
- Rol removido.
- Permiso asignado.
- Permiso removido.

---

## Actores

- Administrador.
- Usuario.

---

## Reglas de negocio

- Todo usuario debe tener un correo válido.
- El correo debe ser único.
- La contraseña nunca se almacena en texto plano.
- Un usuario desactivado no puede iniciar sesión.
- Solo un administrador puede asignar roles.
- Solo un administrador puede desactivar usuarios.