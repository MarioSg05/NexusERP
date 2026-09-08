using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Sales.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class SalesQuantityConverter
    : ValueConverter<SalesQuantity, int>
{
    public SalesQuantityConverter()
        : base(
            quantity => quantity.Value,
            value => new SalesQuantity(value))
    {
    }
}