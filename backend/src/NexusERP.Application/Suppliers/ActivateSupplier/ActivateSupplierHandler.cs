using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Suppliers.ActivateSupplier;

public sealed class ActivateSupplierHandler
{
    private readonly IApplicationDbContext _context;

    public ActivateSupplierHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ActivateSupplierResponse> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (supplier is null)
        {
            throw new NotFoundException(
                "Supplier was not found.");
        }

        supplier.Activate();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new ActivateSupplierResponse(
            supplier.Id,
            supplier.IsActive);
    }
}