using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Identity.Aggregates;
using NexusERP.Domain.Identity.Enums;
using NexusERP.Domain.Identity.ValueObjects;
using NexusERP.Infrastructure.Persistence;

namespace NexusERP.IntegrationTests.Infrastructure;

public static class TestUserSeeder
{
    public static async Task<TestUser> CreateAsync(
        IntegrationTestFactory factory,
        UserRole role = UserRole.Viewer,
        bool isActive = true)
    {
        var email =
            $"integration-{Guid.NewGuid():N}@nexuserp.test";

        const string password =
            "IntegrationTest123!";

        using var scope =
            factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var passwordHasher =
            scope.ServiceProvider
                .GetRequiredService<IPasswordHasher>();

        var user =
            User.Register(
                new PersonName("Integration"),
                new PersonName("User"),
                new Email(email),
                new PasswordHash(
                    passwordHasher.Hash(password)));

        user.ChangeRole(role);

        if (!isActive)
        {
            user.Deactivate();
        }

        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync();

        return new TestUser(
            user.Id,
            email,
            password,
            role);
    }
}

public sealed record TestUser(
    Guid Id,
    string Email,
    string Password,
    UserRole Role);