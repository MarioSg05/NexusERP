using NexusERP.Domain.Common;
using NexusERP.Domain.Sales.ValueObjects;

namespace NexusERP.Domain.Sales.Entities;

public sealed class SalesOrderItem : BaseEntity
{
    public Guid ProductId { get; private set; }

    public SalesQuantity Quantity { get; private set; }

    public SalesUnitPrice UnitPrice { get; private set; }

    public SalesLineTotal LineTotal { get; private set; }

    private SalesOrderItem(
        Guid productId,
        SalesQuantity quantity,
        SalesUnitPrice unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;

        LineTotal = new SalesLineTotal(
            quantity,
            unitPrice);
    }

    public static SalesOrderItem Create(
        Guid productId,
        SalesQuantity quantity,
        SalesUnitPrice unitPrice)
    {
        return new SalesOrderItem(
            productId,
            quantity,
            unitPrice);
    }

    public void ChangeQuantity(
        SalesQuantity quantity)
    {
        Quantity = quantity;

        RecalculateLineTotal();

        UpdateAudit();
    }

    public void ChangeUnitPrice(
        SalesUnitPrice unitPrice)
    {
        UnitPrice = unitPrice;

        RecalculateLineTotal();

        UpdateAudit();
    }

    private void RecalculateLineTotal()
    {
        LineTotal = new SalesLineTotal(
            Quantity,
            UnitPrice);
    }
}