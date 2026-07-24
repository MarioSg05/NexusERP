using NexusERP.Domain.Suppliers.Aggregates;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.UnitTests.Domain.Suppliers.Aggregates;

public class SupplierTests
{
    [Fact]
    public void Register_Should_Create_Supplier()
    {
        // Arrange
        var name = new SupplierName("Microsoft");
        var taxIdentifier = new SupplierTaxIdentifier("RFC123456");
        var email = new SupplierEmail("contact@microsoft.com");
        var phone = new SupplierPhone("+1 555 123 4567");

        // Act
        var supplier = Supplier.Register(
            name,
            taxIdentifier,
            email,
            phone);

        // Assert
        Assert.Equal(name, supplier.Name);
        Assert.Equal(taxIdentifier, supplier.TaxIdentifier);
        Assert.Equal(email, supplier.Email);
        Assert.Equal(phone, supplier.Phone);
        Assert.True(supplier.IsActive);
    }

    [Fact]
    public void ChangeEmail_Should_Update_Email()
    {
        var supplier = Supplier.Register(
            new SupplierName("Microsoft"),
            new SupplierTaxIdentifier("RFC123456"),
            null,
            null);

        var email = new SupplierEmail("sales@microsoft.com");

        supplier.ChangeEmail(email);

        Assert.Equal(email, supplier.Email);
    }

    [Fact]
    public void ChangePhone_Should_Update_Phone()
    {
        var supplier = Supplier.Register(
            new SupplierName("Microsoft"),
            new SupplierTaxIdentifier("RFC123456"),
            null,
            null);

        var phone = new SupplierPhone("+1 555 999 8888");

        supplier.ChangePhone(phone);

        Assert.Equal(phone, supplier.Phone);
    }

    [Fact]
    public void Deactivate_Should_Set_IsActive_To_False()
    {
        var supplier = Supplier.Register(
            new SupplierName("Microsoft"),
            new SupplierTaxIdentifier("RFC123456"),
            null,
            null);

        supplier.Deactivate();

        Assert.False(supplier.IsActive);
    }

    [Fact]
    public void Activate_Should_Set_IsActive_To_True()
    {
        var supplier = Supplier.Register(
            new SupplierName("Microsoft"),
            new SupplierTaxIdentifier("RFC123456"),
            null,
            null);

        supplier.Deactivate();

        supplier.Activate();

        Assert.True(supplier.IsActive);
    }
}