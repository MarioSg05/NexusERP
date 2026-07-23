using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Inventory.ValueObjects;

public sealed record InventoryQuantity
{
    public int Value { get; }

    public InventoryQuantity(int value)
    {
        if (value < 0)
            throw new DomainException(
                "Inventory quantity cannot be negative.");

        Value = value;
    }

    public override string ToString()
        => Value.ToString();
}