using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Customers.GetCustomers;

namespace NexusERP.Infrastructure.Persistence.Queries;

public sealed class CustomerQueries
    : ICustomerQueries
{
    private readonly ApplicationDbContext _context;

    public CustomerQueries(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CustomerListItem>>
        GetCustomersAsync(
            CancellationToken cancellationToken = default)
    {
        var customers = await _context.Customers
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return customers
            .Select(customer => new CustomerListItem
            {
                Id = customer.Id,
                Name = customer.Name.Value,
                Email = customer.Email.Value,
                Phone = customer.Phone?.Value,
                Type = customer.Type.ToString(),
                IsActive = customer.IsActive
            })
            .ToList();
    }

    public async Task<CustomerListItem?>
    GetCustomerByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (customer is null)
            return null;

        return new CustomerListItem
        {
            Id = customer.Id,
            Name = customer.Name.Value,
            Email = customer.Email.Value,
            Phone = customer.Phone?.Value,
            Type = customer.Type.ToString(),
            IsActive = customer.IsActive
        };
    }
}