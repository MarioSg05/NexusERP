using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Sales.GetSalesOrderById;

public sealed class GetSalesOrderByIdHandler
{
    private readonly ISalesQueries _salesQueries;

    public GetSalesOrderByIdHandler(
        ISalesQueries salesQueries)
    {
        _salesQueries = salesQueries;
    }

    public async Task<SalesOrderDetail?> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _salesQueries.GetSalesOrderByIdAsync(
            id,
            cancellationToken);
    }
}