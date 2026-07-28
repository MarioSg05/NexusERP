using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.UnitTests.Domain.Purchasing.ValueObjects;

public class PurchaseLineTotalTests
{
    [Fact]
    public void Constructor_Should_Calculate_LineTotal()
    {
        var quantity = new PurchaseQuantity(5);
        var unitPrice = new PurchaseUnitPrice(10.50m);

        var total = new PurchaseLineTotal(
            quantity,
            unitPrice);

        Assert.Equal(52.50m, total.Value);
    }

    [Fact]
    public void Constructor_Should_Round_To_Two_Decimals()
    {
        var quantity = new PurchaseQuantity(3);
        var unitPrice = new PurchaseUnitPrice(10.125m);

        var total = new PurchaseLineTotal(
            quantity,
            unitPrice);

        Assert.Equal(30.39m, total.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Quantity_Is_Null()
    {
        var unitPrice = new PurchaseUnitPrice(10);

        Assert.Throws<DomainException>(
            () => new PurchaseLineTotal(
                null!,
                unitPrice));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_UnitPrice_Is_Null()
    {
        var quantity = new PurchaseQuantity(5);

        Assert.Throws<DomainException>(
            () => new PurchaseLineTotal(
                quantity,
                null!));
    }
}