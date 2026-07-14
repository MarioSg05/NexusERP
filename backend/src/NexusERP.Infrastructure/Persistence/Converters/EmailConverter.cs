using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class EmailConverter
    : ValueConverter<Email, string>
{
    public EmailConverter()
        : base(
            email => email.Value,
            value => new Email(value))
    {
    }
}