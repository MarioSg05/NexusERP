using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusERP.Domain.Purchasing.Entities;
using NexusERP.Infrastructure.Persistence.Converters;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class PurchaseOrderItemConfiguration
    : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.ToTable("PurchaseOrderItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasConversion<PurchaseQuantityConverter>()
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasConversion<PurchaseUnitPriceConverter>()
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.LineTotal)
            .HasConversion<PurchaseLineTotalConverter>()
            .HasColumnType("decimal(18,2)")
            .IsRequired();

    }
}