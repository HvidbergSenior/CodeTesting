using System.Diagnostics.CodeAnalysis;
using deftq.BuildingBlocks.DataAccess;
using Microsoft.Extensions.Logging;

namespace deftq.BuildingBlocks.Integration.Outbox
{
    public sealed class OutboxPublisher
    {
        private readonly IOutbox outbox;
        private readonly IUnitOfWork unitOfWork;
        private readonly IIntegrationEventPublisher integrationEventPublisher;
        private readonly ILogger<OutboxPublisher> logger;

        public OutboxPublisher(IOutbox outbox, IUnitOfWork unitOfWork, IIntegrationEventPublisher integrationEventPublisher, ILogger<OutboxPublisher> logger)
        {
            this.outbox = outbox;
            this.unitOfWork = unitOfWork;
            this.integrationEventPublisher = integrationEventPublisher;
            this.logger = logger;
        }

        [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "<Pending>")]
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        internal async Task<bool> PublishPendingAsync(CancellationToken cancellationToken)
        {
            var serializer = new OutboxMessageSerializer();
            var messages = outbox.GetUnProcessedMessages().ToList();
            foreach (var message in messages)
            {
                try
                {
                    var cmd = serializer.Deserialize(message.Payload, message.MessageType);
                    if (cmd is not null)
                    {
                        try
                        {
                            await integrationEventPublisher.Publish((IIntegrationEvent)cmd, cancellationToken);
                            message.Processed();
                        }
                        catch (Exception ex)
                        {
                            // We should mark the command as failed. And maybe try again?
                            logger.LogWarning(ex, "Processing InboxMessage {Id} failed", message.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Deserializing InboxMessage {Id} failed", message.Id);
                }
            }
            try
            {
                outbox.Processed(messages);
                await unitOfWork.Commit(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Committing Outbox failed");
                throw;
            }
            return false;
        }
    }
}
