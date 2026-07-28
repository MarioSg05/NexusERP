using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Suppliers.ValueObjects;

public sealed record SupplierName
{
    public const int MaxLength = 200;

    public string Value { get; }

    public SupplierName(string value)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(
                "Supplier name cannot be empty.");
        }

        // Normalize
        var normalizedValue = value.Trim();

        // Business Rules
        if (normalizedValue.Length > MaxLength)
        {
            throw new DomainException(
                $"Supplier name cannot exceed {MaxLength} characters.");
        }

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}