namespace NexusERP.Application.Inventory.GetInventory;

public sealed class InventoryListItem
{
    public Guid Id { get; init; }

    public Guid ProductId { get; init; }

    public string ProductName { get; init; } = string.Empty;

    public string Sku { get; init; } = string.Empty;

    public int Quantity { get; init; }

    public bool IsActive { get; init; }
}