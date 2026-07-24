using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.UnitTests.Domain.Suppliers;

public class SupplierNameTests
{
    [Fact]
    public void Constructor_Should_Create_SupplierName_When_Value_Is_Valid()
    {
        const string value = "Microsoft";

        var name = new SupplierName(value);

        Assert.Equal(value, name.Value);
    }

    [Fact]
    public void Constructor_Should_Trim_Whitespace()
    {
        var name = new SupplierName("   Microsoft   ");

        Assert.Equal("Microsoft", name.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Empty()
    {
        Assert.Throws<DomainException>(
            () => new SupplierName(""));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Whitespace()
    {
        Assert.Throws<DomainException>(
            () => new SupplierName("   "));
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Exceeds_MaxLength()
    {
        var value = new string(
    'A',
    SupplierName.MaxLength + 1);

        Assert.Throws<DomainException>(
            () => new SupplierName(value));
    }
}