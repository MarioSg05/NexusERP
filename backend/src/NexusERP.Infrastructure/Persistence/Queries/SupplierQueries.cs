using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Suppliers.GetSuppliers;

namespace NexusERP.Infrastructure.Persistence.Queries;

public sealed class SupplierQueries
    : ISupplierQueries
{
    private readonly ApplicationDbContext _context;

    public SupplierQueries(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SupplierListItem>>
        GetSuppliersAsync(
            CancellationToken cancellationToken = default)
    {
        var suppliers = await _context.Suppliers
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return suppliers
            .Select(supplier => new SupplierListItem
            {
                Id = supplier.Id,
                Name = supplier.Name.Value,
                TaxIdentifier =
                    supplier.TaxIdentifier.Value,
                IsActive = supplier.IsActive
            })
            .ToList();
    }
}