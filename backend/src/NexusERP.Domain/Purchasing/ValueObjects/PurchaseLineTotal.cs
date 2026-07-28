using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Purchasing.ValueObjects;

public sealed record PurchaseLineTotal
{
    public decimal Value { get; }

    /// <summary>
    /// Initializes a purchase line total from its persisted value.
    /// Intended for Entity Framework Core materialization.
    /// </summary>
    public PurchaseLineTotal(decimal value)
    {
        Value = decimal.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);
    }

    public PurchaseLineTotal(
        PurchaseQuantity quantity,
        PurchaseUnitPrice unitPrice)
    {
        if (quantity is null)
        {
            throw new DomainException(
                "Purchase quantity is required.");
        }

        if (unitPrice is null)
        {
            throw new DomainException(
                "Purchase unit price is required.");
        }

        Value = decimal.Round(
            quantity.Value * unitPrice.Value,
            2,
            MidpointRounding.AwayFromZero);
    }

    public override string ToString()
        => Value.ToString("0.00");
}