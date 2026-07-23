namespace NexusERP.Application.Identity.LoginUser;

public sealed record LoginUserResponse(
    Guid UserId,
    string Email,
    string AccessToken,
    DateTime ExpiresAt);