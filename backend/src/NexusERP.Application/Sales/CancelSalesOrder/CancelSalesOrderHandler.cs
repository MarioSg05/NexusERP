using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Sales.CancelSalesOrder;

public sealed class CancelSalesOrderHandler
{
    private readonly IApplicationDbContext _context;

    public CancelSalesOrderHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CancelSalesOrderResponse> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var salesOrder = await _context.SalesOrders
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (salesOrder is null)
        {
            throw new NotFoundException(
                "Sales order was not found.");
        }

        salesOrder.Cancel();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new CancelSalesOrderResponse(
            salesOrder.Id,
            salesOrder.Status.ToString());
    }
}