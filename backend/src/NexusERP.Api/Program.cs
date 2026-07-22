using NexusERP.Api.Endpoints.Identity;
using NexusERP.Api.Endpoints.Customers;
using NexusERP.Application.Customers.RegisterCustomer;
using NexusERP.Api.Endpoints.Products;
using NexusERP.Application.Products.RegisterProduct;
using NexusERP.Application.Identity.RegisterUser;
using NexusERP.Infrastructure;
using NexusERP.Application.Identity.LoginUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddAuthorization();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application
builder.Services.AddScoped<RegisterProductHandler>();

builder.Services.AddScoped<RegisterCustomerHandler>();

builder.Services.AddScoped<RegisterUserHandler>();

builder.Services.AddScoped<LoginUserHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapRegisterUser();

app.MapLoginUser();

app.MapMeEndpoint();

app.MapRegisterCustomer();

app.MapRegisterProduct();

app.Run();