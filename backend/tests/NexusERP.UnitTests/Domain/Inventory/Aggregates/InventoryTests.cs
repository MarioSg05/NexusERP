using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Inventory.Aggregates;
using NexusERP.Domain.Inventory.Events;
using NexusERP.Domain.Inventory.ValueObjects;
using InventoryAggregate = NexusERP.Domain.Inventory.Aggregates.InventoryItem;

namespace NexusERP.UnitTests.Domain.Inventory.Aggregates;

public class InventoryTests
{
    [Fact]
    public void Create_Should_Create_Active_Inventory()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var quantity = new InventoryQuantity(10);

        // Act
        var inventory = InventoryAggregate.Create(productId, quantity);

        // Assert
        Assert.True(inventory.IsActive);
        Assert.Equal(productId, inventory.ProductId);
        Assert.Equal(quantity, inventory.Quantity);
    }

    [Fact]
    public void IncreaseStock_Should_Increase_Quantity()
    {
        var inventory = InventoryAggregate.Create(
            Guid.NewGuid(),
            new InventoryQuantity(10));

        inventory.IncreaseStock(
            new InventoryQuantity(5));

        Assert.Equal(
            new InventoryQuantity(15),
            inventory.Quantity);
    }

    [Fact]
    public void DecreaseStock_Should_Decrease_Quantity()
    {
        var inventory = InventoryAggregate.Create(
            Guid.NewGuid(),
            new InventoryQuantity(10));

        inventory.DecreaseStock(
            new InventoryQuantity(4));

        Assert.Equal(
            new InventoryQuantity(6),
            inventory.Quantity);
    }

    [Fact]
    public void DecreaseStock_Should_Throw_When_Stock_Is_Insufficient()
    {
        var inventory = InventoryAggregate.Create(
            Guid.NewGuid(),
            new InventoryQuantity(2));

        Assert.Throws<DomainException>(
            () => inventory.DecreaseStock(
                new InventoryQuantity(5)));
    }

    [Fact]
    public void IncreaseStock_Should_Throw_When_Quantity_Is_Zero()
    {
        var inventory = InventoryAggregate.Create(
            Guid.NewGuid(),
            new InventoryQuantity(5));

        Assert.Throws<DomainException>(
            () => inventory.IncreaseStock(
                new InventoryQuantity(0)));
    }

    [Fact]
    public void DecreaseStock_Should_Throw_When_Quantity_Is_Zero()
    {
        var inventory = InventoryAggregate.Create(
            Guid.NewGuid(),
            new InventoryQuantity(5));

        Assert.Throws<DomainException>(
            () => inventory.DecreaseStock(
                new InventoryQuantity(0)));
    }

    [Fact]
    public void AdjustStock_Should_Update_Quantity()
    {
        var inventory = InventoryAggregate.Create(
            Guid.NewGuid(),
            new InventoryQuantity(5));

        inventory.AdjustStock(
            new InventoryQuantity(20));

        Assert.Equal(
            new InventoryQuantity(20),
            inventory.Quantity);
    }

    [Fact]
    public void Create_Should_Add_InventoryCreatedEvent()
    {
        var inventory = InventoryAggregate.Create(
            Guid.NewGuid(),
            new InventoryQuantity(10));

        Assert.Single(inventory.DomainEvents);

        Assert.IsType<InventoryCreatedEvent>(
            inventory.DomainEvents.First());
    }
}