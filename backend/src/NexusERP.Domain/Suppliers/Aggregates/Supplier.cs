using NexusERP.Domain.Common;
using NexusERP.Domain.Suppliers.Events;
using NexusERP.Domain.Suppliers.ValueObjects;

namespace NexusERP.Domain.Suppliers.Aggregates;

public sealed class Supplier : AggregateRoot
{
    public SupplierName Name { get; private set; }

    public SupplierTaxIdentifier TaxIdentifier { get; private set; }

    public SupplierEmail? Email { get; private set; }

    public SupplierPhone? Phone { get; private set; }

    public bool IsActive { get; private set; }

    private Supplier(
        SupplierName name,
        SupplierTaxIdentifier taxIdentifier,
        SupplierEmail? email,
        SupplierPhone? phone)
    {
        Name = name;
        TaxIdentifier = taxIdentifier;
        Email = email;
        Phone = phone;

        IsActive = true;
    }

    public static Supplier Register(
        SupplierName name,
        SupplierTaxIdentifier taxIdentifier,
        SupplierEmail? email,
        SupplierPhone? phone)
    {
        var supplier = new Supplier(
            name,
            taxIdentifier,
            email,
            phone);

        supplier.AddDomainEvent(
            new SupplierRegisteredEvent(supplier.Id));

        return supplier;
    }

    public void ChangeEmail(SupplierEmail? email)
    {
        Email = email;

        UpdateAudit();
    }

    public void ChangePhone(SupplierPhone? phone)
    {
        Phone = phone;

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