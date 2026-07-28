using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusERP.Domain.Suppliers.Aggregates;
using NexusERP.Domain.Suppliers.ValueObjects;
using NexusERP.Infrastructure.Persistence.Converters;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class SupplierConfiguration
    : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasConversion<SupplierNameConverter>()
            .HasMaxLength(SupplierName.MaxLength)
            .IsRequired();

        builder.Property(x => x.TaxIdentifier)
            .HasConversion<SupplierTaxIdentifierConverter>()
            .HasMaxLength(SupplierTaxIdentifier.MaxLength)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasConversion<SupplierEmailConverter>()
            .HasMaxLength(SupplierEmail.MaxLength)
            .IsRequired(false);

        builder.Property(x => x.Phone)
            .HasConversion<SupplierPhoneConverter>()
            .HasMaxLength(SupplierPhone.MaxLength)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.TaxIdentifier)
            .IsUnique();

    }
}