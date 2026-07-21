using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Customers.ValueObjects;

public sealed record CustomerName
{
    public const int MaxLength = 200;

    public string Value { get; }

    public CustomerName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Customer name cannot be empty.");

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
            throw new DomainException($"Customer name cannot exceed {MaxLength} characters.");

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}