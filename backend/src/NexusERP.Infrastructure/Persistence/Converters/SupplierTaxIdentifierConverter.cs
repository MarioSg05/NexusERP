using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class SupplierTaxIdentifierConverter
    : ValueConverter<SupplierTaxIdentifier, string>
{
    public SupplierTaxIdentifierConverter()
        : base(
            taxIdentifier => taxIdentifier.Value,
            value => new SupplierTaxIdentifier(value))
    {
    }
}