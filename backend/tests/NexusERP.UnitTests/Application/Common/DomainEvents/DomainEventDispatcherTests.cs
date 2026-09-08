using Microsoft.Extensions.DependencyInjection;

using NexusERP.Application.Common.DomainEvents;
using NexusERP.Domain.Common;

namespace NexusERP.UnitTests.Application.Common.DomainEvents;

public sealed class DomainEventDispatcherTests
{
    [Fact]
    public async Task DispatchAsync_WithRegisteredHandler_ShouldInvokeHandler()
    {
        var services =
            new ServiceCollection();

        services.AddSingleton<
            TestDomainEventHandler>();

        services.AddSingleton<
            IDomainEventHandler<TestDomainEvent>>(
                provider =>
                    provider.GetRequiredService<
                        TestDomainEventHandler>());

        await using var serviceProvider =
            services.BuildServiceProvider();

        var dispatcher =
            new DomainEventDispatcher(
                serviceProvider);

        var domainEvent =
            new TestDomainEvent();

        await dispatcher.DispatchAsync(
            [domainEvent]);

        var handler =
            serviceProvider
                .GetRequiredService<
                    TestDomainEventHandler>();

        Assert.Equal(
            1,
            handler.InvocationCount);

        Assert.Same(
            domainEvent,
            handler.LastEvent);
    }

    [Fact]
    public async Task DispatchAsync_WithMultipleHandlers_ShouldInvokeAllHandlers()
    {
        var services =
            new ServiceCollection();

        var firstHandler =
            new TestDomainEventHandler();

        var secondHandler =
            new TestDomainEventHandler();

        services.AddSingleton<
            IDomainEventHandler<TestDomainEvent>>(
                firstHandler);

        services.AddSingleton<
            IDomainEventHandler<TestDomainEvent>>(
                secondHandler);

        await using var serviceProvider =
            services.BuildServiceProvider();

        var dispatcher =
            new DomainEventDispatcher(
                serviceProvider);

        await dispatcher.DispatchAsync(
            [new TestDomainEvent()]);

        Assert.Equal(
            1,
            firstHandler.InvocationCount);

        Assert.Equal(
            1,
            secondHandler.InvocationCount);
    }

    [Fact]
    public async Task DispatchAsync_WithNoRegisteredHandlers_ShouldCompleteSuccessfully()
    {
        var services =
            new ServiceCollection();

        await using var serviceProvider =
            services.BuildServiceProvider();

        var dispatcher =
            new DomainEventDispatcher(
                serviceProvider);

        await dispatcher.DispatchAsync(
            [new TestDomainEvent()]);
    }

    [Fact]
    public async Task DispatchAsync_WithMultipleEvents_ShouldDispatchEachEvent()
    {
        var services =
            new ServiceCollection();

        var handler =
            new TestDomainEventHandler();

        services.AddSingleton<
            IDomainEventHandler<TestDomainEvent>>(
                handler);

        await using var serviceProvider =
            services.BuildServiceProvider();

        var dispatcher =
            new DomainEventDispatcher(
                serviceProvider);

        await dispatcher.DispatchAsync(
            [
                new TestDomainEvent(),
                new TestDomainEvent()
            ]);

        Assert.Equal(
            2,
            handler.InvocationCount);
    }

    [Fact]
    public async Task DispatchAsync_ShouldPropagateCancellationToken()
    {
        var services =
            new ServiceCollection();

        var handler =
            new CancellationCapturingHandler();

        services.AddSingleton<
            IDomainEventHandler<TestDomainEvent>>(
                handler);

        await using var serviceProvider =
            services.BuildServiceProvider();

        var dispatcher =
            new DomainEventDispatcher(
                serviceProvider);

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        await dispatcher.DispatchAsync(
            [new TestDomainEvent()],
            cancellationToken);

        Assert.Equal(
            cancellationToken,
            handler.CancellationToken);
    }

    [Fact]
    public async Task DispatchAsync_WhenHandlerThrows_ShouldPropagateException()
    {
        var services =
            new ServiceCollection();

        services.AddSingleton<
            IDomainEventHandler<TestDomainEvent>,
            ThrowingDomainEventHandler>();

        await using var serviceProvider =
            services.BuildServiceProvider();

        var dispatcher =
            new DomainEventDispatcher(
                serviceProvider);

        var exception =
            await Assert.ThrowsAsync<
                InvalidOperationException>(
                    () =>
                        dispatcher.DispatchAsync(
                            [new TestDomainEvent()]));

        Assert.Equal(
            "Domain event handler failed.",
            exception.Message);
    }

    private sealed record TestDomainEvent
        : IDomainEvent
    {
        public DateTime OccurredOn { get; } =
            DateTime.UtcNow;
    }

    private sealed class TestDomainEventHandler
        : IDomainEventHandler<TestDomainEvent>
    {
        public int InvocationCount
        {
            get;
            private set;
        }

        public TestDomainEvent? LastEvent
        {
            get;
            private set;
        }

        public Task Handle(
            TestDomainEvent domainEvent,
            CancellationToken cancellationToken = default)
        {
            InvocationCount++;

            LastEvent =
                domainEvent;

            return Task.CompletedTask;
        }
    }

    private sealed class CancellationCapturingHandler
        : IDomainEventHandler<TestDomainEvent>
    {
        public CancellationToken CancellationToken
        {
            get;
            private set;
        }

        public Task Handle(
            TestDomainEvent domainEvent,
            CancellationToken cancellationToken = default)
        {
            CancellationToken =
                cancellationToken;

            return Task.CompletedTask;
        }
    }

    private sealed class ThrowingDomainEventHandler
        : IDomainEventHandler<TestDomainEvent>
    {
        public Task Handle(
            TestDomainEvent domainEvent,
            CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException(
                "Domain event handler failed.");
        }
    }
}