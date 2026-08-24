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
using NexusERP.Application.Suppliers.GetSuppliers;
using NexusERP.Application.Purchasing.GetPurchaseOrders;
using NexusERP.Application.Purchasing.GetPurchaseOrderById;
using NexusERP.Application.Purchasing.ApprovePurchaseOrder;
using NexusERP.Application.Purchasing.CancelPurchaseOrder;
using NexusERP.Application.Sales.ConfirmSalesOrder;
using NexusERP.Application.Sales.CancelSalesOrder;
using NexusERP.Application.Sales.GetSalesOrderById;
using NexusERP.Application.Sales.GetSalesOrders;
using NexusERP.Application.Identity.GetCurrentUser;
using NexusERP.Application.Identity.GetUsers;
using NexusERP.Application.Identity.GetUserById;
using NexusERP.Application.Identity.UpdateUser;
using NexusERP.Application.Identity.ChangeUserRole;
using NexusERP.Application.Identity.ActivateUser;
using NexusERP.Application.Identity.DeactivateUser;
using NexusERP.Application.Suppliers.GetSupplierById;
using NexusERP.Application.Suppliers.UpdateSupplier;
using NexusERP.Application.Suppliers.ActivateSupplier;
using NexusERP.Application.Suppliers.DeactivateSupplier;
using NexusERP.Application.AI.BusinessInsights;
using NexusERP.Application.Common.DomainEvents;

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

        services.AddScoped<GetSuppliersHandler>();
        services.AddScoped<GetSupplierByIdHandler>();

        services.AddScoped<UpdateSupplierHandler>();
        services.AddScoped<UpdateSupplierValidator>();

        services.AddScoped<ActivateSupplierHandler>();
        services.AddScoped<DeactivateSupplierHandler>();

        // Purchasing
        services.AddScoped<CreatePurchaseOrderHandler>();
        services.AddScoped<CreatePurchaseOrderValidator>();

        services.AddScoped<GetPurchaseOrdersHandler>();
        services.AddScoped<GetPurchaseOrderByIdHandler>();
        services.AddScoped<ApprovePurchaseOrderHandler>();
        services.AddScoped<CancelPurchaseOrderHandler>();

        // Sales
        services.AddScoped<CreateSalesOrderHandler>();
        services.AddScoped<CreateSalesOrderValidator>();

        services.AddScoped<ConfirmSalesOrderHandler>();
        services.AddScoped<CancelSalesOrderHandler>();

        services.AddScoped<GetSalesOrdersHandler>();
        services.AddScoped<GetSalesOrderByIdHandler>();

        // Identity
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<RegisterUserValidator>();

        services.AddScoped<LoginUserHandler>();

        services.AddScoped<GetCurrentUserHandler>();
        services.AddScoped<GetUsersHandler>();
        services.AddScoped<GetUserByIdHandler>();

        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<UpdateUserValidator>();

        services.AddScoped<ChangeUserRoleHandler>();
        services.AddScoped<ChangeUserRoleValidator>();

        services.AddScoped<ActivateUserHandler>();
        services.AddScoped<DeactivateUserHandler>();

        // Reports 
        services.AddScoped<GetInventoryReportHandler>();
        services.AddScoped<GetLowStockReportHandler>();
        services.AddScoped<GetSalesReportHandler>();
        services.AddScoped<GetPurchasingReportHandler>();

        // Dashboard 
        services.AddScoped<GetDashboardHandler>();

        // AI
        services.AddScoped<BusinessInsightsAnalyzer>();
        services.AddScoped<GetBusinessInsightsHandler>();

        // Domain Events
        services.AddScoped<
            IDomainEventDispatcher,
            DomainEventDispatcher>();

        return services;
    }
}
