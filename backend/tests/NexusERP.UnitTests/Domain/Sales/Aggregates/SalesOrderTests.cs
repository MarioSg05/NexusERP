using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Sales.Aggregates;
using NexusERP.Domain.Sales.Entities;
using NexusERP.Domain.Sales.Enums;
using NexusERP.Domain.Sales.ValueObjects;

namespace NexusERP.UnitTests.Domain.Sales.Aggregates;

public class SalesOrderTests
{
    [Fact]
    public void Create_Should_Create_SalesOrder_When_Data_Is_Valid()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var order = SalesOrder.Create(customerId);

        // Assert
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(SalesOrderStatus.Pending, order.Status);
        Assert.Empty(order.Items);
        Assert.Equal(0m, order.Total.Value);
    }

    [Fact]
    public void AddItem_Should_Add_New_Item()
    {
        // Arrange
        var order = SalesOrder.Create(Guid.NewGuid());

        var item = SalesOrderItem.Create(
            Guid.NewGuid(),
            new SalesQuantity(5),
            new SalesUnitPrice(10));

        // Act
        order.AddItem(item);

        // Assert
        Assert.Single(order.Items);
    }

    [Fact]
    public void AddItem_Should_Recalculate_Total()
    {
        // Arrange
        var order = SalesOrder.Create(Guid.NewGuid());

        var item = SalesOrderItem.Create(
            Guid.NewGuid(),
            new SalesQuantity(5),
            new SalesUnitPrice(10));

        // Act
        order.AddItem(item);

        // Assert
        Assert.Equal(50m, order.Total.Value);
    }

    [Fact]
    public void RemoveItem_Should_Remove_Item()
    {
        // Arrange
        var order = SalesOrder.Create(Guid.NewGuid());

        var item = SalesOrderItem.Create(
            Guid.NewGuid(),
            new SalesQuantity(5),
            new SalesUnitPrice(10));

        order.AddItem(item);

        // Act
        order.RemoveItem(item.Id);

        // Assert
        Assert.Empty(order.Items);
    }

    [Fact]
    public void RemoveItem_Should_Recalculate_Total()
    {
        // Arrange
        var order = SalesOrder.Create(Guid.NewGuid());

        var item = SalesOrderItem.Create(
            Guid.NewGuid(),
            new SalesQuantity(5),
            new SalesUnitPrice(10));

        order.AddItem(item);

        // Act
        order.RemoveItem(item.Id);

        // Assert
        Assert.Equal(0m, order.Total.Value);
    }

    [Fact]
    public void Confirm_Should_Change_Status_To_Confirmed()
    {
        // Arrange
        var order = SalesOrder.Create(Guid.NewGuid());

        order.AddItem(
            SalesOrderItem.Create(
                Guid.NewGuid(),
                new SalesQuantity(5),
                new SalesUnitPrice(10)));

        // Act
        order.Confirm();

        // Assert
        Assert.Equal(
            SalesOrderStatus.Confirmed,
            order.Status);
    }

    [Fact]
    public void Confirm_Should_Throw_DomainException_When_Order_Has_No_Items()
    {
        // Arrange
        var order = SalesOrder.Create(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<DomainException>(
            () => order.Confirm());
    }

    [Fact]
    public void Cancel_Should_Change_Status_To_Cancelled()
    {
        // Arrange
        var order = SalesOrder.Create(Guid.NewGuid());

        // Act
        order.Cancel();

        // Assert
        Assert.Equal(
            SalesOrderStatus.Cancelled,
            order.Status);
    }

    [Fact]
    public void AddItem_Should_Throw_DomainException_When_Order_Is_Confirmed()
    {
        // Arrange
        var order = SalesOrder.Create(Guid.NewGuid());

        order.AddItem(
            SalesOrderItem.Create(
                Guid.NewGuid(),
                new SalesQuantity(5),
                new SalesUnitPrice(10)));

        order.Confirm();

        // Act & Assert
        Assert.Throws<DomainException>(
            () => order.AddItem(
                SalesOrderItem.Create(
                    Guid.NewGuid(),
                    new SalesQuantity(1),
                    new SalesUnitPrice(5))));
    }
}