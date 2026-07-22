using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusERP.Domain.Products.Aggregates;
using NexusERP.Domain.Products.ValueObjects;
using NexusERP.Infrastructure.Persistence.Converters;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration
    : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasConversion<ProductNameConverter>()
            .HasMaxLength(ProductName.MaxLength)
            .IsRequired();

        builder.Property(x => x.Sku)
            .HasConversion<ProductSkuConverter>()
            .HasMaxLength(ProductSku.MaxLength)
            .IsRequired();

        builder.HasIndex(x => x.Sku)
            .IsUnique();

        builder.Property(x => x.Price)
            .HasConversion<ProductPriceConverter>()
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}