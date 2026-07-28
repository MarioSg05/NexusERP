using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Customers.RegisterCustomer;
using NexusERP.Application.Products.RegisterProduct;
using NexusERP.Application.Inventory.CreateInventory;
using NexusERP.Application.Suppliers.RegisterSupplier;
using NexusERP.Application.Identity.RegisterUser;
using NexusERP.Application.Identity.LoginUser;
using NexusERP.Application.Purchasing.CreatePurchaseOrder;

namespace NexusERP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Customers
        services.AddScoped<RegisterCustomerHandler>();
        services.AddScoped<RegisterCustomerValidator>();

        // Products
        services.AddScoped<RegisterProductHandler>();
        services.AddScoped<RegisterProductValidator>();

        // Inventory
        services.AddScoped<CreateInventoryHandler>();
        services.AddScoped<CreateInventoryValidator>();

        // Suppliers
        services.AddScoped<RegisterSupplierHandler>();
        services.AddScoped<RegisterSupplierValidator>();

        // Purchasing
        services.AddScoped<CreatePurchaseOrderHandler>();
        services.AddScoped<CreatePurchaseOrderValidator>();

        // Identity
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();

        return services;
    }
}