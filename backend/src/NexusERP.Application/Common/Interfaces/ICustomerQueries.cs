using NexusERP.Application.Customers.GetCustomers;

namespace NexusERP.Application.Common.Interfaces;

public interface ICustomerQueries
{
    Task<IReadOnlyList<CustomerListItem>>
        GetCustomersAsync(
            CancellationToken cancellationToken = default);

    Task<CustomerListItem?>
        GetCustomerByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

}