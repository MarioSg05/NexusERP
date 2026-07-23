using FluentValidation;

namespace NexusERP.Application.Products.RegisterProduct;

public sealed class RegisterProductValidator
    : AbstractValidator<RegisterProductRequest>
{
    public RegisterProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(200)
            .WithMessage("Product name cannot exceed 200 characters.");

        RuleFor(x => x.Sku)
            .NotEmpty()
            .WithMessage("Product SKU is required.")
            .MaximumLength(50)
            .WithMessage("Product SKU cannot exceed 50 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Product price cannot be negative.");
    }
}