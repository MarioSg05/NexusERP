using FluentValidation;

namespace NexusERP.Application.Identity.LoginUser;

public sealed class LoginUserValidator
    : AbstractValidator<LoginUserRequest>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}