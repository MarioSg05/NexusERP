using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Inventory.ValueObjects;

namespace NexusERP.UnitTests.Domain.Inventory.ValueObjects;

public class InventoryQuantityTests
{
    [Fact]
    public void Constructor_Should_Create_InventoryQuantity_When_Value_Is_Valid()
    {
        // Arrange
        const int value = 10;

        // Act
        var quantity = new InventoryQuantity(value);

        // Assert
        Assert.Equal(value, quantity.Value);
    }

    [Fact]
    public void Constructor_Should_Allow_Zero()
    {
        // Arrange
        const int value = 0;

        // Act
        var quantity = new InventoryQuantity(value);

        // Assert
        Assert.Equal(0, quantity.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Negative()
    {
        // Act & Assert
        Assert.Throws<DomainException>(
            () => new InventoryQuantity(-1));
    }
}