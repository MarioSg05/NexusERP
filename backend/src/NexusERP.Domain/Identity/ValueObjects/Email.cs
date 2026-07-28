using System.Text.RegularExpressions;
using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Identity.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    public Email(string value)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(
                "Email cannot be empty.");
        }

        // Normalize
        var normalizedValue =
            value.Trim().ToLowerInvariant();

        // Business Rules
        if (!Regex.IsMatch(
                normalizedValue,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new DomainException(
                "Invalid email.");
        }

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}