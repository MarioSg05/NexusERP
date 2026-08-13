using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Customers.RegisterCustomer;
using NexusERP.Application.Products.RegisterProduct;
using NexusERP.Application.Inventory.CreateInventory;
using NexusERP.Application.Suppliers.RegisterSupplier;
using NexusERP.Application.Identity.RegisterUser;
using NexusERP.Application.Identity.LoginUser;
using NexusERP.Application.Purchasing.CreatePurchaseOrder;
using NexusERP.Application.Sales.CreateSalesOrder;
using NexusERP.Application.Reports.GetInventoryReport;
using NexusERP.Application.Reports.GetLowStockReport;
using NexusERP.Application.Reports.GetSalesReport;
using NexusERP.Application.Reports.GetPurchasingReport;
using NexusERP.Application.Dashboard.GetDashboard;
using NexusERP.Application.Customers.GetCustomers;
using NexusERP.Application.Customers.GetCustomerById;
using NexusERP.Application.Customers.UpdateCustomer;
using NexusERP.Application.Products.GetProducts;
using NexusERP.Application.Products.GetProductById;
using NexusERP.Application.Products.UpdateProduct;
using NexusERP.Application.Inventory.GetInventory;
using NexusERP.Application.Inventory.IncreaseInventoryStock;
using NexusERP.Application.Inventory.AdjustInventoryStock;
using NexusERP.Application.Inventory.DecreaseInventoryStock;

namespace NexusERP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Customers
        services.AddScoped<RegisterCustomerHandler>();
        services.AddScoped<RegisterCustomerValidator>();

        services.AddScoped<GetCustomersHandler>();
        services.AddScoped<GetCustomerByIdHandler>();

        services.AddScoped<UpdateCustomerHandler>();
        services.AddScoped<UpdateCustomerValidator>();

        // Products
        services.AddScoped<RegisterProductHandler>();
        services.AddScoped<RegisterProductValidator>();

        services.AddScoped<GetProductsHandler>();
        services.AddScoped<GetProductByIdHandler>();

        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<UpdateProductValidator>();

        // Inventory
        services.AddScoped<CreateInventoryHandler>();
        services.AddScoped<CreateInventoryValidator>();

        services.AddScoped<GetInventoryHandler>();

        services.AddScoped<IncreaseInventoryStockHandler>();
        services.AddScoped<IncreaseInventoryStockValidator>();

        services.AddScoped<DecreaseInventoryStockHandler>();
        services.AddScoped<DecreaseInventoryStockValidator>();

        services.AddScoped<AdjustInventoryStockHandler>();
        services.AddScoped<AdjustInventoryStockValidator>();

        // Suppliers
        services.AddScoped<RegisterSupplierHandler>();
        services.AddScoped<RegisterSupplierValidator>();

        // Purchasing
        services.AddScoped<CreatePurchaseOrderHandler>();
        services.AddScoped<CreatePurchaseOrderValidator>();

        // Sales
        services.AddScoped<CreateSalesOrderHandler>();
        services.AddScoped<CreateSalesOrderValidator>();

        // Identity
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();

        // Reports 
        services.AddScoped<GetInventoryReportHandler>();
        services.AddScoped<GetLowStockReportHandler>();
        services.AddScoped<GetSalesReportHandler>();
        services.AddScoped<GetPurchasingReportHandler>();

        // Dashboard 
        services.AddScoped<GetDashboardHandler>();

        return services;
    }
}