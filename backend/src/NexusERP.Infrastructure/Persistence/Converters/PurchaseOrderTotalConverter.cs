using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class PurchaseOrderTotalConverter
    : ValueConverter<PurchaseOrderTotal, decimal>
{
    public PurchaseOrderTotalConverter()
        : base(
            total => total.Value,
            value => new PurchaseOrderTotal(value))
    {
    }
}