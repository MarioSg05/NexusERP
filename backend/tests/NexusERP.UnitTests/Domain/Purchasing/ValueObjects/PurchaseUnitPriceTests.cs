using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.UnitTests.Domain.Purchasing.ValueObjects;

public class PurchaseUnitPriceTests
{
    [Fact]
    public void Constructor_Should_Create_PurchaseUnitPrice_When_Value_Is_Valid()
    {
        var price = new PurchaseUnitPrice(150.75m);

        Assert.Equal(150.75m, price.Value);
    }

    [Fact]
    public void Constructor_Should_Allow_Zero()
    {
        var price = new PurchaseUnitPrice(0);

        Assert.Equal(0, price.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Negative()
    {
        Assert.Throws<DomainException>(
            () => new PurchaseUnitPrice(-1));
    }

    [Fact]
    public void Constructor_Should_Round_To_Two_Decimals()
    {
        var price = new PurchaseUnitPrice(10.125m);

        Assert.Equal(10.13m, price.Value);
    }
}