using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Purchasing.CancelPurchaseOrder;

public sealed class CancelPurchaseOrderHandler
{
    private readonly IApplicationDbContext _context;

    public CancelPurchaseOrderHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CancelPurchaseOrderResponse> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var purchaseOrder =
            await _context.PurchaseOrders
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);

        if (purchaseOrder is null)
        {
            throw new NotFoundException(
                "Purchase order was not found.");
        }

        purchaseOrder.Cancel();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new CancelPurchaseOrderResponse(
            purchaseOrder.Id,
            purchaseOrder.Status.ToString());
    }
}