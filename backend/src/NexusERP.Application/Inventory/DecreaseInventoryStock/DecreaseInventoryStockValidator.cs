using FluentValidation;

namespace NexusERP.Application.Inventory.DecreaseInventoryStock;

public sealed class DecreaseInventoryStockValidator
    : AbstractValidator<DecreaseInventoryStockRequest>
{
    public DecreaseInventoryStockValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(
                "Stock decrease must be greater than zero.");
    }
}