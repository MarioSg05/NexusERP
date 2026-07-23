using FluentValidation;

namespace NexusERP.Application.Inventory.CreateInventory;

public sealed class CreateInventoryValidator
    : AbstractValidator<CreateInventoryRequest>
{
    public CreateInventoryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product is required.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Inventory quantity cannot be negative.");
    }
}