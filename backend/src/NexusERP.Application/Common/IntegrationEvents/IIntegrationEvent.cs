namespace NexusERP.Application.Common.IntegrationEvents;

public interface IIntegrationEvent
{
    Guid Id { get; }

    DateTime OccurredOnUtc { get; }

    string Type { get; }
}