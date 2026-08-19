using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Exceptions;
using NexusERP.Domain.Identity.Enums;

namespace NexusERP.Application.Identity.DeactivateUser;

public sealed class DeactivateUserHandler
{
    private readonly IApplicationDbContext _context;

    public DeactivateUserHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeactivateUserResponse> Handle(
        Guid id,
        Guid currentUserId,
        CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                "User was not found.");
        }

        if (user.Id == currentUserId)
        {
            throw new DomainException(
                "You cannot deactivate your own account.");
        }

        if (
            user.Role == UserRole.Administrator &&
            user.IsActive)
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
                    "The last active Administrator cannot be deactivated.");
            }
        }

        user.Deactivate();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new DeactivateUserResponse(
            user.Id,
            user.IsActive);
    }
}