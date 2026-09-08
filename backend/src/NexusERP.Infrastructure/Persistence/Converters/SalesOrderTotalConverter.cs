using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Sales.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class SalesOrderTotalConverter
    : ValueConverter<SalesOrderTotal, decimal>
{
    public SalesOrderTotalConverter()
        : base(
            total => total.Value,
            value => new SalesOrderTotal(value))
    {
    }
}