using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class SupplierPhoneConverter
    : ValueConverter<SupplierPhone?, string?>
{
    public SupplierPhoneConverter()
        : base(
            phone => phone == null ? null : phone.Value,
            value => value == null ? null : new SupplierPhone(value))
    {
    }
}