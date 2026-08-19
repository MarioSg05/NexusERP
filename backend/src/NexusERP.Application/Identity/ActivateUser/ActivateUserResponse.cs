namespace NexusERP.Application.Identity.ActivateUser;

public sealed record ActivateUserResponse(
    Guid Id,
    bool IsActive);