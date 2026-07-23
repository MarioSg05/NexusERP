using System.Text.RegularExpressions;
using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Products.ValueObjects;

public sealed record ProductSku
{
    public const int MaxLength = 50;

    public string Value { get; }

    public ProductSku(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Product SKU cannot be empty.");

        var normalizedValue = value.Trim().ToUpperInvariant();

        if (normalizedValue.Length > MaxLength)
            throw new DomainException(
                $"Product SKU cannot exceed {MaxLength} characters.");

        if (!Regex.IsMatch(
                normalizedValue,
                @"^[A-Z0-9_-]+$"))
        {
            throw new DomainException(
                "Product SKU contains invalid characters.");
        }

        Value = normalizedValue;
    }

    public override string ToString() => Value;
}