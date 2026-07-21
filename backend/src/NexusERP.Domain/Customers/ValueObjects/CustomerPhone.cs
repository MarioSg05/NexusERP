using System.Text.RegularExpressions;
using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Customers.ValueObjects;

public sealed record CustomerPhone
{
    public const int MaxLength = 25;

    public string Value { get; }

    public CustomerPhone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Customer phone cannot be empty.");

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
            throw new DomainException($"Customer phone cannot exceed {MaxLength} characters.");

        if (!Regex.IsMatch(normalizedValue, @"^[0-9+\-()\s]+$"))
            throw new DomainException("Invalid customer phone.");

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}