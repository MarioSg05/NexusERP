namespace NexusERP.Application.Identity.RegisterUser;

public sealed record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);