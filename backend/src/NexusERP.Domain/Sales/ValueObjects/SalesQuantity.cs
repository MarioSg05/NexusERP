using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Sales.ValueObjects;

public sealed record SalesQuantity
{
    public int Value { get; }

    public SalesQuantity(int value)
    {
        if (value <= 0)
        {
            throw new DomainException(
                "Sales quantity must be greater than zero.");
        }

        Value = value;
    }

    public override string ToString()
        => Value.ToString();
}