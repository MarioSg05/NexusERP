using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Suppliers.GetSuppliers;

public sealed class GetSuppliersHandler
{
    private readonly ISupplierQueries _supplierQueries;

    public GetSuppliersHandler(
        ISupplierQueries supplierQueries)
    {
        _supplierQueries = supplierQueries;
    }

    public async Task<IReadOnlyList<SupplierListItem>> Handle(
        CancellationToken cancellationToken = default)
    {
        return await _supplierQueries.GetSuppliersAsync(
            cancellationToken);
    }
}