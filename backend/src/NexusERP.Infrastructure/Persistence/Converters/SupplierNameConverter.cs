using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class SupplierNameConverter
    : ValueConverter<SupplierName, string>
{
    public SupplierNameConverter()
        : base(
            name => name.Value,
            value => new SupplierName(value))
    {
    }
}