using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Domain.Exceptions;

namespace NexusERP.UnitTests.Domain.Customers.ValueObjects;

public class CustomerPhoneTests
{
    [Fact]
    public void Constructor_Should_Create_CustomerPhone_When_Value_Is_Valid()
    {
        // Arrange
        const string value = "+1 555 123 4567";

        // Act
        var phone = new CustomerPhone(value);

        // Assert
        Assert.Equal(value, phone.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Empty()
    {
        // Arrange
        const string value = "";

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new CustomerPhone(value));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Has_Invalid_Format()
    {
        // Arrange
        const string value = "ABC123";

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new CustomerPhone(value));
    }
}