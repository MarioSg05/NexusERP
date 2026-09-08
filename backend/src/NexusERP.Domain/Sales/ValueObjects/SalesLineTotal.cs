using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Sales.ValueObjects;

public sealed record SalesLineTotal
{
    public decimal Value { get; }

    /// <summary>
    /// Initializes a sales line total from its persisted value.
    /// Intended for Entity Framework Core materialization.
    /// </summary>
    public SalesLineTotal(decimal value)
    {
        Value = decimal.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);
    }

    public SalesLineTotal(
        SalesQuantity quantity,
        SalesUnitPrice unitPrice)
    {
        if (quantity is null)
        {
            throw new DomainException(
                "Sales quantity is required.");
        }

        if (unitPrice is null)
        {
            throw new DomainException(
                "Sales unit price is required.");
        }

        Value = decimal.Round(
            quantity.Value * unitPrice.Value,
            2,
            MidpointRounding.AwayFromZero);
    }

    public override string ToString()
        => Value.ToString("0.00");
}