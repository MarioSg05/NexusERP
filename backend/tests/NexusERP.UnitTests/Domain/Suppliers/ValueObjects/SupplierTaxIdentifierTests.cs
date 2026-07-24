using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.UnitTests.Domain.Suppliers.ValueObjects;

public class SupplierTaxIdentifierTests
{
    [Fact]
    public void Constructor_Should_Create_SupplierTaxIdentifier_When_Value_Is_Valid()
    {
        var taxIdentifier = new SupplierTaxIdentifier("abc123");

        Assert.Equal("ABC123", taxIdentifier.Value);
    }

    [Fact]
    public void Constructor_Should_Normalize_To_UpperCase()
    {
        var taxIdentifier = new SupplierTaxIdentifier("ruc-001");

        Assert.Equal("RUC-001", taxIdentifier.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Empty()
    {
        Assert.Throws<DomainException>(
            () => new SupplierTaxIdentifier(""));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Exceeds_MaxLength()
    {
        var value = new string(
            'A',
            SupplierTaxIdentifier.MaxLength + 1);

        Assert.Throws<DomainException>(
            () => new SupplierTaxIdentifier(value));
    }
}