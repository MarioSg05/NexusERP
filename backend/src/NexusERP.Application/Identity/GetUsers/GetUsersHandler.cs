using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Interfaces;

namespace NexusERP.Application.Identity.GetUsers;

public sealed class GetUsersHandler
{
    private readonly IApplicationDbContext _context;

    public GetUsersHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<UserListItem>> Handle(
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .Select(x => new UserListItem(
                x.Id,
                x.FirstName.Value,
                x.LastName.Value,
                x.Email.Value,
                x.Role.ToString(),
                x.IsActive))
            .ToListAsync(cancellationToken);
    }
}