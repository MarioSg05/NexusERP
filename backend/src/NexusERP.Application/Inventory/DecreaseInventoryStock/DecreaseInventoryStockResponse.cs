namespace NexusERP.Application.Inventory.DecreaseInventoryStock;

public sealed record DecreaseInventoryStockResponse(
    Guid Id,
    int Quantity);