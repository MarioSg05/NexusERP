using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Purchasing.ValueObjects;

public sealed record PurchaseOrderTotal
{
    public decimal Value { get; }

    public PurchaseOrderTotal(decimal value)
    {
        if (value < 0)
        {
            throw new DomainException(
                "Purchase order total cannot be negative.");
        }

        Value = decimal.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);
    }

    public override string ToString()
        => Value.ToString("0.00");
}