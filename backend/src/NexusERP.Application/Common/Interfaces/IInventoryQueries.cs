using NexusERP.Application.Inventory.GetInventory;

namespace NexusERP.Application.Common.Interfaces;

public interface IInventoryQueries
{
    Task<IReadOnlyList<InventoryListItem>>
        GetInventoryAsync(
            CancellationToken cancellationToken = default);
}