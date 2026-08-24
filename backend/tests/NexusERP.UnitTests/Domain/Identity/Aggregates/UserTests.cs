using NexusERP.Domain.Identity.Aggregates;
using NexusERP.Domain.Identity.Events;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.UnitTests.Domain.Identity.Aggregates;

public class UserTests
{
    [Fact]
    public void Register_Should_Create_Active_User()
    {
        // Arrange
        var firstName =
            new PersonName("Mario");

        var lastName =
            new PersonName("Suyén");

        var email =
            new Email("mario@test.com");

        var password =
            new PasswordHash("HASH");

        // Act
        var user =
            User.Register(
                firstName,
                lastName,
                email,
                password);

        // Assert
        Assert.True(user.IsActive);

        Assert.Equal(
            email,
            user.Email);

        Assert.Equal(
            firstName,
            user.FirstName);

        Assert.Equal(
            lastName,
            user.LastName);
    }

    [Fact]
    public void Register_Should_Raise_UserRegisteredEvent()
    {
        // Arrange
        var firstName =
            new PersonName("Mario");

        var lastName =
            new PersonName("Suyén");

        var email =
            new Email("mario.event@test.com");

        var password =
            new PasswordHash("HASH");

        // Act
        var user =
            User.Register(
                firstName,
                lastName,
                email,
                password);

        // Assert
        var domainEvent =
            Assert.Single(
                user.DomainEvents);

        var userRegisteredEvent =
            Assert.IsType<UserRegisteredEvent>(
                domainEvent);

        Assert.Equal(
            user.Id,
            userRegisteredEvent.UserId);
    }

    [Fact]
    public void ClearDomainEvents_Should_Remove_Pending_Events()
    {
        // Arrange
        var user =
            User.Register(
                new PersonName("Mario"),
                new PersonName("Suyén"),
                new Email("mario.clear@test.com"),
                new PasswordHash("HASH"));

        // Act
        user.ClearDomainEvents();

        // Assert
        Assert.Empty(
            user.DomainEvents);
    }
}