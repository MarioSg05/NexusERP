using FluentValidation;

namespace NexusERP.Application.Purchasing.CreatePurchaseOrder;

public sealed class CreatePurchaseOrderValidator
    : AbstractValidator<CreatePurchaseOrderRequest>
{
    public CreatePurchaseOrderValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty()
            .WithMessage("Supplier is required.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Purchase order must contain at least one item.");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithMessage("Product is required.");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.");

                item.RuleFor(x => x.UnitPrice)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Unit price cannot be negative.");
            });
    }
}