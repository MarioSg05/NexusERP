namespace NexusERP.Application.Identity.RegisterUser;

public sealed record RegisterUserResponse(
    Guid Id,
    string Email,
    string Role);