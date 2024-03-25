using System.Diagnostics.CodeAnalysis;
using deftq.BuildingBlocks.DataAccess;
using MediatR;
using Microsoft.Extensions.Logging;

namespace deftq.BuildingBlocks.Integration.Inbox
{
    public class InboxPublisher
    {
        private readonly IInbox inbox;
        private readonly ISender sender;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<InboxPublisher> logger;

        public InboxPublisher(IInbox inbox, ISender sender, IUnitOfWork unitOfWork, ILogger<InboxPublisher> logger)
        {
            this.inbox = inbox;
            this.sender = sender;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        internal async Task PublishPendingAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && await PublishBatchAsync(cancellationToken))
            { }
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "<Pending>")]
        private async Task<bool> PublishBatchAsync(CancellationToken cancellationToken)
        {
            var serializer = new InboxMessageSerializer();
            var messages = inbox.GetUnProcessedMessages();

            foreach (var message in messages)
            {
                try
                {
                    var cmd = serializer.Deserialize(message.Payload, message.MessageType);
                    if (cmd is not null)
                    {
                        try
                        {
                            await sender.Send(cmd, cancellationToken);
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
                    // Ending here means that we cant deserialize. We should mark it as failed.
                    logger.LogError(ex, "Processing InboxMessage {Id} failed", message.Id);
                }
            }
            inbox.Processed(messages);
            await unitOfWork.Commit(cancellationToken);
            return false;
        }
    }
}
