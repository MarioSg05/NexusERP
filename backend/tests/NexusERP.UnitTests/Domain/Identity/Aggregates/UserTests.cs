using NexusERP.Domain.Identity.Aggregates;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.UnitTests.Domain.Identity.Aggregates;

public class UserTests
{
    [Fact]
    public void Register_Should_Create_Active_User()
    {
        // Arrange
        var firstName = new PersonName("Mario");
        var lastName = new PersonName("Suyén");
        var email = new Email("mario@test.com");
        var password = new PasswordHash("HASH");

        // Act
        var user = User.Register(
            firstName,
            lastName,
            email,
            password);

        // Assert
        Assert.True(user.IsActive);

        Assert.Equal(email, user.Email);

        Assert.Equal(firstName, user.FirstName);

        Assert.Equal(lastName, user.LastName);
    }
}