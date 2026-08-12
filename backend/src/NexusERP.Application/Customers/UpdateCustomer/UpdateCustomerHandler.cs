using FluentValidation;
using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Application.Common.Exceptions;
using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Domain.Exceptions;


namespace NexusERP.Application.Customers.UpdateCustomer;

public sealed class UpdateCustomerHandler
{
    private readonly IApplicationDbContext _context;
    private readonly UpdateCustomerValidator _validator;

    public UpdateCustomerHandler(
        IApplicationDbContext context,
        UpdateCustomerValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<UpdateCustomerResponse> Handle(
        Guid id,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var customer = await _context.Customers
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (customer is null)
        {
            throw new NotFoundException(
                "Customer was not found.");
        }

        var email = new CustomerEmail(request.Email);

        var emailExists = await _context.Customers
            .AnyAsync(
                x =>
                    x.Id != id &&
                    x.Email == email,
                cancellationToken);

        if (emailExists)
        {
            throw new DomainException(
                "A customer with this email already exists.");
        }

        var name =
            new CustomerName(request.Name);

        CustomerPhone? phone = null;

        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            phone =
                new CustomerPhone(request.Phone);
        }

        customer.ChangeName(name);
        customer.ChangeEmail(email);
        customer.ChangePhone(phone);
        customer.ChangeType(request.Type);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new UpdateCustomerResponse(
            customer.Id,
            customer.Email.Value);
    }
}