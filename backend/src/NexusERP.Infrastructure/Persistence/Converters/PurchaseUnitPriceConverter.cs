using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class PurchaseUnitPriceConverter
    : ValueConverter<PurchaseUnitPrice, decimal>
{
    public PurchaseUnitPriceConverter()
        : base(
            price => price.Value,
            value => new PurchaseUnitPrice(value))
    {
    }
}