using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Sales.ValueObjects;

public sealed record SalesUnitPrice
{
    public decimal Value { get; }

    public SalesUnitPrice(decimal value)
    {
        if (value < 0)
        {
            throw new DomainException(
                "Sales unit price cannot be negative.");
        }

        Value = decimal.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);
    }

    public override string ToString()
        => Value.ToString("0.00");
}