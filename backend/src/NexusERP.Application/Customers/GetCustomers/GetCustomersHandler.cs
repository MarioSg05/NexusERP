using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Customers.GetCustomers;

public sealed class GetCustomersHandler
{
    private readonly ICustomerQueries _customerQueries;

    public GetCustomersHandler(
        ICustomerQueries customerQueries)
    {
        _customerQueries = customerQueries;
    }

    public async Task<IReadOnlyList<CustomerListItem>> Handle(
        CancellationToken cancellationToken = default)
    {
        return await _customerQueries.GetCustomersAsync(
            cancellationToken);
    }
}