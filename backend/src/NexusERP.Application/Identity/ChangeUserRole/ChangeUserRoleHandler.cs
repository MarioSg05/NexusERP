using FluentValidation;
using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Identity.Enums;

namespace NexusERP.Application.Identity.ChangeUserRole;

public sealed class ChangeUserRoleHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ChangeUserRoleValidator _validator;

    public ChangeUserRoleHandler(
        IApplicationDbContext context,
        ChangeUserRoleValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<ChangeUserRoleResponse> Handle(
        Guid id,
        Guid currentUserId,
        ChangeUserRoleRequest request,
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

        var role =
            Enum.Parse<UserRole>(
                request.Role,
                ignoreCase: true);

        if (
            user.Id == currentUserId &&
            user.Role == UserRole.Administrator &&
            role != UserRole.Administrator)
        {
            throw new DomainException(
                "You cannot remove your own Administrator role.");
        }

        if (
            user.Role == UserRole.Administrator &&
            user.IsActive &&
            role != UserRole.Administrator)
        {
            var activeAdministratorCount =
                await _context.Users
                    .CountAsync(
                        x =>
                            x.Role == UserRole.Administrator &&
                            x.IsActive,
                        cancellationToken);

            if (activeAdministratorCount <= 1)
            {
                throw new DomainException(
                    "The last active Administrator cannot be demoted.");
            }
        }

        user.ChangeRole(role);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new ChangeUserRoleResponse(
            user.Id,
            user.Role.ToString());
    }
}