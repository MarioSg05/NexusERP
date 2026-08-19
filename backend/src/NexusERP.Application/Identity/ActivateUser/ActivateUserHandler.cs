using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Identity.ActivateUser;

public sealed class ActivateUserHandler
{
    private readonly IApplicationDbContext _context;

    public ActivateUserHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ActivateUserResponse> Handle(
        Guid id,
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

        user.Activate();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new ActivateUserResponse(
            user.Id,
            user.IsActive);
    }
}