using FluentValidation;

namespace NexusERP.Application.Customers.RegisterCustomer;

public sealed class RegisterCustomerValidator
    : AbstractValidator<RegisterCustomerRequest>
{
    public RegisterCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Phone)
            .MaximumLength(25);

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}