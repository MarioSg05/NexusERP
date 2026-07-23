using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Customers.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class CustomerNameConverter
    : ValueConverter<CustomerName, string>
{
    public CustomerNameConverter()
        : base(
            name => name.Value,
            value => new CustomerName(value))
    {
    }
}