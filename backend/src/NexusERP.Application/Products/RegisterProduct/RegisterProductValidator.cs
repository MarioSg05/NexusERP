using FluentValidation;

namespace NexusERP.Application.Products.RegisterProduct;

public sealed class RegisterProductValidator
    : AbstractValidator<RegisterProductRequest>
{
    public RegisterProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Sku)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);
    }
}