namespace NexusERP.Application.Inventory.AdjustInventoryStock;

public sealed record AdjustInventoryStockResponse(
    Guid Id,
    int Quantity);