using deftq.BuildingBlocks.Configuration;
using deftq.BuildingBlocks.Integration;
using deftq.BuildingBlocks.Tests.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace deftq.BuildingBlocks.Tests.Infrastructure
{
    public class PublishingIntegrationEventTests
    {
        internal sealed class MyEvent : IntegrationEvent
        {
            public MyEvent(Guid id, DateTime occuredOn) : base(id, occuredOn)
            {
            }
        }

        internal sealed class MyEventIntegrationEventHandler : IIntegrationEventListener<MyEvent>
        {
            public Task Handle(MyEvent notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        internal sealed class MyEventIntegrationEventHandler2 : IIntegrationEventListener<MyEvent>
        {
            public Task Handle(MyEvent notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task Will_Publish_IntegrationEvent_To_Handler()
        {
            var services = new ServiceCollection();
            services.UseBuildingBlocks();
            services.AddMediatR(typeof(PublishingIntegrationEventTests), typeof(Test2.MyEventIntegrationEventHandler));

            var serviceProvider = services.BuildServiceProvider();

            var handlers = serviceProvider
                .GetServices<INotificationHandler<MyEvent>>()
                .ToList();
            Assert.Equal(3, handlers.Count);

            var mediator = serviceProvider.GetRequiredService<IMediator>();

            var @event = new MyEvent(Guid.NewGuid(), DateTime.UtcNow);
            await mediator.Publish(@event);

            var publisher = serviceProvider.GetRequiredService<IIntegrationEventPublisher>();

            await publisher.Publish(@event, CancellationToken.None);

            Assert.All(handlers,
                t => Assert.True(t is IIntegrationEventListener<MyEvent>)
                );
        }
    }
}

namespace Test2
{
    internal sealed class MyEventIntegrationEventHandler : IIntegrationEventListener<PublishingIntegrationEventTests.MyEvent>
    {
        public Task Handle(PublishingIntegrationEventTests.MyEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
