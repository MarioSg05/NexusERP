using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Products.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class ProductSkuConverter
    : ValueConverter<ProductSku, string>
{
    public ProductSkuConverter()
        : base(
            sku => sku.Value,
            value => new ProductSku(value))
    {
    }
}