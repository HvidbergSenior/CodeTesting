using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Integration;
using deftq.BuildingBlocks.Integration.Outbox;
using deftq.BuildingBlocks.Integration.Outbox.Configuration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace deftq.BuildingBlocks.Test.Integration.Outbox
{
    public class OutboxPublisherBackgroundServiceTest
    {
        [Fact]
        public async Task GivenBackgroundService_WhenPublishingOutBoxMessage_MessageIsProcessed()
        {
            // Arrange
            using var backgroundServiceCancellationTokenSource = new CancellationTokenSource();
            using var messageCancellationTokenSource = new CancellationTokenSource();
            using var nullLoggerFactory = new NullLoggerFactory();
            var logger = new Logger<OutboxPublisherBackgroundService>(nullLoggerFactory);
            var fakeIntegrationEventPublisher = new FakeIntegrationEventPublisher(messageCancellationTokenSource);
            var serviceScopeFactoryFake = new FakeServiceScopeFactory(fakeIntegrationEventPublisher);
            
            // Add a single unprocessed outbox message
            var outbox = serviceScopeFactoryFake.ServiceProvider.GetService<IOutbox>()!;
            outbox.Add(OutboxMessage.From(new IntegrationEventFake(Guid.NewGuid(), DateTime.UtcNow)));
            
            // Start outbox processor
            using var service = new OutboxPublisherBackgroundService(logger, serviceScopeFactoryFake);
            await service.StartAsync(backgroundServiceCancellationTokenSource.Token);
            await MessageWasReceivedOrTimeout(2000, messageCancellationTokenSource);
            backgroundServiceCancellationTokenSource.Cancel();

            // Assert message was processed
            var fakeOutbox = outbox as FakeOutbox ?? throw new InvalidOperationException("Should be a fake outbox");
            fakeOutbox.Messages.Values.Should().ContainSingle().Which.ProcessedDate.Should().NotBeNull();
        }
        
        [Fact]
        public async Task GivenBackgroundService_WhenPublishingOutBoxMessage_MessageIsProcessedAtLeastOnce()
        {
            // Arrange
            using var backgroundServiceCancellationTokenSource = new CancellationTokenSource();
            using var messageCancellationTokenSource = new CancellationTokenSource();
            using var nullLoggerFactory = new NullLoggerFactory();
            var logger = new Logger<OutboxPublisherBackgroundService>(nullLoggerFactory);
            using var fakeIntegrationEventPublisher = new FakeFailingIntegrationEventPublisher(messageCancellationTokenSource, 2);
            var serviceScopeFactoryFake = new FakeServiceScopeFactory(fakeIntegrationEventPublisher);
            
            // Add a single unprocessed outbox message
            var outbox = serviceScopeFactoryFake.ServiceProvider.GetService<IOutbox>()!;
            outbox.Add(OutboxMessage.From(new IntegrationEventFake(Guid.NewGuid(), DateTime.UtcNow)));
            
            // Start outbox processor
            using var service = new OutboxPublisherBackgroundService(logger, serviceScopeFactoryFake);
            await service.StartAsync(backgroundServiceCancellationTokenSource.Token);
            await MessageWasReceivedOrTimeout(10000, messageCancellationTokenSource);
            backgroundServiceCancellationTokenSource.Cancel();

            // Assert message was never processed
            var fakeOutbox = outbox as FakeOutbox ?? throw new InvalidOperationException("Should be a fake outbox");
            fakeOutbox.Messages.Values.Should().ContainSingle().Which.ProcessedDate.Should().BeNull();
        }
        
        private static async Task MessageWasReceivedOrTimeout(int timeoutMillis, CancellationTokenSource messageCancellationTokenSource)
        {
            try
            {
                await Task.Delay(timeoutMillis, messageCancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                // Ignore
            }
        }
    }

    internal class IntegrationEventFake : IntegrationEvent
    {
        public IntegrationEventFake(Guid id, DateTime occurredOn) : base(id, occurredOn)
        {
            
        }
    }
    
    internal sealed class FakeServiceScopeFactory : IServiceScopeFactory
    {
        public ServiceCollection ServiceCollection { get; } = new ServiceCollection();
        public ServiceProvider ServiceProvider { get; }
        
        public FakeServiceScopeFactory(IIntegrationEventPublisher integrationEventPublisher)
        {
            ServiceCollection.AddOutbox();
            ServiceCollection.Replace(ServiceDescriptor.Scoped<IOutbox>(_ => new FakeOutbox()));
            ServiceCollection.AddScoped<IUnitOfWork>(_ => new FakeUnitOfWork());
            ServiceCollection.AddScoped<IIntegrationEventPublisher>(_ => integrationEventPublisher);
            ServiceCollection.AddSingleton<ILogger<OutboxPublisher>>(_ => new Logger<OutboxPublisher>(new NullLoggerFactory()));
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
        
        public IServiceScope CreateScope()
        {
            return new FakeServiceScope(ServiceProvider);
        }
    }

    internal sealed class FakeServiceScope : IServiceScope
    {
        public IServiceProvider ServiceProvider { get; }

        public FakeServiceScope(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Dispose()
        {
            
        }
    }

    internal class FakeIntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public FakeIntegrationEventPublisher(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
        }
        
        public Task Publish(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();    
            return Task.CompletedTask;
        }
    }
    
    internal class FakeFailingIntegrationEventPublisher : IIntegrationEventPublisher, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private CountdownEvent _countDownEvent;

        public FakeFailingIntegrationEventPublisher(CancellationTokenSource cancellationTokenSource, int expectedEventCount)
        {
            _countDownEvent = new CountdownEvent(expectedEventCount);
            _cancellationTokenSource = cancellationTokenSource;
        }
        
        public Task Publish(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            _countDownEvent.Signal();
            if (_countDownEvent.IsSet)
            {
                _cancellationTokenSource.Cancel();    
            }

            throw new NotSupportedException();
        }

        public void Dispose()
        {
            _countDownEvent.Dispose();
        }
    }
}
