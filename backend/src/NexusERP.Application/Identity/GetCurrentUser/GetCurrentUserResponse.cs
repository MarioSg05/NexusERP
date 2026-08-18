namespace NexusERP.Application.Identity.GetCurrentUser;

public sealed record GetCurrentUserResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    bool IsActive);