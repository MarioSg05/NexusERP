using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Products.ValueObjects;

public sealed record ProductPrice
{
    public decimal Value { get; }

    public ProductPrice(decimal value)
    {
        if (value < 0)
            throw new DomainException(
                "Product price cannot be negative.");

        Value = decimal.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);
    }

    public override string ToString()
        => Value.ToString("0.00");
}