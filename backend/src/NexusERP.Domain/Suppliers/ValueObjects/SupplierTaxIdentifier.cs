using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Suppliers.ValueObjects;

public sealed record SupplierTaxIdentifier
{
    public const int MaxLength = 50;

    public string Value { get; }

    public SupplierTaxIdentifier(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(
                "Supplier tax identifier cannot be empty.");
        }

        var normalizedValue = value.Trim().ToUpperInvariant();

        if (normalizedValue.Length > MaxLength)
        {
            throw new DomainException(
                $"Supplier tax identifier cannot exceed {MaxLength} characters.");
        }

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}