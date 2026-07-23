using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Domain.Exceptions;

namespace NexusERP.UnitTests.Domain.Customers.ValueObjects;

public class CustomerEmailTests
{
    [Fact]
    public void Constructor_Should_Create_CustomerEmail_When_Value_Is_Valid()
    {
        // Arrange
        const string value = "contact@openai.com";

        // Act
        var email = new CustomerEmail(value);

        // Assert
        Assert.Equal(value, email.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Empty()
    {
        // Arrange
        const string value = "";

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new CustomerEmail(value));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Email_Format_Is_Invalid()
    {
        // Arrange
        const string value = "invalid-email";

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new CustomerEmail(value));
    }
}