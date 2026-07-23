using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Products.ValueObjects;

public sealed record ProductName
{
    public const int MaxLength = 200;

    public string Value { get; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Product name cannot be empty.");

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
            throw new DomainException(
                $"Product name cannot exceed {MaxLength} characters.");

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}