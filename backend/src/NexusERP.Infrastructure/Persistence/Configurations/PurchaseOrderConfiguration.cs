using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusERP.Domain.Purchasing.Aggregates;
using NexusERP.Infrastructure.Persistence.Converters;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class PurchaseOrderConfiguration
    : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SupplierId)
            .IsRequired();

        builder.Property(x => x.OrderDate)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Total)
            .HasConversion<PurchaseOrderTotalConverter>()
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey("PurchaseOrderId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}