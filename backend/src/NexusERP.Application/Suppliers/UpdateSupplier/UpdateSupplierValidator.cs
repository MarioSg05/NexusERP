using FluentValidation;

using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.Application.Suppliers.UpdateSupplier;

public sealed class UpdateSupplierValidator
    : AbstractValidator<UpdateSupplierRequest>
{
    public UpdateSupplierValidator()
    {
        RuleFor(x => x.Email)
            .MaximumLength(SupplierEmail.MaxLength)
            .When(x =>
                !string.IsNullOrWhiteSpace(
                    x.Email))
            .WithMessage(
                $"Supplier email cannot exceed {SupplierEmail.MaxLength} characters.");

        RuleFor(x => x.Phone)
            .MaximumLength(SupplierPhone.MaxLength)
            .When(x =>
                !string.IsNullOrWhiteSpace(
                    x.Phone))
            .WithMessage(
                $"Supplier phone cannot exceed {SupplierPhone.MaxLength} characters.");
    }
}