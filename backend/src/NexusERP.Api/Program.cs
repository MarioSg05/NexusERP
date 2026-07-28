using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using NexusERP.Application;
using NexusERP.Infrastructure;
  
using NexusERP.Api.Extensions;
using NexusERP.Api.Endpoints.Purchasing;
using NexusERP.Api.Endpoints.Identity;
using NexusERP.Api.Endpoints.Customers;
using NexusERP.Api.Endpoints.Inventory;
using NexusERP.Api.Endpoints.Suppliers;
using NexusERP.Api.Endpoints.Products;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSection = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSection["Issuer"],

                ValidAudience = jwtSection["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSection["Key"]!))
            };
    });

// Authentication
builder.Services.AddAuthorization();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application
builder.Services.AddApplication();

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

app.UseAuthentication();

app.UseAuthorization();


// Sales Master Data
app.MapRegisterCustomer();
app.MapRegisterProduct();
app.MapCreateInventory();
app.MapRegisterSupplier();

// Purchasing
app.MapCreatePurchaseOrder();

app.Run();