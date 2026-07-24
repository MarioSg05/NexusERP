using System.Text.RegularExpressions;
using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Suppliers.ValueObjects;

public sealed record SupplierEmail
{
    public const int MaxLength = 254;

    public string Value { get; }

    public SupplierEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Supplier email cannot be empty.");

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
            throw new DomainException(
                $"Supplier email cannot exceed {MaxLength} characters.");

        if (!Regex.IsMatch(
                normalizedValue,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new DomainException(
                "Supplier email has an invalid format.");
        }

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}