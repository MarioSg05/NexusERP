namespace NexusERP.Application.Sales.CreateSalesOrder;

public sealed record CreateSalesOrderResponse(
    Guid Id,
    Guid CustomerId);