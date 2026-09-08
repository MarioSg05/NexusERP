using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NexusERP.Infrastructure.Messaging.Outbox;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class OutboxMessageConfiguration
    : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(
        EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(
            "OutboxMessages");

        builder.HasKey(
            x => x.Id);

        builder.Property(
                x => x.OccurredOnUtc)
            .IsRequired();

        builder.Property(
                x => x.Type)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(
                x => x.Payload)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(
                x => x.ProcessedOnUtc)
            .IsRequired(false);

        builder.Property(
                x => x.Error)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);

        builder.HasIndex(
            x => x.ProcessedOnUtc);
    }
}