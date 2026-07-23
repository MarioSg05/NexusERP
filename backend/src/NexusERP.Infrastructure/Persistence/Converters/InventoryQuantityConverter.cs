using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexusERP.Domain.Inventory.ValueObjects;

namespace NexusERP.Infrastructure.Persistence.Converters;

public sealed class InventoryQuantityConverter
    : ValueConverter<InventoryQuantity, int>
{
    public InventoryQuantityConverter()
        : base(
            quantity => quantity.Value,
            value => new InventoryQuantity(value))
    {
    }
}