using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Infrastructure.Persistence.Converters;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration
    : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasConversion<CustomerNameConverter>()
            .HasMaxLength(CustomerName.MaxLength)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasConversion<CustomerEmailConverter>()
            .HasMaxLength(CustomerEmail.MaxLength)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasConversion<CustomerPhoneConverter>()
            .HasMaxLength(CustomerPhone.MaxLength)
            .IsRequired(false);

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}