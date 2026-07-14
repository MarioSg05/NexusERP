using NexusERP.Domain.Common;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.Domain.Identity.Aggregates;

public sealed class User : AggregateRoot
{
    public PersonName FirstName { get; private set; }

    public PersonName LastName { get; private set; }

    public Email Email { get; private set; }

    public PasswordHash PasswordHash { get; private set; }

    public bool IsActive { get; private set; }

    private User(
        PersonName firstName,
        PersonName lastName,
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
    PersonName firstName,
    PersonName lastName,
    Email email,
    PasswordHash passwordHash)
    {

        return new User(
            firstName,
            lastName,
            email,
            passwordHash);
    }

    public void ChangeName(PersonName firstName, PersonName lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        UpdateAudit();
    }

    public void ChangeEmail(Email email)
    {
        Email = email;

        UpdateAudit();

    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;

        UpdateAudit();
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;

        UpdateAudit();
    }
}