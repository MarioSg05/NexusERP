using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Suppliers.ValueObjects;

public sealed record SupplierName
{
    public const int MaxLength = 200;

    public string Value { get; }

    public SupplierName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(
                "Supplier name cannot be empty.");
        }

        value = value.Trim();

        if (value.Length > MaxLength)
        {
            throw new DomainException(
                $"Supplier name cannot exceed {MaxLength} characters.");
        }

        Value = value;
    }

    public override string ToString() => Value;
}