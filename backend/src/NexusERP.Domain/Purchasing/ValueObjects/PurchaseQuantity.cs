using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Purchasing.ValueObjects;

public sealed record PurchaseQuantity
{
    public int Value { get; }

    public PurchaseQuantity(int value)
    {
        if (value <= 0)
        {
            throw new DomainException(
                "Purchase quantity must be greater than zero.");
        }

        Value = value;
    }

    public override string ToString()
        => Value.ToString();
}