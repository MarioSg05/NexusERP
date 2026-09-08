using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.UnitTests.Domain.Purchasing.ValueObjects;

public class PurchaseOrderTotalTests
{
    [Fact]
    public void Constructor_Should_Create_PurchaseOrderTotal_When_Value_Is_Valid()
    {
        var total = new PurchaseOrderTotal(150.75m);

        Assert.Equal(150.75m, total.Value);
    }

    [Fact]
    public void Constructor_Should_Allow_Zero()
    {
        var total = new PurchaseOrderTotal(0);

        Assert.Equal(0m, total.Value);
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_Value_Is_Negative()
    {
        Assert.Throws<DomainException>(
            () => new PurchaseOrderTotal(-1));
    }

    [Fact]
    public void Constructor_Should_Round_To_Two_Decimals()
    {
        var total = new PurchaseOrderTotal(100.125m);

        Assert.Equal(100.13m, total.Value);
    }
}