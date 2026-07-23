using NexusERP.Domain.Common;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Inventory.Events;
using NexusERP.Domain.Inventory.ValueObjects;

namespace NexusERP.Domain.Inventory.Aggregates;

public sealed class InventoryItem : AggregateRoot
{
    public Guid ProductId { get; private set; }

    public InventoryQuantity Quantity { get; private set; }

    public bool IsActive { get; private set; }

    private InventoryItem(
        Guid productId,
        InventoryQuantity quantity)
    {
        ProductId = productId;
        Quantity = quantity;
        IsActive = true;
    }

    public static InventoryItem Create(
        Guid productId,
        InventoryQuantity quantity)
    {
        var inventory = new InventoryItem(
            productId,
            quantity);

        inventory.AddDomainEvent(
            new InventoryCreatedEvent(inventory.Id));

        return inventory;
    }

    public void IncreaseStock(InventoryQuantity quantity)
    {
        if (quantity.Value <= 0)
            throw new DomainException(
                "Stock increase must be greater than zero.");

        Quantity = new InventoryQuantity(
            Quantity.Value + quantity.Value);

        UpdateAudit();
    }

    public void DecreaseStock(InventoryQuantity quantity)
    {
        if (quantity.Value <= 0)
            throw new DomainException(
                "Stock decrease must be greater than zero.");

        if (Quantity.Value < quantity.Value)
            throw new DomainException(
                "Insufficient stock available.");

        Quantity = new InventoryQuantity(
            Quantity.Value - quantity.Value);

        UpdateAudit();
    }

    public void AdjustStock(InventoryQuantity quantity)
    {
        Quantity = quantity;

        UpdateAudit();
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;

        UpdateAudit();
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;

        UpdateAudit();
    }
}