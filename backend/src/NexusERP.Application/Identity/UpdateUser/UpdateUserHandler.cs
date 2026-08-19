using FluentValidation;
using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.Application.Identity.UpdateUser;

public sealed class UpdateUserHandler
{
    private readonly IApplicationDbContext _context;
    private readonly UpdateUserValidator _validator;

    public UpdateUserHandler(
        IApplicationDbContext context,
        UpdateUserValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<UpdateUserResponse> Handle(
        Guid id,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            request,
            cancellationToken);

        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                "User was not found.");
        }

        var email =
            new Email(request.Email);

        var emailExists = await _context.Users
            .AnyAsync(
                x =>
                    x.Id != id &&
                    x.Email == email,
                cancellationToken);

        if (emailExists)
        {
            throw new DomainException(
                "A user with this email already exists.");
        }

        var firstName =
            new PersonName(request.FirstName);

        var lastName =
            new PersonName(request.LastName);

        user.ChangeName(
            firstName,
            lastName);

        user.ChangeEmail(email);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new UpdateUserResponse(
            user.Id,
            user.FirstName.Value,
            user.LastName.Value,
            user.Email.Value,
            user.Role.ToString(),
            user.IsActive);
    }
}