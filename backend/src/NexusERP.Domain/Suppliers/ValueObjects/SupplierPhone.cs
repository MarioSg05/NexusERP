using System.Text.RegularExpressions;
using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Suppliers.ValueObjects;

public sealed record SupplierPhone
{
    public const int MaxLength = 25;

    public string Value { get; }

    public SupplierPhone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Supplier phone cannot be empty.");

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
            throw new DomainException(
                $"Supplier phone cannot exceed {MaxLength} characters.");

        if (!Regex.IsMatch(normalizedValue, @"^[0-9+\-()\s]+$"))
            throw new DomainException(
                "Invalid supplier phone.");

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}