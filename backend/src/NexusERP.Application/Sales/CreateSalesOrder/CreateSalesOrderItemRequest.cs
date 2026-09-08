namespace NexusERP.Application.Sales.CreateSalesOrder;

public sealed record CreateSalesOrderItemRequest(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice);