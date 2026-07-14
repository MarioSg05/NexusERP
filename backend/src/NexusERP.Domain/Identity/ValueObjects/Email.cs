using System.Text.RegularExpressions;
using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Identity.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");

        if (!Regex.IsMatch(value,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new DomainException("Invalid email.");

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}