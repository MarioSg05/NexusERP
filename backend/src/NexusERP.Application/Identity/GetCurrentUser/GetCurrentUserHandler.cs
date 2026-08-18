using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Identity.GetCurrentUser;

public sealed class GetCurrentUserHandler
{
    private readonly IApplicationDbContext _context;

    public GetCurrentUserHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetCurrentUserResponse> Handle(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user =
            await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == userId,
                    cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                "User was not found.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException(
                "User account is inactive.");
        }

        return new GetCurrentUserResponse(
            user.Id,
            user.FirstName.Value,
            user.LastName.Value,
            user.Email.Value,
            user.Role.ToString(),
            user.IsActive);
    }
}