using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Customers.GetCustomers;

namespace NexusERP.Application.Customers.GetCustomerById;

public sealed class GetCustomerByIdHandler
{
    private readonly ICustomerQueries _customerQueries;

    public GetCustomerByIdHandler(
        ICustomerQueries customerQueries)
    {
        _customerQueries = customerQueries;
    }

    public async Task<CustomerListItem?> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _customerQueries.GetCustomerByIdAsync(
            id,
            cancellationToken);
    }
}