using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusERP.Domain.Inventory.Aggregates;
using NexusERP.Domain.Inventory.ValueObjects;
using NexusERP.Infrastructure.Persistence.Converters;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class InventoryItemConfiguration
    : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("Inventory");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.HasIndex(x => x.ProductId)
            .IsUnique();

        builder.Property(x => x.Quantity)
            .HasConversion<InventoryQuantityConverter>()
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}