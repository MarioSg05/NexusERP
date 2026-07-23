using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Products.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class ProductPriceConverter
    : ValueConverter<ProductPrice, decimal>
{
    public ProductPriceConverter()
        : base(
            price => price.Value,
            value => new ProductPrice(value))
    {
    }
}