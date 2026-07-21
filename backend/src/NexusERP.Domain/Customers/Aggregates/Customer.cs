using NexusERP.Domain.Common;
using NexusERP.Domain.Customers.Enums;
using NexusERP.Domain.Customers.Events;
using NexusERP.Domain.Customers.ValueObjects;

namespace NexusERP.Domain.Customers.Aggregates;

public sealed class Customer : AggregateRoot
{
    public CustomerName Name { get; private set; }

    public CustomerEmail Email { get; private set; }

    public CustomerPhone? Phone { get; private set; }

    public CustomerType Type { get; private set; }

    public bool IsActive { get; private set; }

    private Customer(
        CustomerName name,
        CustomerEmail email,
        CustomerPhone? phone,
        CustomerType type)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Type = type;

        IsActive = true;
    }

    public static Customer Register(
        CustomerName name,
        CustomerEmail email,
        CustomerPhone? phone,
        CustomerType type)
    {
        var customer = new Customer(
            name,
            email,
            phone,
            type);

        customer.AddDomainEvent(
            new CustomerRegisteredEvent(customer.Id));

        return customer;
    }

    public void ChangeName(CustomerName name)
    {
        Name = name;
        UpdateAudit();
    }

    public void ChangeEmail(CustomerEmail email)
    {
        Email = email;
        UpdateAudit();
    }

    public void ChangePhone(CustomerPhone? phone)
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