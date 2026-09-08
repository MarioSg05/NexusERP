using NexusERP.Domain.Purchasing.Aggregates;
using NexusERP.Domain.Purchasing.Entities;
using NexusERP.Domain.Purchasing.Enums;
using NexusERP.Domain.Purchasing.ValueObjects;
using NexusERP.Domain.Exceptions;

namespace NexusERP.UnitTests.Domain.Purchasing.Aggregates;

public class PurchaseOrderTests
{
    [Fact]
    public void Create_Should_Create_PurchaseOrder_When_Data_Is_Valid()
    {
        // Arrange
        var supplierId = Guid.NewGuid();

        // Act
        var order = PurchaseOrder.Create(supplierId);

        // Assert
        Assert.Equal(supplierId, order.SupplierId);
        Assert.Equal(PurchaseOrderStatus.Pending, order.Status);
        Assert.Empty(order.Items);
        Assert.Equal(0m, order.Total.Value);
    }

    [Fact]
    public void AddItem_Should_Add_New_Item()
    {
        // Arrange
        var order = PurchaseOrder.Create(Guid.NewGuid());

        var item = PurchaseOrderItem.Create(
            Guid.NewGuid(),
            new PurchaseQuantity(5),
            new PurchaseUnitPrice(10));

        // Act
        order.AddItem(item);

        // Assert
        Assert.Single(order.Items);
    }

    [Fact]
    public void AddItem_Should_Recalculate_Total()
    {
        // Arrange
        var order = PurchaseOrder.Create(Guid.NewGuid());

        var item = PurchaseOrderItem.Create(
            Guid.NewGuid(),
            new PurchaseQuantity(5),
            new PurchaseUnitPrice(10));

        // Act
        order.AddItem(item);

        // Assert
        Assert.Equal(50m, order.Total.Value);
    }

    [Fact]
    public void RemoveItem_Should_Remove_Item()
    {
        // Arrange
        var order = PurchaseOrder.Create(Guid.NewGuid());

        var item = PurchaseOrderItem.Create(
            Guid.NewGuid(),
            new PurchaseQuantity(5),
            new PurchaseUnitPrice(10));

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
        var order = PurchaseOrder.Create(Guid.NewGuid());

        var item = PurchaseOrderItem.Create(
            Guid.NewGuid(),
            new PurchaseQuantity(5),
            new PurchaseUnitPrice(10));

        order.AddItem(item);

        // Act
        order.RemoveItem(item.Id);

        // Assert
        Assert.Equal(0m, order.Total.Value);
    }

    [Fact]
    public void Approve_Should_Change_Status_To_Approved()
    {
        var order = PurchaseOrder.Create(Guid.NewGuid());

        order.AddItem(
            PurchaseOrderItem.Create(
                Guid.NewGuid(),
                new PurchaseQuantity(5),
                new PurchaseUnitPrice(10)));

        order.Approve();

        Assert.Equal(
            PurchaseOrderStatus.Approved,
            order.Status);
    }

    [Fact]
    public void Approve_Should_Throw_DomainException_When_Order_Has_No_Items()
    {
        var order = PurchaseOrder.Create(Guid.NewGuid());

        Assert.Throws<DomainException>(
            () => order.Approve());
    }

    [Fact]
    public void Cancel_Should_Change_Status_To_Cancelled()
    {
        var order = PurchaseOrder.Create(Guid.NewGuid());

        order.Cancel();

        Assert.Equal(
            PurchaseOrderStatus.Cancelled,
            order.Status);
    }

    [Fact]
    public void AddItem_Should_Throw_DomainException_When_Order_Is_Approved()
    {
        var order = PurchaseOrder.Create(Guid.NewGuid());

        order.AddItem(
            PurchaseOrderItem.Create(
                Guid.NewGuid(),
                new PurchaseQuantity(5),
                new PurchaseUnitPrice(10)));

        order.Approve();

        Assert.Throws<DomainException>(
            () => order.AddItem(
                PurchaseOrderItem.Create(
                    Guid.NewGuid(),
                    new PurchaseQuantity(1),
                    new PurchaseUnitPrice(5))));
    }
}