using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class PersonNameConverter
    : ValueConverter<PersonName, string>
{
    public PersonNameConverter()
        : base(
            name => name.Value,
            value => new PersonName(value))
    {
    }
}