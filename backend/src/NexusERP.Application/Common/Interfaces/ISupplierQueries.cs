using NexusERP.Application.Suppliers.GetSuppliers;

namespace NexusERP.Application.Common.Interfaces;

public interface ISupplierQueries
{
    Task<IReadOnlyList<SupplierListItem>>
        GetSuppliersAsync(
            CancellationToken cancellationToken = default);
}