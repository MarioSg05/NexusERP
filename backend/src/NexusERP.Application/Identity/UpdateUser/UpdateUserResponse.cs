namespace NexusERP.Application.Identity.UpdateUser;

public sealed record UpdateUserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    bool IsActive);