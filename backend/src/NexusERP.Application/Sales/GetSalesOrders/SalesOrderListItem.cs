namespace NexusERP.Application.Sales.GetSalesOrders;

public sealed class SalesOrderListItem
{
    public Guid Id { get; init; }

    public Guid CustomerId { get; init; }

    public string CustomerName { get; init; } = string.Empty;

    public DateTime OrderDate { get; init; }

    public string Status { get; init; } = string.Empty;

    public decimal Total { get; init; }
}