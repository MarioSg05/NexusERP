using NexusERP.Domain.Purchasing.Entities;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.UnitTests.Domain.Purchasing.Entities;

public class PurchaseOrderItemTests
{
    [Fact]
    public void Create_Should_Create_Item()
    {
        var item = PurchaseOrderItem.Create(
            Guid.NewGuid(),
            new PurchaseQuantity(5),
            new PurchaseUnitPrice(10));

        Assert.Equal(5, item.Quantity.Value);
        Assert.Equal(10m, item.UnitPrice.Value);
        Assert.Equal(50m, item.LineTotal.Value);
    }

    [Fact]
    public void ChangeQuantity_Should_Recalculate_LineTotal()
    {
        var item = PurchaseOrderItem.Create(
            Guid.NewGuid(),
            new PurchaseQuantity(2),
            new PurchaseUnitPrice(10));

        item.ChangeQuantity(
            new PurchaseQuantity(5));

        Assert.Equal(50m, item.LineTotal.Value);
    }

    [Fact]
    public void ChangeUnitPrice_Should_Recalculate_LineTotal()
    {
        var item = PurchaseOrderItem.Create(
            Guid.NewGuid(),
            new PurchaseQuantity(2),
            new PurchaseUnitPrice(10));

        item.ChangeUnitPrice(
            new PurchaseUnitPrice(15));

        Assert.Equal(30m, item.LineTotal.Value);
    }
}