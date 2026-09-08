namespace NexusERP.Application.Identity.GetUsers;

public sealed record UserListItem(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    bool IsActive);