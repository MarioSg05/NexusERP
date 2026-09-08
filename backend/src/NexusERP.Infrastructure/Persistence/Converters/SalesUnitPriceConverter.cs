using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Sales.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class SalesUnitPriceConverter
    : ValueConverter<SalesUnitPrice, decimal>
{
    public SalesUnitPriceConverter()
        : base(
            price => price.Value,
            value => new SalesUnitPrice(value))
    {
    }
}