using FluentValidation;

using NexusERP.Domain.Identity.Enums;

namespace NexusERP.Application.Identity.RegisterUser;

public sealed class RegisterUserValidator
    : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

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