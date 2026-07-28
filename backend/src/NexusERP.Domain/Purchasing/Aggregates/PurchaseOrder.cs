using NexusERP.Domain.Common;
using NexusERP.Domain.Purchasing.Entities;
using NexusERP.Domain.Purchasing.Enums;
using NexusERP.Domain.Purchasing.Events;
using NexusERP.Domain.Purchasing.ValueObjects;
using NexusERP.Domain.Exceptions;

namespace NexusERP.Domain.Purchasing.Aggregates;

public sealed class PurchaseOrder : AggregateRoot
{
    private readonly List<PurchaseOrderItem> _items = [];

    public Guid SupplierId { get; private set; }

    public DateTime OrderDate { get; private set; }

    public PurchaseOrderStatus Status { get; private set; }

    public PurchaseOrderTotal Total { get; private set; }

    public IReadOnlyCollection<PurchaseOrderItem> Items =>
        _items.AsReadOnly();

    private PurchaseOrder(Guid supplierId)
    {
        if (supplierId == Guid.Empty)
        {
            throw new DomainException(
                "Supplier is required.");
        }

        SupplierId = supplierId;
        OrderDate = DateTime.UtcNow;
        Status = PurchaseOrderStatus.Pending;
        Total = new PurchaseOrderTotal(0);
    }

    public static PurchaseOrder Create(Guid supplierId)
    {
        var order = new PurchaseOrder(supplierId);

        order.AddDomainEvent(
            new PurchaseOrderCreatedEvent(order.Id));

        return order;
    }

    public void AddItem(PurchaseOrderItem item)
    {
        if (item is null)
        {
            throw new DomainException(
                "Purchase order item is required.");
        }

        EnsurePending();

        _items.Add(item);

        RecalculateTotal();

        UpdateAudit();
    }

    public void RemoveItem(Guid itemId)
    {
        EnsurePending();

        var item =
            _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new DomainException(
                "Purchase order item was not found.");
        }

        _items.Remove(item);

        RecalculateTotal();

        UpdateAudit();
    }

    public void Approve()
    {
        EnsurePending();

        EnsureHasItems();

        Status = PurchaseOrderStatus.Approved;

        UpdateAudit();
    }

    public void Cancel()
    {
        EnsurePending();

        Status = PurchaseOrderStatus.Cancelled;

        UpdateAudit();
    }

    private void EnsurePending()
    {
        if (Status != PurchaseOrderStatus.Pending)
        {
            throw new DomainException(
                "Only pending purchase orders can be modified.");
        }
    }

    private void EnsureHasItems()
    {
        if (_items.Count == 0)
        {
            throw new DomainException(
                "Purchase order must contain at least one item.");
        }
    }

    private void RecalculateTotal()
    {
        Total = new PurchaseOrderTotal(
            _items.Sum(x => x.LineTotal.Value));
    }
}