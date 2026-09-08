using NexusERP.Domain.Products.Aggregates;
using NexusERP.Domain.Products.ValueObjects;

namespace NexusERP.UnitTests.Domain.Products.Aggregates;

public class ProductTests
{
    [Fact]
    public void Register_Should_Create_Product()
    {
        // Arrange
        var name = new ProductName("Laptop");
        var sku = new ProductSku("LAPTOP-001");
        var price = new ProductPrice(1500m);

        // Act
        var product = Product.Register(
            name,
            sku,
            price);

        // Assert
        Assert.Equal(name, product.Name);
        Assert.Equal(sku, product.Sku);
        Assert.Equal(price, product.Price);
        Assert.True(product.IsActive);
    }

    [Fact]
    public void ChangePrice_Should_Update_Price()
    {
        // Arrange
        var product = Product.Register(
            new ProductName("Laptop"),
            new ProductSku("LAPTOP-001"),
            new ProductPrice(1500m));

        var newPrice = new ProductPrice(1750m);

        // Act
        product.ChangePrice(newPrice);

        // Assert
        Assert.Equal(newPrice, product.Price);
    }

    [Fact]
    public void ChangePrice_Should_Not_Update_When_Price_Is_The_Same()
    {
        // Arrange
        var price = new ProductPrice(1500m);

        var product = Product.Register(
            new ProductName("Laptop"),
            new ProductSku("LAPTOP-001"),
            price);

        // Act
        product.ChangePrice(price);

        // Assert
        Assert.Equal(price, product.Price);
    }

    [Fact]
    public void Deactivate_Should_Set_IsActive_To_False()
    {
        // Arrange
        var product = Product.Register(
            new ProductName("Laptop"),
            new ProductSku("LAPTOP-001"),
            new ProductPrice(1500m));

        // Act
        product.Deactivate();

        // Assert
        Assert.False(product.IsActive);
    }

    [Fact]
    public void Activate_Should_Set_IsActive_To_True()
    {
        // Arrange
        var product = Product.Register(
            new ProductName("Laptop"),
            new ProductSku("LAPTOP-001"),
            new ProductPrice(1500m));

        product.Deactivate();

        // Act
        product.Activate();

        // Assert
        Assert.True(product.IsActive);
    }
}