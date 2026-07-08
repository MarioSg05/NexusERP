# Identity Domain Model

## Entities

### User

Representa una identidad que puede autenticarse dentro del sistema.

### Role

Representa un conjunto de permisos asignables a usuarios.

### Permission

Representa una acción autorizada dentro del sistema.

---

## Value Objects

### Email

Representa un correo electrónico válido.

### PasswordHash

Representa una contraseña protegida mediante hashing.

---

## Relaciones

User

↓

Role

↓

Permission