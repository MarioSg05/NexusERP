using NexusERP.Domain.Customers.ValueObjects;
using NexusERP.Domain.Exceptions;

namespace NexusERP.UnitTests.Domain.Customers.ValueObjects;

public class CustomerNameTests
{
    [Fact]
    public void Constructor_Should_Create_CustomerName_When_Value_Is_Valid()
    {
        // Arrange
        const string value = "OpenAI";

        // Act
        var name = new CustomerName(value);

        // Assert
        Assert.Equal(value, name.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Empty()
    {
        // Arrange
        const string value = "";

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new CustomerName(value));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Exceeds_MaxLength()
    {
        // Arrange
        var value = new string('A', CustomerName.MaxLength + 1);

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new CustomerName(value));
    }
}