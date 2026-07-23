using Microsoft.EntityFrameworkCore;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Domain.Exceptions;
using FluentValidation;

namespace NexusERP.Application.Customers.RegisterCustomer;

public sealed class RegisterCustomerHandler
{
    private readonly IApplicationDbContext _context;
    private readonly RegisterCustomerValidator _validator;

    public RegisterCustomerHandler(
        IApplicationDbContext context,
        RegisterCustomerValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<RegisterCustomerResponse> Handle(
        RegisterCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

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