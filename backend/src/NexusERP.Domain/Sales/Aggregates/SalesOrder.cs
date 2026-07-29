using NexusERP.Domain.Common;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Sales.Entities;
using NexusERP.Domain.Sales.Enums;
using NexusERP.Domain.Sales.Events;
using NexusERP.Domain.Sales.ValueObjects;

namespace NexusERP.Domain.Sales.Aggregates;

public sealed class SalesOrder : AggregateRoot
{
    private readonly List<SalesOrderItem> _items = [];

    public Guid CustomerId { get; private set; }

    public DateTime OrderDate { get; private set; }

    public SalesOrderStatus Status { get; private set; }

    public SalesOrderTotal Total { get; private set; }

    public IReadOnlyCollection<SalesOrderItem> Items =>
        _items.AsReadOnly();

    private SalesOrder(Guid customerId)
    {
        if (customerId == Guid.Empty)
        {
            throw new DomainException(
                "Customer is required.");
        }

        CustomerId = customerId;
        OrderDate = DateTime.UtcNow;
        Status = SalesOrderStatus.Pending;
        Total = new SalesOrderTotal(0);
    }

    public static SalesOrder Create(Guid customerId)
    {
        var order = new SalesOrder(customerId);

        order.AddDomainEvent(
            new SalesOrderCreatedEvent(order.Id));

        return order;
    }

    public void AddItem(SalesOrderItem item)
    {
        if (item is null)
        {
            throw new DomainException(
                "Sales order item is required.");
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
                "Sales order item was not found.");
        }

        _items.Remove(item);

        RecalculateTotal();

        UpdateAudit();
    }

    public void Confirm()
    {
        EnsurePending();

        EnsureHasItems();

        Status = SalesOrderStatus.Confirmed;

        AddDomainEvent(
            new SalesOrderConfirmedEvent(Id));

        UpdateAudit();
    }

    public void Cancel()
    {
        EnsurePending();

        Status = SalesOrderStatus.Cancelled;

        UpdateAudit();
    }

    private void EnsurePending()
    {
        if (Status != SalesOrderStatus.Pending)
        {
            throw new DomainException(
                "Only pending sales orders can be modified.");
        }
    }

    private void EnsureHasItems()
    {
        if (_items.Count == 0)
        {
            throw new DomainException(
                "Sales order must contain at least one item.");
        }
    }

    private void RecalculateTotal()
    {
        Total = new SalesOrderTotal(
            _items.Sum(x => x.LineTotal.Value));
    }
}