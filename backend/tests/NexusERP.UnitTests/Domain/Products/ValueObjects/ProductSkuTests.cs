using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Products.ValueObjects;

namespace NexusERP.UnitTests.Domain.Products.ValueObjects;

public class ProductSkuTests
{
    [Fact]
    public void Constructor_Should_Create_ProductSku_When_Value_Is_Valid()
    {
        var sku = new ProductSku("LAP-001");

        Assert.Equal("LAP-001", sku.Value);
    }

    [Fact]
    public void Constructor_Should_Normalize_To_UpperCase()
    {
        var sku = new ProductSku("lap-001");

        Assert.Equal("LAP-001", sku.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Empty()
    {
        Assert.Throws<DomainException>(
            () => new ProductSku(""));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Has_Invalid_Characters()
    {
        Assert.Throws<DomainException>(
            () => new ProductSku("LAP@001"));
    }
}