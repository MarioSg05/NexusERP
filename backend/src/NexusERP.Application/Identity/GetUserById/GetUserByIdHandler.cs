using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Identity.GetUserById;

public sealed class GetUserByIdHandler
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDetail> Handle(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new UserDetail(
                x.Id,
                x.FirstName.Value,
                x.LastName.Value,
                x.Email.Value,
                x.Role.ToString(),
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                "User was not found.");
        }

        return user;
    }
}