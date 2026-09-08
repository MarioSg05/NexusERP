using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class PurchaseQuantityConverter
    : ValueConverter<PurchaseQuantity, int>
{
    public PurchaseQuantityConverter()
        : base(
            quantity => quantity.Value,
            value => new PurchaseQuantity(value))
    {
    }
}