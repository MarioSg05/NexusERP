using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class PasswordHashConverter
    : ValueConverter<PasswordHash, string>
{
    public PasswordHashConverter()
        : base(
            hash => hash.Value,
            value => new PasswordHash(value))
    {
    }
}