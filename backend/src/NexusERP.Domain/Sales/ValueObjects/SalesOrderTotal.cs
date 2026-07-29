using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Sales.ValueObjects;

public sealed record SalesOrderTotal
{
    public decimal Value { get; }

    public SalesOrderTotal(decimal value)
    {
        if (value < 0)
        {
            throw new DomainException(
                "Sales order total cannot be negative.");
        }

        Value = decimal.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);
    }

    public override string ToString()
        => Value.ToString("0.00");
}