using FluentValidation;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.Application.Suppliers.RegisterSupplier;

public sealed class RegisterSupplierValidator
    : AbstractValidator<RegisterSupplierRequest>
{
    public RegisterSupplierValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Supplier name is required.")
            .MaximumLength(SupplierName.MaxLength)
            .WithMessage($"Supplier name cannot exceed {SupplierName.MaxLength} characters.");

        RuleFor(x => x.TaxIdentifier)
            .NotEmpty()
            .WithMessage("Supplier tax identifier is required.")
            .MaximumLength(SupplierTaxIdentifier.MaxLength)
            .WithMessage($"Supplier tax identifier cannot exceed {SupplierTaxIdentifier.MaxLength} characters.");

        RuleFor(x => x.Email)
            .MaximumLength(SupplierEmail.MaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage($"Supplier email cannot exceed {SupplierEmail.MaxLength} characters.");

        RuleFor(x => x.Phone)
            .MaximumLength(SupplierPhone.MaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage($"Supplier phone cannot exceed {SupplierPhone.MaxLength} characters.");
    }
}