namespace NexusERP.Application.Sales.GetSalesOrderById;

public sealed class SalesOrderDetail
{
    public Guid Id { get; init; }

    public Guid CustomerId { get; init; }

    public string CustomerName { get; init; } = string.Empty;

    public DateTime OrderDate { get; init; }

    public string Status { get; init; } = string.Empty;

    public decimal Total { get; init; }

    public IReadOnlyList<SalesOrderItemDetail> Items { get; init; }
        = [];
}