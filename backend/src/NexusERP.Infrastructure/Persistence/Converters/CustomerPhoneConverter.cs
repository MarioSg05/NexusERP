using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Customers.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class CustomerPhoneConverter
    : ValueConverter<CustomerPhone?, string?>
{
    public CustomerPhoneConverter()
        : base(
            phone => phone == null ? null : phone.Value,
            value => string.IsNullOrWhiteSpace(value)
                ? null
                : new CustomerPhone(value))
    {
    }
}