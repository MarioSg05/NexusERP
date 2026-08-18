using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using NexusERP.Application;
using NexusERP.Infrastructure;

using NexusERP.Api.Authorization;
using NexusERP.Api.Extensions;
using NexusERP.Api.Endpoints.Purchasing;
using NexusERP.Api.Endpoints.Identity;
using NexusERP.Api.Endpoints.Customers;
using NexusERP.Api.Endpoints.Inventory;
using NexusERP.Api.Endpoints.Suppliers;
using NexusERP.Api.Endpoints.Products;
using NexusERP.Api.Endpoints.Sales;
using NexusERP.Api.Endpoints.Reports;
using NexusERP.Api.Endpoints.Dashboard;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "bearer",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description =
                "JWT Authorization header using the Bearer scheme."
        });

    options.AddSecurityRequirement(
        document =>
            new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecuritySchemeReference(
                        "bearer",
                        document)
                ] = []
            });
});

// Authentication
builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSection =
            builder.Configuration
                .GetSection("Jwt");

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    jwtSection["Issuer"],

                ValidAudience =
                    jwtSection["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSection["Key"]!))
            };
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy =
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

    options.AddPolicy(
        AuthorizationPolicies.ManageErp,
        policy =>
        {
            policy.RequireRole(
                "Administrator",
                "Manager");
        });

    options.AddPolicy(
        AuthorizationPolicies.ManageUsers,
        policy =>
        {
            policy.RequireRole(
                "Administrator");
        });
});

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application
builder.Services.AddApplication();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Frontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Identity
app.MapRegisterUser();
app.MapLoginUser();
app.MapMeEndpoint();

app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();

app.UseAuthorization();


// Sales Master Data
app.MapRegisterCustomer();
app.MapGetCustomers();
app.MapGetCustomerById();
app.MapUpdateCustomer();

app.MapRegisterProduct();
app.MapGetProducts();
app.MapGetProductById();
app.MapUpdateProduct();

app.MapCreateInventory();
app.MapGetInventory();
app.MapIncreaseInventoryStock();
app.MapDecreaseInventoryStock();
app.MapAdjustInventoryStock();

app.MapRegisterSupplier();
app.MapGetSuppliers();

app.MapCreateSalesOrder();
app.MapGetSalesOrders();
app.MapGetSalesOrderById();
app.MapConfirmSalesOrder();
app.MapCancelSalesOrder();

// Purchasing
app.MapCreatePurchaseOrder();
app.MapGetPurchaseOrders();
app.MapGetPurchaseOrderById();
app.MapApprovePurchaseOrder();
app.MapCancelPurchaseOrder();

// Reports
app.MapGetInventoryReport();
app.MapGetLowStockReport();
app.MapGetSalesReport();
app.MapGetPurchasingReport();

// Dashboard
app.MapGetDashboard();

app.Run();