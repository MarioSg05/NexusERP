using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Customers.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class CustomerEmailConverter
    : ValueConverter<CustomerEmail, string>
{
    public CustomerEmailConverter()
        : base(
            email => email.Value,
            value => new CustomerEmail(value))
    {
    }
}