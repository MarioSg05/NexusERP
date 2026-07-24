using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Suppliers.Aggregates;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.Application.Suppliers.RegisterSupplier;

public sealed class RegisterSupplierHandler
{
    private readonly IApplicationDbContext _context;
    private readonly RegisterSupplierValidator _validator;

    public RegisterSupplierHandler(
        IApplicationDbContext context,
        RegisterSupplierValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<RegisterSupplierResponse> Handle(
        RegisterSupplierRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var taxIdentifier = new SupplierTaxIdentifier(
            request.TaxIdentifier);

        var exists = await _context.Suppliers
            .AnyAsync(
                x => x.TaxIdentifier == taxIdentifier,
                cancellationToken);

        if (exists)
        {
            throw new DomainException(
                "A supplier with this tax identifier already exists.");
        }

        var name = new SupplierName(request.Name);

        SupplierEmail? email = null;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            email = new SupplierEmail(request.Email);
        }

        SupplierPhone? phone = null;

        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            phone = new SupplierPhone(request.Phone);
        }

        var supplier = Supplier.Register(
            name,
            taxIdentifier,
            email,
            phone);

        await _context.Suppliers.AddAsync(
            supplier,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new RegisterSupplierResponse(
            supplier.Id,
            supplier.TaxIdentifier.Value);
    }
}