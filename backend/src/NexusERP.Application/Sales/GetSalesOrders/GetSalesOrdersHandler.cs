using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Sales.GetSalesOrders;

public sealed class GetSalesOrdersHandler
{
    private readonly ISalesQueries _salesQueries;

    public GetSalesOrdersHandler(
        ISalesQueries salesQueries)
    {
        _salesQueries = salesQueries;
    }

    public async Task<IReadOnlyList<SalesOrderListItem>> Handle(
        CancellationToken cancellationToken = default)
    {
        return await _salesQueries.GetSalesOrdersAsync(
            cancellationToken);
    }
}