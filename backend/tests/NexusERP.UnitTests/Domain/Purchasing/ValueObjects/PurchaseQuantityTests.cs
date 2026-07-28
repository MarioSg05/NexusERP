using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.UnitTests.Domain.Purchasing.ValueObjects;

public class PurchaseQuantityTests
{
    [Fact]
    public void Constructor_Should_Create_PurchaseQuantity_When_Value_Is_Valid()
    {
        // Arrange
        const int value = 10;

        // Act
        var quantity = new PurchaseQuantity(value);

        // Assert
        Assert.Equal(value, quantity.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Zero()
    {
        Assert.Throws<DomainException>(
            () => new PurchaseQuantity(0));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Negative()
    {
        Assert.Throws<DomainException>(
            () => new PurchaseQuantity(-1));
    }
}