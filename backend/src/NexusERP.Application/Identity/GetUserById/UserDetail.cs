namespace NexusERP.Application.Identity.GetUserById;

public sealed record UserDetail(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    bool IsActive);