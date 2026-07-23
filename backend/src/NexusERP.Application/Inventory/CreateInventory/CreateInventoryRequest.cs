namespace NexusERP.Application.Inventory.CreateInventory;

public sealed record CreateInventoryRequest(
    Guid ProductId,
    int Quantity);