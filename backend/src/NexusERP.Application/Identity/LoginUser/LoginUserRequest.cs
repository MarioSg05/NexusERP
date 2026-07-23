namespace NexusERP.Application.Identity.LoginUser;

public sealed record LoginUserRequest(
    string Email,
    string Password);