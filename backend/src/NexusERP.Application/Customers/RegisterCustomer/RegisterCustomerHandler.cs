using Microsoft.EntityFrameworkCore;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Domain.Exceptions;

namespace NexusERP.Application.Customers.RegisterCustomer;

public sealed class RegisterCustomerHandler
{
    private readonly IApplicationDbContext _context;

    public RegisterCustomerHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterCustomerResponse> Handle(
        RegisterCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
            throw new DomainException("Request cannot be null.");

        var email = new CustomerEmail(request.Email);

        var exists = await _context.Customers
            .AnyAsync(
                x => x.Email == email,
                cancellationToken);

        if (exists)
            throw new DomainException(
                "A customer with this email already exists.");

        var name = new CustomerName(request.Name);

        CustomerPhone? phone = null;

        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            phone = new CustomerPhone(request.Phone);
        }

        var customer = Customer.Register(
            name,
            email,
            phone,
            request.Type);

        await _context.Customers.AddAsync(
            customer,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterCustomerResponse(
            customer.Id,
            customer.Email.Value);
    }
}