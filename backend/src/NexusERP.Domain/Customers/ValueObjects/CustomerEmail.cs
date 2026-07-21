using System.Text.RegularExpressions;
using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Customers.ValueObjects;

public sealed record CustomerEmail
{
    public const int MaxLength = 254;

    public string Value { get; }

    public CustomerEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Customer email cannot be empty.");

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
            throw new DomainException($"Customer email cannot exceed {MaxLength} characters.");

        if (!Regex.IsMatch(
                normalizedValue,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new DomainException("Customer email has an invalid format.");
        }

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}