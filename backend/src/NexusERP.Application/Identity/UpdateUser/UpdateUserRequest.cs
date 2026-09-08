namespace NexusERP.Application.Identity.UpdateUser;

public sealed record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Email);