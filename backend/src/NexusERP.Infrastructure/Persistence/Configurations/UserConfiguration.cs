using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NexusERP.Domain.Identity.Aggregates;
using NexusERP.Infrastructure.Persistence.Converters;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration
    : IEntityTypeConfiguration<User>
{
    public void Configure(
        EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasConversion<PersonNameConverter>()
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasConversion<PersonNameConverter>()
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasConversion<EmailConverter>()
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .HasConversion<PasswordHashConverter>()
            .IsRequired();

        builder.Property(x => x.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}