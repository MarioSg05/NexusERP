namespace NexusERP.Domain.Identity.ValueObjects;
using NexusERP.Domain.Exceptions;
public sealed record PasswordHash
{
    public string Value { get; }

    public PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Password hash cannot be empty.");

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}