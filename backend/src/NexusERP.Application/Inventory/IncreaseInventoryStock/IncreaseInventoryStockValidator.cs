using FluentValidation;

namespace NexusERP.Application.Inventory.IncreaseInventoryStock;

public sealed class IncreaseInventoryStockValidator
    : AbstractValidator<IncreaseInventoryStockRequest>
{
    public IncreaseInventoryStockValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(
                "Stock increase must be greater than zero.");
    }
}