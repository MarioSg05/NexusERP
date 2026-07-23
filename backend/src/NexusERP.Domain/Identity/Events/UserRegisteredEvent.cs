using NexusERP.Domain.Common;

namespace NexusERP.Domain.Identity.Events;

public sealed class UserRegisteredEvent : IDomainEvent
{
    public Guid UserId { get; }

    public DateTime OccurredOn { get; }

    public UserRegisteredEvent(Guid userId)
    {
        UserId = userId;
        OccurredOn = DateTime.UtcNow;
    }
}