using FluentValidation;

namespace NexusERP.Application.Products.UpdateProduct;

public sealed class UpdateProductValidator
    : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(200)
            .WithMessage(
                "Product name cannot exceed 200 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "Product price cannot be negative.");
    }
}