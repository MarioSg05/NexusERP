using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NexusERP.Infrastructure.Messaging.Inbox;

namespace NexusERP.Infrastructure.Persistence.Configurations;

public sealed class InboxMessageConfiguration
    : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(
        EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable(
            "InboxMessages");

        builder.HasKey(
            x => x.Id);

        builder.Property(
                x => x.ReceivedOnUtc)
            .IsRequired();

        builder.Property(
                x => x.Type)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(
                x => x.ProcessedOnUtc)
            .IsRequired(false);

        builder.HasIndex(
            x => x.ProcessedOnUtc);
    }
}