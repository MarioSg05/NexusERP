using FluentValidation;

using NexusERP.Domain.Identity.Enums;

namespace NexusERP.Application.Identity.ChangeUserRole;

public sealed class ChangeUserRoleValidator
    : AbstractValidator<ChangeUserRoleRequest>
{
    public ChangeUserRoleValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(BeValidRole)
            .WithMessage(
                "Role must be Administrator, Manager, or Viewer.");
    }

    private static bool BeValidRole(
        string role)
    {
        return Enum.TryParse<UserRole>(
            role,
            ignoreCase: true,
            out var parsedRole)
            && Enum.IsDefined(parsedRole);
    }
}