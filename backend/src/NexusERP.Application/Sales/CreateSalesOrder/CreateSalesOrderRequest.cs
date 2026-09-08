namespace NexusERP.Application.Sales.CreateSalesOrder;

public sealed record CreateSalesOrderRequest(
    Guid CustomerId,
    IReadOnlyCollection<CreateSalesOrderItemRequest> Items);