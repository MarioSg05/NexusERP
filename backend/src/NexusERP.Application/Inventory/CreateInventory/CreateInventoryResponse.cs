namespace NexusERP.Application.Inventory.CreateInventory;

public sealed record CreateInventoryResponse(
    Guid Id,
    Guid ProductId,
    int Quantity);