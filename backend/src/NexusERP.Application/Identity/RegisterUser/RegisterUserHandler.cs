using FluentValidation;
using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Identity.Aggregates;
using NexusERP.Domain.Identity.Enums;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.Application.Identity.RegisterUser;

public sealed class RegisterUserHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly RegisterUserValidator _validator;

    public RegisterUserHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        RegisterUserValidator validator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    public async Task<RegisterUserResponse> Handle(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new DomainException(
                "Request cannot be null.");
        }

        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var email =
            new Email(request.Email);

        var exists = await _context.Users
            .AnyAsync(
                x => x.Email == email,
                cancellationToken);

        if (exists)
        {
            throw new DomainException(
                "A user with this email already exists.");
        }

        var firstName =
            new PersonName(request.FirstName);

        var lastName =
            new PersonName(request.LastName);

        var hashedPassword =
            _passwordHasher.Hash(
                request.Password);

        var passwordHash =
            new PasswordHash(
                hashedPassword);

        var user =
            User.Register(
                firstName,
                lastName,
                email,
                passwordHash);

        var role =
            Enum.Parse<UserRole>(
                request.Role,
                ignoreCase: true);

        user.ChangeRole(role);

        await _context.Users.AddAsync(
            user,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new RegisterUserResponse(
            user.Id,
            user.Email.Value,
            user.Role.ToString());
    }
}