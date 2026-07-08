# Identity Module

## User

### Properties

- Id
- FirstName
- LastName
- Email
- PasswordHash
- IsActive
- EmailConfirmed

### Behaviors

- Activate()
- Deactivate()
- ChangeEmail()
- ChangePassword()
- ChangeName()
- ConfirmEmail()

## Business Rules

- Todo usuario debe tener un correo válido.
- El correo no puede repetirse.
- La contraseña nunca se almacena en texto plano.
- Un usuario desactivado no puede iniciar sesión.
- El correo debe confirmarse antes de acceder al sistema.