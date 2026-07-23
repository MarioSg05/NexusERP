using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Products.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class ProductNameConverter
    : ValueConverter<ProductName, string>
{
    public ProductNameConverter()
        : base(
            name => name.Value,
            value => new ProductName(value))
    {
    }
}