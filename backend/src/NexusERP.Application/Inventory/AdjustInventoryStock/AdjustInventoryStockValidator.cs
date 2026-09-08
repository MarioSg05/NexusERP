using FluentValidation;

namespace NexusERP.Application.Inventory.AdjustInventoryStock;

public sealed class AdjustInventoryStockValidator
    : AbstractValidator<AdjustInventoryStockRequest>
{
    public AdjustInventoryStockValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage(
                "Inventory quantity cannot be negative.");
    }
}