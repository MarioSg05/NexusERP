using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Inventory.GetInventory;

public sealed class GetInventoryHandler
{
    private readonly IInventoryQueries _inventoryQueries;

    public GetInventoryHandler(
        IInventoryQueries inventoryQueries)
    {
        _inventoryQueries = inventoryQueries;
    }

    public async Task<IReadOnlyList<InventoryListItem>> Handle(
        CancellationToken cancellationToken = default)
    {
        return await _inventoryQueries.GetInventoryAsync(
            cancellationToken);
    }
}