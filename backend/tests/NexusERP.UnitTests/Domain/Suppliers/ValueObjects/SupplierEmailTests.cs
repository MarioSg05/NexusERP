using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.UnitTests.Domain.Suppliers.ValueObjects;

public class SupplierEmailTests
{
    [Fact]
    public void Constructor_Should_Create_SupplierEmail_When_Value_Is_Valid()
    {
        // Arrange
        const string value = "contact@supplier.com";

        // Act
        var email = new SupplierEmail(value);

        // Assert
        Assert.Equal(value, email.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Empty()
    {
        // Arrange
        const string value = "";

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new SupplierEmail(value));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Email_Format_Is_Invalid()
    {
        // Arrange
        const string value = "invalid-email";

        // Act & Assert
        Assert.Throws<DomainException>(
            () => new SupplierEmail(value));
    }
}