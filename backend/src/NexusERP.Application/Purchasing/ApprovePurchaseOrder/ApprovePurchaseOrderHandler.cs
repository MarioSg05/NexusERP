using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Purchasing.ApprovePurchaseOrder;

public sealed class ApprovePurchaseOrderHandler
{
    private readonly IApplicationDbContext _context;

    public ApprovePurchaseOrderHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApprovePurchaseOrderResponse> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var purchaseOrder =
            await _context.PurchaseOrders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);

        if (purchaseOrder is null)
        {
            throw new NotFoundException(
                "Purchase order was not found.");
        }

        purchaseOrder.Approve();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new ApprovePurchaseOrderResponse(
            purchaseOrder.Id,
            purchaseOrder.Status.ToString());
    }
}