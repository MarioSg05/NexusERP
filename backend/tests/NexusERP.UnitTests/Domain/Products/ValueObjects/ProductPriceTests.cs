using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Products.ValueObjects;

namespace NexusERP.UnitTests.Domain.Products.ValueObjects;

public class ProductPriceTests
{
    [Fact]
    public void Constructor_Should_Create_ProductPrice_When_Value_Is_Valid()
    {
        var price = new ProductPrice(1499.99m);

        Assert.Equal(1499.99m, price.Value);
    }

    [Fact]
    public void Constructor_Should_Allow_Zero()
    {
        var price = new ProductPrice(0);

        Assert.Equal(0, price.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Negative()
    {
        Assert.Throws<DomainException>(
            () => new ProductPrice(-1));
    }

    [Fact]
    public void Constructor_Should_Round_To_Two_Decimals()
    {
        var price = new ProductPrice(10.125m);

        Assert.Equal(10.13m, price.Value);
    }
}