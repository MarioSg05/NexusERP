using NexusERP.Domain.Common;
using NexusERP.Domain.Purchasing.ValueObjects;

namespace NexusERP.Domain.Purchasing.Entities;

public sealed class PurchaseOrderItem : BaseEntity
{
    public Guid ProductId { get; private set; }

    public PurchaseQuantity Quantity { get; private set; }

    public PurchaseUnitPrice UnitPrice { get; private set; }

    public PurchaseLineTotal LineTotal { get; private set; }

    private PurchaseOrderItem(
        Guid productId,
        PurchaseQuantity quantity,
        PurchaseUnitPrice unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;

        LineTotal = new PurchaseLineTotal(
            quantity,
            unitPrice);
    }

    public static PurchaseOrderItem Create(
        Guid productId,
        PurchaseQuantity quantity,
        PurchaseUnitPrice unitPrice)
    {
        return new PurchaseOrderItem(
            productId,
            quantity,
            unitPrice);
    }

    public void ChangeQuantity(
        PurchaseQuantity quantity)
    {
        Quantity = quantity;

        RecalculateLineTotal();

        UpdateAudit();
    }

    public void ChangeUnitPrice(
        PurchaseUnitPrice unitPrice)
    {
        UnitPrice = unitPrice;

        RecalculateLineTotal();

        UpdateAudit();
    }

    private void RecalculateLineTotal()
    {
        LineTotal = new PurchaseLineTotal(
            Quantity,
            UnitPrice);
    }
}