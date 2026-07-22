using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Products.ValueObjects;

namespace NexusERP.UnitTests.Domain.Products.ValueObjects;

public class ProductNameTests
{
    [Fact]
    public void Constructor_Should_Create_ProductName_When_Value_Is_Valid()
    {
        // Arrange
        const string value = "Laptop Dell";

        // Act
        var name = new ProductName(value);

        // Assert
        Assert.Equal(value, name.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Empty()
    {
        Assert.Throws<DomainException>(
            () => new ProductName(""));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Exceeds_MaxLength()
    {
        var value = new string('A', ProductName.MaxLength + 1);

        Assert.Throws<DomainException>(
            () => new ProductName(value));
    }
}