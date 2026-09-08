using FluentValidation;
using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.Application.Suppliers.UpdateSupplier;

public sealed class UpdateSupplierHandler
{
    private readonly IApplicationDbContext _context;
    private readonly UpdateSupplierValidator _validator;

    public UpdateSupplierHandler(
        IApplicationDbContext context,
        UpdateSupplierValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<UpdateSupplierResponse> Handle(
        Guid id,
        UpdateSupplierRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (supplier is null)
        {
            throw new NotFoundException(
                "Supplier was not found.");
        }

        SupplierEmail? email = null;

        if (!string.IsNullOrWhiteSpace(
                request.Email))
        {
            email =
                new SupplierEmail(
                    request.Email);
        }

        SupplierPhone? phone = null;

        if (!string.IsNullOrWhiteSpace(
                request.Phone))
        {
            phone =
                new SupplierPhone(
                    request.Phone);
        }

        supplier.ChangeEmail(email);
        supplier.ChangePhone(phone);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new UpdateSupplierResponse(
            supplier.Id,
            supplier.Name.Value,
            supplier.TaxIdentifier.Value,
            supplier.Email?.Value,
            supplier.Phone?.Value,
            supplier.IsActive);
    }
}