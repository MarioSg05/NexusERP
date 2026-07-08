using NexusERP.Domain.Common;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.Domain.Identity.Aggregates;

public sealed class User : AggregateRoot
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public Email Email { get; private set; }

    public PasswordHash PasswordHash { get; private set; }

    public bool IsActive { get; private set; }

    private User(
        string firstName,
        string lastName,
        Email email,
        PasswordHash passwordHash)
    {
        FirstName = firstName;

        LastName = lastName;

        Email = email;

        PasswordHash = passwordHash;

        IsActive = true;
    }
    public static User Register(
    string firstName,
    string lastName,
    Email email,
    PasswordHash passwordHash)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.");

        return new User(
            firstName.Trim(),
            lastName.Trim(),
            email,
            passwordHash);
    }
}