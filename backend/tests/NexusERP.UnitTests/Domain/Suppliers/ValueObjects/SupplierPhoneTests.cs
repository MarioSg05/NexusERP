using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.UnitTests.Domain.Suppliers.ValueObjects;

public class SupplierPhoneTests
{
    [Fact]
    public void Constructor_Should_Create_SupplierPhone_When_Value_Is_Valid()
    {
        // Arrange
        const string value = "+1 555 123 4567";

        // Act
        var phone = new SupplierPhone(value);

        // Assert
        Assert.Equal(value, phone.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Empty()
    {
        // Arrange
        const string value = "";

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new SupplierPhone(value));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Has_Invalid_Format()
    {
        // Arrange
        const string value = "ABC123";

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new SupplierPhone(value));
    }
}