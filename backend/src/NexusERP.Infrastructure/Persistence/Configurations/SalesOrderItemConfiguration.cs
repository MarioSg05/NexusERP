using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusERP.Domain.Sales.Entities;
using NexusERP.Infrastructure.Persistence.Converters;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class SalesOrderItemConfiguration
    : IEntityTypeConfiguration<SalesOrderItem>
{
    public void Configure(EntityTypeBuilder<SalesOrderItem> builder)
    {
        builder.ToTable("SalesOrderItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasConversion<SalesQuantityConverter>()
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasConversion<SalesUnitPriceConverter>()
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.LineTotal)
            .HasConversion<SalesLineTotalConverter>()
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}