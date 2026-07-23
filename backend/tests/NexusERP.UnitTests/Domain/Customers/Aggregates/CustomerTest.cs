using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Customers.Enums;
using NexusERP.Domain.Customers.Events;
using NexusERP.Domain.Customers.ValueObjects;

namespace NexusERP.UnitTests.Domain.Customers.Aggregates;

public class CustomerTests
{
    [Fact]
    public void Register_Should_Create_Active_Customer()
    {
        // Arrange
        var name = new CustomerName("OpenAI");
        var email = new CustomerEmail("contact@openai.com");
        var phone = new CustomerPhone("+1 555 123 4567");

        // Act
        var customer = Customer.Register(
            name,
            email,
            phone,
            CustomerType.Corporate);

        // Assert
        Assert.True(customer.IsActive);

        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
        Assert.Equal(phone, customer.Phone);
        Assert.Equal(CustomerType.Corporate, customer.Type);
    }
        [Fact]
    public void Register_Should_Add_CustomerRegisteredEvent()
    {
        // Arrange
        var name = new CustomerName("OpenAI");
        var email = new CustomerEmail("contact@openai.com");

        // Act
        var customer = Customer.Register(
            name,
            email,
            null,
            CustomerType.Corporate);

        // Assert
        Assert.Single(customer.DomainEvents);

        Assert.IsType<CustomerRegisteredEvent>(
            customer.DomainEvents.First());
    }
        [Fact]
    public void Deactivate_Should_Set_IsActive_To_False()
    {
        // Arrange
        var customer = Customer.Register(
            new CustomerName("OpenAI"),
            new CustomerEmail("contact@openai.com"),
            null,
            CustomerType.Corporate);

        // Act
        customer.Deactivate();

        // Assert
        Assert.False(customer.IsActive);
    }
        [Fact]
    public void Activate_Should_Set_IsActive_To_True()
    {
        // Arrange
        var customer = Customer.Register(
            new CustomerName("OpenAI"),
            new CustomerEmail("contact@openai.com"),
            null,
            CustomerType.Corporate);

        customer.Deactivate();

        // Act
        customer.Activate();

        // Assert
        Assert.True(customer.IsActive);
    }
}