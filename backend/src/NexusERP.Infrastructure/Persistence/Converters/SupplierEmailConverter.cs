using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class SupplierEmailConverter
    : ValueConverter<SupplierEmail?, string?>
{
    public SupplierEmailConverter()
        : base(
            email => email == null ? null : email.Value,
            value => value == null ? null : new SupplierEmail(value))
    {
    }
}