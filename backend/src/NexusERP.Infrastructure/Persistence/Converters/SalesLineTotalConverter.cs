using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Sales.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class SalesLineTotalConverter
    : ValueConverter<SalesLineTotal, decimal>
{
    public SalesLineTotalConverter()
        : base(
            total => total.Value,
            value => new SalesLineTotal(value))
    {
    }
}