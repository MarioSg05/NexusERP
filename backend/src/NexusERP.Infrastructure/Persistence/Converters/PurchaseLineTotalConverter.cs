using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class PurchaseLineTotalConverter
    : ValueConverter<PurchaseLineTotal, decimal>
{
    public PurchaseLineTotalConverter()
        : base(
            total => total.Value,
            value => new PurchaseLineTotal(value))
    {
    }
}