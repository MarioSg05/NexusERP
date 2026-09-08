using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Common.IntegrationEvents;
using NexusERP.Domain.Common;

namespace NexusERP.UnitTests.Application.Common.IntegrationEvents;

public sealed class IntegrationEventCollectorTests
{
    [Fact]
    public void Collect_WithRegisteredMapper_ShouldReturnMappedEvent()
    {
        var services =
            new ServiceCollection();

        services.AddSingleton<
            IIntegrationEventMapper<TestDomainEvent>,
            TestIntegrationEventMapper>();

        using var serviceProvider =
            services.BuildServiceProvider();

        var collector =
            new IntegrationEventCollector(
                serviceProvider);

        var domainEvent =
            new TestDomainEvent();

        var result =
            collector.Collect(
                [domainEvent]);

        var integrationEvent =
            Assert.Single(
                result);

        var testIntegrationEvent =
            Assert.IsType<TestIntegrationEvent>(
                integrationEvent);

        Assert.Equal(
            domainEvent.Value,
            testIntegrationEvent.Value);
    }

    [Fact]
    public void Collect_WithNoRegisteredMapper_ShouldReturnEmptyCollection()
    {
        var services =
            new ServiceCollection();

        using var serviceProvider =
            services.BuildServiceProvider();

        var collector =
            new IntegrationEventCollector(
                serviceProvider);

        var result =
            collector.Collect(
                [new TestDomainEvent()]);

        Assert.Empty(
            result);
    }

    [Fact]
    public void Collect_WithMultipleDomainEvents_ShouldMapSelectedEvents()
    {
        var services =
            new ServiceCollection();

        services.AddSingleton<
            IIntegrationEventMapper<TestDomainEvent>,
            TestIntegrationEventMapper>();

        using var serviceProvider =
            services.BuildServiceProvider();

        var collector =
            new IntegrationEventCollector(
                serviceProvider);

        var result =
            collector.Collect(
                [
                    new TestDomainEvent(),
                    new UnsupportedDomainEvent(),
                    new TestDomainEvent()
                ]);

        Assert.Equal(
            2,
            result.Count);
    }

    private sealed record TestDomainEvent
        : IDomainEvent
    {
        public Guid Value { get; } =
            Guid.NewGuid();

        public DateTime OccurredOn { get; } =
            DateTime.UtcNow;
    }

    private sealed record UnsupportedDomainEvent
        : IDomainEvent
    {
        public DateTime OccurredOn { get; } =
            DateTime.UtcNow;
    }

    private sealed record TestIntegrationEvent(
        Guid Id,
        DateTime OccurredOnUtc,
        Guid Value)
        : IIntegrationEvent
    {
        public string Type =>
            "test-event";
    }

    private sealed class TestIntegrationEventMapper
        : IIntegrationEventMapper<TestDomainEvent>
    {
        public IIntegrationEvent Map(
            TestDomainEvent domainEvent)
        {
            return new TestIntegrationEvent(
                Guid.NewGuid(),
                domainEvent.OccurredOn,
                domainEvent.Value);
        }
    }
}