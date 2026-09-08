namespace NexusERP.Application.Inventory.IncreaseInventoryStock;

public sealed record IncreaseInventoryStockResponse(
    Guid Id,
    int Quantity);