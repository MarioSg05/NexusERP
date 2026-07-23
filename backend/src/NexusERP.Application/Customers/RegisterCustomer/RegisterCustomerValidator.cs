using FluentValidation;

namespace NexusERP.Application.Customers.RegisterCustomer;

public sealed class RegisterCustomerValidator
    : AbstractValidator<RegisterCustomerRequest>
{
    public RegisterCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Customer name is required.")
            .MaximumLength(200)
            .WithMessage("Customer name cannot exceed 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Customer email is required.")
            .EmailAddress()
            .WithMessage("Customer email is not valid.");

        RuleFor(x => x.Phone)
            .MaximumLength(25)
            .WithMessage("Customer phone cannot exceed 25 characters.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Customer type is invalid.");
    }
}