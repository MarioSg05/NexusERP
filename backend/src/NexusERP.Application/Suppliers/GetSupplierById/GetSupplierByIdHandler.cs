using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Suppliers.GetSupplierById;

public sealed class GetSupplierByIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetSupplierByIdHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SupplierDetail> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var supplier = await _context.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (supplier is null)
        {
            throw new NotFoundException(
                "Supplier was not found.");
        }

        return new SupplierDetail(
            supplier.Id,
            supplier.Name.Value,
            supplier.TaxIdentifier.Value,
            supplier.Email?.Value,
            supplier.Phone?.Value,
            supplier.IsActive);
    }
}