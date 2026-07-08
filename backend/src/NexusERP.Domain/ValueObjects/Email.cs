using System.Text.RegularExpressions;

namespace NexusERP.Domain.ValueObjects;

public sealed class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        if (!Regex.IsMatch(value,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email.");

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}