namespace NexusERP.Application.Identity.DeactivateUser;

public sealed record DeactivateUserResponse(
    Guid Id,
    bool IsActive);