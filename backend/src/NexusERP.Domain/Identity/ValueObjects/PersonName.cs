namespace NexusERP.Domain.Identity.ValueObjects;
using NexusERP.Domain.Exceptions;
public sealed record PersonName
{
    public string Value { get; }

    public PersonName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Name cannot be empty.");

        value = value.Trim();

        if (value.Length > 100)
            throw new DomainException("Name cannot exceed 100 characters.");

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}