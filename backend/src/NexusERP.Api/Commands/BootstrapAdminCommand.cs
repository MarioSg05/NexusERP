using System.Data;

using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Identity.RegisterUser;
using NexusERP.Domain.Identity.Enums;
using NexusERP.Infrastructure.Persistence;

namespace NexusERP.Api.Commands;

public static class BootstrapAdminCommand
{
    public static async Task ExecuteAsync(
        IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        // Keep the existence check and creation in one transaction.
        await using var transaction =
            await dbContext.Database.BeginTransactionAsync(
                IsolationLevel.Serializable,
                cancellationToken);

        var administratorExists = await dbContext.Users
            .AnyAsync(
                user => user.Role == UserRole.Administrator,
                cancellationToken);

        if (administratorExists)
        {
            Console.WriteLine(
                "An administrator already exists. No changes were made.");

            return;
        }

        var section = configuration.GetSection("BootstrapAdmin");

        var request = new RegisterUserRequest(
            FirstName: GetRequiredValue(section, "FirstName"),
            LastName: GetRequiredValue(section, "LastName"),
            Email: GetRequiredValue(section, "Email"),
            Password: GetRequiredValue(section, "Password"),
            Role: nameof(UserRole.Administrator));

        var handler = scope.ServiceProvider
            .GetRequiredService<RegisterUserHandler>();

        await handler.Handle(request, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        Console.WriteLine(
            "The initial administrator was created successfully.");
    }

    private static string GetRequiredValue(
        IConfiguration section,
        string key)
    {
        var value = section[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"Missing configuration: BootstrapAdmin:{key}.");
        }

        return value;
    }
}