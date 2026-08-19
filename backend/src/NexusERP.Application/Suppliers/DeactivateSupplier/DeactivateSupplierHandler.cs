using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Suppliers.DeactivateSupplier;

public sealed class DeactivateSupplierHandler
{
    private readonly IApplicationDbContext _context;

    public DeactivateSupplierHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeactivateSupplierResponse> Handle(
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

        supplier.Deactivate();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new DeactivateSupplierResponse(
            supplier.Id,
            supplier.IsActive);
    }
}